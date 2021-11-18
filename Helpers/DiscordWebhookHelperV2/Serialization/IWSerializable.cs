namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Serialization
{
    /// <summary>
    ///     Indicates whether webhook data can be serialized.
    /// </summary>
    public interface IWSerializable
    {
        /// <summary>
        ///     Serializes data to a type format.
        /// </summary>
        SerializeContext Serialize();
    }
}
