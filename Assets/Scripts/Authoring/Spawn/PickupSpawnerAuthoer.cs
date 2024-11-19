using Authoring.Component;
using Unity.Entities;
using UnityEngine;

namespace Authoring.Spawn
{
    public class PickupSpawnerAuthoer : MonoBehaviour
    {
        public GameObject Prefab;
        public int SpawnCount;

        private class PickupSpawnerAuthoerBaker : Baker<PickupSpawnerAuthoer>
        {
            public override void Bake(PickupSpawnerAuthoer authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new PickupSpawnerConfig()
                {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                    SpawnCount = authoring.SpawnCount,
                });
            }
        }
    }
}