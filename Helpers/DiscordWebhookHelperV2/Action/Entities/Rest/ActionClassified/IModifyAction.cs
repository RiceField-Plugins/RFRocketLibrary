using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ResultClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Serialization;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ActionClassified
{
    /// <summary>
    ///     Webhook update action, name change, avatar change.
    /// </summary>
    public interface IModifyAction : IRestAction<IModifyResult>
    {
        /// <summary>
        ///     The serialized data contains name and avatar_url.
        /// </summary>
        public SerializeContext Context { get; }
    }
}
