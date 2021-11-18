using System.Linq;
using Rocket.Unturned.Player;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace RFRocketLibrary.Utils
{
    public static class PlayerExtensions
    {
        public static RaycastInfo GetRaycastInfo(this UnturnedPlayer player, float distance, int masks) =>
            DamageTool.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward), distance,
                masks);

        public static bool GetRaycastHit(this UnturnedPlayer player, float distance, int masks, out RaycastHit hit) =>
            PhysicsUtility.raycast(new Ray(player.Player.look.aim.position, player.Player.look.aim.forward),
                out hit, distance, masks);

        public static SteamPending GetSteamPending(this CSteamID cSteamID) =>
            Provider.pending.FirstOrDefault(x => x.playerID.steamID == cSteamID);
    }
}