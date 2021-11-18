using System.IO;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ActionClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ActionClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Core;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util
{
    /// <summary>
    ///     Auxiliary tool for working with images.
    /// </summary>
    public static class ImageUtil
    {
        /// <summary>
        ///     Retrieves the image from the url.
        /// </summary>
        /// <param name="webhook">
        ///     Webhook that provides BaseRestProvider.
        /// </param>
        /// <param name="url">
        ///     Endpoint to the image.
        /// </param>
        /// <returns>
        ///     Image data.
        /// </returns>
        public static IAvatarAction GetImageByUrl(this IWebhook webhook, string url)
        {
            Checks.CheckForNull(webhook, nameof(webhook));
            Checks.CheckForArgument(string.IsNullOrEmpty(url), nameof(url));
            return new AvatarAction(webhook, webhook.RestSettings, null);
        }

        /// <summary>
        ///     Retrieves an image from the file system.
        /// </summary>
        /// <param name="fileInfo">
        ///     File.
        /// </param>
        public static IWebhookImage GetImage(FileInfo fileInfo)
        {
            return new WebhookImage(fileInfo);
        }
    }
}
