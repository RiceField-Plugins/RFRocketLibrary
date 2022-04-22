using DSharp4Webhook.Core.Constructor.Embed;
using DSharp4Webhook.Core.Entities.Embed;
using DSharp4Webhook.Util;
using Newtonsoft.Json;

namespace DSharp4Webhook.Internal.Message.Embed
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    internal readonly struct EmbedProvider : IEmbedProvider
    {
        private readonly string? _name;
        private readonly string? _url;

        public EmbedProvider(EmbedProviderBuilder builder)
        {
            Checks.CheckForNull(builder, nameof(builder));

            _name = builder.Name;
            _url = builder.Url;
        }

        [JsonProperty(PropertyName = "name")]
        public string? Name
        {
            get => _name;
        }

        [JsonProperty(PropertyName = "url")]
        public string? Url
        {
            get => _url;
        }
    }
}
