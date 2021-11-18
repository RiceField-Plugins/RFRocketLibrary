using System.Collections.ObjectModel;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util.Extensions;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Action.Rest
{
    internal class RestResult : IRestResult
    {
        public RestResponse? LastResponse { get => !(_responses is null) && _responses.Count != 0 ? (RestResponse?)_responses[_responses.Count - 1] : null; }
        public ReadOnlyCollection<RestResponse> Responses { get => _responses; }

        private readonly ReadOnlyCollection<RestResponse> _responses;

        public RestResult(RestResponse[] responses)
        {
            _responses = responses.ToReadOnlyCollection()!;
        }
    }
}
