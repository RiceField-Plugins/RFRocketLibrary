using Newtonsoft.Json;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message.Embed
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    internal readonly struct EmbedField : IEmbedField
    {
        private readonly string _name;
        private readonly string _value;
        private readonly bool? _inline;

        public EmbedField(EmbedFieldBuilder builder)
        {
            Checks.CheckForNull(builder, nameof(builder));

            _name = builder.Name!;
            _value = builder.Value!;
            _inline = builder.Inline;
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get => _name;
        }

        [JsonProperty(PropertyName = "value")]
        public string Value
        {
            get => _value;
        }

        [JsonProperty(PropertyName = "inline")]
        public bool? Inline
        {
            get => _inline;
        }
    }
}
