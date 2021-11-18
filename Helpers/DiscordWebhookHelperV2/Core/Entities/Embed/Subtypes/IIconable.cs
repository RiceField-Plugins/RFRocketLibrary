namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed.Subtypes
{
    public interface IIconable
    {
        /// <summary>
        ///     Url of icon.
        /// </summary>
        public string? IconUrl { get; }

        /// <summary>
        ///     A proxied url of icon.
        /// </summary>
        public string? ProxyIconUrl { get; }
    }
}
