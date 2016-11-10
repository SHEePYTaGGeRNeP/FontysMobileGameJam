namespace Assets.Scripts.Networking
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;
    using Networking;

    public class PhotonManager : Photon.PunBehaviour
    {
        public static PhotonManager Instance;

        public bool Host;

        //public Roeiboot Boot { get; set; }

        public event EventHandler OnJoinedRoomEvent;
        public event EventHandler OnReceivedRoomListUpdateEvent;

        // [SerializeField]
        // private RoeiButtonHandler _roeiButtonHandler;
        [SerializeField]
        private LobbiesManager _lobbiesManager;

        /// <summary>PlayerID, PhotonRoeierViewID</summary>
        private readonly Dictionary<int, PhotonView> _playerRoeiers = new Dictionary<int, PhotonView>();


        // ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            Instance = this;
        }

        // ReSharper disable once UnusedMember.Local
        private void Start()
        {
            PhotonNetwork.logLevel = PhotonLogLevel.Informational;
            PhotonNetwork.ConnectUsingSettings("0.1");
        }

        // ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }

        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (!PhotonNetwork.isMasterClient) return;
            this.photonView.RPC("Disconnect", PhotonTargets.All, this._playerRoeiers[otherPlayer.ID].viewID);
            PhotonNetwork.Destroy(this._playerRoeiers[otherPlayer.ID]);
        }

        public override void OnConnectedToMaster()
        {
            //// when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
            //if (this._lobbiesManager == null)
            //{
            //    Debug.Log("Joining random room!");
            //    PhotonNetwork.JoinRandomRoom();
            //}
            //else
            //{
            //    //this._lobbiesManager.StartUpdating();
            //}
        }

        public void CreateRoom(string roomname, byte maxPlayers)
        {
            LogHelper.Log(typeof(PhotonManager), "Creating room " + roomname);
            this.Host = true;
            RoomOptions ro = new RoomOptions() { IsVisible = true, MaxPlayers = maxPlayers, IsOpen = true };
            PhotonNetwork.CreateRoom(roomname, ro, TypedLobby.Default);
        }

        public override void OnReceivedRoomListUpdate()
        {
            if (this.OnReceivedRoomListUpdateEvent != null)
                this.OnReceivedRoomListUpdateEvent.Invoke(null, null);
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joining random room!");
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            Debug.Log("Can't join random room - Creating room");
            this.Host = true;
            PhotonNetwork.CreateRoom(null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() : You Have Joined a Room : " + PhotonNetwork.room.name);
            if (PhotonNetwork.isMasterClient)
            {
                //GameObject boot = PhotonNetwork.Instantiate("Boat_Mobile_Roeien", this.transform.position, Quaternion.identity, 0);
                //this.Boot = boot.GetComponent<Roeiboot>();
                //PhotonNetwork.Instantiate("AI_Boat_Mobile_Roeien", this.transform.position - new Vector3(0, 0, 10f), Quaternion.identity, 0);
            }
            this.OnJoinedRoomReached(EventArgs.Empty);
        }
        protected virtual void OnJoinedRoomReached(EventArgs e)
        {
            EventHandler handler = this.OnJoinedRoomEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            Debug.Log("Player connected: " + player.ID);
            if (!PhotonNetwork.isMasterClient) return;
            //if (this.Boot.FreePaddles.Count == 0)
            //{
            //    // TODO: Send RPC to client.
            //    Debug.Log("No Paddles avaiable bro");
            //    return;
            //}
            GameObject spawnedPlayer = PhotonNetwork.Instantiate("NetworkCube", new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10)),
                Quaternion.identity, 0);
            spawnedPlayer.GetComponent<PhotonRoeier>().OwnerID = player.ID;

            //Paddle paddleToAssign = this.Boot.NextPaddle;
            //GameObject spawnedPlayer = PhotonNetwork.Instantiate("Roeier", paddleToAssign.transform.position, Quaternion.identity, 0);
            //spawnedPlayer.transform.SetParent(this.Boot.transform);
            //this.Boot.AssignPlayer(spawnedPlayer.GetComponent<PhotonRoeier>());
            //spawnedPlayer.transform.position = paddleToAssign.transform.position;
            //PhotonView tempPlayerView = spawnedPlayer.GetPhotonView();
            //PhotonView tempPaddleView = paddleToAssign.gameObject.GetPhotonView();

            //this._playerRoeiers.Add(player.ID, tempPlayerView);

            //this.photonView.RPC("AssignPaddle", player, tempPlayerView.viewID, tempPaddleView.viewID, (int)tempPaddleView.GetComponent<Paddle>().RowSide);
        }



        [PunRPC]
        public void UpdateBoatTransform(Vector3 pos, Vector3 rot)
        {
            //if (PhotonNetwork.isMasterClient) return;
            //this.Boot.transform.position = pos;
            //this.Boot.transform.eulerAngles = rot;
        }


        /// <summary>
        /// Called on connecting client
        /// </summary>
        [PunRPC]
        public void AssignPaddle(int roeierViewId, int paddleViewID, int rowside)
        {
            //Helpers.LogHelper.WriteErrorMessage(typeof(PhotonManager), "AssignPaddle", "Assigning paddle to player");
            //PhotonRoeier myPlayer = PhotonView.Find(roeierViewId).gameObject.GetComponent<PhotonRoeier>();
            //myPlayer.RoeierCamera.gameObject.SetActive(true);
            //myPlayer.LocalRoeier = true;
            //this._roeiButtonHandler.Roeier = myPlayer;
            //myPlayer.PaddleViewId = paddleViewID;
            //myPlayer.Side = (RowTiltController.RowSide)rowside;
        }


        [PunRPC]
        public void AddForce(int paddleViewId, float force)
        {
            //Paddle paddle = PhotonView.Find(paddleViewId).GetComponent<Paddle>();
            //this.Boot.AddForce(paddle.transform.position, force);
        }

        [PunRPC]
        public void Disconnect(int roeierViewId)
        {
            //Debug.Log(roeierViewId + " has left the game.");
            //PhotonRoeier leftPlayer = PhotonView.Find(roeierViewId).gameObject.GetComponent<PhotonRoeier>();
            //this.Boot.RemovePlayer(leftPlayer);
        }
    }
}
