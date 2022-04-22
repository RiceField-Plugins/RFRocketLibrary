using DSharp4Webhook.Action.Entities.Rest.ResultClassified;
using DSharp4Webhook.Core.Entities;
using DSharp4Webhook.Rest.Entities;

namespace DSharp4Webhook.Internal.Action.Rest.ResultClassified
{
    internal sealed class AvatarResult : RestResult, IAvatarResult
    {
        public IWebhookImage Image { get; }

        public AvatarResult(IWebhookImage image, RestResponse[] responses) : base(responses)
        {
            Image = image;
        }
    }
}
