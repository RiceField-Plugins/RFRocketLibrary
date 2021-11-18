using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RFRocketLibrary.Helpers.DiscordWebhookHelperV2.Util.Extensions
{
    internal static class CollectionUtil
    {
        internal static ReadOnlyCollection<T>? ToReadOnlyCollection<T>(this IList<T>? array)
        {
            return array is null ? null : new ReadOnlyCollection<T>(array);
        }
    }
}
