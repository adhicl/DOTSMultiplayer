using Controllers;
using Unity.Entities;
using UnityEngine;

    public class EntityGO : MonoBehaviour
    {
        
        public Entity entity;
        public World world;
        public bool isPlayerOwned = false;

        public void AssignEntity(Entity e, World w, bool isOwned)
        {
            entity = e;
            world = w;

            isPlayerOwned= isOwned;
            
            if (isOwned)
            {
                CameraController.Instance.SetCameraTarget(this.transform);    
            }
        }

        private void OnDestroy()
        {
            if (world == null) return;
            if (world.IsCreated && world.EntityManager.Exists(entity))
            {
                world.EntityManager.DestroyEntity(entity);
            }
        }
    }