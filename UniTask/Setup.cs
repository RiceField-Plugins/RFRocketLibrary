using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Rocket.Core.Logging;
using UnityEngine.LowLevel;

namespace RFRocketLibrary.UniTask
{
    public static class Setup
    {
        private static bool Initialized { get; set; }

        public static void CheckInit()
        {
            if (Initialized)
                return;
            Initialized = true;
            Init();
        }

        private static void Init()
        {
            // Original from https://github.com/openmod/openmod/blob/main/unityengine/OpenMod.UnityEngine/UnityHostLifetime.cs
            // Original Author: Trojaner

            try
            {
                if (IsOpenModPresent())
                    return;
                if (Cysharp.Threading.Tasks.PlayerLoopHelper.IsInjectedUniTaskPlayerLoop())
                    return;
                var unitySynchronizationContextField = typeof(Cysharp.Threading.Tasks.PlayerLoopHelper).GetField("unitySynchronizationContext",
                    BindingFlags.Static | BindingFlags.NonPublic);

                unitySynchronizationContextField?.SetValue(null, SynchronizationContext.Current);

                var mainThreadIdField =
                    typeof(Cysharp.Threading.Tasks.PlayerLoopHelper).GetField("mainThreadId", BindingFlags.Static | BindingFlags.NonPublic) ??
                    throw new Exception("Could not find PlayerLoopHelper.mainThreadId field");
                mainThreadIdField.SetValue(null, Thread.CurrentThread.ManagedThreadId);

                var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
                Cysharp.Threading.Tasks.PlayerLoopHelper.Initialize(ref playerLoop);
            }
            catch (Exception e)
            {
                var caller = Assembly.GetCallingAssembly().GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] UniTask Init: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
            }
        }

        public static bool IsOpenModPresent() =>
            AppDomain.CurrentDomain.GetAssemblies().Any(x => x.GetName().Name == "OpenMod.Core");
    }
}