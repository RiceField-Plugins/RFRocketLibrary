using System.IO;
using DSharp4Webhook.Action.Entities.Rest.ActionClassified;
using DSharp4Webhook.Core.Entities;
using DSharp4Webhook.Internal.Action.Rest.ActionClassified;
using DSharp4Webhook.Internal.Core;

namespace DSharp4Webhook.Util
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
