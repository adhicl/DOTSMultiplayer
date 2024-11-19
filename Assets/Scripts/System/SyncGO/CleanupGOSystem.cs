using Unity.Entities;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation)]
partial struct CleanupGOSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer cmd = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach((PresentationGOCleanup cleanup, Entity entity) in SystemAPI.Query<PresentationGOCleanup>().WithNone<PlayerPresentationGO>().WithEntityAccess())
        {
            if (cleanup.Instance == null) continue;
            DestructionManager manager = cleanup.Instance.GetComponent<DestructionManager>();
            if (manager != null)
            {
                manager.Destroy();
            }
            else
            {
                GameObject.Destroy(cleanup.Instance.gameObject);
            }
            cmd.RemoveComponent<PresentationGOCleanup>(entity);
        }


    }
}
