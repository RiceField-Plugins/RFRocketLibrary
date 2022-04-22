using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Storages
{
    public class JsonDataStore<T> where T : class
    {
        #region Methods

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
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JsonDataStore Load: {e.Message}");
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
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JsonDataStore LoadAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
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
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JsonDataStore SaveAsync: {e.Message}");
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
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] JsonDataStore SaveAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return false;
            }
        }

        #endregion

        public JsonDataStore(string dir, string fileName)
        {
            DataPath = Path.Combine(dir, fileName);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            
            if (!File.Exists(DataPath))
                File.CreateText(DataPath).Dispose();
        }

        private string DataPath { get; set; }
    }
}