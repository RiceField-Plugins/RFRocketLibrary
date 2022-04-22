using DSharp4Webhook.Core.Entities.Embed.Subtypes;

namespace DSharp4Webhook.Core.Entities.Embed
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
