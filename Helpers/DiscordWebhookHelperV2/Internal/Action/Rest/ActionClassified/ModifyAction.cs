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
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Serialization;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ActionClassified
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
