using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Meta;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message.Embed;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Embed
{
    public sealed class EmbedImageBuilder : IBuilder
    {
        #region Properties

        public string? Url { get; set; }

        public string? ProxyUrl { get; set; }

        public uint? Height { get; set; }

        public uint? Width { get; set; }

        #endregion

        public static EmbedImageBuilder New() => new();

        private EmbedImageBuilder() { }

        public IEmbedImage Build()
        {
            return new EmbedImage(this);
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
