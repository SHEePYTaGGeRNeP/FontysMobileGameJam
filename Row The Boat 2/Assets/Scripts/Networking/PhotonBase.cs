namespace Assets.Scripts.Networking
{
    using UnityEngine;
    public class PhotonBase : MonoBehaviour
    {
        protected PhotonManager PhotonManager { get; private set; }

        protected virtual void Awake()
        {
            this.PhotonManager = FindObjectOfType<PhotonManager>();
        }
    }
}
