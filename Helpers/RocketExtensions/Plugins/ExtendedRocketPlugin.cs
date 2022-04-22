using System;
using System.Diagnostics;
using System.Threading.Tasks;
// using Cysharp.Threading.Tasks;
using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using RocketExtensions.Utilities.ShimmyMySherbet.Extensions;

namespace RocketExtensions.Plugins
{
    public abstract class ExtendedRocketPlugin : RocketPlugin
    {
        public override void LoadPlugin()
        {
            base.LoadPlugin();
            // UniTask.RunOnThreadPool(RunLoadAsync);
            Task.Run(async () => await RunLoadAsync());
        }

        public void LogError(Exception e)
        {
            var asm = GetType().Assembly.GetName();
            var caller = asm.Name;
            Logger.LogError($"[{caller}] [ERROR] Error: {e.Message}");
            Logger.LogError($"[{caller}] [ERROR] Plugin: {asm.Name} v{asm.Version}");
            Logger.LogError($"[{caller}] [ERROR] Source: {e.Source}");
            Logger.LogError($"[{caller}] [ERROR] {e.StackTrace}");

            if (e.InnerException != null)
            {
                Logger.LogError($"[{caller}] [ERROR] Inner: {e.InnerException.Message}");
                Logger.LogError($"[{caller}] [ERROR] {e.InnerException.StackTrace}");
            }
        }

        // private async UniTask RunLoadAsync()
        private async Task RunLoadAsync()
        {
            try
            {
                await LoadAsync();
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] An exception occurred during plugin load (async)");
                LogError(e);
                await UnloadAsync(PluginState.Failure);
            }
        }

        // public async UniTask UnloadAsync(PluginState state = PluginState.Unloaded)
        public async Task UnloadAsync(PluginState state = PluginState.Unloaded)
        {
            await ThreadTool.RunOnGameThreadAsync(UnloadPlugin, state);
        }

        // public virtual UniTask LoadAsync()
        public virtual Task LoadAsync()
        {
            return Task.CompletedTask;
        }
    }
}