using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Photon;
namespace Assets.Scripts.Networking
{
    class PhotonRoeier : PunBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private void Start()
        {
            if (this.photonView.isMine)
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
