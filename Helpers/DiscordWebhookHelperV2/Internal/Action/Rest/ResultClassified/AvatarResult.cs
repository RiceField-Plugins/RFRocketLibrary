using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ResultClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ResultClassified
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
