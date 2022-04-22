using System;
using DSharp4Webhook.Core.Constructor.Meta;
using DSharp4Webhook.Core.Entities.Embed;
using DSharp4Webhook.Internal.Message.Embed;
using DSharp4Webhook.Util;

namespace DSharp4Webhook.Core.Constructor.Embed
{
    public sealed class EmbedFieldBuilder : IBuilder
    {
        private string? _name;
        private string? _value;

        #region Properties

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Exceeds the allowed length.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Attempt to assign a null value.
        ///     Use <see cref="Reset"/> for reset.
        /// </exception>
        public string? Name
        {
            get => _name;
            set
            {
                if (value is null)
                    throw new ArgumentNullException($"Name cannot be null", nameof(Name));

                value = value.Trim();
                Checks.CheckBounds(nameof(Name), $"Must be no more than {WebhookProvider.MAX_EMBED_FIELD_NAME_LENGTH} in length",
                    WebhookProvider.MAX_EMBED_FIELD_NAME_LENGTH + 1, value.Length);
                _name = value;
            }
        }

        /// <exception cref="ArgumentOutOfRangeException">
        ///     Exceeds the allowed length.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Attempt to assign a null value.
        ///     Use <see cref="Reset"/> for reset.
        /// </exception>
        public string? Value
        {
            get => _value;
            set
            {
                if (value is null)
                    throw new ArgumentNullException($"Value cannot be null", nameof(Value));

                value = value.Trim();
                Checks.CheckBounds(nameof(Value), $"Must be no more than {WebhookProvider.MAX_EMBED_FIELD_VALUE_LENGTH} in length",
                    WebhookProvider.MAX_EMBED_FIELD_VALUE_LENGTH + 1, value.Length);
                _value = value;
            }
        }

        public bool? Inline { get; set; }

        #endregion

        public static EmbedFieldBuilder New() => new();

        private EmbedFieldBuilder() { }

        /// <exception cref="InvalidOperationException">
        ///     Attempt to build without set variables.
        /// </exception>
        /// <remarks>
        ///     Discord doesn't allow using 'name' or 'value' as null.
        /// </remarks>
        public IEmbedField Build()
        {
            if (_name is null || _value is null)
                throw new InvalidOperationException($"{nameof(Name)} or {nameof(Value)} has an invalid value");

            return new EmbedField(this);
        }

        public void Reset()
        {
            _name = null;
            _value = null;
            Inline = null;
        }
    }
}
