using System;
using RFRocketLibrary.Utils;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class BarricadeWrapper
    {
        public ushort Id { get; set; }
        public uint InstanceId { get; set; }
        public ulong Owner { get; set; }
        public ulong Group { get; set; }
        public ushort Health { get; set; }
        public byte[] State { get; set; } = Array.Empty<byte>();
        public Vector3Wrapper Position { get; set; }
        public Vector3Wrapper Rotation { get; set; }

        public BarricadeWrapper()
        {
        }

        public BarricadeWrapper(ushort id, uint instanceId, ulong owner, ulong group, ushort health, byte[] state,
            Vector3Wrapper position, Vector3Wrapper rotation)
        {
            Id = id;
            InstanceId = instanceId;
            Owner = owner;
            Group = group;
            Health = health;
            State = state;
            Position = position;
            Rotation = rotation;
        }

        public static BarricadeWrapper Create(BarricadeDrop drop)
        {
            var data = drop.GetServersideData();
            return new BarricadeWrapper(data.barricade.id, drop.instanceID, data.owner, data.group,
                data.barricade.health, data.barricade.state, new Vector3Wrapper(data.point),
                new Vector3Wrapper(drop.model.transform.localEulerAngles));
        }

        public static BarricadeWrapper Create(BarricadeDrop drop, BarricadeData data)
        {
            return new BarricadeWrapper(data.barricade.id, drop.instanceID, data.owner, data.group,
                data.barricade.health, data.barricade.state, new Vector3Wrapper(data.point),
                new Vector3Wrapper(drop.model.transform.localEulerAngles));
        }

        public ItemBarricadeAsset GetItemBarricadeAsset()
        {
            return (ItemBarricadeAsset) Assets.find(EAssetType.ITEM, Id);
        }

        public Transform SpawnBarricade(Transform? hit = null)
        {
            var barricade = new Barricade(Id, Health, State, GetItemBarricadeAsset());
            BarricadeUtil.ChangeOwnerAndGroup(barricade.asset.build, Owner, Group, ref barricade.state);
            var spawnedBarricade = hit != null
                ? BarricadeManager.dropPlantedBarricade(hit, barricade, Position.ToVector3(),
                    Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), Owner, Group)
                : BarricadeManager.dropNonPlantedBarricade(barricade, Position.ToVector3(),
                    Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), Owner, Group);
            BarricadeManager.updateReplicatedState(spawnedBarricade, barricade.state, barricade.state.Length);
            BarricadeManager.changeOwnerAndGroup(spawnedBarricade, Owner, Group);
            return spawnedBarricade;
        }

        public Transform SpawnBarricade(ulong owner, ulong group, Transform? hit = null)
        {
            var barricade = new Barricade(Id, Health, State, GetItemBarricadeAsset());
            BarricadeUtil.ChangeOwnerAndGroup(barricade.asset.build, owner, group, ref barricade.state);
            var spawnedBarricade = hit != null
                ? BarricadeManager.dropPlantedBarricade(hit, barricade, Position.ToVector3(),
                    Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), owner, group)
                : BarricadeManager.dropNonPlantedBarricade(barricade, Position.ToVector3(),
                    Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), owner, group);
            BarricadeManager.updateReplicatedState(spawnedBarricade, barricade.state, barricade.state.Length);
            BarricadeManager.changeOwnerAndGroup(spawnedBarricade, owner, group);
            return spawnedBarricade;
        }

        public Transform SpawnBarricade(UnturnedPlayer player, Vector3 position, Quaternion rotation,
            Transform? hit = null, bool isGive = false)
        {
            var owner = !isGive ? Owner : player.CSteamID.m_SteamID;
            var group = !isGive ? Owner : player.CSteamID.m_SteamID;
            var barricade = new Barricade(Id, Health, State, GetItemBarricadeAsset());
            BarricadeUtil.ChangeOwnerAndGroup(barricade.asset.build, owner, group, ref barricade.state);
            var spawnedBarricade = hit != null
                ? BarricadeManager.dropPlantedBarricade(hit, barricade, position, rotation, owner, group)
                : BarricadeManager.dropNonPlantedBarricade(barricade, position, rotation, owner, group);
            BarricadeManager.updateReplicatedState(spawnedBarricade, barricade.state, barricade.state.Length);
            BarricadeManager.changeOwnerAndGroup(spawnedBarricade, owner, group);
            return spawnedBarricade;
        }
    }
}