using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action
{
    public readonly struct ActionContext
    {
        /// <summary>
        ///     Executed action.
        /// </summary>
        public IAction Action { get; }

        /// <summary>
        ///     Success of the action.
        /// </summary>
        public bool IsSuccessfully { get; }

        public ActionContext(IAction action, bool isSuccessful)
        {
            Action = action;
            IsSuccessfully = isSuccessful;
        }
    }
}
