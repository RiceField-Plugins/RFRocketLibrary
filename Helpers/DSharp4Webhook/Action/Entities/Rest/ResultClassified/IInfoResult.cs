using DSharp4Webhook.Core.Entities;

namespace DSharp4Webhook.Action.Entities.Rest.ResultClassified
{
    public interface IInfoResult : IRestResult
    {
        /// <summary>
        ///     Returns information about webhook.
        /// </summary>
        public IWebhookInfo WebhookInfo { get; }
    }
}
