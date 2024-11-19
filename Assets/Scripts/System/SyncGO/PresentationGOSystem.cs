using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;
using Object = UnityEngine.Object;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[RequireMatchingQueriesForUpdate]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
public partial struct PresentationGOSystem : ISystem
{
	void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PlayerPresentationGO>();
	}

	void OnUpdate(ref SystemState state)
	{
		foreach(Entity entity in SystemAPI.QueryBuilder().WithAll<LocalToWorld, PlayerPresentationGO>().WithNone<PresentationGOCleanup>().Build().ToEntityArray(Allocator.Temp))
		{
			PlayerPresentationGO presentationGO = SystemAPI.ManagedAPI.GetComponent<PlayerPresentationGO>(entity);
			GameObject go = Object.Instantiate(presentationGO.Prefab);

			Component[] components = go.GetComponents(typeof(Component));
			foreach(Component component in components)
			{
				if (component != null)
				{
					state.EntityManager.AddComponentObject(entity, component);
				}
			}

			go.AddComponent<EntityGO>().AssignEntity(entity, state.World, state.EntityManager.IsComponentEnabled<GhostOwnerIsLocal>(entity));
			state.EntityManager.AddComponentData(entity, new PresentationGOCleanup {
				Instance = go
			});

			LocalToWorld localToWorld = SystemAPI.GetComponent<LocalToWorld>(entity);
			go.transform.position = localToWorld.Position;
			go.transform.rotation = localToWorld.Rotation;
		}
	}
}
