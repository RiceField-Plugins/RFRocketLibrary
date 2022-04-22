using System.Linq;
using System.Threading.Tasks;
using DSharp4Webhook.Action.Entities.Rest;
using DSharp4Webhook.Action.Entities.Rest.ActionClassified;
using DSharp4Webhook.Core.Entities;
using DSharp4Webhook.Rest.Entities;
using DSharp4Webhook.Rest.Manipulation;

namespace DSharp4Webhook.Internal.Action.Rest.ActionClassified
{
    internal sealed class MessageAction : BaseRestAction<IRestResult>, IMessageAction
    {
        public IMessage Message { get; }

        public MessageAction(IMessage message, IWebhook webhook, RestSettings restSettings) : base(webhook, restSettings)
        {
            Message = message;
        }

        public override async Task<bool> ExecuteAsync()
        {
            CheckExecution();
            Result = new RestResult(await Webhook.RestProvider.POST(Webhook.GetWebhookUrl(), Message.Serialize(), RestSettings));
            SettingRateLimit();
            return Result.LastResponse.HasValue && BaseRestProvider.POST_ALLOWED_STATUSES.Contains(Result.LastResponse.Value.StatusCode);
        }
    }
}
