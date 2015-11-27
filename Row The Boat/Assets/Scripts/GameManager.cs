using UnityEngine;

namespace Assets.Scripts
{
    using System;

    using Assets.Scripts.MapGeneration;
    using Assets.Scripts.PhotonNetworking;

    public class GameManager : MonoBehaviour {

        [SerializeField]
        private PhotonManager _photonManager;
        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private BoatAI _boatAi;
        // Use this for initialization
        public void Start () {
	        this._photonManager.OnJoinedRoomEvent += this.PhotonManagerOnOnJoinedRoomEvent;
        }

        private void PhotonManagerOnOnJoinedRoomEvent(object sender, EventArgs eventArgs)
        {
            if (!PhotonNetwork.isMasterClient) return;
            this._mapGenerator.Generate();
            this._boatAi.gameObject.SetActive(true);
        }
    }
}
