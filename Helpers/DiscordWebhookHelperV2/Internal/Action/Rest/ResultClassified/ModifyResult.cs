using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ResultClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ResultClassified
{
    internal sealed class ModifyResult : RestResult, IModifyResult
    {
        public IWebhookInfo WebhookInfo { get; }

        public ModifyResult(IWebhookInfo webhookInfo, RestResponse[] responses) : base(responses)
        {
            WebhookInfo = webhookInfo;
        }
    }
}
