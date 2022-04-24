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
    internal static class UnturnedPatchEvent
    {
        #region Delegates

        public delegate void AnimalDamaged(Animal animal, ushort damage, EPlayerKill kill, uint xp);

        public delegate void AnimalKilled(Animal? animal, ref Vector3 ragdoll, ref ERagdollEffect ragdollEffect);

        public delegate void AnimalMovementChanged(Animal animal, Vector3 lastPosition);
        public delegate void PreAnimalMovementChanged(Animal animal, Vector3 lastPosition, ref bool shouldAllow);

        public delegate void BarricadeDestroyed(BarricadeDrop drop, byte x, byte y, ushort plant);

        public delegate void PlayerAttack(Player player);

        public delegate void PlayerDamaged(Player player, byte amount, EDeathCause newCause, ELimb newLimb,
            CSteamID newKiller, EPlayerKill kill);

        public delegate void PlayerFiremodeChanged(Player player, EFiremode newFiremode);

        public delegate void PlayerForagedResource(Player player, ResourceSpawnpoint resourceSpawnpoint);

        public delegate void PlayerMovementChanged(Player player, Vector3 lastPosition);
        public delegate void PrePlayerMovementChanged(Player player, Vector3 lastPosition, ref bool shouldAllow);

        public delegate void PreAnimalKilled(Animal animal, ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp,
            ref bool trackKill, ref bool dropLoot, ref ERagdollEffect ragdollEffect, ref bool shouldAllow);

        public delegate void PreAnimalSpawned(Animal animal, ref Vector3 position, ref byte angle,
            ref bool shouldAllow);
        public delegate void AnimalSpawned(Animal animal, Vector3 position, byte angle);

        public delegate void PreBarricadeDestroyed(BarricadeDrop drop, byte x, byte y, ushort plant,
            ref bool shouldAllow);

        public delegate void PreItemSpawned(byte x, byte y, ref ushort id, ref byte amount, ref byte quality,
            ref byte[] state, ref Vector3 point, uint instanceID, ref bool shouldAllow);

        public delegate void ItemSpawned(byte x, byte y, ushort id, byte amount, byte quality,
            byte[] state, Vector3 point, uint instanceID);

        public delegate void PrePlayerCaughtFish(Player player, ref bool giveFishReward);

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

        public delegate void PreVehicleDestroyed(InteractableVehicle vehicle, ref bool shouldAllow);

        public delegate void PreVehicleExploded(InteractableVehicle vehicle, ref bool shouldAllow);

        public delegate void PreResourceSpawned(ResourceSpawnpoint resourceSpawnpoint, ref bool shouldAllow);

        public delegate void ResourceSpawned(ResourceSpawnpoint resourceSpawnpoint);

        public delegate void PreServerSentMessage(ref string text, ref Color color, ref SteamPlayer fromPlayer,
            ref SteamPlayer toPlayer, ref EChatMode mode, ref string iconURL, ref bool useRichTextFormatting,
            ref bool shouldAllow);

        public delegate void PreStructureDestroyed(StructureDrop drop, byte x, byte y, ref Vector3 ragdoll,
            ref bool shouldAllow);

        public delegate void PreVehicleSpawned(VehicleSpawnpoint vehicleSpawnpoint, ref bool shouldAllow);

        public delegate void VehicleSpawned(VehicleSpawnpoint vehicleSpawnpoint, ref InteractableVehicle vehicle);

        public delegate void PreZombieKilled(Zombie zombie, ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp,
            ref bool trackKill, ref bool dropLoot, ref EZombieStunOverride zombieStunOverride,
            ref ERagdollEffect ragdollEffect, ref bool shouldAllow);

        public delegate void PreZombieSpawned(Zombie zombie, ref byte newType, ref byte newSpeciality,
            ref byte newShirt, ref byte newPants, ref byte newHat, ref byte newGear, ref Vector3 newPosition,
            ref byte newAngle, ref bool shouldAllow);

        public delegate void ZombieSpawned(Zombie zombie, byte newType, byte newSpeciality,
            byte newShirt, byte newPants, byte newHat, byte newGear, Vector3 newPosition,
            byte newAngle);

        public delegate void ResourceDead(ResourceSpawnpoint resourceSpawnpoint, ref Vector3 ragdoll);

        public delegate void StructureDestroyed(StructureDrop drop, byte x, byte y, Vector3 ragdoll);

        public delegate void PreVehicleMovementChanged(InteractableVehicle vehicle, Vector3 lastPosition,
            ref bool shouldAllow);

        public delegate void VehicleMovementChanged(InteractableVehicle vehicle, Vector3 lastPosition);

        public delegate void PreVehicleMovementChangedByPlayer(InteractableVehicle vehicle, Player? player,
            Vector3 lastPosition, ref bool shouldAllow);

        public delegate void VehicleMovementChangedByPlayer(InteractableVehicle vehicle, Player? player,
            Vector3 lastPosition);

        public delegate void ZombieDamaged(Zombie zombie, ushort damage, EPlayerKill kill, uint xp);

        public delegate void ZombieKilled(Zombie? zombie, ZombieRegion? zombieRegion, ref Vector3 ragdoll,
            ref ERagdollEffect ragdollEffect);

        public delegate void PreZombieMovementChanged(Zombie zombie, Vector3 lastPosition, ref bool shouldAllow);

        public delegate void ZombieMovementChanged(Zombie zombie, Vector3 lastPosition);

        #endregion

        #region Events

        // Events
        public static event AnimalDamaged? OnAnimalDamaged;
        public static event AnimalKilled? OnAnimalKilled;
        public static event AnimalMovementChanged? OnAnimalMovementChanged;
        public static event PreAnimalMovementChanged? OnPreAnimalMovementChanged;
        public static event BarricadeDestroyed? OnBarricadeDestroyed;

        public static event PlayerAttack? OnPlayerAttack;
        public static event PlayerDamaged? OnPlayerDamaged;
        public static event PlayerFiremodeChanged? OnPlayerFiremodeChanged;
        public static event PlayerForagedResource? OnPlayerForagedResource;
        public static event PlayerMovementChanged? OnPlayerMovementChanged;
        public static event PrePlayerMovementChanged? OnPrePlayerMovementChanged;
        public static event PreAnimalKilled? OnPreAnimalKilled;
        public static event PreAnimalSpawned? OnPreAnimalSpawned;
        public static event AnimalSpawned? OnAnimalSpawned;
        public static event PreBarricadeDestroyed? OnPreBarricadeDestroyed;
        public static event PreItemSpawned? OnPreItemSpawned;
        public static event ItemSpawned? OnItemSpawned;

        public static event PrePlayerCaughtFish? OnPrePlayerCaughtFish;
        public static event PrePlayerChangedEquipment? OnPrePlayerChangedEquipment;
        public static event PrePlayerChangedGesture? OnPrePlayerChangedGesture;
        public static event PrePlayerChatted? OnPrePlayerChatted;
        public static event PrePlayerDraggedItem? OnPrePlayerDraggedItem;
        public static event PrePlayerDroppedItem? OnPrePlayerDroppedItem;
        public static event PrePlayerFiremodeChanged? OnPrePlayerFiremodeChanged;
        public static event PrePlayerForagedResource? OnPrePlayerForagedResource;
        public static event PrePlayerKilled? OnPrePlayerKilled;
        public static event PrePlayerSwappedItem? OnPrePlayerSwappedItem;
        public static event PreVehicleDestroyed? OnPreVehicleDestroyed;
        public static event PreVehicleExploded? OnPreVehicleExploded;
        public static event PreResourceSpawned? OnPreResourceSpawned;
        public static event ResourceSpawned? OnResourceSpawned;
        public static event PreServerSentMessage? OnPreServerMessageSent;
        public static event PreStructureDestroyed? OnPreStructureDestroyed;
        public static event PreVehicleSpawned? OnPreVehicleSpawned;
        public static event VehicleSpawned? OnVehicleSpawned;
        public static event PreZombieKilled? OnPreZombieKilled;
        public static event PreZombieSpawned? OnPreZombieSpawned;
        public static event ZombieSpawned? OnZombieSpawned;
        public static event ResourceDead? OnResourceChopped;
        public static event ResourceDead? OnResourceForaged;
        public static event ResourceDead? OnResourceMined;
        public static event StructureDestroyed? OnStructureDestroyed;
        public static event PreVehicleMovementChanged? OnPreVehicleMovementChanged;
        public static event VehicleMovementChanged? OnVehicleMovementChanged;
        public static event PreVehicleMovementChangedByPlayer? OnPreVehicleMovementChangedByPlayer;
        public static event VehicleMovementChangedByPlayer? OnVehicleMovementChangedByPlayer;
        public static event ZombieDamaged? OnZombieDamaged;
        public static event ZombieKilled? OnZombieKilled;
        public static event PreZombieMovementChanged? OnPreZombieMovementChanged;
        public static event ZombieMovementChanged? OnZombieMovementChanged;

        #endregion

        #region Nested type: InternalPatches

        [HarmonyPatch]
        internal static class InternalPatches
        {
            [HarmonyCleanup]
            internal static Exception? Cleanup(Exception? ex, MethodBase original)
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
            internal static bool OnAnimalMovementChangedInvoker(Animal __instance, Vector3 ___lastUpdatePos)
            {
                var shouldAllow = true;
                OnPreAnimalMovementChanged?.Invoke(__instance, ___lastUpdatePos, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(Animal), "tick")]
            [HarmonyPostfix]
            internal static void OnAnimalMovementChangedInvoker(bool __runOriginal, Animal __instance, Vector3 ___lastUpdatePos)
            {
                if (!__runOriginal)
                    return;

                OnAnimalMovementChanged?.Invoke(__instance, ___lastUpdatePos);
            }

            [HarmonyPatch(typeof(PlayerMovement), "simulate", new Type[0])]
            [HarmonyPrefix]
            internal static bool OnPlayerMovementChangedInvoker(PlayerMovement __instance, Vector3 ___lastUpdatePos)
            {
                var shouldAllow = true;
                OnPrePlayerMovementChanged?.Invoke(__instance.player, ___lastUpdatePos, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerMovement), "simulate", new Type[0])]
            [HarmonyPostfix]
            internal static void OnPlayerMovementChangedInvoker(bool __runOriginal, PlayerMovement __instance, Vector3 ___lastUpdatePos)
            {
                if (!__runOriginal)
                    return;

                OnPlayerMovementChanged?.Invoke(__instance.player, ___lastUpdatePos);
            }

            [HarmonyPatch(typeof(AnimalManager), "sendAnimalAlive")]
            [HarmonyPrefix]
            internal static bool OnPreAnimalSpawnedInvoker(Animal animal, ref Vector3 newPosition, ref byte newAngle)
            {
                var shouldAllow = true;
                OnPreAnimalSpawned?.Invoke(animal, ref newPosition, ref newAngle, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(AnimalManager), "sendAnimalAlive")]
            [HarmonyPostfix]
            internal static void OnPreAnimalSpawnedInvoker(bool __runOriginal, Animal animal, Vector3 newPosition, byte newAngle)
            {
                if (!__runOriginal)
                    return;

                OnAnimalSpawned?.Invoke(animal, newPosition, newAngle);
            }

            [HarmonyPatch(typeof(ItemManager), "ReceiveItem")]
            [HarmonyPrefix]
            internal static bool OnPreItemSpawnedInvoker(byte x, byte y, ref ushort id, ref byte amount,
                ref byte quality,
                ref byte[] state, ref Vector3 point, uint instanceID)
            {
                var shouldAllow = true;
                OnPreItemSpawned?.Invoke(x, y, ref id, ref amount, ref quality, ref state, ref point, instanceID,
                    ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ItemManager), "ReceiveItem")]
            [HarmonyPostfix]
            internal static void OnPreItemSpawnedInvoker(bool __runOriginal, byte x, byte y, ushort id, byte amount,
                byte quality, byte[] state, Vector3 point, uint instanceID)
            {
                if (!__runOriginal)
                    return;

                OnItemSpawned?.Invoke(x, y, id, amount, quality, state, point, instanceID);
            }

            [HarmonyPatch(typeof(ResourceManager), "ServerSetResourceAlive")]
            [HarmonyPrefix]
            internal static bool OnPreResourceSpawnedInvoker(byte x, byte y, ushort index)
            {
                var shouldAllow = true;
                OnPreResourceSpawned?.Invoke(LevelGround.trees[x, y][index], ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ResourceManager), "ServerSetResourceAlive")]
            [HarmonyPostfix]
            internal static void OnPreResourceSpawnedInvoker(bool __runOriginal, byte x, byte y, ushort index)
            {
                if (!__runOriginal)
                    return;

                OnResourceSpawned?.Invoke(LevelGround.trees[x, y][index]);
            }

            [HarmonyPatch(typeof(VehicleManager), "addVehicleAtSpawn")]
            [HarmonyPrefix]
            internal static bool OnPreVehicleSpawnedInvoker(VehicleSpawnpoint spawn)
            {
                var shouldAllow = true;
                OnPreVehicleSpawned?.Invoke(spawn, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(VehicleManager), "addVehicleAtSpawn")]
            [HarmonyPostfix]
            internal static void OnPreVehicleSpawnedInvoker(bool __runOriginal, ref InteractableVehicle __result,
                VehicleSpawnpoint spawn)
            {
                if (!__runOriginal)
                    return;

                OnVehicleSpawned?.Invoke(spawn, ref __result);
            }

            [HarmonyPatch(typeof(ZombieManager), "sendZombieAlive")]
            [HarmonyPrefix]
            internal static bool OnPreZombieSpawnedInvoker(Zombie zombie, ref byte newType,
                ref byte newSpeciality, ref byte newShirt, ref byte newPants, ref byte newHat, ref byte newGear,
                ref Vector3 newPosition, ref byte newAngle)
            {
                var shouldAllow = true;
                OnPreZombieSpawned?.Invoke(zombie, ref newType, ref newSpeciality, ref newShirt, ref newPants,
                    ref newHat, ref newGear, ref newPosition, ref newAngle, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ZombieManager), "sendZombieAlive")]
            [HarmonyPostfix]
            internal static void OnPreZombieSpawnedInvoker(bool __runOriginal, Zombie zombie, byte newType,
                byte newSpeciality, byte newShirt, byte newPants, byte newHat, byte newGear,
                Vector3 newPosition, byte newAngle)
            {
                if (!__runOriginal)
                    return;

                OnZombieSpawned?.Invoke(zombie, newType, newSpeciality, newShirt, newPants,
                    newHat, newGear, newPosition, newAngle);
            }

            [HarmonyPatch(typeof(InteractableVehicle), "simulate", typeof(uint), typeof(int),
                typeof(bool), typeof(Vector3), typeof(Quaternion), typeof(float), typeof(float), typeof(int),
                typeof(float))]
            [HarmonyPrefix]
            internal static bool OnVehicleMovementChangedByPlayerInvoker(InteractableVehicle __instance,
                Vector3 ___lastUpdatedPos, uint simulation, int recov, bool inputStamina,
                Vector3 point, Quaternion angle, float newSpeed, float newPhysicsSpeed, int newTurn, float delta)
            {
                if (__instance.transform.position == ___lastUpdatedPos)
                    return true;

                var shouldAllow = true;
                OnPreVehicleMovementChangedByPlayer?.Invoke(__instance,
                    __instance.passengers.ElementAtOrDefault(0)?.player?.player, ___lastUpdatedPos, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(InteractableVehicle), "simulate", typeof(uint), typeof(int),
                typeof(bool), typeof(Vector3), typeof(Quaternion), typeof(float), typeof(float), typeof(int),
                typeof(float))]
            [HarmonyPostfix]
            internal static void OnVehicleMovementChangedByPlayerInvoker(bool __runOriginal,
                InteractableVehicle __instance,
                Vector3 ___lastUpdatedPos, uint simulation, int recov, bool inputStamina,
                Vector3 point, Quaternion angle, float newSpeed, float newPhysicsSpeed, int newTurn, float delta)
            {
                if (!__runOriginal)
                    return;

                if (__instance.transform.position == ___lastUpdatedPos)
                    return;

                OnVehicleMovementChangedByPlayer?.Invoke(__instance,
                    __instance.passengers.ElementAtOrDefault(0)?.player?.player, ___lastUpdatedPos);
            }

            // [HarmonyPatch(typeof(InteractableVehicle), "updateSafezoneStatus")]
            // [HarmonyPrefix]
            // internal static void OnVehicleMovementChangedInvoker(InteractableVehicle __instance,
            //     Vector3 ___lastUpdatedPos, float deltaSeconds)
            // {
            //     if (__instance.transform.position == ___lastUpdatedPos)
            //         return;
            //     
            //     var shouldAllow = true;
            //     OnPreVehicleMovementChanged?.Invoke(__instance, ___lastUpdatedPos, ref shouldAllow);
            //     return shouldAllow;
            // }

            [HarmonyPatch(typeof(InteractableVehicle), "updateSafezoneStatus")]
            [HarmonyPostfix]
            internal static void OnVehicleMovementChangedInvoker(bool __runOriginal, InteractableVehicle __instance,
                Vector3 ___lastUpdatedPos, float deltaSeconds)
            {
                if (!__runOriginal)
                    return;

                if (__instance.transform.position == ___lastUpdatedPos)
                    return;

                OnVehicleMovementChanged?.Invoke(__instance, ___lastUpdatedPos);
            }

            [HarmonyPatch(typeof(Zombie), "tick")]
            [HarmonyPrefix]
            internal static bool OnZombieMovementChangedInvoker(Zombie __instance, Vector3 ___lastUpdatedPos)
            {
                if (__instance.transform.position == ___lastUpdatedPos)
                    return true;

                var shouldAllow = true;
                OnPreZombieMovementChanged?.Invoke(__instance, ___lastUpdatedPos, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(Zombie), "tick")]
            [HarmonyPostfix]
            internal static void OnZombieMovementChangedInvoker(bool __runOriginal, Zombie __instance,
                Vector3 ___lastUpdatedPos)
            {
                if (!__runOriginal)
                    return;

                if (__instance.transform.position == ___lastUpdatedPos)
                    return;

                OnZombieMovementChanged?.Invoke(__instance, ___lastUpdatedPos);
            }

            [HarmonyPatch(typeof(AnimalManager), "ReceiveAnimalDead")]
            [HarmonyPostfix]
            internal static void OnAnimalKilledInvoker(bool __runOriginal, ushort index, ref Vector3 newRagdoll,
                ref ERagdollEffect newRagdollEffect)
            {
                if (!__runOriginal)
                    return;

                OnAnimalKilled?.Invoke(AnimalManager.animals.ElementAtOrDefault(index), ref newRagdoll,
                    ref newRagdollEffect);
            }

            [HarmonyPatch(typeof(Animal), "askDamage")]
            [HarmonyPrefix]
            internal static bool OnPreAnimalKilledInvoker(Animal __instance, ushort ___health, ushort amount,
                ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp, ref bool trackKill, ref bool dropLoot,
                ref ERagdollEffect ragdollEffect)
            {
                if (___health > amount)
                    return true;

                var shouldAllow = true;
                OnPreAnimalKilled?.Invoke(__instance, ref newRagdoll, ref kill, ref xp, ref trackKill, ref dropLoot,
                    ref ragdollEffect, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(Animal), "askDamage")]
            [HarmonyPostfix]
            internal static void OnPreAnimalKilledInvoker(bool __runOriginal, Animal __instance, ushort ___health,
                ushort amount,
                ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp, ref bool trackKill, ref bool dropLoot,
                ref ERagdollEffect ragdollEffect)
            {
                if (!__runOriginal)
                    return;

                OnAnimalDamaged?.Invoke(__instance, amount, kill, xp);
            }

            [HarmonyPatch(typeof(BarricadeManager), "destroyBarricade")]
            [HarmonyPatch(new[] {typeof(BarricadeDrop), typeof(byte), typeof(byte), typeof(ushort)})]
            [HarmonyPrefix]
            internal static bool OnPreBarricadeDestroyedInvoker(BarricadeDrop barricade, byte x, byte y, ushort plant)
            {
                var shouldAllow = true;
                OnPreBarricadeDestroyed?.Invoke(barricade, x, y, plant, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(BarricadeManager), "destroyBarricade")]
            [HarmonyPatch(new[] {typeof(BarricadeDrop), typeof(byte), typeof(byte), typeof(ushort)})]
            [HarmonyPostfix]
            internal static void OnPreBarricadeDestroyedInvoker(bool __runOriginal, BarricadeDrop barricade, byte x,
                byte y, ushort plant)
            {
                if (!__runOriginal)
                    return;

                OnBarricadeDestroyed?.Invoke(barricade, x, y, plant);
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

            [HarmonyPatch(typeof(PlayerInventory), nameof(PlayerInventory.forceAddItem), typeof(Item), typeof(bool))]
            [HarmonyPrefix]
            internal static bool ForceAddItem(PlayerInventory __instance, Item item, bool auto)
            {
                if (__instance.player.equipment.isBusy && auto == false && item.state.Length == 0)
                {
                    var giveFish = true;
                    OnPrePlayerCaughtFish?.Invoke(__instance.player, ref giveFish);
                    return giveFish;
                }

                return true;
            }

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
            [HarmonyPostfix]
            internal static void OnPlayerAttackInvoker(bool __runOriginal, UseableMelee __instance)
            {
                if (!__runOriginal)
                    return;

                OnPlayerAttack?.Invoke(__instance.player);
            }

            [HarmonyPatch(typeof(UseableGun), "jab")]
            [HarmonyPostfix]
            internal static void OnPlayerAttackInvoker2(bool __runOriginal, UseableGun __instance)
            {
                if (!__runOriginal)
                    return;

                OnPlayerAttack?.Invoke(__instance.player);
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
                if (__instance.health > amount)
                    return true;

                var shouldAllow = true;
                OnPrePlayerKilled?.Invoke(__instance.player, ref newRagdoll, ref newCause, ref newLimb,
                    ref newKiller, ref kill, ref trackKill, ref newRagdollEffect, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(PlayerLife), "doDamage")]
            [HarmonyPostfix]
            internal static void OnPrePlayerKilledInvoker(bool __runOriginal, PlayerLife __instance, byte amount,
                ref Vector3 newRagdoll,
                ref EDeathCause newCause, ref ELimb newLimb, ref CSteamID newKiller, ref EPlayerKill kill,
                ref bool trackKill, ref ERagdollEffect newRagdollEffect, bool canCauseBleeding = true)
            {
                if (!__runOriginal)
                    return;

                OnPlayerDamaged?.Invoke(__instance.player, amount, newCause, newLimb, newKiller, kill);
            }

            [HarmonyPatch(typeof(ResourceManager), "ReceiveResourceDead")]
            [HarmonyPostfix]
            internal static void OnPreResourceDeadInvoker(bool __runOriginal, byte x, byte y, ushort index,
                ref Vector3 ragdoll)
            {
                if (!__runOriginal)
                    return;

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
                return shouldAllow;
            }

            [HarmonyPatch(typeof(UseableGun), "ReceiveChangeFiremode")]
            [HarmonyPostfix]
            internal static void OnPrePlayerFiremodeChangedInvoker(bool __runOriginal, UseableGun __instance,
                EFiremode newFiremode)
            {
                var player = __instance.player;
                if (player == null)
                    return;

                OnPlayerFiremodeChanged?.Invoke(player, newFiremode);
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
                return shouldAllow;
            }

            [HarmonyPatch(typeof(ResourceManager), "ReceiveForageRequest")]
            [HarmonyPostfix]
            internal static void OnPrePlayerForagedResourceInvoker(bool __runOriginal,
                in ServerInvocationContext context, byte x, byte y,
                ushort index)
            {
                if (!__runOriginal)
                    return;

                var resourceSpawnpoint = LevelGround.trees[x, y]?[index];
                if (resourceSpawnpoint == null)
                    return;

                var player = context.GetPlayer();
                if (player == null)
                    return;

                OnPlayerForagedResource?.Invoke(player, resourceSpawnpoint);
            }

            [HarmonyPatch(typeof(StructureManager), "destroyStructure")]
            [HarmonyPatch(new[] {typeof(StructureDrop), typeof(byte), typeof(byte), typeof(Vector3)})]
            [HarmonyPrefix]
            internal static bool OnPreStructureDestroyedInvoker(StructureDrop structure, byte x, byte y,
                ref Vector3 ragdoll)
            {
                var shouldAllow = true;
                OnPreStructureDestroyed?.Invoke(structure, x, y, ref ragdoll, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(StructureManager), "destroyStructure")]
            [HarmonyPatch(new[] {typeof(StructureDrop), typeof(byte), typeof(byte), typeof(Vector3)})]
            [HarmonyPostfix]
            internal static void OnPreStructureDestroyedInvoker(bool __runOriginal, StructureDrop structure, byte x,
                byte y,
                ref Vector3 ragdoll)
            {
                if (!__runOriginal)
                    return;

                OnStructureDestroyed?.Invoke(structure, x, y, ragdoll);
            }

            [HarmonyPatch(typeof(VehicleManager), "ReceiveDestroySingleVehicle")]
            [HarmonyPrefix]
            internal static bool OnPreVehicleDestroyedInvoker(uint instanceID)
            {
                var vehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);
                if (vehicle == null)
                    return true;

                var flag = true;
                OnPreVehicleDestroyed?.Invoke(vehicle, ref flag);
                return flag;
            }

            [HarmonyPatch(typeof(VehicleManager), "ReceiveVehicleExploded")]
            [HarmonyPrefix]
            internal static bool OnPrePreVehicleExplodedInvoker(uint instanceID)
            {
                var vehicle = VehicleManager.findVehicleByNetInstanceID(instanceID);

                var flag = true;
                OnPreVehicleExploded?.Invoke(vehicle, ref flag);
                return flag;
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
                if (___health > amount)
                    return true;

                var shouldAllow = true;
                OnPreZombieKilled?.Invoke(__instance, ref newRagdoll, ref kill, ref xp, ref trackKill, ref dropLoot,
                    ref stunOverride, ref ragdollEffect, ref shouldAllow);
                return shouldAllow;
            }

            [HarmonyPatch(typeof(Zombie), "askDamage")]
            [HarmonyPostfix]
            internal static void OnPreZombieKilledInvoker(bool __runOriginal, Zombie __instance, ushort amount,
                Vector3 newRagdoll, EPlayerKill kill, uint xp, bool trackKill, bool dropLoot,
                EZombieStunOverride stunOverride, ERagdollEffect ragdollEffect)
            {
                if (!__runOriginal)
                    return;

                OnZombieDamaged?.Invoke(__instance, amount, kill, xp);
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