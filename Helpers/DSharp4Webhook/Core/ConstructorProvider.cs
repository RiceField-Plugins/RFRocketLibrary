using System;
using DSharp4Webhook.Core.Constructor;
using DSharp4Webhook.Core.Constructor.Embed;
using DSharp4Webhook.Core.Entities;
using DSharp4Webhook.Core.Enums;
using DSharp4Webhook.Internal.Message;

namespace DSharp4Webhook.Core
{
    /// <summary>
    ///     Provides various constructors
    ///     for interacting with a webhook.
    /// </summary>
    public static class ConstructorProvider
    {
        /// <summary>
        ///     Gets default mentions in the message.
        /// </summary>
        public static IMessageMention GetDefaultMessageMention() => new MessageMention(AllowedMention.NONE);

        /// <summary>
        ///     Gets the specified message metinon.
        /// </summary>
        /// <param name="mention">
        ///     Mentions that will be allowed.
        /// </param>
        public static IMessageMention GetMessageMention(AllowedMention mention) => new MessageMention(mention);

        /// <summary>
        ///     Gets a new mention constructor.
        /// </summary>
        public static MessageMentionBuilder GetMentionBuilder() => MessageMentionBuilder.New();

        /// <summary>
        ///     Gets a new mention constructor with a predefined allowed mention.
        /// </summary>
        public static MessageMentionBuilder GetMentionBuilder(AllowedMention mention) => MessageMentionBuilder.New(mention);

        /// <summary>
        ///     Gets a new mention constructor with a preset of allowed mentions from the webhook.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="webhook"/> is null.
        /// </exception>
        public static MessageMentionBuilder GetMentionBuilder(IWebhook webhook) => MessageMentionBuilder.New(webhook);

        /// <summary>
        ///     Gets a new message constructor.
        /// </summary>
        public static MessageBuilder GetMessageBuilder() => MessageBuilder.New();

        /// <summary>
        ///     Gets a new message constructor with source presets.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="source"/> is null
        /// </exception>
        public static MessageBuilder GetMessageBuilder(MessageBuilder source) => MessageBuilder.New(source);

        /// <summary>
        ///     Gets a new message constructor with a preset of allowed mentions from the webhook.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="webhook"/> is null.
        /// </exception>
        public static MessageBuilder GetMessageBuilder(IWebhook webhook) => MessageBuilder.New(webhook);

        /// <summary>
        ///     Gets a new Builder for a embed.
        /// </summary>
        public static EmbedBuilder GetEmbedBuilder() => EmbedBuilder.New();

        public static EmbedAuthorBuilder GetEmbedAuthorBuilder() => EmbedAuthorBuilder.New();

        public static EmbedFieldBuilder GetEmbedFieldBuilder() => EmbedFieldBuilder.New();

        public static EmbedFooterBuilder GetEmbedFooterBuilder() => EmbedFooterBuilder.New();

        public static EmbedImageBuilder GetEmbedImageBuilder() => EmbedImageBuilder.New();

        public static EmbedProviderBuilder GetEmbedProviderBuilder() => EmbedProviderBuilder.New();

        public static EmbedThumbnailBuilder GetEmbedThumbnailBuilder() => EmbedThumbnailBuilder.New();

        public static EmbedVideoBuilder GetEmbedVideoBuilder() => EmbedVideoBuilder.New();

        /// <summary>
        ///     Gets a new modifier content constructor.
        /// </summary>
        /// <returns></returns>
        public static ModifyContentBuilder GetModifyContentBuilder() => ModifyContentBuilder.New();
    }
}