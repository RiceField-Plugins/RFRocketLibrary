using System.Linq;
using System.Threading.Tasks;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ActionClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Manipulation;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ActionClassified
{
    internal sealed class DeleteAction : BaseRestAction<IRestResult>, IDeleteAction
    {
        public DeleteAction(IWebhook webhook, RestSettings restSettings) : base(webhook, restSettings) { }

        public override async Task<bool> ExecuteAsync()
        {
            CheckExecution();
            Result = new RestResult(await Webhook.RestProvider.DELETE(Webhook.GetWebhookUrl(), RestSettings));
            Webhook.Dispose();
            return Result.LastResponse.HasValue && BaseRestProvider.DELETE_ALLOWED_STATUSES.Contains(Result.LastResponse.Value.StatusCode);
        }
    }
}
