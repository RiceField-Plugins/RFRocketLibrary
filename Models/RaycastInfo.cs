using HarmonyLib;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace RFRocketLibrary.Models
{
    public class RaycastInfo
    {
        public Player? Player;
        public InteractableVehicle? Vehicle;
        public Animal? Animal;
        public Zombie? Zombie;
        public BarricadeDrop? Barricade;
        public StructureDrop? Structure;
        public ResourceSpawnpoint? Resource;
        public LevelObject? Object;

        public static RaycastInfo FromPlayerLook(Player player, int masks, float distance = float.MaxValue)
        {
            var result = new RaycastInfo();
            var flag = PhysicsUtility.raycast(new Ray(player.look.aim.position, player.look.aim.forward),
                out var hit, distance, masks);
            if (!flag)
                goto Result;

            var transform = hit.collider != null ? hit.collider.transform : hit.transform;
            if (transform)
                goto Result;

            // Player
            var resPlayer = transform.GetComponentInParent<Player>();
            if (resPlayer != null)
            {
                result.Player = resPlayer;
                goto Result;
            }

            // Vehicle
            var root = transform.root;
            var vehicle = root.GetComponent<InteractableVehicle>();
            var vehicleRef = root.GetComponent<VehicleRef>();
            if (vehicle != null)
            {
                result.Vehicle = vehicle;
                goto Result;
            }

            if (vehicleRef != null)
            {
                result.Vehicle = vehicleRef.vehicle;
                goto Result;
            }

            // Animal
            var animal = transform.GetComponentInParent<Animal>();
            if (animal != null)
            {
                result.Animal = transform.GetComponentInParent<Animal>();
                goto Result;
            }

            // Zombie
            var zombie = transform.GetComponentInParent<Zombie>();
            if (zombie != null)
            {
                result.Zombie = transform.GetComponentInParent<Zombie>();
                goto Result;
            }

            // Barricade
            if (transform.CompareTag("Barricade"))
            {
                var reflect = Traverse.Create(typeof(BarricadeDrop));
                result.Barricade = reflect.Method("FindByRootFast").GetValue(transform) as BarricadeDrop;
                goto Result;
            }

            // Structure
            if (transform.CompareTag("Structure"))
            {
                var reflect = Traverse.Create(typeof(StructureDrop));
                result.Structure = reflect.Method("FindByRootFast").GetValue(transform) as StructureDrop;
                goto Result;
            }

            byte x, y;
            ushort index;

            // Resource
            if (transform.CompareTag("Resource"))
            {
                if (ResourceManager.tryGetRegion(transform, out x, out y, out index))
                    result.Resource = ResourceManager.getResourceSpawnpoint(x, y, index);
                goto Result;
            }

            // Object
            flag = ObjectManager.tryGetRegion(transform, out x, out y, out index);
            if (flag)
            {
                result.Object = ObjectManager.getObject(x, y, index);
                goto Result;
            }

            Result:
            return result;
        }
    }
}