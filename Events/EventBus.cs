using System;
using System.Diagnostics;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Permissions;
using SDG.Unturned;

namespace RFRocketLibrary.Events
{
    internal static class EventBus
    {
        #region Methods

        internal static void Load()
        {
            if (Initialized)
                return;

            // Unturned Events
            BarricadeDrop.OnSalvageRequested_Global += UnturnedEvent.OnPreBarricadeSalvagedInvoker;
            BarricadeManager.onBarricadeSpawned += UnturnedEvent.OnBarricadeSpawnedInvoker;
            BarricadeManager.onDamageBarricadeRequested += UnturnedEvent.OnPreBarricadeDamagedInvoker;
            BarricadeManager.onDeployBarricadeRequested += UnturnedEvent.OnPreBarricadeSpawnedInvoker;
            BarricadeManager.onModifySignRequested += UnturnedEvent.OnPreSignModifiedInvoker;
            BarricadeManager.onOpenStorageRequested += UnturnedEvent.OnPreStorageOpenedInvoker;
            BarricadeManager.OnRepaired += UnturnedEvent.OnBarricadeRepairedInvoker;
            BarricadeManager.OnRepairRequested += UnturnedEvent.OnPreBarricadeRepairedInvoker;
            BarricadeManager.onTransformRequested += UnturnedEvent.OnPreBarricadeTransformedInvoker;
            ChatManager.onChatted += UnturnedEvent.OnPlayerChattedInvoker;
            ChatManager.onServerSendingMessage += UnturnedEvent.OnServerSentMessageInvoker;
            DamageTool.damageAnimalRequested += UnturnedEvent.OnPreAnimalDamagedInvoker;
            DamageTool.damagePlayerRequested += UnturnedEvent.OnPrePlayerDamagedInvoker;
            DamageTool.damageZombieRequested += UnturnedEvent.OnPreZombieDamagedInvoker;
            EffectManager.onEffectButtonClicked += UnturnedEvent.OnPlayerEffectButtonClickedInvoker;
            EffectManager.onEffectTextCommitted += UnturnedEvent.OnPlayerEffectInputFieldChangedInvoker;
            InteractableFarm.OnHarvestRequested_Global += UnturnedEvent.OnPrePlantHarvestedInvoker;
            ItemManager.onItemDropAdded += UnturnedEvent.OnItemSpawnedInvoker;
            ItemManager.onServerSpawningItemDrop += UnturnedEvent.OnPreItemSpawnedInvoker;
            ItemManager.onItemDropRemoved += UnturnedEvent.OnItemTakenInvoker;
            ItemManager.onTakeItemRequested += UnturnedEvent.OnPreItemTakenInvoker;
            Player.onPlayerStatIncremented += UnturnedEvent.OnPlayerStatIncrementedInvoker;
            PlayerAnimator.OnGestureChanged_Global += UnturnedEvent.OnPlayerChangedGestureInvoker;
            PlayerCrafting.onCraftBlueprintRequested += UnturnedEvent.OnPrePlayerItemCraftedInvoker;
            PlayerEquipment.OnInspectingUseable_Global += UnturnedEvent.OnPlayerInspectedEquipmentInvoker;
            PlayerEquipment.OnPunch_Global += UnturnedEvent.OnPlayerPunchedInvoker;
            PlayerEquipment.OnUseableChanged_Global += UnturnedEvent.OnPlayerChangedEquipmentInvoker;
            PlayerLife.OnPreDeath += UnturnedEvent.OnPrePlayerDiedInvoker;
            PlayerLife.onPlayerDied += UnturnedEvent.OnPlayerDiedInvoker;
            PlayerLife.OnSelectingRespawnPoint += UnturnedEvent.OnPrePlayerRespawnedInvoker;
            PlayerLife.OnTellBleeding_Global += UnturnedEvent.OnPlayerBledInvoker;
            PlayerLife.OnTellBroken_Global += UnturnedEvent.OnPlayerBrokeBoneInvoker;
            PlayerSkills.OnExperienceChanged_Global += UnturnedEvent.OnPlayerExperienceChangedInvoker;
            PlayerSkills.OnReputationChanged_Global += UnturnedEvent.OnPlayerReputationChangedInvoker;
            PlayerSkills.OnSkillUpgraded_Global += UnturnedEvent.OnPlayerSkillUpgradedInvoker;
            Provider.onCheckValidWithExplanation += UnturnedEvent.OnPrePlayerJoinedInvoker;
            UseableConsumeable.onConsumePerformed += UnturnedEvent.OnPlayerItemConsumedInvoker;
            UseableConsumeable.onConsumeRequested += UnturnedEvent.OnPrePlayerItemConsumedInvoker;
            UseableConsumeable.onPerformedAid += UnturnedEvent.OnPlayerAidedInvoker;
            UseableConsumeable.onPerformingAid += UnturnedEvent.OnPrePlayerAidedInvoker;
            UseableGun.onBulletHit += UnturnedEvent.OnPrePlayerGunHitEntityInvoker;
            UseableGun.onBulletSpawned += UnturnedEvent.OnPlayerGunFiredInvoker;
            UseableGun.OnReloading_Global += UnturnedEvent.OnPlayerReloadedGunInvoker;
            UseableThrowable.onThrowableSpawned += UnturnedEvent.OnPlayerThrewThrowableInvoker;
            ObjectManager.onDamageObjectRequested += UnturnedEvent.OnPreObjectDamagedInvoker;
            ObjectManager.OnQuestObjectUsed += UnturnedEvent.OnQuestObjectUsedInvoker;
            ResourceManager.onDamageResourceRequested += UnturnedEvent.OnPreResourceDamagedInvoker;
            StructureDrop.OnSalvageRequested_Global += UnturnedEvent.OnPreStructureSalvagedInvoker;
            StructureManager.onDamageStructureRequested += UnturnedEvent.OnPreStructureDamagedInvoker;
            StructureManager.onDeployStructureRequested += UnturnedEvent.OnPreStructureSpawnedInvoker;
            StructureManager.OnRepaired += UnturnedEvent.OnStructureRepairedInvoker;
            StructureManager.OnRepairRequested += UnturnedEvent.OnPreStructureRepairedInvoker;
            StructureManager.onStructureSpawned += UnturnedEvent.OnStructureSpawnedInvoker;
            StructureManager.onTransformRequested += UnturnedEvent.OnPreStructureTransformedInvoker;
            VehicleManager.OnPreDestroyVehicle += UnturnedEvent.OnVehicleDestroyedInvoker;
            VehicleManager.onVehicleCarjacked += UnturnedEvent.OnPrePlayerCarjackedVehicleInvoker;
            VehicleManager.OnVehicleExploded += UnturnedEvent.OnVehicleExplodedInvoker;
            VehicleManager.onVehicleLockpicked += UnturnedEvent.OnPrePlayerLockpickedVehicleInvoker;

            // Rocket Events
            R.OnRockedInitialized += RocketEvent.OnInitializedInvoker;
            U.Events.OnShutdown += RocketEvent.OnShutdownInvoker;
            R.Plugins.OnPluginsLoaded += RocketEvent.OnPluginLoadedInvoker;
            UnturnedPermissions.OnJoinRequested += RocketEvent.OnPrePlayerJoinedInvoker;
            R.Commands.OnExecuteCommand += RocketEvent.OnPreCommandExecutedInvoker;
            
            Initialized = true;
        }

        internal static void LoadHarmony()
        {
            try
            {
                if (InitializedHarmony)
                    return;
                
                var harmony = new HarmonyLib.Harmony(HarmonyId);
                harmony.PatchAll();

                InitializedHarmony = true;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] LoadHarmony: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
            }
        }

        internal static void Unload()
        {
            if (!Initialized)
                return;

            // Unturned Events
            BarricadeDrop.OnSalvageRequested_Global -= UnturnedEvent.OnPreBarricadeSalvagedInvoker;
            BarricadeManager.onBarricadeSpawned -= UnturnedEvent.OnBarricadeSpawnedInvoker;
            BarricadeManager.onDamageBarricadeRequested -= UnturnedEvent.OnPreBarricadeDamagedInvoker;
            BarricadeManager.onDeployBarricadeRequested -= UnturnedEvent.OnPreBarricadeSpawnedInvoker;
            BarricadeManager.onModifySignRequested -= UnturnedEvent.OnPreSignModifiedInvoker;
            BarricadeManager.onOpenStorageRequested -= UnturnedEvent.OnPreStorageOpenedInvoker;
            BarricadeManager.OnRepaired -= UnturnedEvent.OnBarricadeRepairedInvoker;
            BarricadeManager.OnRepairRequested -= UnturnedEvent.OnPreBarricadeRepairedInvoker;
            BarricadeManager.onTransformRequested -= UnturnedEvent.OnPreBarricadeTransformedInvoker;
            ChatManager.onChatted -= UnturnedEvent.OnPlayerChattedInvoker;
            ChatManager.onServerSendingMessage -= UnturnedEvent.OnServerSentMessageInvoker;
            DamageTool.damageAnimalRequested -= UnturnedEvent.OnPreAnimalDamagedInvoker;
            DamageTool.damagePlayerRequested -= UnturnedEvent.OnPrePlayerDamagedInvoker;
            DamageTool.damageZombieRequested -= UnturnedEvent.OnPreZombieDamagedInvoker;
            EffectManager.onEffectButtonClicked -= UnturnedEvent.OnPlayerEffectButtonClickedInvoker;
            EffectManager.onEffectTextCommitted -= UnturnedEvent.OnPlayerEffectInputFieldChangedInvoker;
            InteractableFarm.OnHarvestRequested_Global -= UnturnedEvent.OnPrePlantHarvestedInvoker;
            ItemManager.onItemDropAdded -= UnturnedEvent.OnItemSpawnedInvoker;
            ItemManager.onServerSpawningItemDrop -= UnturnedEvent.OnPreItemSpawnedInvoker;
            ItemManager.onItemDropRemoved -= UnturnedEvent.OnItemTakenInvoker;
            ItemManager.onTakeItemRequested -= UnturnedEvent.OnPreItemTakenInvoker;
            Player.onPlayerStatIncremented -= UnturnedEvent.OnPlayerStatIncrementedInvoker;
            PlayerAnimator.OnGestureChanged_Global -= UnturnedEvent.OnPlayerChangedGestureInvoker;
            PlayerCrafting.onCraftBlueprintRequested -= UnturnedEvent.OnPrePlayerItemCraftedInvoker;
            PlayerEquipment.OnInspectingUseable_Global -= UnturnedEvent.OnPlayerInspectedEquipmentInvoker;
            PlayerEquipment.OnPunch_Global -= UnturnedEvent.OnPlayerPunchedInvoker;
            PlayerEquipment.OnUseableChanged_Global -= UnturnedEvent.OnPlayerChangedEquipmentInvoker;
            PlayerLife.OnPreDeath -= UnturnedEvent.OnPrePlayerDiedInvoker;
            PlayerLife.onPlayerDied -= UnturnedEvent.OnPlayerDiedInvoker;
            PlayerLife.OnSelectingRespawnPoint -= UnturnedEvent.OnPrePlayerRespawnedInvoker;
            PlayerLife.OnTellBleeding_Global -= UnturnedEvent.OnPlayerBledInvoker;
            PlayerLife.OnTellBroken_Global -= UnturnedEvent.OnPlayerBrokeBoneInvoker;
            PlayerSkills.OnExperienceChanged_Global -= UnturnedEvent.OnPlayerExperienceChangedInvoker;
            PlayerSkills.OnReputationChanged_Global -= UnturnedEvent.OnPlayerReputationChangedInvoker;
            PlayerSkills.OnSkillUpgraded_Global -= UnturnedEvent.OnPlayerSkillUpgradedInvoker;
            Provider.onCheckValidWithExplanation -= UnturnedEvent.OnPrePlayerJoinedInvoker;
            UseableConsumeable.onConsumePerformed -= UnturnedEvent.OnPlayerItemConsumedInvoker;
            UseableConsumeable.onConsumeRequested -= UnturnedEvent.OnPrePlayerItemConsumedInvoker;
            UseableConsumeable.onPerformedAid -= UnturnedEvent.OnPlayerAidedInvoker;
            UseableConsumeable.onPerformingAid -= UnturnedEvent.OnPrePlayerAidedInvoker;
            UseableGun.onBulletHit -= UnturnedEvent.OnPrePlayerGunHitEntityInvoker;
            UseableGun.onBulletSpawned -= UnturnedEvent.OnPlayerGunFiredInvoker;
            UseableGun.OnReloading_Global -= UnturnedEvent.OnPlayerReloadedGunInvoker;
            UseableThrowable.onThrowableSpawned -= UnturnedEvent.OnPlayerThrewThrowableInvoker;
            ObjectManager.onDamageObjectRequested -= UnturnedEvent.OnPreObjectDamagedInvoker;
            ObjectManager.OnQuestObjectUsed -= UnturnedEvent.OnQuestObjectUsedInvoker;
            ResourceManager.onDamageResourceRequested -= UnturnedEvent.OnPreResourceDamagedInvoker;
            StructureDrop.OnSalvageRequested_Global -= UnturnedEvent.OnPreStructureSalvagedInvoker;
            StructureManager.onDamageStructureRequested -= UnturnedEvent.OnPreStructureDamagedInvoker;
            StructureManager.onDeployStructureRequested -= UnturnedEvent.OnPreStructureSpawnedInvoker;
            StructureManager.OnRepaired -= UnturnedEvent.OnStructureRepairedInvoker;
            StructureManager.OnRepairRequested -= UnturnedEvent.OnPreStructureRepairedInvoker;
            StructureManager.onStructureSpawned -= UnturnedEvent.OnStructureSpawnedInvoker;
            StructureManager.onTransformRequested -= UnturnedEvent.OnPreStructureTransformedInvoker;
            VehicleManager.OnPreDestroyVehicle -= UnturnedEvent.OnVehicleDestroyedInvoker;
            VehicleManager.onVehicleCarjacked -= UnturnedEvent.OnPrePlayerCarjackedVehicleInvoker;
            VehicleManager.OnVehicleExploded -= UnturnedEvent.OnVehicleExplodedInvoker;
            VehicleManager.onVehicleLockpicked -= UnturnedEvent.OnPrePlayerLockpickedVehicleInvoker;

            // Rocket Events
            R.OnRockedInitialized -= RocketEvent.OnInitializedInvoker;
            U.Events.OnShutdown -= RocketEvent.OnShutdownInvoker;
            R.Plugins.OnPluginsLoaded -= RocketEvent.OnPluginLoadedInvoker;
            UnturnedPermissions.OnJoinRequested -= RocketEvent.OnPrePlayerJoinedInvoker;
            R.Commands.OnExecuteCommand -= RocketEvent.OnPreCommandExecutedInvoker;
            
            Initialized = false;
        }

        internal static void UnloadHarmony()
        {
            if (!InitializedHarmony)
                return;
            
            try
            {
                var harmony = new HarmonyLib.Harmony(HarmonyId);
                harmony.UnpatchAll(HarmonyId);
        
                InitializedHarmony = false;
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] UnloadHarmony: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
            }
        }

        #endregion

        private const string HarmonyId = "RFRocketLibrary.Patches";
        private static bool Initialized { get; set; }
        private static bool InitializedHarmony { get; set; }
    }
}