using Newtonsoft.Json;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message.Embed
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    internal readonly struct EmbedAuthor : IEmbedAuthor
    {
        private readonly string? _name;
        private readonly string? _iconUrl;
        private readonly string? _proxyIconUrl;
        private readonly string? _url;

        public EmbedAuthor(EmbedAuthorBuilder builder)
        {
            Checks.CheckForNull(builder, nameof(builder));

            _name = builder.Name;
            _iconUrl = builder.IconUrl;
            _proxyIconUrl = builder.ProxyIconUrl;
            _url = builder.Url;
        }

        [JsonProperty(PropertyName = "name")]
        public string? Name
        {
            get => _name;
        }

        [JsonProperty(PropertyName = "icon_url")]
        public string? IconUrl
        {
            get => _iconUrl;
        }

        [JsonProperty(PropertyName = "proxy_icon_url")]
        public string? ProxyIconUrl
        {
            get => _proxyIconUrl;
        }

        [JsonProperty(PropertyName = "url")]
        public string? Url
        {
            get => _url;
        }
    }
}
