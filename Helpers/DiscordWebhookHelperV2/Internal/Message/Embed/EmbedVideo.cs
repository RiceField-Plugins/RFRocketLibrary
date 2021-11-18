using Newtonsoft.Json;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message.Embed
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    internal readonly struct EmbedVideo : IEmbedVideo
    {
        private readonly uint? _height;
        private readonly uint? _width;
        private readonly string? _url;

        public EmbedVideo(EmbedVideoBuilder builder)
        {
            Checks.CheckForNull(builder, nameof(builder));

            _height = builder.Height;
            _width = builder.Width;
            _url = builder.Url;
        }

        [JsonProperty(PropertyName = "height")]
        public uint? Height
        {
            get => _height;
        }

        [JsonProperty(PropertyName = "width")]
        public uint? Width
        {
            get => _width;
        }

        [JsonProperty(PropertyName = "url")]
        public string? Url
        {
            get => _url;
        }
    }
}
