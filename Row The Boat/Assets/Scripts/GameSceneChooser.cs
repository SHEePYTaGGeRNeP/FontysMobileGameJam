using System;
using Assets.Scripts.PhotonNetworking;
using UnityEngine;

namespace Assets.Scripts
{
    using UnityEngine.SceneManagement;

    internal class GameSceneChooser : MonoBehaviour
	{
        public void Start()
		{
			PhotonManager.Instance.OnJoinedRoomEvent += CheckSceneToLoad;
		}

		private static void CheckSceneToLoad(object sender, EventArgs e)
		{
		    Debug.Log("Checking scene to load...");
		    SceneManager.LoadScene(PhotonNetwork.isMasterClient ? "Game" : "PhotonNetworking");
		}
	}
}
