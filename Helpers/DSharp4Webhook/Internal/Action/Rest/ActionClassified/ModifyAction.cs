using System.Linq;
using System.Threading.Tasks;
using DSharp4Webhook.Action.Entities.Rest.ActionClassified;
using DSharp4Webhook.Action.Entities.Rest.ResultClassified;
using DSharp4Webhook.Core.Entities;
using DSharp4Webhook.Internal.Action.Rest.ResultClassified;
using DSharp4Webhook.Internal.Core;
using DSharp4Webhook.Rest.Entities;
using DSharp4Webhook.Rest.Manipulation;
using DSharp4Webhook.Serialization;
using Newtonsoft.Json;

namespace DSharp4Webhook.Internal.Action.Rest.ActionClassified
{
    internal sealed class ModifyAction : BaseRestAction<IModifyResult>, IModifyAction
    {
        public SerializeContext Context { get; }

        public ModifyAction(SerializeContext context, IWebhook webhook, RestSettings restSettings) : base(webhook, restSettings)
        {
            Context = context;
        }

        public override async Task<bool> ExecuteAsync()
        {
            CheckExecution();
            var responses = await Webhook.RestProvider.PATCH(Webhook.GetWebhookUrl(), Context, RestSettings);
            var lastResponse = responses[responses.Length - 1];
            WebhookInfo webhookInfo = JsonConvert.DeserializeObject<WebhookInfo>(lastResponse.Content!);
            webhookInfo._webhook = Webhook;
            Result = new ModifyResult(webhookInfo, responses);
            SettingRateLimit();
            return BaseRestProvider.GET_ALLOWED_STATUSES.Contains(lastResponse.StatusCode);
        }
    }
}
