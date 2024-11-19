using System.ComponentModel;
using Unity.Entities;
using UnityEngine;

public class CharacterSpawnAuthor : MonoBehaviour
{
	public GameObject Prefab;
	public class Baker : Baker<CharacterSpawnAuthor>
	{
		public override void Bake(CharacterSpawnAuthor authoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.None);
			AddComponent(entity, new CharacterSpawnConfig
			{
				prefabEntity = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic)
			});
		}
	}
}