using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ResultClassified
{
    public interface IInfoResult : IRestResult
    {
        /// <summary>
        ///     Returns information about webhook.
        /// </summary>
        public IWebhookInfo WebhookInfo { get; }
    }
}
