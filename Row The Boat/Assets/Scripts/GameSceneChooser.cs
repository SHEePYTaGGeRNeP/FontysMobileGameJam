using System;
using Assets.Scripts.PhotonNetworking;
using UnityEngine;

namespace Assets.Scripts
{
	class GameSceneChooser : MonoBehaviour
	{
		[SerializeField]
		private PhotonManager _photonManager;


		void Start()
		{
			PhotonManager.Instance.OnJoinedRoomEvent += this.CheckSceneToLoad;
		}
		private void CheckSceneToLoad(object sender, EventArgs e)
		{
			Debug.Log("Checking scene to load...");
			if (PhotonNetwork.isMasterClient)
				Application.LoadLevel("Game");
			else
				Application.LoadLevel("PhotonNetworking");
		}

	}
}
