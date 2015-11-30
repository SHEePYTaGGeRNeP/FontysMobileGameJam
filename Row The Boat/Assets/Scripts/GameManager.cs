using UnityEngine;

namespace Assets.Scripts
{
    using System;

    using Assets.Scripts.MapGeneration;
    using Assets.Scripts.PhotonNetworking;

    using UnityEngine.UI;

    public class GameManager : MonoBehaviour {

        [SerializeField]
        private PhotonManager _photonManager;
        [SerializeField]
        private MapGenerator _mapGenerator;
        [SerializeField]
        private BoatAI _boatAi;
        [SerializeField]
        private Button _rowButton;

        // Use this for initialization
        public void Start () {
	        this._photonManager.OnJoinedRoomEvent += this.PhotonManagerOnOnJoinedRoomEvent;
        }

        private void PhotonManagerOnOnJoinedRoomEvent(object sender, EventArgs eventArgs)
        {
            if (!PhotonNetwork.isMasterClient) return;
            this._rowButton.gameObject.SetActive(false);
            this._mapGenerator.Generate();
            this._boatAi.gameObject.SetActive(true);
        }
    }
}
