using System.Collections.ObjectModel;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Action.Entities.Rest
{
    /// <summary>
    ///     Result of the rest action.
    /// </summary>
    public interface IRestResult : IResult
    {
        /// <summary>
        ///     Returns the last rest response.
        /// </summary>
        public RestResponse? LastResponse { get; }

        /// <summary>
        ///     All responses to rest queries.
        /// </summary>
        public ReadOnlyCollection<RestResponse>? Responses { get; }
    }
}
