using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
public partial class InputSystem : SystemBase
{
	private InputControls _control;

	protected override void OnCreate()
	{
		_control = new InputControls();
		_control.Enable();
		
		var builder = new EntityQueryBuilder(Allocator.Temp);
		builder.WithAll<InputComponent>();
		RequireForUpdate(GetEntityQuery(builder));
	}

	protected override void OnUpdate()
	{
		if (Camera.main == null) return;
		
		Vector2 moveVector = _control.Player.Move.ReadValue<Vector2>();
		bool isJump = _control.Player.Jump.IsPressed();
		bool isAttack = _control.Player.Attack.IsPressed();

		foreach ((RefRW<InputComponent> input, RefRO<LocalTransform> trans) in SystemAPI.Query<RefRW<InputComponent>, RefRO<LocalTransform>>().WithAll<GhostOwnerIsLocal>())
		{
			Vector3 playerPos = new Vector3(trans.ValueRO.Position.x, trans.ValueRO.Position.y, trans.ValueRO.Position.z);
			Vector3 pointToLook = playerPos;
			
			Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(moveVector.x, moveVector.y, Camera.main.nearClipPlane));
			Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
			float rayLength;

			if (groundPlane.Raycast(cameraRay, out rayLength))
			{
				pointToLook = cameraRay.GetPoint(rayLength);
			}
			
			float3 dirPos = math.normalize(pointToLook - playerPos);
				
			input.ValueRW.MovementDirection = dirPos;
			input.ValueRW.Jump = isJump;
			input.ValueRW.Attack = isAttack;
		}
	}

	protected override void OnDestroy()
	{
		_control.Disable();
	}
}
