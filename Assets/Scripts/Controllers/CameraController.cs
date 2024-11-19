using Unity.Cinemachine;
using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }
        
        private void Awake() 
        { 
            // If there is an instance, and it's not me, delete myself.
    
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            } 
        }

        public void SetCameraTarget(Transform target)
        {
            CinemachineCamera camera = this.GetComponent<CinemachineCamera>();
            camera.Follow = target;
            camera.LookAt = target;
        }
    }
}