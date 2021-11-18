using System;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Meta;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Embed
{
    public sealed class EmbedFooterBuilder : IBuilder
    {
        private string? _text;

        #region Properties

        /// <exception cref="ArgumentNullException">
        ///     Attempt to assign a null value.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Exceeds the allowed length.
        /// </exception>
        public string? Text
        {
            get => _text;
            set
            {
                if (value is null)
                    throw new ArgumentNullException($"Value cannot be null", nameof(Text));

                value = value.Trim();
                Checks.CheckBounds(nameof(Text), $"Must be no more than {WebhookProvider.MAX_EMBED_FOOTER_TEXT_LENGTH} in length",
                    WebhookProvider.MAX_EMBED_FOOTER_TEXT_LENGTH + 1, value.Length);
                _text = value;
            }
        }

        public string? IconUrl { get; set; }

        public string? ProxyIconUrl { get; set; }

        #endregion

        public static EmbedFooterBuilder New() => new();

        private EmbedFooterBuilder() { }

        /// <exception cref="InvalidOperationException">
        ///     Attempt to build without set text.
        /// </exception>
        public IEmbedFooter Build()
        {
            if (_text is null)
                throw new InvalidOperationException("Text cannot be null");

            return new EmbedFooter(this);
        }

        public void Reset()
        {
            _text = null;
            IconUrl = null;
            ProxyIconUrl = null;
        }
    }
}
