using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest.Manipulation;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Serialization;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Rest
{
    public sealed class DefaultProvider : BaseRestProvider
    {
        private readonly HttpClient _httpClient;

        // This type of request is not available in the .NET Framework 4.7.1.
        private static readonly HttpMethod PatchMethod = new("PATCH");

        /// <summary>
        ///     Sets the given provider is the default.
        /// </summary>
        public static void SetupAsDefault()
        {
            RestProviderLoader.SetProviderType(typeof(DefaultProvider));
        }

        public DefaultProvider(IWebhook webhook) : base(webhook)
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.UserAgent.Clear();
            _httpClient.DefaultRequestHeaders.Add("X-RateLimit-Precision", "millisecond");
        }

        public override async Task<RestResponse[]> GET(string url, RestSettings restSettings)
        {
            Checks.CheckForArgument(string.IsNullOrEmpty(url), nameof(url));
            return await Raw(_httpClient.GetAsync(url), GET_ALLOWED_STATUSES, restSettings).ConfigureAwait(false);
        }

        public override async Task<RestResponse[]> POST(string url, SerializeContext data, RestSettings restSettings)
        {
            Checks.CheckForArgument(string.IsNullOrEmpty(url), nameof(url));

            using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            PrepareContent(requestMessage, data);
            return await Raw(_httpClient.SendAsync(requestMessage), POST_ALLOWED_STATUSES, restSettings)
                .ConfigureAwait(false);
        }

        public override async Task<RestResponse[]> DELETE(string url, RestSettings restSettings)
        {
            Checks.CheckForArgument(string.IsNullOrEmpty(url), nameof(url));
            return await Raw(_httpClient.DeleteAsync(url), DELETE_ALLOWED_STATUSES, restSettings).ConfigureAwait(false);
        }

        public override async Task<RestResponse[]> PATCH(string url, SerializeContext data, RestSettings restSettings)
        {
            Checks.CheckForArgument(string.IsNullOrEmpty(url), nameof(url));
            Checks.CheckForSerializeType(data, SerializeType.APPLICATION_JSON);

            using HttpRequestMessage requestMessage = new(PatchMethod, url);
            PrepareContent(requestMessage, data);
            return await Raw(_httpClient.SendAsync(requestMessage), PATCH_ALLOWED_STATUSES, restSettings)
                .ConfigureAwait(false);
        }

        private async Task<RestResponse[]> Raw(Task<HttpResponseMessage> func,
            IReadOnlyCollection<HttpStatusCode> allowedStatuses, RestSettings restSettings)
        {
            Checks.CheckForNull(restSettings, nameof(restSettings));
            Checks.CheckWebhookStatus(Webhook.Status);
            Checks.CheckForNull(allowedStatuses, nameof(allowedStatuses));

            List<RestResponse> responses = new();
            uint attempts = 0;
            // Used to prevent calls if something went wrong
            var forceStop = false;

            do
            {
                if (responses.Count != 0)
                    await Webhook.ActionManager.FollowRateLimit(responses.Last().RateLimit).ConfigureAwait(false);

                HttpResponseMessage response = await func.ConfigureAwait(false);
                var rateLimitInfo =
                    new RateLimitInfo(response.Headers.ToDictionary(h => h.Key, h => h.Value.FirstOrDefault()));
                var restResponse = new RestResponse(response.StatusCode, rateLimitInfo,
                    await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false), attempts);
                responses.Add(restResponse);

                // Processing the necessary status codes
                ProcessStatusCode(response.StatusCode, ref forceStop, allowedStatuses);
                //Log(new LogContext(LogSensitivity.VERBOSE, $"[A {attempts}] [SC {(int)responses.Last().StatusCode}] [RLR {restResponse.RateLimit.Reset:yyyy-MM-dd HH:mm:ss.fff zzz}] [RLMW {restResponse.RateLimit.MustWait}] Request completed:{(restResponse.Content.Length != 0 ? string.Concat(Environment.NewLine, restResponse.Content) : " No content")}", _webhook.Id));

#pragma warning disable IDE0075 // Simplify conditional expression
            } while (!forceStop && !allowedStatuses.Contains(responses.Last().StatusCode) &&
                     (restSettings.MaxAttempts <= 0 || ++attempts <= restSettings.MaxAttempts));
#pragma warning restore IDE0075 // Simplify conditional expression

            return responses.ToArray();
        }

        private void PrepareContent(HttpRequestMessage requestMessage, SerializeContext data)
        {
            switch (data.Type)
            {
                case SerializeType.APPLICATION_JSON:
                {
                    requestMessage.Content = new ByteArrayContent((data.Content ?? throw new InvalidOperationException()).ToArray());
                    requestMessage.Content.Headers.ContentType =
                        MediaTypeHeaderValue.Parse(SerializeTypeConverter.Convert(SerializeType.APPLICATION_JSON));
                    break;
                }

                case SerializeType.MULTIPART_FORM_DATA:
                {
                    var multipartContent = new MultipartFormDataContent();
                    if (!(data.Files is null) && data.Files.Keys.Count != 0)
                    {
                        int index = 0;
                        foreach (var filePair in data.Files)
                        {
                            index++;
                            multipartContent.Add(new ByteArrayContent(filePair.Value.ToArray()), $"file{index}",
                                filePair.Key);
                        }
                    }

                    if (!(data.Content is null))
                        multipartContent.Add(new ByteArrayContent(data.Content.ToArray()), "payload_json");

                    requestMessage.Content = multipartContent;
                    // it doesn't seem to be necessary, it works automatically
                    //requestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(SerializeTypeConverter.Convert(SerializeType.MULTIPART_FROM_DATA));
                    break;
                }
            }
        }

        public override void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}