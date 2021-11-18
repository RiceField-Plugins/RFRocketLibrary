using System;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Meta;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor.Embed
{
    public sealed class EmbedAuthorBuilder : IBuilder
    {
        private string? _name;

        #region Properties

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Exceeds the allowed length.
        /// </exception>
        public string? Name
        {
            get => _name;
            set
            {
                if (value is not null)
                {
                    value = value.Trim();
                    Checks.CheckBounds(nameof(Name), $"Must be no more than {WebhookProvider.MAX_EMBED_AUTHOR_NAME_LENGTH} in length",
                        WebhookProvider.MAX_EMBED_AUTHOR_NAME_LENGTH + 1, value.Length);
                    _name = value;
                }
                else
                    _name = value;
            }
        }

        public string? Url { get; set; }

        public string? IconUrl { get; set; }

        public string? ProxyIconUrl { get; set; }

        #endregion

        public static EmbedAuthorBuilder New() => new();

        private EmbedAuthorBuilder() { }

        public IEmbedAuthor Build()
        {
            return new EmbedAuthor(this);
        }

        public void Reset()
        {
            _name = null;
            Url = null;
            IconUrl = null;
            ProxyIconUrl = null;
        }
    }
}
