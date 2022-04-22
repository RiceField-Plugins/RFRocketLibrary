using DSharp4Webhook.Action.Entities.Rest.ResultClassified;
using DSharp4Webhook.Serialization;

namespace DSharp4Webhook.Action.Entities.Rest.ActionClassified
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
