using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ActionClassified
{
    /// <summary>
    ///     Sending a message action.
    /// </summary>
    public interface IMessageAction : IRestAction
    {
        public IMessage Message { get; }
    }
}
