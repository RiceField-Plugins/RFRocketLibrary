using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RFRocketLibrary.Utils;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

// ReSharper disable InconsistentNaming

namespace RFRocketLibrary.Events
{
    public static class UnturnedPatchEvent
    {
        #region Delegates

        public delegate void AnimalDamaged(Animal animal, ushort damage, EPlayerKill kill, uint xp);

        public delegate void AnimalKilled(Animal? animal, ref Vector3 ragdoll, ref ERagdollEffect ragdollEffect);

        public delegate void AnimalMovementChanged(Animal animal, Vector3 lastPosition);

        public delegate void BarricadeDestroyed(BarricadeDrop drop, byte x, byte y, ushort plant);

        public delegate void PlayerAttack(Player player, RaycastInfo raycastInfo);

        public delegate void PlayerDamaged(Player player, byte amount, EDeathCause newCause, ELimb newLimb,
            CSteamID newKiller, EPlayerKill kill);

        public delegate void PlayerFiremodeChanged(Player player, EFiremode newFiremode);

        public delegate void PlayerForagedResource(Player player, ResourceSpawnpoint resourceSpawnpoint);

        public delegate void PlayerMovementChanged(Player player, Vector3 lastPosition);

        public delegate void PreAnimalKilled(Animal animal, ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp,
            ref bool trackKill, ref bool dropLoot, ref ERagdollEffect ragdollEffect, ref bool shouldAllow);

        public delegate void PreAnimalSpawned(Animal animal, ref Vector3 position, ref byte angle,
            ref bool shouldAllow);

        public delegate void PreBarricadeDestroyed(BarricadeDrop drop, byte x, byte y, ushort plant,
            ref bool shouldAllow);

        public delegate void PreItemDropSpawned(byte x, byte y, ref ushort id, ref byte amount, ref byte quality,
            ref byte[] state, ref Vector3 point, uint instanceID, ref bool shouldAllow);

        // public delegate void PrePlayerCaughtFish(Player player, ref bool shouldCatch);

        public delegate void PrePlayerChangedEquipment(Player player, byte page, byte x, byte y, ref Guid newAssetGuid,
            ref byte newQuality, ref byte[] newState, ref NetId useableNetId, ref bool shouldAllow);

        public delegate void PrePlayerChangedGesture(Player player, ref EPlayerGesture gesture, ref bool shouldAllow);

        public delegate void PrePlayerChatted(Player player, ref EChatMode chatMode, ref string text,
            ref bool shouldAllow);

        public delegate void PrePlayerDraggedItem(PlayerInventory inventory, byte page_0, byte x_0, byte y_0,
            byte page_1, byte x_1, byte y_1, byte rot_1, ref bool shouldAllow);

        public delegate void PrePlayerDroppedItem(Player inventory, byte page, byte x, byte y, ref bool shouldAllow);

        public delegate void PrePlayerFiremodeChanged(Player player, EFiremode newFiremode, ref bool shouldAllow);

        public delegate void PrePlayerForagedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            ref bool shouldAllow);

        public delegate void PrePlayerKilled(Player player, ref Vector3 newRagdoll, ref EDeathCause newCause,
            ref ELimb newLimb, ref CSteamID newKiller, ref EPlayerKill kill, ref bool trackKill,
            ref ERagdollEffect newRagdollEffect,
            ref bool shouldAllow);

        public delegate void PrePlayerSwappedItem(PlayerInventory inventory, byte page_0, byte x_0, byte y_0,
            byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1, ref bool shouldAllow);

        public delegate void PrePreVehicleDestroyed(InteractableVehicle vehicle, ref bool shouldAllow);

        public delegate void PrePreVehicleExploded(InteractableVehicle vehicle, ref bool shouldAllow);

        public delegate void PreResourceSpawned(ResourceSpawnpoint resourceSpawnpoint, ref bool shouldAllow);

        public delegate void PreServerSentMessage(ref string text, ref Color color, ref SteamPlayer fromPlayer,
            ref SteamPlayer toPlayer, ref EChatMode mode, ref string iconURL, ref bool useRichTextFormatting,
            ref bool shouldAllow);

        public delegate void PreStructureDestroyed(StructureDrop drop, byte x, byte y, ref Vector3 ragdoll,
            ref bool shouldAllow);

        public delegate void PreVehicleDestroyed(InteractableVehicle vehicle);

        public delegate void PreVehicleExploded(InteractableVehicle vehicle);

        public delegate void PreVehicleSpawned(VehicleSpawnpoint vehicleSpawnpoint, ref bool shouldAllow);

        public delegate void PreZombieKilled(Zombie zombie, ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp,
            ref bool trackKill, ref bool dropLoot, ref EZombieStunOverride zombieStunOverride,
            ref ERagdollEffect ragdollEffect, ref bool shouldAllow);

        public delegate void PreZombieSpawned(byte reference, ushort id, ref byte newType, ref byte newSpeciality,
            ref byte newShirt, ref byte newPants, ref byte newHat, ref byte newGear, ref Vector3 newPosition,
            ref byte newAngle, ref bool shouldAllow);

        public delegate void ResourceDead(ResourceSpawnpoint resourceSpawnpoint, ref Vector3 ragdoll);

        public delegate void StructureDestroyed(StructureDrop drop, byte x, byte y, Vector3 ragdoll);

        public delegate void VehicleMovementChanged(InteractableVehicle vehicle, Vector3 lastPosition);

        public delegate void VehicleMovementChangedByPlayer(InteractableVehicle vehicle, Player? player,
            Vector3 lastPosition);

        public delegate void ZombieDamaged(Zombie zombie, ushort damage, EPlayerKill kill, uint xp);

        public delegate void ZombieKilled(Zombie? zombie, ZombieRegion? zombieRegion, ref Vector3 ragdoll,
            ref ERagdollEffect ragdollEffect);

        public delegate void ZombieMovementChanged(Zombie zombie, Vector3 lastPosition);

        #endregion

        #region Events

        // Events
        public static event AnimalDamaged? OnAnimalDamaged;
        public static event AnimalKilled? OnAnimalKilled;
        public static event AnimalMovementChanged? OnAnimalMovementChanged;
        public static event BarricadeDestroyed? OnBarricadeDestroyed;

        public static event PlayerAttack? OnPlayerAttack;
        public static event PlayerDamaged? OnPlayerDamaged;
        public static event PlayerFiremodeChanged? OnPlayerFiremodeChanged;
        public static event PlayerForagedResource? OnPlayerForagedResource;
        public static event PlayerMovementChanged? OnPlayerMovementChanged;
        public static event PreAnimalKilled? OnPreAnimalKilled;
        public static event PreAnimalSpawned? OnPreAnimalSpawned;
        public static event PreBarricadeDestroyed? OnPreBarricadeDestroyed;
        public static event PreItemDropSpawned? OnPreItemDropSpawned;

        // public static event PrePlayerCaughtFish? OnPrePlayerCaughtFish;
        public static event PrePlayerChangedEquipment? OnPrePlayerChangedEquipment;
        public static event PrePlayerChangedGesture? OnPrePlayerChangedGesture;
        public static event PrePlayerChatted? OnPrePlayerChatted;
        public static event PrePlayerDraggedItem? OnPrePlayerDraggedItem;
        public static event PrePlayerDroppedItem? OnPrePlayerDroppedItem;
        public static event PrePlayerFiremodeChanged? OnPrePlayerFiremodeChanged;
        public static event PrePlayerForagedResource? OnPrePlayerForagedResource;
        public static event PrePlayerKilled? OnPrePlayerKilled;
        public static event PrePlayerSwappedItem? OnPrePlayerSwappedItem;
        public static event PrePreVehicleDestroyed? OnPrePreVehicleDestroyed;
        public static event PrePreVehicleExploded? OnPrePreVehicleExploded;
        public static event PreResourceSpawned? OnPreResourceSpawned;
        public static event PreServerSentMessage? OnPreServerMessageSent;
        public static event PreStructureDestroyed? OnPreStructureDestroyed;
        public static event PreVehicleDestroyed? OnPreVehicleDestroyed;
        public static event PreVehicleExploded? OnPreVehicleExploded;
        public static event PreVehicleSpawned? OnPreVehicleSpawned;
        public static event PreZombieKilled? OnPreZombieKilled;
        public static event PreZombieSpawned? OnPreZombieSpawned;
        public static event ResourceDead? OnResourceChopped;
        public static event ResourceDead? OnResourceForaged;
        public static event ResourceDead? OnResourceMined;
        public static event StructureDestroyed? OnStructureDestroyed;
        public static event VehicleMovementChanged? OnVehicleMovementChanged;
        public static event VehicleMovementChangedByPlayer? OnVehicleMovementChangedByPlayer;
        public static event ZombieDamaged? OnZombieDamaged;
        public static event ZombieKilled? OnZombieKilled;
        public static event ZombieMovementChanged? OnZombieMovementChanged;

        #endregion

        #region Nested type: InternalPatches

        [HarmonyPatch]
        internal static class InternalPatches
        {
            #region Methods

            [HarmonyCleanup]
            public static Exception? Cleanup(Exception? ex, MethodBase original)
            {
                if (ex == null)
                    return null;

                Logger.LogError($"[RFRocketLibrary] [ERROR] InternalPatches Exception: {ex}");
                Logger.LogError(
                    $"[RFRocketLibrary] [ERROR] InternalPatches Exception: Failed to patch original method {original.FullDescription()} from patching type {typeof(InternalPatches).FullDescription()}");
                return null;
            }

            [HarmonyPatch(typeof(Animal), "tick")]
            [HarmonyPrefix]
            public static void OnAnimalMovementChangedInvoker(Animal __instance, Vector3 ___lastUpdatePos)
            {
                OnAnimalMovementChanged?.Invoke(__instance, ___lastUpdatePos);
            }

            [HarmonyPatch(typeof(PlayerMovement), "simulate", new Type[0])]
            [HarmonyPrefix]
            public static void OnPlayerMovementChangedInvoker(PlayerMovement __instance, Vector3 ___lastUpdatePos)
            {
                OnPlayerMovementChanged?.Invoke(__instance.player, ___lastUpdatePos);
            }

            [HarmonyPatch(typeof(AnimalManager), "ReceiveAnimalAlive")]
            [HarmonyPrefix]
            public static bool OnPreAnimalSpawnedInvoker(ushort index, ref Vector3 newPosition, ref byte newAngle)
            {
                if (index >= AnimalManager.animals.Count)
                    return false;
                
                var shouldAllow = true;
                OnPreAnimalSpawned?.Invoke(AnimalManager.animals[index], ref newPosition, ref newAngle, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ItemManager), "ReceiveItem")]
            [HarmonyPrefix]
            public static bool OnPreItemDropSpawnedInvoker(byte x, byte y, ref ushort id, ref byte amount,
                ref byte quality,
                ref byte[] state, ref Vector3 point, uint instanceID)
            {
                var shouldAllow = true;
                OnPreItemDropSpawned?.Invoke(x, y, ref id, ref amount, ref quality, ref state, ref point, instanceID,
                    ref shouldAllow);
                if (!shouldAllow)
                    return false;
                return true;
            }

            [HarmonyPatch(typeof(ResourceManager), "ReceiveResourceAlive")]
            [HarmonyPrefix]
            public static bool OnPreResourceSpawnedInvoker(byte x, byte y, ushort index)
            {
                var shouldAllow = true;
                OnPreResourceSpawned?.Invoke(LevelGround.trees[x, y][index], ref shouldAllow);
                if (!shouldAllow)
                    return false;
                return true;
            }

            [HarmonyPatch(typeof(VehicleManager), "addVehicleAtSpawn")]
            [HarmonyPrefix]
            public static bool OnPreVehicleSpawnedInvoker(VehicleSpawnpoint spawn)
            {
                var shouldAllow = true;
                OnPreVehicleSpawned?.Invoke(spawn, ref shouldAllow);
                if (!shouldAllow)
                    return false;
                return true;
            }

            [HarmonyPatch(typeof(ZombieManager), "ReceiveAnimalAlive")]
            [HarmonyPrefix]
            public static bool OnPreZombieSpawnedInvoker(byte reference, ushort id, ref byte newType,
                ref byte newSpeciality, ref byte newShirt, ref byte newPants, ref byte newHat, ref byte newGear,
                ref Vector3 newPosition, ref byte newAngle)
            {
                var shouldAllow = true;
                OnPreZombieSpawned?.Invoke(reference, id, ref newType, ref newSpeciality, ref newShirt, ref newPants,
                    ref newHat, ref newGear, ref newPosition, ref newAngle, ref shouldAllow);
                if (!shouldAllow)
                    return false;
                return true;
            }

            [HarmonyPatch(typeof(InteractableVehicle), "simulate", typeof(uint), typeof(int), typeof(bool),
                typeof(Vector3),
                typeof(Quaternion), typeof(float), typeof(float), typeof(int), typeof(float))]
            [HarmonyPrefix]
            public static void OnVehicleMovementChangedByPlayerInvoker(InteractableVehicle __instance,
                Vector3 ___lastUpdatedPos, uint simulation, int recov, bool inputStamina,
                Vector3 point, Quaternion angle, float newSpeed, float newPhysicsSpeed, int newTurn, float delta)
            {
                if (__instance.transform.position == ___lastUpdatedPos)
                    return;
                OnVehicleMovementChangedByPlayer?.Invoke(__instance,
                    __instance.passengers.ElementAtOrDefault(0)?.player?.player, ___lastUpdatedPos);
            }

            [HarmonyPatch(typeof(InteractableVehicle), "updateSafezoneStatus")]
            [HarmonyPrefix]
            public static void OnVehicleMovementChangedInvoker(InteractableVehicle __instance,
                Vector3 ___lastUpdatedPos, float deltaSeconds)
            {
                if (__instance.transform.position == ___lastUpdatedPos)
                    return;
                OnVehicleMovementChanged?.Invoke(__instance, ___lastUpdatedPos);
            }

            [HarmonyPatch(typeof(Zombie), "tick")]
            [HarmonyPrefix]
            public static void OnZombieMovementChangedInvoker(Zombie __instance, Vector3 ___lastUpdatedPos)
            {
                OnZombieMovementChanged?.Invoke(__instance, ___lastUpdatedPos);
            }

            #endregion

            [HarmonyPatch(typeof(AnimalManager), "ReceiveAnimalDead")]
            [HarmonyPrefix]
            internal static void OnAnimalKilledInvoker(ushort index, ref Vector3 newRagdoll,
                ref ERagdollEffect newRagdollEffect)
            {
                OnAnimalKilled?.Invoke(AnimalManager.animals.ElementAtOrDefault(index), ref newRagdoll,
                    ref newRagdollEffect);
            }

            [HarmonyPatch(typeof(Animal), "askDamage")]
            [HarmonyPrefix]
            internal static bool OnPreAnimalKilledInvoker(Animal __instance, ushort ___health, ushort amount,
                ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp, ref bool trackKill, ref bool dropLoot,
                ref ERagdollEffect ragdollEffect)
            {
                if (___health <= amount)
                {
                    var shouldAllow = true;
                    OnPreAnimalKilled?.Invoke(__instance, ref newRagdoll, ref kill, ref xp, ref trackKill, ref dropLoot,
                        ref ragdollEffect, ref shouldAllow);
                    if (!shouldAllow)
                        return false;
                }

                OnAnimalDamaged?.Invoke(__instance, amount, kill, xp);
                return true;
            }

            [HarmonyPatch(typeof(BarricadeManager), "destroyBarricade")]
            [HarmonyPatch(new[] {typeof(BarricadeDrop), typeof(byte), typeof(byte), typeof(ushort)})]
            [HarmonyPrefix]
            internal static bool OnPreBarricadeDestroyedInvoker(BarricadeDrop barricade, byte x, byte y, ushort plant)
            {
                var shouldAllow = true;
                OnPreBarricadeDestroyed?.Invoke(barricade, x, y, plant, ref shouldAllow);
                if (!shouldAllow)
                    return false;
                OnBarricadeDestroyed?.Invoke(barricade, x, y, plant);
                return true;
            }

            // [HarmonyLib.HarmonyPatch(typeof(UseableFisher), "ReceiveCatch")]
            // [HarmonyLib.HarmonyPrefix]
            // internal static bool OnPreFishCaughtInvoker(UseableFisher __instance, ref bool ___isCatch,
            //     float ___lastLuck, float ___luckTime, bool ___hasLuckReset)
            // {
            //     if (OnPrePlayerCaughtFish == null)
            //         return true;
            //     if (Time.realtimeSinceStartup - ___lastLuck > ___luckTime - 2.4f ||
            //         ___hasLuckReset && Time.realtimeSinceStartup - ___lastLuck < 1f)
            //         ___isCatch = true;
            //     OnPrePlayerCaughtFish.Invoke(__instance.player, ref ___isCatch);
            //     return false;
            // }

            [HarmonyPatch(typeof(PlayerInventory), "ReceiveDragItem")]
            [HarmonyPrefix]
            internal static bool OnPreItemDraggedInvoker(PlayerInventory __instance, byte page_0, byte x_0, byte y_0,
                byte page_1, byte x_1, byte y_1, byte rot_1)
            {
                var shouldAllow = true;
                OnPrePlayerDraggedItem?.Invoke(__instance, page_0, x_0, y_0, page_1, x_1, y_1, rot_1, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerInventory), "ReceiveSwapItem")]
            [HarmonyPrefix]
            internal static bool OnPreItemSwappedInvoker(PlayerInventory __instance, byte page_0, byte x_0, byte y_0,
                byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1)
            {
                var shouldAllow = true;
                OnPrePlayerSwappedItem?.Invoke(__instance, page_0, x_0, y_0, rot_0, page_1, x_1, y_1, rot_1,
                    ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(UseableMelee), "fire")]
            [HarmonyPrefix]
            internal static void OnPlayerAttackInvoker(UseableMelee __instance)
            {
                var raycastInfo =
                    DamageTool.raycast(new Ray(__instance.player.look.aim.position, __instance.player.look.aim.forward),
                        ((ItemWeaponAsset) __instance.player.equipment.asset).range, RayMasks.DAMAGE_SERVER,
                        __instance.player);
                OnPlayerAttack?.Invoke(__instance.player, raycastInfo);
            }

            [HarmonyPatch(typeof(UseableGun), "jab")]
            [HarmonyPrefix]
            internal static void OnPlayerAttackInvoker2(UseableGun __instance)
            {
                var raycastInfo =
                    DamageTool.raycast(new Ray(__instance.player.look.aim.position, __instance.player.look.aim.forward),
                        2f, RayMasks.DAMAGE_SERVER, __instance.player);
                OnPlayerAttack?.Invoke(__instance.player, raycastInfo);
            }

            [HarmonyPatch(typeof(PlayerEquipment), "ReceiveEquip")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerChangedEquipmentInvoker(PlayerEquipment __instance, byte page, byte x,
                byte y, ref Guid newAssetGuid, ref byte newQuality, ref byte[] newState, ref NetId useableNetId)
            {
                var shouldAllow = true;
                OnPrePlayerChangedEquipment?.Invoke(__instance.player, page, x, y, ref newAssetGuid, ref newQuality,
                    ref newState, ref useableNetId, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerInventory), "ReceiveDropItem")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerDroppedItemInvoker(PlayerInventory __instance, byte page, byte x,
                byte y)
            {
                var shouldAllow = true;
                OnPrePlayerDroppedItem?.Invoke(__instance.player, page, x, y, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "doDamage")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerKilledInvoker(PlayerLife __instance, byte amount, ref Vector3 newRagdoll,
                ref EDeathCause newCause, ref ELimb newLimb, ref CSteamID newKiller, ref EPlayerKill kill,
                ref bool trackKill, ref ERagdollEffect newRagdollEffect, bool canCauseBleeding = true)
            {
                if (__instance.health <= amount)
                {
                    var shouldAllow = true;
                    OnPrePlayerKilled?.Invoke(__instance.player, ref newRagdoll, ref newCause, ref newLimb,
                        ref newKiller, ref kill, ref trackKill, ref newRagdollEffect, ref shouldAllow);
                    if (!shouldAllow)
                        return false;
                }

                OnPlayerDamaged?.Invoke(__instance.player, amount, newCause, newLimb, newKiller, kill);
                return true;
            }

            [HarmonyPatch(typeof(ResourceManager), "ReceiveResourceDead")]
            [HarmonyPrefix]
            internal static void OnPreResourceDeadInvoker(byte x, byte y, ushort index, ref Vector3 ragdoll)
            {
                var resourceSpawnpoint = LevelGround.trees[x, y]?[index];
                if (resourceSpawnpoint == null)
                    return;
                if (resourceSpawnpoint.asset.isForage)
                    OnResourceForaged?.Invoke(resourceSpawnpoint, ref ragdoll);
                if (resourceSpawnpoint.asset.hasDebris && !resourceSpawnpoint.asset.isForage)
                    OnResourceChopped?.Invoke(resourceSpawnpoint, ref ragdoll);
                if (!resourceSpawnpoint.asset.hasDebris && !resourceSpawnpoint.asset.isForage)
                    OnResourceMined?.Invoke(resourceSpawnpoint, ref ragdoll);
            }

            [HarmonyPatch(typeof(UseableGun), "ReceiveChangeFiremode")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerFiremodeChangedInvoker(UseableGun __instance, EFiremode newFiremode)
            {
                var player = __instance.player;
                if (player == null)
                    return true;
                var shouldAllow = true;
                OnPrePlayerFiremodeChanged?.Invoke(player, newFiremode, ref shouldAllow);
                if (!shouldAllow)
                    return false;
                OnPlayerFiremodeChanged?.Invoke(player, newFiremode);
                return true;
            }

            [HarmonyPatch(typeof(ResourceManager), "ReceiveForageRequest")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerForagedResourceInvoker(in ServerInvocationContext context, byte x, byte y,
                ushort index)
            {
                var resourceSpawnpoint = LevelGround.trees[x, y]?[index];
                if (resourceSpawnpoint == null)
                    return true;
                var player = context.GetPlayer();
                if (player == null)
                    return true;
                var shouldAllow = true;
                OnPrePlayerForagedResource?.Invoke(player, resourceSpawnpoint, ref shouldAllow);
                if (!shouldAllow)
                    return false;
                OnPlayerForagedResource?.Invoke(player, resourceSpawnpoint);
                return true;
            }

            [HarmonyPatch(typeof(StructureManager), "destroyStructure")]
            [HarmonyPatch(new[] {typeof(StructureDrop), typeof(byte), typeof(byte), typeof(Vector3)})]
            [HarmonyPrefix]
            internal static bool OnPreStructureDestroyedInvoker(StructureDrop structure, byte x, byte y,
                ref Vector3 ragdoll)
            {
                var shouldAllow = true;
                OnPreStructureDestroyed?.Invoke(structure, x, y, ref ragdoll, ref shouldAllow);
                if (!shouldAllow)
                    return false;
                OnStructureDestroyed?.Invoke(structure, x, y, ragdoll);
                return true;
            }

            [HarmonyPatch(typeof(VehicleManager), "ReceiveDestroySingleVehicle")]
            [HarmonyPrefix]
            internal static bool OnPrePreVehicleDestroyedInvoker(uint instanceID)
            {
                var vehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);
                if (vehicle == null)
                    return true;
                
                var flag = true;
                OnPrePreVehicleDestroyed?.Invoke(vehicle, ref flag);
                if (!flag)
                    return false;
                OnPreVehicleDestroyed?.Invoke(vehicle);
                return true;
            }

            [HarmonyPatch(typeof(VehicleManager), "ReceiveVehicleExploded")]
            [HarmonyPrefix]
            internal static bool OnPrePreVehicleExplodedInvoker(uint instanceID)
            {
                var vehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);

                var flag = true;
                OnPrePreVehicleExploded?.Invoke(vehicle, ref flag);
                if (!flag)
                    return false;
                OnPreVehicleExploded?.Invoke(vehicle);
                return true;
            }

            [HarmonyPatch(typeof(ZombieManager), "ReceiveZombieDead")]
            [HarmonyPrefix]
            internal static void OnZombieKilledInvoker(byte reference, ushort id, ref Vector3 newRagdoll,
                ref ERagdollEffect newRagdollEffect)
            {
                var region = ZombieManager.regions.ElementAtOrDefault(reference);
                var zombie = region?.zombies.ElementAtOrDefault(id);
                OnZombieKilled?.Invoke(zombie, region, ref newRagdoll, ref newRagdollEffect);
            }

            [HarmonyPatch(typeof(Zombie), "askDamage")]
            [HarmonyPrefix]
            internal static bool OnPreZombieKilledInvoker(Zombie __instance, ushort ___health, ushort amount,
                ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp, ref bool trackKill, ref bool dropLoot,
                ref EZombieStunOverride stunOverride, ref ERagdollEffect ragdollEffect)
            {
                if (___health <= amount)
                {
                    var shouldAllow = true;
                    OnPreZombieKilled?.Invoke(__instance, ref newRagdoll, ref kill, ref xp, ref trackKill, ref dropLoot,
                        ref stunOverride, ref ragdollEffect, ref shouldAllow);
                    if (!shouldAllow)
                        return false;
                }

                OnZombieDamaged?.Invoke(__instance, amount, kill, xp);
                return true;
            }

            [HarmonyPatch(typeof(ChatManager), "ReceiveChatRequest")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerChattedInvoker(in ServerInvocationContext context, byte flags, string text)
            {
                var callingPlayer = context.GetCallingPlayer();
                if (callingPlayer == null || callingPlayer.player == null)
                    return true;
                var shouldAllow = true;
                var chatMode = (EChatMode) (flags & 127);
                OnPrePlayerChatted?.Invoke(callingPlayer.player, ref chatMode, ref text, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ChatManager), "serverSendMessage")]
            [HarmonyPrefix]
            internal static bool OnPreServerMessageSentInvoker(ref string text, ref Color color,
                ref SteamPlayer fromPlayer, ref SteamPlayer toPlayer, ref EChatMode mode, ref string iconURL,
                ref bool useRichTextFormatting)
            {
                var shouldAllow = true;
                OnPreServerMessageSent?.Invoke(ref text, ref color, ref fromPlayer, ref toPlayer, ref mode, ref iconURL,
                    ref useRichTextFormatting, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerAnimator), "sendGesture")]
            [HarmonyPrefix]
            internal static bool OnPrePlayerChangedGestureInvoker(PlayerAnimator __instance, ref EPlayerGesture gesture,
                bool all)
            {
                var shouldAllow = true;
                OnPrePlayerChangedGesture?.Invoke(__instance.player, ref gesture, ref shouldAllow);
                return shouldAllow;
            }
        }

        #endregion
    }
}