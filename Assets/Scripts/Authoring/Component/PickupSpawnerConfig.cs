using Unity.Entities;

namespace Authoring.Component
{
    public struct PickupSpawnerConfig : IComponentData
    {
        public Entity Prefab;
        public int SpawnCount;
    }
}