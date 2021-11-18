using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ResultClassified
{
    public interface IAvatarResult : IRestResult
    {
        /// <summary>
        ///     Webhook image.
        /// </summary>
        public IWebhookImage Image { get; }
    }
}
