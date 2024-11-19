using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Entities.SystemAPI.ManagedAPI;

[UpdateAfter(typeof(TransformSystemGroup))]
[RequireMatchingQueriesForUpdate]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
public partial struct TransformGOSystem : ISystem
{
	public void OnCreate(ref SystemState state)
	{
		state.RequireForUpdate<PlayerPresentationGO>();
	}

	public void OnUpdate(ref SystemState state)
	{
		foreach((RefRO<LocalToWorld> LocalToWorld, UnityEngineComponent<Transform> transform) in SystemAPI.Query<RefRO<LocalToWorld>, SystemAPI.ManagedAPI.UnityEngineComponent<Transform>>())
		{
			transform.Value.position = LocalToWorld.ValueRO.Position;
			transform.Value.rotation = LocalToWorld.ValueRO.Rotation;
		}
	}
}
