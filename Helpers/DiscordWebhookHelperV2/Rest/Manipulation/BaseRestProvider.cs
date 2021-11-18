using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Enums;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Logging;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Serialization;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util.Extensions;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Manipulation
{
    public abstract class BaseRestProvider : IDisposable
    {
        // When we send a file, we get the status code 200 with detailed information in response, also when 'wait=true' as a query parameter
        public static readonly IReadOnlyCollection<HttpStatusCode> POST_ALLOWED_STATUSES =
            new[] {HttpStatusCode.NoContent, HttpStatusCode.OK}.ToReadOnlyCollection()!;

        public static readonly IReadOnlyCollection<HttpStatusCode> GET_ALLOWED_STATUSES =
            new[] {HttpStatusCode.OK}.ToReadOnlyCollection()!;

        public static readonly IReadOnlyCollection<HttpStatusCode> DELETE_ALLOWED_STATUSES =
            new[] {HttpStatusCode.NoContent}.ToReadOnlyCollection()!;

        public static readonly IReadOnlyCollection<HttpStatusCode> PATCH_ALLOWED_STATUSES =
            new[] {HttpStatusCode.OK}.ToReadOnlyCollection()!;

        protected readonly IWebhook Webhook;

        protected BaseRestProvider(IWebhook webhook)
        {
            Checks.CheckForNull(webhook, nameof(webhook));

            Webhook = webhook;
        }

        public abstract Task<RestResponse[]> POST(string url, SerializeContext data, RestSettings restSettings);

        public abstract Task<RestResponse[]> GET(string url, RestSettings restSettings);

        public abstract Task<RestResponse[]> DELETE(string url, RestSettings restSettings);

        public abstract Task<RestResponse[]> PATCH(string url, SerializeContext data, RestSettings restSettings);

        /// <remarks>
        ///     Wrapper for processing returned status codes.
        /// </remarks>
        /// <param name="allowedStatuses">
        ///     Allowed statuses that are considered successful requests.
        /// </param>
        protected void ProcessStatusCode(HttpStatusCode statusCode, ref bool forceStop,
            IReadOnlyCollection<HttpStatusCode> allowedStatuses)
        {
            Checks.CheckForNull(allowedStatuses, nameof(allowedStatuses));

            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    Webhook.Status = WebhookStatus.NOT_EXISTING;
                    Log(new LogContext(LogSensitivity.ERROR,
                        "A REST request returned 404, the webhack does not exist, and we are deleting it...",
                        Webhook.Id));
                    forceStop = true;
                    Webhook.Dispose();
                    break;
                case HttpStatusCode.BadRequest:
                    Log(new LogContext(LogSensitivity.ERROR, "A REST request returnet 400, something went wrong...",
                        Webhook.Id));
                    forceStop = true;
                    break;
                case HttpStatusCode.RequestEntityTooLarge:
                    Log(new LogContext(LogSensitivity.WARN, "A REST request returned 413, you sent too much data",
                        Webhook.Id));
                    forceStop = true;
                    break;
            }

            if (allowedStatuses.Contains(statusCode))
            {
                if (Webhook.Status != WebhookStatus.EXISTING)
                {
                    Webhook.Status = WebhookStatus.EXISTING;
                    Log(new LogContext(LogSensitivity.INFO, "Webhook confirmed its status", Webhook.Id));
                }
            }
        }

        protected void Log(LogContext context)
        {
            //todo: webhook logs
        }

        public abstract void Dispose();
    }
}