using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DestructionManager : MonoBehaviour
{
	public float Delay;
	public UnityEvent Events;

	public void Destroy()
	{
		Events.Invoke();
		Destroy(gameObject, Delay);
	}
}