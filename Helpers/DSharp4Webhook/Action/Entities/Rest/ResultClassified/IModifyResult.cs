using DSharp4Webhook.Core.Entities;

namespace DSharp4Webhook.Action.Entities.Rest.ResultClassified
{
    public interface IModifyResult : IRestResult
    {
        /// <summary>
        ///     Returned updated information about webhook.
        /// </summary>
        public IWebhookInfo WebhookInfo { get; }
    }
}
