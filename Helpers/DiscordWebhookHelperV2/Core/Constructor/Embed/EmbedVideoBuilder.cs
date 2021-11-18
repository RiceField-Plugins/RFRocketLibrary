using System;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Meta;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message.Embed;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Embed
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
