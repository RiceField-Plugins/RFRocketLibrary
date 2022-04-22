namespace DSharp4Webhook.Core.Constructor.Meta
{
    /// <summary>
    ///     Abstraction for constructors.
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        ///     Resets the constructor to the default preset.
        /// </summary>
        public void Reset();
    }
}