using Authoring.Component;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace System
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct PickupSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PickupSpawnerConfig>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            
            var pickupSpawnerConfig = SystemAPI.GetSingleton<PickupSpawnerConfig>();
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            int totalCreate = pickupSpawnerConfig.SpawnCount;
            var pickupPrefab = pickupSpawnerConfig.Prefab;

            for (int i = 0; i < totalCreate; i++)
            {
                var newEntity = entityCommandBuffer.Instantiate(pickupPrefab);
                entityCommandBuffer.SetName(newEntity, "Pickup");
                var newSpawnPosition = new float3(UnityEngine.Random.Range(-80f,80f), 1f, UnityEngine.Random.Range(-60f,60f));
            
                entityCommandBuffer.SetComponent(newEntity, new LocalTransform
                {
                    Position = newSpawnPosition,
                    Scale = 0.5f,
                    Rotation = Quaternion.identity,
                });
            }
            entityCommandBuffer.Playback(state.EntityManager);
            entityCommandBuffer.Dispose();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}