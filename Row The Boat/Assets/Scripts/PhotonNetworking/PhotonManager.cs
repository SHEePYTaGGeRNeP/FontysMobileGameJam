
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PhotonNetworking
{
	class PhotonManager : Photon.PunBehaviour
	{
		public static PhotonManager Instance;

		private PhotonView _photonView;
		public bool Host;

		private Roeiboot _boot;
		public Roeiboot Boot { get { return this._boot; } }

		void Awake()
		{
			Instance = this;
			this._photonView = this.GetComponent<PhotonView>();
		}

		void Start()
		{
			this._boot = GameObject.Find("Boat_Mobile_Roeien").GetComponent<Roeiboot>();
			PhotonNetwork.logLevel = PhotonLogLevel.Informational;
			PhotonNetwork.ConnectUsingSettings("0.1");
		}

		void OnGUI()
		{
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		}
		public override void OnConnectedToMaster()
		{
			// when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
			Debug.Log("Joining random room!");
			PhotonNetwork.JoinRandomRoom();
		}
		public override void OnJoinedLobby()
		{
			Debug.Log("Joining random room!");
			PhotonNetwork.JoinRandomRoom();
		}
		void OnPhotonRandomJoinFailed()
		{
			Debug.Log("Can't join random room - Creating room");
			this.Host = true;
			PhotonNetwork.CreateRoom(null);
		}

		public override void OnJoinedRoom()
		{
			Debug.Log("OnJoinedRoom() : You Have Joined a Room : " + PhotonNetwork.room.name);
			GameObject.Find("MasterClient").GetComponent<Text>().text = "Master: " + PhotonNetwork.isMasterClient.ToString();
		}

		public override void OnPhotonPlayerConnected(PhotonPlayer player)
		{
			GameObject.Find("MasterClient").GetComponent<Text>().text = "Master: " + PhotonNetwork.isMasterClient.ToString();
			if (this.Host != PhotonNetwork.isMasterClient)
			{
				Debug.Log("WTF IS DIT");
				Debug.Log("WTF IS DIT1");
				Debug.Log("WTF IS DIT2");
				Debug.Log("WTF IS DIT3");
				Debug.Log("WTF IS DIT4");
				Debug.Log("WTF IS DIT");
			}
			if (PhotonNetwork.isMasterClient)
			{
				if (this._boot.Paddles == null || this._boot.Paddles.Count == 0)
				{
					// TODO: Send RPC to client.
					Debug.Log("No Paddles avaiable bro");
					return;
				}
				Debug.Log("Spawn");
				GameObject spawnedPlayer = PhotonNetwork.Instantiate("Roeier", Vector3.zero, Quaternion.identity, 0);
				spawnedPlayer.transform.SetParent(this._boot.transform);
				Paddle paddleToAssign = this._boot.AssignPlayer(spawnedPlayer.GetComponent<PhotonRoeier>());
				spawnedPlayer.transform.position = paddleToAssign.transform.position;
				PhotonView tempPlayerView = spawnedPlayer.GetPhotonView();
				PhotonView tempPaddleView = paddleToAssign.gameObject.GetPhotonView();
				photonView.RPC("AssignPaddle", player, tempPlayerView.viewID, tempPaddleView.viewID);
			}
			else
				GameObject.Find("MasterClient").GetComponent<Text>().text = "Master: False, but other client joined";


		}


		/// <summary>
		/// Called on connecting client
		/// </summary
		[PunRPC]
		public void AssignPaddle(int playerID, int paddleID)
		{
			Debug.Log("assign paddle");
			GameObject myPlayer = PhotonView.Find(playerID).gameObject;

			myPlayer.GetComponent<PhotonRoeier>().PaddleViewId = paddleID;
			GameObject.Find("Side").GetComponent<Text>().text = "Paddle ID: " + paddleID.ToString();
		}


		[PunRPC]
		public void AddForce(int paddleViewId, float force)
		{
			Debug.Log("Force on :" + paddleViewId);
			PhotonView view = PhotonView.Find(paddleViewId);
			Debug.Log("View: " + view.viewID);
			Paddle paddle = PhotonView.Find(paddleViewId).GetComponent<Paddle>();
			this._boot.AddForce(paddle.transform.position, force);
		}
	}
}
