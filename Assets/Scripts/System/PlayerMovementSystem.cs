using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
[BurstCompile]
public partial struct PlayerMovementSystem : ISystem
{
	[BurstCompile]
	public void OnCreate(ref SystemState state)
	{
		var builder = new EntityQueryBuilder(Allocator.Temp);
		builder.WithAll<PlayerMovement, InputComponent, LocalTransform, PhysicsVelocity>();
		state.RequireForUpdate(state.GetEntityQuery(builder));
	}

	[BurstCompile]
	public void OnUpdate(ref SystemState state)
	{
		var job = new PlayerMovementJob
		{
			DeltaTime = SystemAPI.Time.DeltaTime,
		};
		state.Dependency = job.ScheduleParallel(state.Dependency);
		/*
		foreach (var (input, playerMove, trans) in SystemAPI.Query<RefRO<InputComponent>, RefRO<PlayerMovement>, RefRW<LocalTransform>>().WithAll<Simulate>())
		{
			float2 addMovement = input.ValueRO.Movement * playerMove.ValueRO.Speed * SystemAPI.Time.DeltaTime;
			trans.ValueRW.Position += new float3(addMovement.x, 0f, addMovement.y);
		}
		//*/
	}

}

[BurstCompile]
public partial struct PlayerMovementJob : IJobEntity
{
	public float DeltaTime;

	private void Execute(InputComponent input, PlayerMovement playerMove, ref LocalTransform trans, ref PhysicsVelocity velocity)
	{
		float angle = math.degrees(math.atan2(input.MovementDirection.x, input.MovementDirection.z));
		trans.Rotation = Quaternion.Euler(new Vector3(0, angle, 0));

		float3 addMovement = trans.Forward() * playerMove.Speed * DeltaTime;
		trans.Position += new float3(addMovement.x, 0f, addMovement.z);

		velocity.Angular.x = 0f;
		velocity.Angular.z = 0f;
	}
}
