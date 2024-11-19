using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerPresentationAuthor : MonoBehaviour
{
	public GameObject Prefab;

	public class Baker : Baker<PlayerPresentationAuthor>
	{
		public override void Bake(PlayerPresentationAuthor authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.WorldSpace);

			PlayerPresentationGO prefab = new PlayerPresentationGO
			{
				Prefab = authoring.Prefab
			};

			AddComponentObject(entity, prefab);
		}
	}
}

