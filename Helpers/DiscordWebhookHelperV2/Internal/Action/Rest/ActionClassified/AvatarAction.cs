using System.Threading.Tasks;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ActionClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest.ResultClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ResultClassified;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Core;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest.ActionClassified
{
    internal sealed class AvatarAction : BaseRestAction<IAvatarResult>, IAvatarAction
    {
        private IWebhookInfo? _webhookInfo;

        public AvatarAction(IWebhook webhook, RestSettings restSettings, IWebhookInfo? webhookInfo = null) : base(webhook, restSettings)
        {
            _webhookInfo = webhookInfo;
        }

        public override async Task<bool> ExecuteAsync()
        {
            CheckExecution();

            string? avatarUrl;
            if (_webhookInfo is null)
            {
                var infoAction = Webhook.GetInfo();
                await infoAction.ExecuteAsync();
                _webhookInfo = infoAction.Result.WebhookInfo;
            }

            avatarUrl = _webhookInfo.AvatarUrl;

            if (string.IsNullOrEmpty(avatarUrl))
                return false;

            if (!string.IsNullOrEmpty(avatarUrl))
            {
                var responses = await Webhook.RestProvider.GET(avatarUrl!, RestSettings);
                var lastResponse = responses[responses.Length - 1];
                Result = new AvatarResult(new WebhookImage(lastResponse.Data!), responses);
                return true;
            }
            return false;
        }
    }
}
