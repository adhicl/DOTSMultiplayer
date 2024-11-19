using Unity.Entities;
using UnityEngine;

public class PlayerAuthor : MonoBehaviour
{
    public float Speed;

    public class Baker : Baker<PlayerAuthor>
    {
        public override void Bake(PlayerAuthor authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerMovement
            {
                Speed = authoring.Speed,
            });
            AddComponent<InputComponent>(entity);
        }
    }
}
