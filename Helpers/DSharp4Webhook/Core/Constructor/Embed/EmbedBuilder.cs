using System;
using System.Collections.Generic;
using System.Text;
using DSharp4Webhook.Core.Constructor.Meta;
using DSharp4Webhook.Core.Entities.Embed;
using DSharp4Webhook.Core.Enums;
using DSharp4Webhook.Util;

namespace DSharp4Webhook.Core.Constructor.Embed
{
    /// <remarks>
    ///     Don't forget about <see cref="WebhookProvider.MAX_EMBED_DATA_LENGTH"/>.
    /// </remarks>
    public sealed class EmbedBuilder : IBuilder
    {
        private readonly StringBuilder _builder;

        private string? _title;
        internal List<IEmbedField>? Fields;

        /// <summary>
        ///     Gets a new builder.
        /// </summary>
        public static EmbedBuilder New() => new();

        private EmbedBuilder()
        {
            _builder = new StringBuilder();
        }

        #region Properties

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Exceeds the allowed length.
        /// </exception>
        public string? Title
        {
            get => _title;
            set
            {
                if (value is not null)
                {
                    value = value.Trim();
                    Checks.CheckBounds(nameof(Title), $"Must be no more then {WebhookProvider.MAX_EMBED_TITLE_LENGTH} in length",
                        WebhookProvider.MAX_EMBED_TITLE_LENGTH, value.Length);
                    _title = value;
                }
                else
                    _title = value;
            }
        }

        public EmbedType? Type { get; set; }

        public string? Url { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        /// <remarks>
        ///     Use in conjunction with <see cref="ColorUtil.FromHex(string)"/>.
        /// </remarks>
        public uint? Color { get; set; }

        public IEmbedFooter? Footer { get; set; }

        public IEmbedImage? Image { get; set; }

        public IEmbedThumbnail? Thumbnail { get; set; }

        public IEmbedVideo? Video { get; set; }

        public IEmbedProvider? Provider { get; set; }

        public IEmbedAuthor? Author { get; set; }

        #endregion


        #region Methods

        /// <summary>
        ///     Return the description string builder.
        /// </summary>
        public StringBuilder GetStringBuilder()
        {
            return _builder;
        }

        /// <summary>
        ///     Gets a list of fields.
        /// </summary>
        public List<IEmbedField> GetFields()
        {
            return Fields ??= new List<IEmbedField>();
        }

        /// <summary>
        ///     Adds text to the current description.
        /// </summary>
        public EmbedBuilder Append(string? text)
        {
            // If we put null, it will still be null in the description
            Checks.CheckBounds(nameof(text), $"Must be no more than {WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH} in length",
                WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH, text?.Length ?? 4, _builder.Length);
            _builder.Append(text ?? "null");

            return this;
        }

        /// <summary>
        ///     Tries to add text, 
        ///     without causing an exception when the bounds are exceeded.
        /// </summary>
        public EmbedBuilder TryAppend(string? text)
        {
            if (!Checks.CheckBoundsSafe(WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH, text?.Length ?? 4, _builder.Length))
                _builder.Append(text ?? "null");

            return this;
        }

        /// <summary>
        ///     Adds a new line to the current description.
        /// </summary>
        public EmbedBuilder AppendLine()
        {
            Checks.CheckBounds(null, $"Must be no more than {WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH} in length",
                WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH + 1, 1, _builder.Length);
            _builder.AppendLine();

            return this;
        }

        /// <summary>
        ///     Tries to add a new line to the current description,
        ///     without causing an exception when the bounds are exceeded.
        /// </summary>
        public EmbedBuilder TryAppendLine()
        {
            if (!Checks.CheckBoundsSafe(WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH + 1, 1, _builder.Length))
                _builder.AppendLine();

            return this;
        }

        /// <summary>
        ///     Adds text to the current description in a new line.
        /// </summary>
        public EmbedBuilder AppendLine(string? text)
        {
            // If we put null, it will still be null in the description, a line break is also added
            Checks.CheckBounds(nameof(text), $"Must be no more than {WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH} in length",
                WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH + 1, (text?.Length ?? 4) + 1, _builder.Length);
            _builder.AppendLine(text ?? "null");

            return this;
        }

        /// <summary>
        ///     Tries to add text to the current description in a new line,
        ///     without causing an exception when the bounds are exceeded.
        /// </summary>
        public EmbedBuilder TryAppendLine(string? text)
        {
            if (!Checks.CheckBoundsSafe(WebhookProvider.MAX_EMBED_DESCRIPTION_LENGTH + 1, text?.Length ?? 4 + 1, _builder.Length))
                _builder.AppendLine(text ?? "null");

            return this;
        }

        /// <summary>
        ///     Adds a field.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Field is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Field exceeds the allowed limit.
        /// </exception>
        public EmbedBuilder AddField(IEmbedField field)
        {
            Checks.CheckForNull(field, nameof(field));
            // Just safely get it instead
            if (GetFields().Count + 1 > WebhookProvider.MAX_EMBED_FIELDS_COUNT)
                throw new ArgumentOutOfRangeException();
            Fields!.Add(field);

            return this;
        }

        /// <summary>
        ///     Same, but does not cause an exception.
        /// </summary>
        public EmbedBuilder TryAddField(IEmbedField field)
        {
            if (GetFields().Count + 1 <= WebhookProvider.MAX_EMBED_FIELDS_COUNT)
                Fields!.Add(field);

            return this;
        }

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Embed exceeds its limit.
        /// </exception>
        public IEmbed Build()
        {
            return new Internal.Message.Embed.Embed(this);
        }

        public void Reset()
        {
            _title = null;
            Type = null;
            Url = null;
            Timestamp = null;
            Color = null;
            Footer = null;
            Image = null;
            Thumbnail = null;
            Video = null;
            Provider = null;
            Author = null;
            Fields?.Clear();
            _builder.Clear();
        }

        #endregion
    }
}
