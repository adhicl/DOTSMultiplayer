using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;

[BurstCompile]
public struct SendMessageCommand : IRpcCommand
{
	public FixedString64Bytes message;
}

[BurstCompile]
public struct GoInGameRpcCommand: IRpcCommand
{
	
}