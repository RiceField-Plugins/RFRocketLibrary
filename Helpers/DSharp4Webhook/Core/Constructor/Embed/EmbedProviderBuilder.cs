using DSharp4Webhook.Core.Constructor.Meta;
using DSharp4Webhook.Core.Entities.Embed;
using DSharp4Webhook.Internal.Message.Embed;

namespace DSharp4Webhook.Core.Constructor.Embed
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
