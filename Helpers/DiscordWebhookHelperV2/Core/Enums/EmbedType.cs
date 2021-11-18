namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Enums
{
    /// <summary>
    ///     Type of embed.
    /// </summary>
    /// <remarks>
    ///     Webhook embed always has the <see cref="Rich"/> type,
    ///     regardless of installations.
    /// </remarks>
    public enum EmbedType
    {
        Rich,
        Image,
        Video,
        GifV,
        Article,
        Link
    }
}
