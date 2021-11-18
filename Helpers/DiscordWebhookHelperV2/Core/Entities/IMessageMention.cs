using System.Collections.ObjectModel;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Enums;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities
{
    /// <remarks>
    ///     Provided as 'allowed_mentions' in the message object.
    /// </remarks>
    public interface IMessageMention
    {
        /// <summary>
        ///     Allowed types of mentions.
        /// </summary>
        public AllowedMention AllowedMention { get; }

        /// <summary>
        ///     Users allowed to be mentioned.
        ///     Can be null when building with a null value.
        /// </summary>
        public ReadOnlyCollection<string>? Users { get; }

        /// <summary>
        ///     Roles allowed to be mentioned.
        ///     Can be null when building with a null value.
        /// </summary>
        public ReadOnlyCollection<string>? Roles { get; }
    }
}
