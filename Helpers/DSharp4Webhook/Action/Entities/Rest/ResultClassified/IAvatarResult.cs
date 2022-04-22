using DSharp4Webhook.Core.Entities;

namespace DSharp4Webhook.Action.Entities.Rest.ResultClassified
{
    public interface IAvatarResult : IRestResult
    {
        /// <summary>
        ///     Webhook image.
        /// </summary>
        public IWebhookImage Image { get; }
    }
}
