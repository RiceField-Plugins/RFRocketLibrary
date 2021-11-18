using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFRocketLibrary.Utils
{
    // Original from https://github.com/ShimmyMySherbet/RocketExtensions
    // Original Author: ShimmyMySherbet
    public class TaskDispatcher : MonoBehaviour
    {
        private static bool Initialized { get; set; }
        private static GameObject? Object { get; set; }
        private static TaskDispatcher? Component { get; set; }

        private static void CheckInit()
        {
            if (Initialized) 
                return;
            Initialized = true;
            Object = new GameObject("RFRocketLibrary.TaskDispatcher");
            DontDestroyOnLoad(Object);
            Component = Object.AddComponent<TaskDispatcher>();
        }

        public static void QueueOnMainThread(Action action)
        {
            CheckInit();

            if (Component is null) 
                return;
            lock (Component.Queue)
            {
                Component.Queue.Add(action);
                Component.QueueLength++;
            }
        }

        private int QueueLength { get; set; }
        private List<Action> Queue { get; } = new();

        public void FixedUpdate()
        {
            while (QueueLength > 0)
            {
                Action act;
                lock (Queue)
                {
                    QueueLength -= 1;
                    act = Queue[0];
                    Queue.RemoveAtFast(0);
                }

                try
                {
                    act();
                }
                catch (Exception e)
                {
                    var caller = Assembly.GetCallingAssembly().GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] TaskDispatcher: {e.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                }
            }
        }
    }
}