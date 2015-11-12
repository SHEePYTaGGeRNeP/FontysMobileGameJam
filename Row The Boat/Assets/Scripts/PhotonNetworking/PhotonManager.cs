
using UnityEngine;

namespace Assets.Scripts.PhotonNetworking
{
	class PhotonManager : Photon.PunBehaviour
	{
		public static PhotonManager Instance;


		public bool Host;

		private Roeiboot _boot;
		public Roeiboot Boot { get { return this._boot; } }

		void Awake()
		{
			Instance = this;
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
		public override void OnJoinedRoom()
		{
			if (this._boot == null)
				this._boot = GameObject.Find("Boat_Mobile_Roeien").GetComponent<Roeiboot>();
			if (this._boot.Paddles == null || this._boot.Paddles.Count == 0)
			{
				Debug.Log("No Paddles avaiable bro");
				return;
			}
			GameObject player = PhotonNetwork.Instantiate("Roeier", Vector3.zero, Quaternion.identity, 0);
			player.transform.position = new Vector3(1, 0, 0);
			player.GetComponent<PhotonRoeier>().Paddle = this._boot.AssignPlayer(player.GetComponent<PhotonRoeier>());
			player.transform.position = player.GetComponent<PhotonRoeier>().Paddle.transform.position;
			//monster.GetComponent<myThirdPersonController>().isControllable = true;
			//myPhotonView = monster.GetComponent<PhotonView>();
		}


		[PunRPC]
		public void AddForce(Vector3 paddle, float force)
		{
			if (!this.Host) return;
			this._boot.AddForce(paddle, force);
		}
	}
}
