using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed.Subtypes;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed
{
    /// <summary>
    ///     Provider of embed.
    /// </summary>
    public interface IEmbedProvider : IUrlable
    {
        /// <summary>
        ///     Name of provider.
        /// </summary>
        public string? Name { get; }
    }
}
