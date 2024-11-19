using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct GoInGameServerSystem : ISystem
{
    private ComponentLookup<NetworkId> networkIdFromEntity;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CharacterSpawnConfig>();
        
        var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<ReceiveRpcCommandRequest, GoInGameRpcCommand>();
        state.RequireForUpdate(state.GetEntityQuery(builder));
        networkIdFromEntity = state.GetComponentLookup<NetworkId>(true);
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var newCharacterPrefab = SystemAPI.GetSingleton<CharacterSpawnConfig>().prefabEntity;
        
        state.EntityManager.GetName(newCharacterPrefab, out var prefabName);
        //var worldName = state.WorldUnmanaged.Name;
        
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
        networkIdFromEntity.Update(ref state);

        foreach (var (reqSrc, reqEntity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRpcCommand>().WithEntityAccess())
        {
            entityCommandBuffer.AddComponent<NetworkStreamInGame>(reqSrc.ValueRO.SourceConnection);
            var networkId = networkIdFromEntity[reqSrc.ValueRO.SourceConnection];

            //Debug.Log($"'{worldName}' setting connection '{networkId.Value}' to in game");
            
            var newEntity = entityCommandBuffer.Instantiate(newCharacterPrefab);
            entityCommandBuffer.SetName(newEntity, "Client");
            
            entityCommandBuffer.SetComponent(newEntity, new GhostOwner
            {
                NetworkId = networkId.Value,
            });
            entityCommandBuffer.AppendToBuffer(reqSrc.ValueRO.SourceConnection, new LinkedEntityGroup
            {
                Value = newEntity,
            });
            var newSpawnPosition = new float3(UnityEngine.Random.Range(-1f,1f), 0f, UnityEngine.Random.Range(-1f,1f));
            var newPosition = LocalTransform.FromPosition(newSpawnPosition);
            
            entityCommandBuffer.SetComponent(newEntity, newPosition);
            
            entityCommandBuffer.DestroyEntity(reqEntity);
        }
    
        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}