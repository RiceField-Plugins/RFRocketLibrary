using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util
{
    internal static class DictionaryUtil
    {
        /// <summary>
        ///     Returns the size of these values.
        /// </summary>
        internal static long SizeOf(this IDictionary<string, ReadOnlyCollection<byte>> source)
        {
            Checks.CheckForNull(source);
            return source.Sum(pair => pair.Value.LongCount());
        }
    }
}
