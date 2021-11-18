using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed.Subtypes;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed
{
    /// <summary>
    ///     Footer of embed.
    /// </summary>
    public interface IEmbedFooter : IIconable
    {
        /// <summary>
        ///     Footer text.
        /// </summary>
        public string Text { get; }
    }
}
