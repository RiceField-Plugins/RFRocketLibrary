using DSharp4Webhook.Core.Constructor.Embed;
using DSharp4Webhook.Core.Entities.Embed;
using DSharp4Webhook.Util;
using Newtonsoft.Json;

namespace DSharp4Webhook.Internal.Message.Embed
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
