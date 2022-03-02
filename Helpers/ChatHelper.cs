using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace RFRocketLibrary.Helpers
{
    public static class ChatHelper
    {
        public static void Broadcast(string text, Color? color = null, string? iconURL = null)
        {
            ChatManager.serverSendMessage(text, color ?? Color.green, null, null, EChatMode.GLOBAL, iconURL, true);
        }

        public static void Say(UnturnedPlayer player, string text, Color? color = null, string? iconURL = null)
        {
            Say(player.SteamPlayer(), text, color, iconURL);
        }

        public static void Say(Player player, string text, Color? color = null, string? iconURL = null)
        {
            Say(player.channel.owner, text, color, iconURL);
        }

        public static void Say(SteamPlayer player, string text, Color? color = null, string? iconURL = null)
        {
            ChatManager.serverSendMessage(text, color ?? Color.green, null, player, EChatMode.SAY, iconURL, true);
        }

        public static void Say(CSteamID steamID, string text, Color? color = null, string? iconURL = null)
        {
            if (steamID == CSteamID.Nil || steamID.m_SteamID == 0)
                return;
            var exist = PlayerTool.getSteamPlayer(steamID);
            if (exist == null)
                return;
            Say(exist, text, color, iconURL);
        }

        public static void Say(IRocketPlayer player, string text, Color? color = null, string? iconURL = null)
        {
            if (player is ConsolePlayer)
            {
                Logger.Log(text);
                return;
            }
            
            if (!ulong.TryParse(player.Id, out var steamId))
                return;
            
            Say(new CSteamID(steamId), text, color, iconURL);
        }
    }
}