using System;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    [Serializable]
    public class StructureWrapper
    {
        public ushort Id { get; set; }
        public uint InstanceId { get; set; }
        public ulong Owner { get; set; }
        public ulong Group { get; set; }
        public ushort Health { get; set; }
        public Vector3Wrapper Position { get; set; }
        public Vector3Wrapper Rotation { get; set; }

        public StructureWrapper()
        {
        }

        public StructureWrapper(ushort id, uint instanceId, ulong owner, ulong @group, ushort health,
            Vector3Wrapper position, Vector3Wrapper rotation)
        {
            Id = id;
            InstanceId = instanceId;
            Owner = owner;
            Group = @group;
            Health = health;
            Position = position;
            Rotation = rotation;
        }

        public static StructureWrapper Create(StructureDrop drop)
        {
            var data = drop.GetServersideData();
            var structure = new StructureWrapper
            {
                Id = data.structure.id,
                InstanceId = drop.instanceID,
                Position = new Vector3Wrapper(data.point),
                Rotation = new Vector3Wrapper(drop.model.transform.localEulerAngles),
                Owner = data.owner,
                Group = data.@group,
                Health = data.structure.health,
            };

            return structure;
        }

        public static StructureWrapper Create(StructureDrop drop, StructureData data)
        {
            var structure = new StructureWrapper
            {
                Id = data.structure.id,
                InstanceId = drop.instanceID,
                Position = new Vector3Wrapper(data.point),
                Rotation = new Vector3Wrapper(drop.model.transform.localEulerAngles),
                Owner = data.owner,
                Group = data.group,
                Health = data.structure.health,
            };

            return structure;
        }

        public ItemStructureAsset GetItemStructureAsset()
        {
            return (ItemStructureAsset) Assets.find(EAssetType.ITEM, Id);
        }

        public bool SpawnStructure()
        {
            var structure = new Structure(Id, Health, GetItemStructureAsset());
            return StructureManager.dropReplicatedStructure(structure, Position.ToVector3(),
                Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), Owner, Group);
        }

        public bool SpawnStructure(ulong owner, ulong group)
        {
            var structure = new Structure(Id, Health, GetItemStructureAsset());
            return StructureManager.dropReplicatedStructure(structure, Position.ToVector3(),
                Quaternion.Euler(Rotation.X, Rotation.Y, Rotation.Z), owner, group);
        }

        public bool SpawnStructure(UnturnedPlayer player, Vector3 position, Quaternion rotation, bool isGive = false)
        {
            var structure = new Structure(Id, Health, GetItemStructureAsset());
            return StructureManager.dropReplicatedStructure(structure, position, rotation, !isGive ? Owner : player.CSteamID.m_SteamID,
                !isGive ? Group : player.Player.quests.groupID.m_SteamID);
        }
    }
}