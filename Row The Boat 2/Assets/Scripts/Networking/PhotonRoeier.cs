using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Networking
{
    class PhotonRoeier : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public int OwnerID = -1;

        private void Start()
        {
            if (this.OwnerID == -1)
                this.OwnerID = PhotonNetwork.player.ID;
            if (PhotonNetwork.player.ID == this.OwnerID)
                this.DoLocalPlayer();
            else
                this.DoNotLocalPlayer();
        }

        private void DoLocalPlayer()
        {
            this._camera.gameObject.SetActive(true);
        }
        private void DoNotLocalPlayer()
        {
            this._camera.gameObject.SetActive(false);
            GameObject.Destroy(this.GetComponent<TestMovementStuff>());

        }
    }
}
