using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Storages
{
    public class DataStore<T> where T : class
    {
        private string DataPath { get; set; }

        public DataStore(string dir, string fileName)
        {
            DataPath = Path.Combine(dir, fileName);
            if (!File.Exists(DataPath))
                File.CreateText(DataPath).Dispose();
        }

        public bool Save(T obj)
        {
            try
            {
                var objData = JsonConvert.SerializeObject(obj, Formatting.Indented);

                using var stream = new FileStream(DataPath, FileMode.Create, FileAccess.ReadWrite,
                    FileShare.ReadWrite);
                using (var s = new StreamWriter(stream))
                {
                    s.Write(objData);
                    s.Close();
                }
                stream.Close();

                return true;
            }
            catch (Exception e)
            {
                var caller = Assembly.GetCallingAssembly().GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JSON SaveAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return false;
            }
        }

        public async Task<bool> SaveAsync(T obj)
        {
            try
            {
                var objData = JsonConvert.SerializeObject(obj, Formatting.Indented);

                using var stream = new FileStream(DataPath, FileMode.Create, FileAccess.ReadWrite,
                    FileShare.ReadWrite);
                using (var s = new StreamWriter(stream))
                {
                    await s.WriteAsync(objData);
                    s.Close();
                }
                stream.Close();

                return true;
            }
            catch (Exception e)
            {
                var caller = Assembly.GetCallingAssembly().GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JSON SaveAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return false;
            }
        }

        public T? Load()
        {
            try
            {
                if (!File.Exists(DataPath))
                    return null;
                string dataText;
                using (var stream = File.Open(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var s = new StreamReader(stream))
                    {
                        dataText = s.ReadToEnd();
                        s.Close();
                    }
                    stream.Close();
                }

                return JsonConvert.DeserializeObject<T>(dataText);
            }
            catch (Exception e)
            {
                var caller = Assembly.GetCallingAssembly().GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] DataStore Load: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public async Task<T?> LoadAsync()
        {
            try
            {
                if (!File.Exists(DataPath))
                    return null;
                string dataText;
                using (var stream = File.Open(DataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var s = new StreamReader(stream))
                    {
                        dataText = await s.ReadToEndAsync();
                        s.Close();
                    }
                    stream.Close();
                }

                return JsonConvert.DeserializeObject<T>(dataText);
            }
            catch (Exception e)
            {
                var caller = Assembly.GetCallingAssembly().GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] DataStore LoadAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }
    }
}