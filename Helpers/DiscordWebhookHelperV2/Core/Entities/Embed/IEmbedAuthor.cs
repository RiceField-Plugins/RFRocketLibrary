using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed.Subtypes;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed
{
    /// <summary>
    ///     Author of embed.
    /// </summary>
    public interface IEmbedAuthor : IIconable, IUrlable
    {
        /// <summary>
        ///     Name of author.
        /// </summary>
        public string? Name { get; }
    }
}
