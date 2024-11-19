using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;
using Unity.Physics.Systems;
using Unity.Assertions;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace System.Connection.Client
{
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(StatefulTriggerEventBufferSystem))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial class PickupTriggerEventSystem : SystemBase
    {
        private EndFixedStepSimulationEntityCommandBufferSystem m_CommandBufferSystem;
        private EntityQuery m_NonTriggerQuery;
        private EntityQueryMask m_NonTriggerMask;
        
        protected override void OnCreate()
        {
            m_CommandBufferSystem = World.GetOrCreateSystemManaged<EndFixedStepSimulationEntityCommandBufferSystem>();
            EntityQueryBuilder builder = new EntityQueryBuilder(Unity.Collections.Allocator.Temp)
                .WithAll<PlayerMovement>()
                .WithAll<LocalTransform>()
                .WithNone<StatefulTriggerEvent>();
            m_NonTriggerQuery = GetEntityQuery(builder);
            Assert.IsFalse(m_NonTriggerQuery.HasFilter(), "The use of EntityQueryMask in this system will not respect the query's active filter settings.");
            m_NonTriggerMask = m_NonTriggerQuery.GetEntityQueryMask();

            RequireForUpdate<PickUpItem>();
            
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer commandBuffer = m_CommandBufferSystem.CreateCommandBuffer();

            // Need this extra variable here so that it can
            // be captured by Entities.ForEach loop below
            var nonTriggerMask = m_NonTriggerMask;

            //ComponentLookup<MaterialMeshInfo> materialMeshInfoFromEntity = GetComponentLookup<MaterialMeshInfo>();

            foreach (var (triggerEventBuffer, pickup, entity) in SystemAPI
                         .Query<DynamicBuffer<StatefulTriggerEvent>, RefRW<PickUpItem>>()
                         .WithEntityAccess())
            {
                for (int i = 0; i < triggerEventBuffer.Length; i++)
                {
                    var triggerEvent = triggerEventBuffer[i];
                    var otherEntity = triggerEvent.GetOtherEntity(entity);

                    // exclude other triggers and processed events
                    if (triggerEvent.State == StatefulEventState.Stay ||
                        !nonTriggerMask.MatchesIgnoreFilter(otherEntity))
                    {
                        continue;
                    }

                    if (triggerEvent.State == StatefulEventState.Enter)
                    {
                        //move position when hit
                        var newPosition = LocalTransform.FromPosition(new float3(0f, 0f, -10f));
                        commandBuffer.SetComponent(entity, newPosition);
                    }
                }
            }

            m_CommandBufferSystem.AddJobHandleForProducer(Dependency);
        }


        protected override void OnDestroy()
        {

        }
    }
}