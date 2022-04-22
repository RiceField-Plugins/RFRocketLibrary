using System.Linq;
using System.Threading.Tasks;
using DSharp4Webhook.Action.Entities.Rest.ActionClassified;
using DSharp4Webhook.Action.Entities.Rest.ResultClassified;
using DSharp4Webhook.Core.Entities;
using DSharp4Webhook.Internal.Action.Rest.ResultClassified;
using DSharp4Webhook.Internal.Core;
using DSharp4Webhook.Rest.Entities;
using DSharp4Webhook.Rest.Manipulation;
using Newtonsoft.Json;

namespace DSharp4Webhook.Internal.Action.Rest.ActionClassified
{
    internal sealed class InfoAction : BaseRestAction<IInfoResult>, IInfoAction
    {
        public InfoAction(IWebhook webhook, RestSettings restSettings) : base(webhook, restSettings) { }

        public override async Task<bool> ExecuteAsync()
        {
            CheckExecution();
            var responses = await Webhook.RestProvider.GET(Webhook.GetWebhookUrl(), RestSettings);
            var lastResponse = responses[responses.Length - 1];
            if (!(lastResponse.Content is null))
            {
                WebhookInfo webhookInfo = JsonConvert.DeserializeObject<WebhookInfo>(lastResponse.Content);
                webhookInfo._webhook = Webhook;
                Result = new InfoResult(webhookInfo, responses);
            }
            SettingRateLimit();
            return BaseRestProvider.GET_ALLOWED_STATUSES.Contains(lastResponse.StatusCode);
        }
    }
}
