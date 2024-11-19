#if UNITY_EDITOR

using Unity.Entities;
using UnityEngine.SceneManagement;

namespace System
{
    public partial class LoadLobbyScene : SystemBase
    {
        protected override void OnCreate()
        {
            Enabled = false;
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) return;
            SceneManager.LoadScene(0);
        }
        
        protected override void OnUpdate()
        {
            
        }
    }
}

#endif