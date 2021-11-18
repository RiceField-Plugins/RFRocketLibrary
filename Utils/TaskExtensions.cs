using System;
using System.Reflection;
using System.Threading.Tasks;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Utils
{
    public static class TaskExtensions
    {
        public static void Forget(this Task task)
        {
            var awaiter = task.GetAwaiter();
            if (awaiter.IsCompleted)
            {
                try
                {
                    awaiter.GetResult();
                }
                catch (Exception e)
                {
                    var caller = Assembly.GetCallingAssembly().GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] Task Forget: {e.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                }
            }
            else
                awaiter.OnCompleted(() =>
                {
                    try
                    {
                        awaiter.GetResult();
                    }
                    catch (Exception e)
                    {
                        var caller = Assembly.GetCallingAssembly().GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] Task Forget: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                    }
                });
        }

        public static void Forget(
            this Task task,
            Action<Exception> exceptionHandler,
            bool handleExceptionOnMainThread = true)
        {
            if (exceptionHandler == null)
                task.Forget();
            else
                ForgetCoreWithCatch(task, exceptionHandler, handleExceptionOnMainThread).Forget();
        }

        private static async Task ForgetCoreWithCatch(
            Task task,
            Action<Exception> exceptionHandler,
            bool handleExceptionOnMainThread)
        {
            try
            {
                await task;
            }
            catch (Exception ex1)
            {
                try
                {
                    if (handleExceptionOnMainThread)
                        TaskDispatcher.QueueOnMainThread(() => exceptionHandler(ex1));
                    else
                        exceptionHandler(ex1);
                }
                catch (Exception e2)
                {
                    var caller = Assembly.GetCallingAssembly().GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] Task ForgetCoreWithCatch: {e2.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e2}");
                }
            }
        }

        public static void Forget<T>(this Task<T> task)
        {
            var awaiter = task.GetAwaiter();
            if (awaiter.IsCompleted)
            {
                try
                {
                    awaiter.GetResult();
                }
                catch (Exception e)
                {
                    var caller = Assembly.GetCallingAssembly().GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] Task Forget: {e.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                }
            }
            else
                awaiter.OnCompleted(() =>
                {
                    try
                    {
                        awaiter.GetResult();
                    }
                    catch (Exception e)
                    {
                        var caller = Assembly.GetCallingAssembly().GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] Task Forget: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                    }
                });
        }

        public static void Forget<T>(
            this Task<T> task,
            Action<Exception> exceptionHandler,
            bool handleExceptionOnMainThread = true)
        {
            if (exceptionHandler == null)
                task.Forget();
            else
                ForgetCoreWithCatch(task, exceptionHandler, handleExceptionOnMainThread).Forget();
        }

        private static async Task ForgetCoreWithCatch<T>(
            Task<T> task,
            Action<Exception> exceptionHandler,
            bool handleExceptionOnMainThread)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                try
                {
                    if (handleExceptionOnMainThread)
                        TaskDispatcher.QueueOnMainThread(() => exceptionHandler(e));
                    else
                        exceptionHandler(e);
                }
                catch (Exception e2)
                {
                    var caller = Assembly.GetCallingAssembly().GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] Task ForgetCoreWithCatch: {e2.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e2}");
                }
            }
        }

        /// <summary>
        /// This method does not catch any exception within Task and just forget everything
        /// </summary>
        public static void JustForget(this Task task)
        {
            if (!task.IsCompleted || task.IsFaulted)
            {
                _ = ForgetAwaited(task);
            }
            
            static async Task ForgetAwaited(Task task)
            {
                try
                {
                    await task.ConfigureAwait(false);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public static void Wait(this Task task)
        {
            task.GetAwaiter().GetResult();
        }
    }
}