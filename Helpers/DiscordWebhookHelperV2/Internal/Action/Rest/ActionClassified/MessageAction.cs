using System.Linq;
using System.Threading.Tasks;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ActionClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Manipulation;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ActionClassified
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
