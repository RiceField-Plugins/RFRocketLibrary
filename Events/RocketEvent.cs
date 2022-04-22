using Rocket.API;
using Rocket.Core;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;
using Rocket.Unturned.Permissions;
using SDG.Unturned;
using Steamworks;

namespace RFRocketLibrary.Events
{
    public static class RocketEvent
    {
        #region Events

        public static event R.RockedInitialized? OnInitialized;
        public static event RocketPluginManager.PluginsLoaded? OnPluginLoaded;
        public static event RocketCommandManager.ExecuteCommand? OnPreCommandExecuted;
        public static event UnturnedPermissions.JoinRequested? OnPrePlayerJoined;
        public static event RocketPlugin.PluginLoading? OnPrePluginLoaded;
        public static event ImplementationShutdown? OnShutdown;

        #endregion

        #region Methods

        public static void OnPluginLoadedInvoker()
        {
            OnPluginLoaded?.Invoke();
        }

        public static void OnPreCommandExecutedInvoker(IRocketPlayer player, IRocketCommand command, ref bool cancel)
        {
            OnPreCommandExecuted?.Invoke(player, command, ref cancel);
        }

        public static void OnPrePlayerJoinedInvoker(CSteamID player, ref ESteamRejection? rejectionReason)
        {
            OnPrePlayerJoined?.Invoke(player, ref rejectionReason);
        }

        public static void OnPrePluginLoadedInvoker(IRocketPlugin rocketPlugin, ref bool cancelLoading)
        {
            OnPrePluginLoaded?.Invoke(rocketPlugin, ref cancelLoading);
        }

        public static void OnShutdownInvoker()
        {
            OnShutdown?.Invoke();
        }

        #endregion

        internal static void OnInitializedInvoker()
        {
            OnInitialized?.Invoke();
        }
    }
}