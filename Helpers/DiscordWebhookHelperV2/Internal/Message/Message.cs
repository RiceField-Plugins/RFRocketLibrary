using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Constructor;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Core.Entities.Embed;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Serialization;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util;
using RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util.Extensions;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Internal.Message
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore, MemberSerialization = MemberSerialization.OptIn)]
    internal struct Message : IMessage
    {
        private readonly string? _content;
        private readonly string? _username;
        private readonly string? _avatarUrl;
        private readonly bool _isTTS;
        private readonly IMessageMention _mention;
        private readonly ReadOnlyCollection<IEmbed>? _embeds;
        private readonly ReadOnlyDictionary<string, ReadOnlyCollection<byte>>? _files;

        private SerializeContext? _cache;

        #region Properties

        [JsonProperty(PropertyName = "content")]
        public string? Content { get => _content; }

        [JsonProperty(PropertyName = "tts")]
        public bool IsTTS { get => _isTTS; }

        [JsonProperty(PropertyName = "username")]
        public string? Username { get => _username; }

        [JsonProperty(PropertyName = "avatar_url")]
        public string? AvatarUrl { get => _avatarUrl; }

        [JsonProperty(PropertyName = "embeds")]
        public ReadOnlyCollection<IEmbed>? Embeds { get => _embeds; }

        [JsonProperty(PropertyName = "allowed_mention")]
        public IMessageMention Mention { get => _mention; }

        public ReadOnlyDictionary<string, ReadOnlyCollection<byte>>? Files { get => _files; }

        #endregion

        public Message(string message, IMessageMention messageMention, bool isTTS = false)
        {
            Checks.CheckForNull(messageMention, nameof(messageMention));

            _content = message;
            _isTTS = isTTS;
            _mention = messageMention;

            _username = null;
            _avatarUrl = null;
            _embeds = null;
            _files = null;
            _cache = null;
        }

        public Message(IEnumerable<IEmbed> embeds, IMessageMention messageMention)
        {
            Checks.CheckForNull(messageMention, nameof(messageMention));

            _embeds = embeds.ToArray().ToReadOnlyCollection();
            _mention = messageMention;

            _content = null;
            _username = null;
            _isTTS = false;
            _username = null;
            _avatarUrl = null;
            _files = null;
            _cache = null;
        }

        public Message(IMessage source)
        {
            Checks.CheckForNull(source, nameof(source));
            Checks.CheckForAttachments(source.Files!);

            _username = source.Username;
            _avatarUrl = source.AvatarUrl;
            _content = source.Content;
            _isTTS = source.IsTTS;

            _mention = source.Mention;
            _files = source.Files;
            _embeds = source.Embeds;

            _cache = null;
        }

        public Message(MessageBuilder builder)
        {
            Checks.CheckForAttachments(builder.Files);

            if (builder.Embeds.Count > WebhookProvider.MAX_EMBED_COUNT ||
                builder.Builder.Length > WebhookProvider.MAX_CONTENT_LENGTH)
                throw new ArgumentOutOfRangeException();

            _content = builder.Builder.ToString();
            _username = builder.Username;
            _avatarUrl = builder.AvatarUrl;
            _isTTS = builder.IsTTS;
            _mention = builder.MessageMention;                                                               // Create a new object bypassing readonly objects that simply don't allow writing
            _files = builder._files is null ? null : new ReadOnlyDictionary<string, ReadOnlyCollection<byte>>(new Dictionary<string, ReadOnlyCollection<byte>>(builder._files));
            _embeds = builder._embeds.ToReadOnlyCollection();

            _cache = null;
        }

        public SerializeContext Serialize()
        {
            return _cache ?? (_cache = new SerializeContext(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(this)), _files)).Value;
        }
    }
}
