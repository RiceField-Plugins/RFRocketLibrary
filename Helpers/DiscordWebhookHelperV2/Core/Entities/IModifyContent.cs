using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Serialization;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities
{
    /// <summary>
    ///     Data that modifies the webhook.
    /// </summary>
    public interface IModifyContent : IWSerializable
    {
        /// <summary>
        ///     Webhook name.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        ///     Webhook image.
        /// </summary>
        public IWebhookImage? Image { get; }
    }
}
