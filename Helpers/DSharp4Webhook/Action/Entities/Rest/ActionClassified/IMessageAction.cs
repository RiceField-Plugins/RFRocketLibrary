using DSharp4Webhook.Core.Entities;

namespace DSharp4Webhook.Action.Entities.Rest.ActionClassified
{
    /// <summary>
    ///     Sending a message action.
    /// </summary>
    public interface IMessageAction : IRestAction
    {
        public IMessage Message { get; }
    }
}
