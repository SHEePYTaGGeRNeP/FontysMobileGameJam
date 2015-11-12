
using UnityEngine;

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
			PhotonNetwork.logLevel = PhotonLogLevel.Full;
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

		public override void OnPhotonPlayerConnected(PhotonPlayer player)
		{

			if (PhotonNetwork.isMasterClient)
			{
				Debug.Log("Spawn");
				GameObject spawnedPlayer = PhotonNetwork.Instantiate("Roeier", Vector3.zero, Quaternion.identity, 0);
					Paddle paddleToAssign = this._boot.AssignPlayer(spawnedPlayer.GetComponent<PhotonRoeier>());

				photonView.RPC("AssignPaddle", player, spawnedPlayer.GetPhotonView().viewID, paddleToAssign.gameObject.GetPhotonView().viewID);

			}


		}

		[PunRPC]
		public void AssignPaddle(int playerID, int paddleID)
		{
			Debug.Log("assign paddle");
			GameObject myPlayer = PhotonView.Find(playerID).gameObject;

			Paddle myPaddle = PhotonView.Find(paddleID).GetComponent<Paddle>();
			myPlayer.GetComponent<PhotonRoeier>().Paddle = myPaddle;

		}


		public override void OnJoinedRoom()
		{
			if (this._boot == null)
				this._boot = GameObject.Find("Boat_Mobile_Roeien").GetComponent<Roeiboot>();
			if (this._boot.Paddles == null || this._boot.Paddles.Count == 0)
			{
				Debug.Log("No Paddles avaiable bro");
				return;
			}


		

			// ben ik de master client? 
			// dan spawn ik een gameObject
			// check welke paddle vrij is
			// stuur naar degene die joined de gespawnde gameobject + pos van paddle


			/*
			GameObject player = PhotonNetwork.Instantiate("Roeier", Vector3.zero, Quaternion.identity, 0);S
			player.GetComponent<PhotonRoeier>().Paddle = this._boot.AssignPlayer(player.GetComponent<PhotonRoeier>());
			player.transform.position = player.GetComponent<PhotonRoeier>().Paddle.transform.position;
			//monster.GetComponent<myThirdPersonController>().isControllable = true;
			//myPhotonView = monster.GetComponent<PhotonView>();*/
		}


		[PunRPC]
		public void AddForce(Vector3 paddle, float force)
		{
			this._boot.AddForce(paddle, force);
		}

		[PunRPC]
		public void RequestPaddle(PhotonRoeier roeier, PhotonMessageInfo info)
		{
			Paddle paddle = this._boot.AssignPlayer(roeier);
			this._photonView.RPC("SetPaddle", info.sender, paddle);
		}

		void SetPaddle()
		{

		}
	}
}
