using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ActionClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ResultClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ResultClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Core;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Manipulation;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ActionClassified
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
