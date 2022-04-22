using System.Collections.Generic;
using System.Reflection;
using RFRocketLibrary.Events;
using SDG.Unturned;

namespace RFRocketLibrary
{
    public static class Library
    {
        private static bool Initialized { get; set; }
        private static uint AttachedAssembly { get; set; }
        private static uint AttachedAssemblyWithHarmony { get; set; }

        private static void Initialize()
        {
            if (Initialized)
                return;
            
            Initialized = true;
        }

        public static void AttachEvent(bool withHarmony = false)
        {
            EventBus.Load();
            AttachedAssembly++;
            if (withHarmony)
            {
                EventBus.LoadHarmony();
                AttachedAssemblyWithHarmony++;
            }
        }

        public static void DetachEvent(bool withHarmony = false)
        {
            AttachedAssembly--;
            if (AttachedAssembly == 0)
                EventBus.Unload();
            
            if (withHarmony)
            {
                AttachedAssemblyWithHarmony--;
                if (AttachedAssemblyWithHarmony == 0)
                    EventBus.UnloadHarmony();
            }
        }
    }
}