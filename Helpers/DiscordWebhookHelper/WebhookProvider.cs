using Newtonsoft.Json;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelper
{
    public class WebhookProvider
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string? Name;

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string? URL;
    }
}