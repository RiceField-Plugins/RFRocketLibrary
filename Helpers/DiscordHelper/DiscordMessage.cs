namespace RFRocketLibrary.Helpers.DiscordHelper
{
    public class DiscordMessage
    {
        public string Id = "";
        public string ChannelId = "";
        public string? GuildId;
        public DiscordAuthor Author = new();
    }
}