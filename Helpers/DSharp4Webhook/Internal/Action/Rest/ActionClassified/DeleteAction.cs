using System.Linq;
using System.Threading.Tasks;
using DSharp4Webhook.Action.Entities.Rest;
using DSharp4Webhook.Action.Entities.Rest.ActionClassified;
using DSharp4Webhook.Core.Entities;
using DSharp4Webhook.Rest.Entities;
using DSharp4Webhook.Rest.Manipulation;

namespace DSharp4Webhook.Internal.Action.Rest.ActionClassified
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
