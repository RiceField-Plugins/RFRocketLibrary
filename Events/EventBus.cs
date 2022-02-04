using System;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Permissions;
using SDG.Unturned;

namespace RFRocketLibrary.Events
{
    public static class EventBus
    {
        private static readonly string HarmonyId = "RFRocketLibrary.Patches";
        private static bool Initialized { get; set; }
        private static bool InitializedHarmony { get; set; }

        public static void Load()
        {
            if (Initialized)
                return;

            // Unturned Events
            DamageTool.damageAnimalRequested += UnturnedEvent.OnPreAnimalDamagedInvoker;
            BarricadeDrop.OnSalvageRequested_Global += UnturnedEvent.OnPreBarricadeSalvagedInvoker;
            BarricadeManager.onBarricadeSpawned += UnturnedEvent.OnBarricadeSpawnedInvoker;
            BarricadeManager.onDeployBarricadeRequested += UnturnedEvent.OnPreBarricadeSpawnedInvoker;
            BarricadeManager.OnRepaired += UnturnedEvent.OnBarricadeRepairedInvoker;
            BarricadeManager.OnRepairRequested += UnturnedEvent.OnPreBarricadeRepairedInvoker;
            BarricadeManager.onTransformRequested += UnturnedEvent.OnPreBarricadeTransformedInvoker;
            ItemManager.onItemDropAdded += UnturnedEvent.OnItemSpawnedInvoker;
            ItemManager.onServerSpawningItemDrop += UnturnedEvent.OnPreItemSpawnedInvoker;
            ItemManager.onItemDropRemoved += UnturnedEvent.OnItemTakenInvoker;
            ItemManager.onTakeItemRequested += UnturnedEvent.OnPreItemTakenInvoker;
            VehicleManager.onVehicleCarjacked += UnturnedEvent.OnPrePlayerCarjackedVehicleInvoker;
            VehicleManager.onVehicleLockpicked += UnturnedEvent.OnPrePlayerLockpickedVehicleInvoker;
            InteractableFarm.OnHarvestRequested_Global += UnturnedEvent.OnPrePlantHarvestedInvoker;
            UseableConsumeable.onPerformedAid += UnturnedEvent.OnPlayerAidedInvoker;
            UseableConsumeable.onPerformingAid += UnturnedEvent.OnPrePlayerAidedInvoker;
            PlayerLife.OnTellBleeding_Global += UnturnedEvent.OnPlayerBledInvoker;
            PlayerLife.OnTellBroken_Global += UnturnedEvent.OnPlayerBrokeBoneInvoker;
            PlayerEquipment.OnUseableChanged_Global += UnturnedEvent.OnPlayerChangedEquipmentInvoker;
            PlayerAnimator.OnGestureChanged_Global += UnturnedEvent.OnPlayerChangedGestureInvoker;
            ChatManager.onChatted += UnturnedEvent.OnPlayerChattedInvoker;
            DamageTool.damagePlayerRequested += UnturnedEvent.OnPrePlayerDamagedInvoker;
            PlayerLife.onPlayerDied += UnturnedEvent.OnPlayerDiedInvoker;
            PlayerLife.OnPreDeath += UnturnedEvent.OnPrePlayerDiedInvoker;
            EffectManager.onEffectButtonClicked += UnturnedEvent.OnPlayerEffectButtonClickedInvoker;
            EffectManager.onEffectTextCommitted += UnturnedEvent.OnPlayerEffectInputFieldChangedInvoker;
            PlayerSkills.OnExperienceChanged_Global += UnturnedEvent.OnPlayerExperienceChangedInvoker;
            UseableGun.onBulletSpawned += UnturnedEvent.OnPlayerGunFiredInvoker;
            UseableGun.onBulletHit += UnturnedEvent.OnPrePlayerGunHitEntityInvoker;
            PlayerEquipment.OnInspectingUseable_Global += UnturnedEvent.OnPlayerInspectedEquipmentInvoker;
            UseableConsumeable.onConsumePerformed += UnturnedEvent.OnPlayerItemConsumedInvoker;
            UseableConsumeable.onConsumeRequested += UnturnedEvent.OnPrePlayerItemConsumedInvoker;
            PlayerCrafting.onCraftBlueprintRequested += UnturnedEvent.OnPrePlayerItemCraftedInvoker;
            Provider.onCheckValidWithExplanation += UnturnedEvent.OnPrePlayerJoinedInvoker;
            PlayerLife.onPlayerDied += UnturnedEvent.OnPlayerKilledInvoker;
            PlayerEquipment.OnPunch_Global += UnturnedEvent.OnPlayerPunchedInvoker;
            UseableGun.OnReloading_Global += UnturnedEvent.OnPlayerReloadedGunInvoker;
            PlayerSkills.OnReputationChanged_Global += UnturnedEvent.OnPlayerReputationChangedInvoker;
            PlayerLife.OnSelectingRespawnPoint += UnturnedEvent.OnPrePlayerRespawnedInvoker;
            PlayerSkills.OnSkillUpgraded_Global += UnturnedEvent.OnPlayerSkillUpgradedInvoker;
            Player.onPlayerStatIncremented += UnturnedEvent.OnPlayerStatIncrementedInvoker;
            UseableThrowable.onThrowableSpawned += UnturnedEvent.OnPlayerThrewThrowableInvoker;
            ObjectManager.onDamageObjectRequested += UnturnedEvent.OnPreObjectDamagedInvoker;
            ResourceManager.onDamageResourceRequested += UnturnedEvent.OnPreResourceDamagedInvoker;
            ChatManager.onServerSendingMessage += UnturnedEvent.OnServerSentMessageInvoker;
            StructureDrop.OnSalvageRequested_Global += UnturnedEvent.OnPreStructureSalvagedInvoker;
            StructureManager.onStructureSpawned += UnturnedEvent.OnStructureSpawnedInvoker;
            StructureManager.onDeployStructureRequested += UnturnedEvent.OnPreStructureSpawnedInvoker;
            StructureManager.OnRepaired += UnturnedEvent.OnStructureRepairedInvoker;
            StructureManager.OnRepairRequested += UnturnedEvent.OnPreStructureRepairedInvoker;
            StructureManager.onTransformRequested += UnturnedEvent.OnPreStructureTransformedInvoker;
            VehicleManager.OnVehicleExploded += UnturnedEvent.OnVehicleExplodedInvoker;
            VehicleManager.OnPreDestroyVehicle += UnturnedEvent.OnVehicleDestroyedInvoker;
            DamageTool.damageZombieRequested += UnturnedEvent.OnPreZombieDamagedInvoker;

            // Rocket Events
            R.OnRockedInitialized += RocketEvent.OnInitializedInvoker;
            U.Events.OnShutdown += RocketEvent.OnShutdownInvoker;
            R.Plugins.OnPluginsLoaded += RocketEvent.OnPluginLoadedInvoker;
            UnturnedPermissions.OnJoinRequested += RocketEvent.OnPrePlayerJoinedInvoker;
            R.Commands.OnExecuteCommand += RocketEvent.OnPreCommandExecutedInvoker;
            
            // Harmony Events
            var harmony = new HarmonyLib.Harmony(HarmonyId);
            harmony.PatchAll();

            Initialized = true;
        }

        // public static void LoadHarmony()
        // {
        //     try
        //     {
        //         if (InitializedHarmony)
        //             return;
        //         var harmony = new HarmonyLib.Harmony(HarmonyId);
        //         harmony.PatchAll();
        //     }
        //     catch (Exception e)
        //     {
        //         var caller = Assembly.GetCallingAssembly().GetName().Name;
        //         Logger.LogError($"[{caller}] [ERROR] EventLoad: {e.Message}");
        //         Logger.LogError($"[{caller}] [ERROR] Details: {e}");
        //     }
        // }

        public static void Unload()
        {
            if (!Initialized)
                return;
            
            Initialized = false;
        }


        // public static void UnloadHarmony()
        // {
        //     if (InitializedHarmony)
        //         return;
        //     try
        //     {
        //         HarmonyLib.Harmony harmony = null;
        //         harmony.UnpatchAll(HarmonyId);
        //     }
        //     catch (Exception e)
        //     {
        //         var caller = Assembly.GetCallingAssembly().GetName().Name;
        //         Logger.LogError($"[{caller}] [ERROR] EventUnload: {e.Message}");
        //         Logger.LogError($"[{caller}] [ERROR] Details: {e}");
        //     }
        //
        //     InitializedHarmony = false;
        // }
    }
}