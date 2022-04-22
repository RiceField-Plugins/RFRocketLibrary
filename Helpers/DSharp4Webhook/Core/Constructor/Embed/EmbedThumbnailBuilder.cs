using DSharp4Webhook.Core.Constructor.Meta;
using DSharp4Webhook.Core.Entities.Embed;
using DSharp4Webhook.Internal.Message.Embed;

namespace DSharp4Webhook.Core.Constructor.Embed
{
    public sealed class EmbedThumbnailBuilder : IBuilder
    {
        #region Properties

        public string? Url { get; set; }

        public string? ProxyUrl { get; set; }

        public uint? Height { get; set; }

        public uint? Width { get; set; }

        #endregion

        public static EmbedThumbnailBuilder New() => new();

        private EmbedThumbnailBuilder() { }

        public IEmbedThumbnail Build()
        {
            return new EmbedThumbnail(this);
        }

        public void Reset()
        {
            Height = null;
            Width = null;
            Url = null;
            ProxyUrl = null;
        }
    }
}
