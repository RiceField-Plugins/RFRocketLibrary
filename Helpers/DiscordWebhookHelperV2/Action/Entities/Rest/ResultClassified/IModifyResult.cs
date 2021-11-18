using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ResultClassified
{
    public interface IModifyResult : IRestResult
    {
        /// <summary>
        ///     Returned updated information about webhook.
        /// </summary>
        public IWebhookInfo WebhookInfo { get; }
    }
}
