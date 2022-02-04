using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using RFRocketLibrary.Constants;
using RFRocketLibrary.Enum;

namespace RFRocketLibrary.Utils
{
    public static class DependencyUtil
    {
        public static void Load(EDependency dependency)
        {
            using var wc = new WebClient();
            wc.Proxy = null;
            byte[] assembly;
            var cachePath = GetCachePath(dependency);
            if (IsCacheExists(dependency))
            {
                using var fs = File.OpenRead(cachePath);
                assembly = StreamToByteArray(fs);
                fs.Close();
            }
            else
            {
                assembly = wc.DownloadData(typeof(DependencyLink).GetField(dependency.ToString()).GetValue(null)
                    .ToString());
                File.WriteAllBytes(cachePath, assembly);
            }

            Assembly.Load(assembly);
        }

        public static async void LoadAsync(EDependency dependency)
        {
            using var wc = new WebClient();
            wc.Proxy = null;
            byte[] assembly;
            var cachePath = GetCachePath(dependency);
            if (IsCacheExists(dependency))
            {
                using var fs = File.OpenRead(cachePath);
                assembly = StreamToByteArray(fs);
                fs.Close();
            }
            else
            {
                assembly = await wc.DownloadDataTaskAsync(typeof(DependencyLink).GetField(dependency.ToString())
                    .GetValue(null).ToString());
                File.WriteAllBytes(cachePath, assembly);
            }

            Assembly.Load(assembly);
        }

        public static bool CanBeLoaded(EDependency dependency)
        {
            return AppDomain.CurrentDomain.GetAssemblies().All(x =>
                x.FullName != typeof(DependencyFullName).GetField(dependency.ToString()).GetValue(null).ToString());
        }

        public static bool IsCacheExists(EDependency dependency)
        {
            var cachePath = GetCachePath(dependency);
            return File.Exists(cachePath);
        }

        public static string GetCachePath(EDependency dependency)
        {
            var rocketDir = Environment.CurrentDirectory;
            var librariesDir = Path.Combine(rocketDir, "Libraries");
            var cacheDir = Path.Combine(librariesDir, "Cache");
            if (!Directory.Exists(cacheDir))
                Directory.CreateDirectory(cacheDir);
            return Path.Combine(cacheDir, "");
        }

        public static string GetIntegrity(string filePath)
        {
            using var md = MD5.Create();
            using var fileStream = File.OpenRead(filePath);
            var value = md.ComputeHash(fileStream);
            var result = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
            fileStream.Close();

            return result;
        }

        public static string GetIntegrity(byte[] buffer)
        {
            using var md = MD5.Create();
            var value = md.ComputeHash(buffer);
            var result = BitConverter.ToString(value).Replace("-", "").ToLowerInvariant();
            return result;
        }

        public static byte[] StreamToByteArray(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
            {
                return memoryStream.ToArray();
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}