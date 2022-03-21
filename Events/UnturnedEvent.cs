﻿using System;
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
    public static class UnturnedEvent
    {
        // Delegates
        public delegate void AnimalDamaged(Animal animal, ushort damage, EPlayerKill kill, uint xp);

        public delegate void AnimalKilled(Animal? animal, ref Vector3 ragdoll, ref ERagdollEffect ragdollEffect);

        public delegate void PreAnimalKilled(Animal animal, ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp,
            ref bool trackKill, ref bool dropLoot, ref ERagdollEffect ragdollEffect, ref bool shouldAllow);

        public delegate void AnimalMovementChanged(Animal animal, Vector3 lastPosition);

        public delegate void PreAnimalSpawned(Animal animal, ref Vector3 position, ref byte angle,
            ref bool shouldAllow);

        public delegate void BarricadeDestroyed(BarricadeDrop drop, byte x, byte y, ushort plant);

        public delegate void PreBarricadeDestroyed(BarricadeDrop drop, byte x, byte y, ushort plant,
            ref bool shouldAllow);

        public delegate void PreItemDropSpawned(byte x, byte y, ref ushort id, ref byte amount, ref byte quality,
            ref byte[] state, ref Vector3 point, uint instanceID, ref bool shouldAllow);

        public delegate void PlayerAttack(Player player, RaycastInfo raycastInfo);

        public delegate void PlayerBled(Player player);

        public delegate void PlayerBrokeBone(Player player);

        public delegate void PlayerCaughtFish(Player player);

        // public delegate void PrePlayerCaughtFish(Player player, ref bool shouldCatch);

        public delegate void PlayerChangedEquipment(Player player);

        public delegate void PrePlayerChangedEquipment(Player player, byte page, byte x, byte y, ref Guid newAssetGuid,
            ref byte newQuality, ref byte[] newState, ref NetId useableNetId, ref bool shouldAllow);

        public delegate void PlayerChangedGesture(Player player, EPlayerGesture gesture);

        public delegate void PrePlayerChangedGesture(Player player, ref EPlayerGesture gesture, ref bool shouldAllow);

        public delegate void PlayerChatted(Player player, EChatMode mode, ref Color chatted, ref bool isRich,
            string text, ref bool isVisible);

        public delegate void PrePlayerChatted(Player player, ref EChatMode chatMode, ref string text,
            ref bool shouldAllow);

        public delegate void PlayerChoppedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            EDamageOrigin damageOrigin);

        public delegate void PrePlayerChoppedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            EDamageOrigin damageOrigin, ref bool shouldAllow);

        public delegate void PlayerDamaged(Player player, byte amount, EDeathCause newCause, ELimb newLimb,
            CSteamID newKiller, EPlayerKill kill);

        public delegate void PrePlayerDraggedItem(PlayerInventory inventory, byte page_0, byte x_0, byte y_0,
            byte page_1, byte x_1, byte y_1, byte rot_1, ref bool shouldAllow);

        public delegate void PrePlayerDroppedItem(Player inventory, byte page, byte x, byte y, ref bool shouldAllow);

        public delegate void PlayerExperienceChanged(Player player, uint oldExperience, uint newExperience);

        public delegate void PlayerFiremodeChanged(Player player, EFiremode newFiremode);

        public delegate void PrePlayerFiremodeChanged(Player player, EFiremode newFiremode, ref bool shouldAllow);

        public delegate void PlayerForagedResource(Player player, ResourceSpawnpoint resourceSpawnpoint);

        public delegate void PrePlayerForagedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            ref bool shouldAllow);

        public delegate void PlayerInspectedEquipment(Player player);

        public delegate void PrePlayerJoined(SteamPending steamPending, ref bool shouldAllow);

        public delegate void PrePlayerKilled(Player player, ref Vector3 newRagdoll, ref EDeathCause newCause,
            ref ELimb newLimb, ref CSteamID newKiller, ref EPlayerKill kill, ref bool trackKill,
            ref ERagdollEffect newRagdollEffect,
            ref bool shouldAllow);

        public delegate void PlayerMinedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            EDamageOrigin damageOrigin);

        public delegate void PrePlayerMinedResource(Player player, ResourceSpawnpoint resourceSpawnpoint,
            EDamageOrigin damageOrigin, ref bool shouldAllow);

        public delegate void PlayerMovementChanged(Player player, Vector3 lastPosition);

        public delegate void PlayerPunched(Player player, EPlayerPunch punch);

        public delegate void PlayerReloadedGun(UseableGun gun);

        public delegate void PlayerReputationChanged(Player player, int oldReputation, int newReputation);

        public delegate void PlayerSkillUpgraded(Player player, byte speciality, byte index, byte level);

        public delegate void PrePlayerSwappedItem(PlayerInventory inventory, byte page_0, byte x_0, byte y_0,
            byte rot_0, byte page_1, byte x_1, byte y_1, byte rot_1, ref bool shouldAllow);

        public delegate void PlayerTookItem(Player player);

        public delegate void PlayerWonArena(Player player);

        public delegate void ResourceDead(ResourceSpawnpoint resourceSpawnpoint, ref Vector3 ragdoll);

        public delegate void ResourceDamaged(CSteamID cSteamID, ResourceSpawnpoint resourceSpawnpoint, ushort damage,
            EDamageOrigin damageOrigin);

        public delegate void PreResourceSpawned(ResourceSpawnpoint resourceSpawnpoint, ref bool shouldAllow);

        public delegate void PreServerSentMessage(ref string text, ref Color color, ref SteamPlayer fromPlayer,
            ref SteamPlayer toPlayer, ref EChatMode mode, ref string iconURL, ref bool useRichTextFormatting,
            ref bool shouldAllow);

        public delegate void StructureDestroyed(StructureDrop drop, byte x, byte y, Vector3 ragdoll);

        public delegate void PreStructureDestroyed(StructureDrop drop, byte x, byte y, ref Vector3 ragdoll,
            ref bool shouldAllow);

        public delegate void VehicleDestroyed(InteractableVehicle vehicle);

        public delegate void PreVehicleDestroyed(InteractableVehicle vehicle);

        public delegate void PrePreVehicleDestroyed(InteractableVehicle vehicle, ref bool shouldAllow);

        public delegate void VehicleExploded(InteractableVehicle vehicle);

        public delegate void PreVehicleExploded(InteractableVehicle vehicle);

        public delegate void PrePreVehicleExploded(InteractableVehicle vehicle, ref bool shouldAllow);

        public delegate void VehicleMovementChangedByPlayer(InteractableVehicle vehicle, Player? player,
            Vector3 lastPosition);

        public delegate void VehicleMovementChanged(InteractableVehicle vehicle, Vector3 lastPosition);

        public delegate void PreVehicleSpawned(VehicleSpawnpoint vehicleSpawnpoint, ref bool shouldAllow);

        public delegate void ZombieDamaged(Zombie zombie, ushort damage, EPlayerKill kill, uint xp);

        public delegate void ZombieKilled(Zombie? zombie, ZombieRegion? zombieRegion, ref Vector3 ragdoll,
            ref ERagdollEffect ragdollEffect);

        public delegate void PreZombieKilled(Zombie zombie, ref Vector3 newRagdoll, ref EPlayerKill kill, ref uint xp,
            ref bool trackKill, ref bool dropLoot, ref EZombieStunOverride zombieStunOverride,
            ref ERagdollEffect ragdollEffect, ref bool shouldAllow);

        public delegate void ZombieMovementChanged(Zombie zombie, Vector3 lastPosition);

        public delegate void PreZombieSpawned(byte reference, ushort id, ref byte newType, ref byte newSpeciality,
            ref byte newShirt, ref byte newPants, ref byte newHat, ref byte newGear, ref Vector3 newPosition,
            ref byte newAngle, ref bool shouldAllow);

        // Events
        public static event AnimalDamaged? OnAnimalDamaged;
        public static event DamageTool.DamageAnimalHandler? OnPreAnimalDamaged;
        public static event AnimalKilled? OnAnimalKilled;
        public static event PreAnimalKilled? OnPreAnimalKilled;
        public static event AnimalMovementChanged? OnAnimalMovementChanged;
        public static event PreAnimalSpawned? OnPreAnimalSpawned;
        public static event BarricadeDestroyed? OnBarricadeDestroyed;
        public static event PreBarricadeDestroyed? OnPreBarricadeDestroyed;
        public static event BarricadeDrop.SalvageRequestHandler? OnPreBarricadeSalvaged;
        public static event BarricadeSpawnedHandler? OnBarricadeSpawned;
        public static event DeployBarricadeRequestHandler? OnPreBarricadeSpawned;
        public static event RepairedBarricadeHandler? OnBarricadeRepaired;
        public static event RepairBarricadeRequestHandler? OnPreBarricadeRepaired;
        public static event TransformBarricadeRequestHandler? OnPreBarricadeTransformed;
        public static event PreItemDropSpawned? OnPreItemDropSpawned;
        public static event ItemDropAdded? OnItemSpawned;
        public static event ServerSpawningItemDropHandler? OnPreItemSpawned;
        public static event ItemDropRemoved? OnItemTaken;
        public static event TakeItemRequestHandler? OnPrePlayerTookItem;
        public static event InteractableFarm.HarvestRequestHandler? OnPrePlantHarvested;
        public static event UseableConsumeable.PerformedAidHandler? OnPlayerAided;
        public static event UseableConsumeable.PerformingAidHandler? OnPrePlayerAided;
        public static event PlayerAttack? OnPlayerAttack;
        public static event PlayerBled? OnPlayerBled;
        public static event PlayerBrokeBone? OnPlayerBrokeBone;

        public static event PlayerCaughtFish? OnPlayerCaughtFish;

        // public static event PrePlayerCaughtFish? OnPrePlayerCaughtFish;
        public static event VehicleCarjackedSignature? OnPrePlayerCarjackedVehicle;
        public static event EffectManager.EffectTextCommittedHandler? OnPlayerChangedEffectInputField;
        public static event PlayerChangedEquipment? OnPlayerChangedEquipment;
        public static event PrePlayerChangedEquipment? OnPrePlayerChangedEquipment;
        public static event PlayerChangedGesture? OnPlayerChangedGesture;
        public static event PrePlayerChangedGesture? OnPrePlayerChangedGesture;
        public static event PlayerChatted? OnPlayerChatted;
        public static event EffectManager.EffectButtonClickedHandler? OnPlayerClickedEffectButton;
        public static event PrePlayerChatted? OnPrePlayerChatted;
        public static event PlayerChoppedResource? OnPlayerChoppedResource;
        public static event PrePlayerChoppedResource? OnPrePlayerChoppedResource;
        public static event UseableConsumeable.ConsumePerformedHandler? OnPlayerConsumedItem;
        public static event UseableConsumeable.ConsumeRequestedHandler? OnPrePlayerConsumedItem;
        public static event PlayerCraftingRequestHandler? OnPrePlayerCraftedItem;
        public static event PlayerDamaged? OnPlayerDamaged;
        public static event DamageTool.DamagePlayerHandler? OnPrePlayerDamaged;
        public static event Action<PlayerLife>? OnPrePlayerDied;
        public static event PlayerLife.PlayerDiedCallback? OnPlayerDied;
        public static event PrePlayerDraggedItem? OnPrePlayerDraggedItem;
        public static event PrePlayerDroppedItem? OnPrePlayerDroppedItem;
        public static event PlayerExperienceChanged? OnPlayerExperienceChanged;
        public static event PlayerFiremodeChanged? OnPlayerFiremodeChanged;
        public static event PrePlayerFiremodeChanged? OnPrePlayerFiremodeChanged;
        public static event PlayerForagedResource? OnPlayerForagedResource;
        public static event PrePlayerForagedResource? OnPrePlayerForagedResource;
        public static event UseableGun.BulletSpawnedHandler? OnPlayerGunFired;
        public static event UseableGun.BulletHitHandler? OnPrePlayerGunHitEntity;
        public static event PlayerInspectedEquipment? OnPlayerInspectedEquipment;
        public static event PrePlayerJoined? OnPrePlayerJoined;
        public static event PlayerLife.PlayerDiedCallback? OnPlayerKilled;
        public static event PrePlayerKilled? OnPrePlayerKilled;
        public static event VehicleLockpickedSignature? OnPrePlayerLockpickedVehicle;
        public static event PlayerMinedResource? OnPlayerMinedResource;
        public static event PrePlayerMinedResource? OnPrePlayerMinedResource;
        public static event PlayerMovementChanged? OnPlayerMovementChanged;
        public static event PlayerPunched? OnPlayerPunched;
        public static event PlayerReloadedGun? OnPlayerReloadedGun;
        public static event PlayerReputationChanged? OnPlayerReputationChanged;
        public static event PlayerLife.RespawnPointSelector? OnPrePlayerRespawned;
        public static event PlayerSkillUpgraded? OnPlayerSkillUpgraded;
        public static event PrePlayerSwappedItem? OnPrePlayerSwappedItem;
        public static event UseableThrowable.ThrowableSpawnedHandler? OnPlayerThrewThrowable;
        public static event PlayerTookItem? OnPlayerTookItem;
        public static event PlayerWonArena? OnPlayerWonArena;
        public static event DamageObjectRequestHandler? OnPreObjectDamaged;
        public static event ResourceDead? OnResourceChopped;
        public static event ResourceDead? OnResourceForaged;
        public static event ResourceDamaged? OnResourceDamaged;
        public static event DamageResourceRequestHandler? OnPreResourceDamaged;
        public static event ResourceDead? OnResourceMined;
        public static event PreResourceSpawned? OnPreResourceSpawned;
        public static event ServerSendingChatMessageHandler? OnServerMessageSent;
        public static event PreServerSentMessage? OnPreServerMessageSent;
        public static event StructureDestroyed? OnStructureDestroyed;
        public static event PreStructureDestroyed? OnPreStructureDestroyed;
        public static event StructureDrop.SalvageRequestHandler? OnPreStructureSalvaged;
        public static event StructureSpawnedHandler? OnStructureSpawned;
        public static event DeployStructureRequestHandler? OnPreStructureSpawned;
        public static event RepairedStructureHandler? OnStructureRepaired;
        public static event RepairStructureRequestHandler? OnPreStructureRepaired;
        public static event TransformStructureRequestHandler? OnPreStructureTransformed;
        public static event VehicleDestroyed? OnVehicleDestroyed;
        public static event PreVehicleDestroyed? OnPreVehicleDestroyed;
        public static event PrePreVehicleDestroyed? OnPrePreVehicleDestroyed;
        public static event VehicleExploded? OnVehicleExploded;
        public static event PreVehicleExploded? OnPreVehicleExploded;
        public static event PrePreVehicleExploded? OnPrePreVehicleExploded;
        public static event VehicleMovementChangedByPlayer? OnVehicleMovementChangedByPlayer;
        public static event VehicleMovementChanged? OnVehicleMovementChanged;
        public static event PreVehicleSpawned? OnPreVehicleSpawned;
        public static event ZombieDamaged? OnZombieDamaged;
        public static event ZombieMovementChanged? OnZombieMovementChanged;
        public static event DamageTool.DamageZombieHandler? OnPreZombieDamaged;
        public static event ZombieKilled? OnZombieKilled;
        public static event PreZombieKilled? OnPreZombieKilled;
        public static event PreZombieSpawned? OnPreZombieSpawned;

        // Invokers
        internal static void OnPreAnimalDamagedInvoker(ref DamageAnimalParameters parameters, ref bool shouldAllow)
        {
            OnPreAnimalDamaged?.Invoke(ref parameters, ref shouldAllow);
        }

        internal static void OnPreBarricadeSalvagedInvoker(BarricadeDrop barricade, SteamPlayer instigatorClient,
            ref bool shouldAllow)
        {
            OnPreBarricadeSalvaged?.Invoke(barricade, instigatorClient, ref shouldAllow);
        }

        internal static void OnBarricadeSpawnedInvoker(BarricadeRegion region, BarricadeDrop drop)
        {
            OnBarricadeSpawned?.Invoke(region, drop);
        }

        internal static void OnPreBarricadeSpawnedInvoker(Barricade barricade, ItemBarricadeAsset asset, Transform hit,
            ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner,
            ref ulong @group, ref bool shouldAllow)
        {
            OnPreBarricadeSpawned?.Invoke(barricade, asset, hit, ref point, ref angle_x, ref angle_y, ref angle_z,
                ref owner, ref group, ref shouldAllow);
        }

        internal static void OnPlayerEffectButtonClickedInvoker(Player player, string buttonName)
        {
            OnPlayerClickedEffectButton?.Invoke(player, buttonName);
        }

        internal static void OnPlayerEffectInputFieldChangedInvoker(Player player, string buttonName, string text)
        {
            OnPlayerChangedEffectInputField?.Invoke(player, buttonName, text);
        }

        internal static void OnPlayerItemConsumedInvoker(Player instigatingPlayer,
            ItemConsumeableAsset consumeableAsset)
        {
            OnPlayerConsumedItem?.Invoke(instigatingPlayer, consumeableAsset);
        }

        internal static void OnPrePlayerItemConsumedInvoker(Player instigatingPlayer,
            ItemConsumeableAsset consumeableAsset,
            ref bool shouldAllow)
        {
            OnPrePlayerConsumedItem?.Invoke(instigatingPlayer, consumeableAsset, ref shouldAllow);
        }

        internal static void OnPrePlayerItemCraftedInvoker(PlayerCrafting crafting, ref ushort itemId,
            ref byte blueprintIndex,
            ref bool shouldAllow)
        {
            OnPrePlayerCraftedItem?.Invoke(crafting, ref itemId, ref blueprintIndex, ref shouldAllow);
        }

        internal static void OnItemSpawnedInvoker(Transform model, InteractableItem interactableItem)
        {
            OnItemSpawned?.Invoke(model, interactableItem);
        }

        internal static void OnPreItemSpawnedInvoker(Item item, ref Vector3 location, ref bool shouldAllow)
        {
            OnPreItemSpawned?.Invoke(item, ref location, ref shouldAllow);
        }

        internal static void OnItemTakenInvoker(Transform model, InteractableItem interactableItem)
        {
            OnItemTaken?.Invoke(model, interactableItem);
        }

        internal static void OnPreItemTakenInvoker(Player player, byte x, byte y, uint instanceId, byte to_x, byte to_y,
            byte to_rot, byte to_page, ItemData itemData, ref bool shouldAllow)
        {
            OnPrePlayerTookItem?.Invoke(player, x, y, instanceId, to_x, to_y, to_rot, to_page, itemData,
                ref shouldAllow);
        }

        internal static void OnPrePlantHarvestedInvoker(InteractableFarm harvestable, SteamPlayer instigatorPlayer,
            ref bool shouldAllow)
        {
            OnPrePlantHarvested?.Invoke(harvestable, instigatorPlayer, ref shouldAllow);
        }

        internal static void OnPlayerAidedInvoker(Player instigator, Player target)
        {
            OnPlayerAided?.Invoke(instigator, target);
        }

        internal static void OnPrePlayerAidedInvoker(Player instigator, Player target, ItemConsumeableAsset asset,
            ref bool shouldAllow)
        {
            OnPrePlayerAided?.Invoke(instigator, target, asset, ref shouldAllow);
        }

        internal static void OnPlayerChattedInvoker(SteamPlayer player, EChatMode mode, ref Color chatted,
            ref bool isRich, string text, ref bool isVisible)
        {
            OnPlayerChatted?.Invoke(player.player, mode, ref chatted, ref isRich, text, ref isVisible);
        }

        internal static void OnPrePlayerDamagedInvoker(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            OnPrePlayerDamaged?.Invoke(ref parameters, ref shouldAllow);
        }

        internal static void OnPrePlayerDiedInvoker(PlayerLife life)
        {
            OnPrePlayerDied?.Invoke(life);
        }

        internal static void OnPrePlayerJoinedInvoker(ValidateAuthTicketResponse_t callback, ref bool isValid,
            ref string explanation)
        {
            OnPrePlayerJoined?.Invoke(callback.m_SteamID.GetSteamPending(), ref isValid);
        }

        internal static void OnPlayerKilledInvoker(PlayerLife sender, EDeathCause cause, ELimb limb,
            CSteamID instigator)
        {
            OnPlayerKilled?.Invoke(sender, cause, limb, instigator);
        }

        internal static void OnPlayerStatIncrementedInvoker(Player player, EPlayerStat stat)
        {
            switch (stat)
            {
                case EPlayerStat.ARENA_WINS:
                    OnPlayerWonArena?.Invoke(player);
                    break;
                case EPlayerStat.FOUND_FISHES:
                    OnPlayerCaughtFish?.Invoke(player);
                    break;
                case EPlayerStat.FOUND_ITEMS:
                    OnPlayerTookItem?.Invoke(player);
                    break;
            }
        }

        internal static void OnServerSentMessageInvoker(ref string text, ref Color color, SteamPlayer fromPlayer,
            SteamPlayer toPlayer, EChatMode mode, ref string iconURL, ref bool useRichTextFormatting)
        {
            OnServerMessageSent?.Invoke(ref text, ref color, fromPlayer, toPlayer, mode, ref iconURL,
                ref useRichTextFormatting);
        }

        internal static void OnPreStructureSalvagedInvoker(StructureDrop structure, SteamPlayer instigatorClient,
            ref bool shouldAllow)
        {
            OnPreStructureSalvaged?.Invoke(structure, instigatorClient, ref shouldAllow);
        }

        internal static void OnStructureSpawnedInvoker(StructureRegion region, StructureDrop drop)
        {
            OnStructureSpawned?.Invoke(region, drop);
        }

        internal static void OnPreStructureSpawnedInvoker(Structure structure, ItemStructureAsset asset,
            ref Vector3 point, ref float angle_x, ref float angle_y, ref float angle_z, ref ulong owner,
            ref ulong @group, ref bool shouldAllow)
        {
            OnPreStructureSpawned?.Invoke(structure, asset, ref point, ref angle_x, ref angle_y, ref angle_z, ref owner,
                ref group, ref shouldAllow);
        }

        internal static void OnPreZombieDamagedInvoker(ref DamageZombieParameters parameters, ref bool shouldAllow)
        {
            OnPreZombieDamaged?.Invoke(ref parameters, ref shouldAllow);
        }

        internal static void OnPreResourceDamagedInvoker(CSteamID instigatorSteamId, Transform transform,
            ref ushort pendingTotalDamage, ref bool shouldAllow, EDamageOrigin damageOrigin)
        {
            OnPreResourceDamaged?.Invoke(instigatorSteamId, transform, ref pendingTotalDamage, ref shouldAllow,
                damageOrigin);
            if (!shouldAllow)
                return;
            Regions.tryGetCoordinate(transform.position, out var x, out var y);
            var resource = LevelGround.trees[x, y]
                .FirstOrDefault(resourceSpawnpoint => resourceSpawnpoint.model == transform);
            if (resource == null)
                return;
            if (resource.health <= pendingTotalDamage)
            {
                var player = PlayerTool.getPlayer(instigatorSteamId);
                if (player != null)
                {
                    if (!resource.asset.isForage && resource.asset.hasDebris)
                    {
                        var shouldAllow2 = true;
                        OnPrePlayerChoppedResource?.Invoke(player, resource, damageOrigin, ref shouldAllow2);
                        if (!shouldAllow2)
                            return;
                        OnPlayerChoppedResource?.Invoke(player, resource, damageOrigin);
                    }

                    if (!resource.asset.isForage && !resource.asset.hasDebris)
                    {
                        var shouldAllow2 = true;
                        OnPrePlayerMinedResource?.Invoke(player, resource, damageOrigin, ref shouldAllow2);
                        if (!shouldAllow2)
                            return;
                        OnPlayerMinedResource?.Invoke(player, resource, damageOrigin);
                    }
                }
            }

            OnResourceDamaged?.Invoke(instigatorSteamId, resource, pendingTotalDamage, damageOrigin);
        }

        internal static void OnPlayerPunchedInvoker(PlayerEquipment equipment, EPlayerPunch punch)
        {
            OnPlayerPunched?.Invoke(equipment.player, punch);
        }

        internal static void OnPlayerChangedEquipmentInvoker(PlayerEquipment equipment)
        {
            OnPlayerChangedEquipment?.Invoke(equipment.player);
        }

        internal static void OnPlayerInspectedEquipmentInvoker(PlayerEquipment equipment)
        {
            OnPlayerInspectedEquipment?.Invoke(equipment.player);
        }

        [HarmonyPatch]
        internal static class InternalPatches
        {
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

            [HarmonyPatch(typeof(Animal), "tick")]
            [HarmonyPrefix]
            public static void OnAnimalMovementChangedInvoker(Animal __instance, Vector3 ___lastUpdatePos)
            {
                OnAnimalMovementChanged?.Invoke(__instance, ___lastUpdatePos);
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

            [HarmonyPatch(typeof(PlayerMovement), "simulate", new Type[0])]
            [HarmonyPrefix]
            public static void OnPlayerMovementChangedInvoker(PlayerMovement __instance, Vector3 ___lastUpdatePos)
            {
                OnPlayerMovementChanged?.Invoke(__instance.player, ___lastUpdatePos);
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

            [HarmonyPatch(typeof(AnimalManager), "ReceiveAnimalAlive")]
            [HarmonyPrefix]
            public static bool OnPreAnimalSpawnedInvoker(ushort index, ref Vector3 newPosition, ref byte newAngle)
            {
                var shouldAllow = true;
                OnPreAnimalSpawned?.Invoke(AnimalManager.animals.ElementAtOrDefault(index), ref newPosition,
                    ref newAngle, ref shouldAllow);
                if (!shouldAllow)
                    return false;
                return true;
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
        }

        internal static void OnPlayerExperienceChangedInvoker(PlayerSkills skills, uint oldExperience)
        {
            OnPlayerExperienceChanged?.Invoke(skills.player, oldExperience, skills.experience);
        }

        internal static void OnPlayerReputationChangedInvoker(PlayerSkills skills, int oldReputation)
        {
            OnPlayerReputationChanged?.Invoke(skills.player, oldReputation, skills.reputation);
        }

        internal static void OnPlayerSkillUpgradedInvoker(PlayerSkills skills, byte speciality, byte index, byte level)
        {
            OnPlayerSkillUpgraded?.Invoke(skills.player, speciality, index, level);
        }

        internal static void OnPlayerChangedGestureInvoker(PlayerAnimator animator, EPlayerGesture gesture)
        {
            OnPlayerChangedGesture?.Invoke(animator.player, gesture);
        }

        internal static void OnPrePlayerCarjackedVehicleInvoker(InteractableVehicle vehicle, Player instigatingPlayer,
            ref bool allow, ref Vector3 force, ref Vector3 torque)
        {
            OnPrePlayerCarjackedVehicle?.Invoke(vehicle, instigatingPlayer, ref allow, ref force, ref torque);
        }

        internal static void OnPrePlayerLockpickedVehicleInvoker(InteractableVehicle vehicle, Player instigatingPlayer,
            ref bool allow)
        {
            OnPrePlayerLockpickedVehicle?.Invoke(vehicle, instigatingPlayer, ref allow);
        }

        internal static void OnPlayerBledInvoker(PlayerLife life)
        {
            OnPlayerBled?.Invoke(life.player);
        }

        internal static void OnPlayerBrokeBoneInvoker(PlayerLife life)
        {
            OnPlayerBrokeBone?.Invoke(life.player);
        }

        internal static void OnPlayerReloadedGunInvoker(UseableGun gun)
        {
            OnPlayerReloadedGun?.Invoke(gun);
        }

        internal static void OnPrePlayerGunHitEntityInvoker(UseableGun gun, BulletInfo bullet, InputInfo hit,
            ref bool shouldAllow)
        {
            OnPrePlayerGunHitEntity?.Invoke(gun, bullet, hit, ref shouldAllow);
        }

        internal static void OnPlayerThrewThrowableInvoker(UseableThrowable useable, GameObject throwable)
        {
            OnPlayerThrewThrowable?.Invoke(useable, throwable);
        }

        internal static void OnPlayerGunFiredInvoker(UseableGun gun, BulletInfo bullet)
        {
            OnPlayerGunFired?.Invoke(gun, bullet);
        }

        internal static void OnPlayerDiedInvoker(PlayerLife sender, EDeathCause cause, ELimb limb, CSteamID instigator)
        {
            OnPlayerDied?.Invoke(sender, cause, limb, instigator);
        }

        internal static void OnVehicleDestroyedInvoker(InteractableVehicle vehicle)
        {
            OnVehicleDestroyed?.Invoke(vehicle);
        }

        internal static void OnVehicleExplodedInvoker(InteractableVehicle vehicle)
        {
            OnVehicleExploded?.Invoke(vehicle);
        }

        public static void OnBarricadeRepairedInvoker(CSteamID instigatorsteamid, Transform barricadetransform,
            float totalhealing)
        {
            OnBarricadeRepaired?.Invoke(instigatorsteamid, barricadetransform, totalhealing);
        }

        public static void OnPreBarricadeRepairedInvoker(CSteamID instigatorsteamid, Transform barricadetransform,
            ref float pendingtotalhealing, ref bool shouldallow)
        {
            OnPreBarricadeRepaired?.Invoke(instigatorsteamid, barricadetransform, ref pendingtotalhealing,
                ref shouldallow);
        }

        public static void OnStructureRepairedInvoker(CSteamID instigatorsteamid, Transform structuretransform,
            float totalhealing)
        {
            OnStructureRepaired?.Invoke(instigatorsteamid, structuretransform, totalhealing);
        }

        public static void OnPreStructureRepairedInvoker(CSteamID instigatorsteamid, Transform structuretransform,
            ref float pendingtotalhealing, ref bool shouldallow)
        {
            OnPreStructureRepaired?.Invoke(instigatorsteamid, structuretransform, ref pendingtotalhealing,
                ref shouldallow);
        }

        public static void OnPreBarricadeTransformedInvoker(CSteamID instigator, byte x, byte y, ushort plant,
            uint instanceid, ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z,
            ref bool shouldallow)
        {
            OnPreBarricadeTransformed?.Invoke(instigator, x, y, plant, instanceid, ref point, ref angle_x, ref angle_y,
                ref angle_z, ref shouldallow);
        }

        public static void OnPreStructureTransformedInvoker(CSteamID instigator, byte x, byte y, uint instanceid,
            ref Vector3 point, ref byte angle_x, ref byte angle_y, ref byte angle_z, ref bool shouldallow)
        {
            OnPreStructureTransformed?.Invoke(instigator, x, y, instanceid, ref point, ref angle_x, ref angle_y,
                ref angle_z, ref shouldallow);
        }

        public static void OnPrePlayerRespawnedInvoker(PlayerLife sender, bool wantstospawnathome, ref Vector3 position,
            ref float yaw)
        {
            OnPrePlayerRespawned?.Invoke(sender, wantstospawnathome, ref position, ref yaw);
        }

        public static void OnPreObjectDamagedInvoker(CSteamID instigatorsteamid, Transform objecttransform,
            byte section, ref ushort pendingtotaldamage, ref bool shouldallow, EDamageOrigin damageorigin)
        {
            OnPreObjectDamaged?.Invoke(instigatorsteamid, objecttransform, section, ref pendingtotaldamage,
                ref shouldallow, damageorigin);
        }
    }
}