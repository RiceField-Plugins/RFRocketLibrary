using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Meta;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message.Embed;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Embed
{
    public sealed class EmbedProviderBuilder : IBuilder
    {
        public string? Name { get; set; }

        public string? Url { get; set; }

        public static EmbedProviderBuilder New() => new();

        private EmbedProviderBuilder() { }

        public IEmbedProvider Build()
        {
            return new EmbedProvider(this);
        }

        public void Reset()
        {
            Name = null;
            Url = null;
        }
    }
}
