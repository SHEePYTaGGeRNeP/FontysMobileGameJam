using UnityEngine;

namespace Assets.Scripts.Networking
{
    class LevelLoadSpawnPlayer : MonoBehaviour
    {

        private void Start()
        {
            PhotonManager manager = GameObject.FindObjectOfType<PhotonManager>();
            Debug.Log("manager " + manager != null);
            // later check for IsMaster
            if (manager != null)
                manager.LocalPlayerJoined();
        }

        
    }
}
