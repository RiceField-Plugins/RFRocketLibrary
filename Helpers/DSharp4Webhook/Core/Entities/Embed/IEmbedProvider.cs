using DSharp4Webhook.Core.Entities.Embed.Subtypes;

namespace DSharp4Webhook.Core.Entities.Embed
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
