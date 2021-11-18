using System;
using System.Collections.Generic;
using System.Linq;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

#pragma warning disable 612

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class VehicleWrapper
    {
        public ushort Id { get; set; }
        public uint InstanceId { get; set; }
        public ushort SkinId { get; set; }
        public ushort MythicId { get; set; }
        public float RoadPosition { get; set; }
        public ushort Health { get; set; }
        public ushort Fuel { get; set; }
        public ushort BatteryCharge { get; set; }
        public ulong Owner { get; set; }
        public ulong Group { get; set; }
        public bool[] Tires { get; set; } = Array.Empty<bool>();
        public List<byte[]> Turrets { get; set; } = new();
        public ItemsWrapper TrunkItems { get; set; } = new();
        public List<BarricadeWrapper> Barricades { get; set; } = new();
        public Vector3Wrapper Position { get; set; }
        public QuartenionWrapper Rotation { get; set; }

        public VehicleWrapper()
        {
        }

        public VehicleWrapper(ushort id, uint instanceId, ushort skinId, ushort mythicId, float roadPosition,
            ushort health, ushort fuel, ushort batteryCharge, ulong owner, ulong @group, bool[] tires,
            List<byte[]> turrets, ItemsWrapper trunkItems, List<BarricadeWrapper> barricades, Vector3Wrapper position,
            QuartenionWrapper rotation)
        {
            Id = id;
            InstanceId = instanceId;
            SkinId = skinId;
            MythicId = mythicId;
            RoadPosition = roadPosition;
            Health = health;
            Fuel = fuel;
            BatteryCharge = batteryCharge;
            Owner = owner;
            Group = group;
            Tires = tires;
            Turrets = turrets;
            TrunkItems = trunkItems;
            Barricades = barricades;
            Position = position;
            Rotation = rotation;
        }

        public static VehicleWrapper Create(InteractableVehicle vehicle)
        {
            var vehicleTurret = new List<byte[]>();
            if (vehicle.turrets != null && vehicle.turrets?.Length != 0)
            {
                byte index = 0;
                while (index < vehicle.turrets?.Length)
                {
                    vehicleTurret.Add(vehicle.turrets[index].state);
                    index++;
                }
            }

            var result = new VehicleWrapper
            {
                Health = vehicle.health,
                Fuel = vehicle.fuel,
                BatteryCharge = vehicle.batteryCharge,
                Id = vehicle.id,
                TrunkItems = vehicle.trunkItems?.items != null && vehicle.trunkItems.items.Count != 0
                    ? ItemsWrapper.Create(vehicle.trunkItems)
                    : new ItemsWrapper(),
                Tires = vehicle.tires?.Select(c => c.isAlive).ToArray() ?? Array.Empty<bool>(),
                Turrets = vehicleTurret,
                Group = vehicle.lockedGroup.m_SteamID,
            };

            if (!BarricadeManager.tryGetPlant(vehicle.transform, out _, out _, out _, out var region))
                return result;
            foreach (var barricade in from barricadeDrop in region.drops
                where !barricadeDrop.GetServersideData().barricade.isDead
                select BarricadeWrapper.Create(barricadeDrop))
            {
                result.Barricades.Add(barricade);
            }

            return result;
        }

        public VehicleAsset GetVehicleAsset()
        {
            return (VehicleAsset) Assets.find(EAssetType.VEHICLE, Id);
        }

        public InteractableVehicle SpawnVehicle(UnturnedPlayer player, Vector3 position, Quaternion rotation,
            bool isGive = false)
        {
            // Spawn Vehicle
            var vehicle = VehicleManager.spawnLockedVehicleForPlayerV2(Id, position, rotation, player.Player);

            // Set Group
            VehicleManager.ServerSetVehicleLock(vehicle, !isGive ? new CSteamID(Owner) : player.CSteamID,
                !isGive ? new CSteamID(Group) : player.Player.quests.groupID, true);

            // Set Fuel
            VehicleManager.instance.channel.send("tellVehicleFuel", ESteamCall.ALL,
                ESteamPacket.UPDATE_UNRELIABLE_BUFFER, vehicle.instanceID, Fuel);
            // VehicleManager.sendVehicleFuel(vehicle, Fuel);
            vehicle.fuel = Fuel;

            // Set Health
            VehicleManager.sendVehicleHealth(vehicle, Health);
            vehicle.health = Health;

            // Set Battery Charge
            VehicleManager.sendVehicleBatteryCharge(vehicle, BatteryCharge);
            vehicle.batteryCharge = BatteryCharge;

            // Set Tires
            for (var i = 0; i < (Tires?.Length ?? 0); i++)
                if (Tires != null)
                    vehicle.tires[i].isAlive = Tires[i];
            vehicle.sendTireAliveMaskUpdate();

            // Spawn Trunk Items
            if (vehicle.trunkItems?.items != null && vehicle.trunkItems.items.Count != 0)
                foreach (var item in TrunkItems.Items)
                    vehicle.trunkItems.addItem(item.X, item.Y, item.Rotation, item.Item.ToItem());

            // Spawn Barricades
            if (Barricades.Count != 0)
                foreach (var barricade in Barricades.Where(barricade => barricade.Id != 0))
                    barricade.SpawnBarricade(player.CSteamID.m_SteamID, player.Player.quests.groupID.m_SteamID,
                        vehicle.transform);

            // Set Turrets
            if (vehicle.turrets != null && Turrets.Count == vehicle.turrets.Length)
            {
                byte index = 0;
                while (index < vehicle.turrets.Length)
                {
                    vehicle.turrets[index].state = Turrets[index];
                    index += 1;
                }
            }
            else
            {
                byte index = 0;
                while (index < vehicle.turrets?.Length)
                {
                    var vehicleAsset = (VehicleAsset) Assets.find(EAssetType.VEHICLE, Id);
                    var itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, vehicleAsset.turrets[index].itemID);
                    vehicle.turrets[index].state = itemAsset.getState();

                    index += 1;
                }
            }

            return vehicle;
        }

        public InteractableVehicle SpawnVehicle(ulong owner, ulong group)
        {
            // Spawn Vehicle
            var asset = GetVehicleAsset();
            var vehicle = VehicleManager.SpawnVehicleV3(asset, SkinId, MythicId, RoadPosition, Position.ToVector3(),
                Rotation.ToQuaternion(), false, false, false, false, asset.fuel, asset.health, 10000,
                new CSteamID(owner), new CSteamID(group), true, Array.Empty<byte[]>(), byte.MaxValue);

            // Set Group
            VehicleManager.ServerSetVehicleLock(vehicle, new CSteamID(owner), new CSteamID(group), true);

            // Set Fuel
            VehicleManager.instance.channel.send("tellVehicleFuel", ESteamCall.ALL,
                ESteamPacket.UPDATE_UNRELIABLE_BUFFER, vehicle.instanceID, Fuel);
            // VehicleManager.sendVehicleFuel(vehicle, Fuel);
            vehicle.fuel = Fuel;

            // Set Health
            VehicleManager.sendVehicleHealth(vehicle, Health);
            vehicle.health = Health;

            // Set Battery Charge
            VehicleManager.sendVehicleBatteryCharge(vehicle, BatteryCharge);
            vehicle.batteryCharge = BatteryCharge;

            // Set Tires
            for (var i = 0; i < (Tires?.Length ?? 0); i++)
                if (Tires != null)
                    vehicle.tires[i].isAlive = Tires[i];
            vehicle.sendTireAliveMaskUpdate();

            // Spawn Trunk Items
            if (vehicle.trunkItems?.items != null && vehicle.trunkItems.items.Count != 0)
                foreach (var item in TrunkItems.Items)
                    vehicle.trunkItems.addItem(item.X, item.Y, item.Rotation, item.Item.ToItem());

            // Spawn Barricades
            if (Barricades.Count != 0)
                foreach (var barricade in Barricades.Where(barricade => barricade.Id != 0))
                    barricade.SpawnBarricade(owner, group, vehicle.transform);

            // Set Turrets
            if (vehicle.turrets != null && Turrets.Count == vehicle.turrets.Length)
            {
                byte index = 0;
                while (index < vehicle.turrets.Length)
                {
                    vehicle.turrets[index].state = Turrets[index];
                    index += 1;
                }
            }
            else
            {
                byte index = 0;
                while (index < vehicle.turrets?.Length)
                {
                    var vehicleAsset = (VehicleAsset) Assets.find(EAssetType.VEHICLE, Id);
                    var itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, vehicleAsset.turrets[index].itemID);
                    vehicle.turrets[index].state = itemAsset.getState();

                    index += 1;
                }
            }

            return vehicle;
        }

        public InteractableVehicle SpawnVehicle()
        {
            // Spawn Vehicle
            var vehicle = VehicleManager.spawnVehicleV2(Id, Position.ToVector3(), Rotation.ToQuaternion());

            // Set Group
            VehicleManager.ServerSetVehicleLock(vehicle, new CSteamID(Owner), new CSteamID(Group), Owner != 0);

            // Set Fuel
            VehicleManager.instance.channel.send("tellVehicleFuel", ESteamCall.ALL,
                ESteamPacket.UPDATE_UNRELIABLE_BUFFER, vehicle.instanceID, Fuel);
            // VehicleManager.sendVehicleFuel(vehicle, Fuel);
            vehicle.fuel = Fuel;

            // Set Health
            VehicleManager.sendVehicleHealth(vehicle, Health);
            vehicle.health = Health;

            // Set Battery Charge
            VehicleManager.sendVehicleBatteryCharge(vehicle, BatteryCharge);
            vehicle.batteryCharge = BatteryCharge;

            // Set Tires
            for (var i = 0; i < (Tires?.Length ?? 0); i++)
                if (Tires != null)
                    vehicle.tires[i].isAlive = Tires[i];
            vehicle.sendTireAliveMaskUpdate();

            // Spawn Trunk Items
            if (vehicle.trunkItems?.items != null && vehicle.trunkItems.items.Count != 0)
                foreach (var item in TrunkItems.Items)
                    vehicle.trunkItems.addItem(item.X, item.Y, item.Rotation, item.Item.ToItem());

            // Spawn Barricades
            if (Barricades.Count != 0)
                foreach (var barricade in Barricades)
                    barricade.SpawnBarricade(vehicle.transform);

            // Set Turrets
            if (vehicle.turrets != null && Turrets.Count == vehicle.turrets.Length)
            {
                byte index = 0;
                while (index < vehicle.turrets.Length)
                {
                    vehicle.turrets[index].state = Turrets[index];
                    index += 1;
                }
            }
            else
            {
                byte index = 0;
                var vehicleAsset = GetVehicleAsset();
                while (index < vehicle.turrets?.Length)
                {
                    var itemAsset = (ItemAsset) Assets.find(EAssetType.ITEM, vehicleAsset.turrets[index].itemID);
                    vehicle.turrets[index].state = itemAsset.getState();

                    index++;
                }
            }

            return vehicle;
        }
    }
}