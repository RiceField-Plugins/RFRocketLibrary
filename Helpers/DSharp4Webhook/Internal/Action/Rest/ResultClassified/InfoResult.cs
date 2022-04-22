using DSharp4Webhook.Action.Entities.Rest.ResultClassified;
using DSharp4Webhook.Core.Entities;
using DSharp4Webhook.Rest.Entities;

namespace DSharp4Webhook.Internal.Action.Rest.ResultClassified
{
    internal sealed class InfoResult : RestResult, IInfoResult
    {
        public IWebhookInfo WebhookInfo { get; }

        public InfoResult(IWebhookInfo webhookInfo, RestResponse[] responses) : base(responses)
        {
            WebhookInfo = webhookInfo;
        }
    }
}
