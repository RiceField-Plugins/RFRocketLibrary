using System.Reflection;
using SDG.Unturned;

namespace RFRocketLibrary.Utils
{
    public static class ChatUtil
    {
        #region Methods

        public static void DisableMaxMessageLengthLimit()
        {
            typeof(ChatManager).GetField("MAX_MESSAGE_LENGTH", BindingFlags.Public | BindingFlags.Static)?
                .SetValue(null, int.MaxValue);
        }

        #endregion
    }
}