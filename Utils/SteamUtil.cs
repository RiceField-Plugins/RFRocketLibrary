using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Cysharp.Threading.Tasks;
using RFRocketLibrary.Models;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Utils
{
    public static class SteamUtil
    {
        #region Methods

        public static string GetGroupName(ulong groupId)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load("https://steamcommunity.com/gid/" + groupId + "/memberslistxml/?xml=1&p=1");
                var node = xml.SelectSingleNode("//*[local-name()='groupName']");
                return node?.InnerText ?? string.Empty;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetGroupName: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return string.Empty;
            }
        }

        public static SteamGroup? GetSteamGroup(ulong groupId, uint page = 1, bool useNode = false)
        {
            if (groupId == 0)
                return null;

            var url = $"https://steamcommunity.com/gid/{groupId}/memberslistxml/?xml=1&p={page}";
            switch (useNode)
            {
                case true:
                    var xml = new XmlDocument();
                    try
                    {
                        xml.Load(url);
                        var serializer = new XmlSerializer(typeof(SteamProfile));
                        using XmlReader reader = new XmlNodeReader(xml);
                        return (SteamGroup) serializer.Deserialize(reader);
                    }
                    catch (Exception e)
                    {
                        var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamGroup: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                        return null;
                    }

                case false:
                    var wc = new WebClient();
                    wc.Proxy = null;
                    try
                    {
                        var doc = wc.DownloadString(url);
                        var serializer = new XmlSerializer(typeof(SteamProfile));
                        using var reader = new StringReader(doc);
                        return (SteamGroup) serializer.Deserialize(reader);
                    }
                    catch (Exception e)
                    {
                        var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamGroup: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                        return null;
                    }
            }
        }

        public static async Task<SteamGroup?> GetSteamGroupAsync(ulong groupId)
        {
            var url = $"https://steamcommunity.com/gid/{groupId}/memberslistxml/?xml=1&p=1";
            var wc = new WebClient();
            wc.Proxy = null;
            try
            {
                var doc = await wc.DownloadStringTaskAsync(url);
                var serializer = new XmlSerializer(typeof(SteamProfile));
                using var reader = new StringReader(doc);
                return (SteamGroup) serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamGroupAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public static async UniTask<SteamGroup?> GetSteamGroupUniTaskAsync(ulong groupId)
        {
            var url = $"https://steamcommunity.com/gid/{groupId}/memberslistxml/?xml=1&p=1";
            var wc = new WebClient();
            wc.Proxy = null;
            try
            {
                var doc = await wc.DownloadStringTaskAsync(url);
                var serializer = new XmlSerializer(typeof(SteamProfile));
                using var reader = new StringReader(doc);
                return (SteamGroup) serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamGroupUniTaskAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public static string GetSteamName(ulong steamId)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load("https://steamcommunity.com/profiles/" + steamId + "?xml=1");
                var node = xml.SelectSingleNode("//*[local-name()='steamID']");
                return node?.InnerText ?? string.Empty;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamName: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return string.Empty;
            }
        }

        public static SteamProfile? GetSteamProfile(ulong steamId, bool useNode = false)
        {
            var url = $"https://steamcommunity.com/profiles/{steamId}?xml=1";
            switch (useNode)
            {
                case true:
                    var xml = new XmlDocument();
                    try
                    {
                        xml.Load(url);
                        var serializer = new XmlSerializer(typeof(SteamProfile));
                        using XmlReader reader = new XmlNodeReader(xml);
                        return (SteamProfile) serializer.Deserialize(reader);
                    }
                    catch (Exception e)
                    {
                        var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamProfile: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                        return null;
                    }

                case false:
                    var wc = new WebClient();
                    wc.Proxy = null;
                    try
                    {
                        var doc = wc.DownloadString(url);
                        var serializer = new XmlSerializer(typeof(SteamProfile));
                        using var reader = new StringReader(doc);
                        return (SteamProfile) serializer.Deserialize(reader);
                    }
                    catch (Exception e)
                    {
                        var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                        Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamProfile: {e.Message}");
                        Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                        return null;
                    }
            }
        }

        public static async Task<SteamProfile?> GetSteamProfileAsync(ulong steamId)
        {
            if (steamId == 0)
                return null;
            
            var url = $"https://steamcommunity.com/profiles/{steamId}?xml=1";
            var wc = new WebClient();
            wc.Proxy = null;
            try
            {
                var doc = await wc.DownloadStringTaskAsync(url);
                var serializer = new XmlSerializer(typeof(SteamProfile));
                using var reader = new StringReader(doc);
                return (SteamProfile) serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamProfileAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public static async UniTask<SteamProfile?> GetSteamProfileUniTaskAsync(ulong steamId)
        {
            var url = $"https://steamcommunity.com/profiles/{steamId}?xml=1";
            var wc = new WebClient();
            wc.Proxy = null;
            try
            {
                var doc = await wc.DownloadStringTaskAsync(url);
                var serializer = new XmlSerializer(typeof(SteamProfile));
                using var reader = new StringReader(doc);
                return (SteamProfile) serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] SteamUtil GetSteamProfileUniTaskAsync: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        #endregion
    }
}