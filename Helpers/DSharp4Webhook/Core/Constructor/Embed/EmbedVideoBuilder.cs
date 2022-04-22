using System;
using DSharp4Webhook.Core.Constructor.Meta;
using DSharp4Webhook.Core.Entities.Embed;
using DSharp4Webhook.Internal.Message.Embed;

namespace DSharp4Webhook.Core.Constructor.Embed
{
    public sealed class EmbedVideoBuilder : IBuilder
    {
        #region Properties

        public string? Url { get; set; }

        public uint? Height { get; set; }

        public uint? Width { get; set; }

        #endregion

        public static EmbedVideoBuilder New() => new();

        private EmbedVideoBuilder() { }

        public IEmbedVideo Build()
        {
            return new EmbedVideo(this);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
