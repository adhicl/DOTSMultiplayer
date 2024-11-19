using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
public struct InputComponent : IInputComponentData
{
	public float3 MovementDirection;
	public bool Attack;
	public bool Jump;
}
