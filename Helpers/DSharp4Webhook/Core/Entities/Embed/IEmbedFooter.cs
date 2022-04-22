using DSharp4Webhook.Core.Entities.Embed.Subtypes;

namespace DSharp4Webhook.Core.Entities.Embed
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
