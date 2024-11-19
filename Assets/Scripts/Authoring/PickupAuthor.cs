using Unity.Entities;
using UnityEngine;

public class PickupAuthor : MonoBehaviour
{
    public int value;

    public class Baker : Baker<PickupAuthor>
    {
        public override void Bake(PickupAuthor authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PickUpItem()
            {
                Value = authoring.value,
            });
        }
    }
}
