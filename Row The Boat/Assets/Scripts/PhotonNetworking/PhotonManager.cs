﻿
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PhotonNetworking
{
    using System.Collections.Generic;

    class PhotonManager : Photon.PunBehaviour
    {
        public static PhotonManager Instance;       

        private PhotonView _photonView;
        public bool Host;

        private Roeiboot _boot;
        public Roeiboot Boot { get { return this._boot; } set { this._boot = value; } }

        public event EventHandler OnJoinedRoomEvent;

        [SerializeField]
        private RoeiButtonHandler _roeiButtonHandler;
        [SerializeField]
        private LobbiesManager _lobbiesManager;


        private bool _aPlayerHasJoined;


        private void Awake()
        {
            Instance = this;
            this._photonView = this.GetComponent<PhotonView>();
        }

        private void Start()
        {
            PhotonNetwork.logLevel = PhotonLogLevel.Informational;
            PhotonNetwork.ConnectUsingSettings("0.1");
        }

        private void OnGUI()
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }

        public override void OnConnectedToMaster()
        {
            // when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
            if (this._lobbiesManager == null)
            {
                Debug.Log("Joining random room!");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                this._lobbiesManager.StartUpdating();
            }
        }

        public void CreateRoom(string roomname)
        {
            Debug.Log("Creating room " + roomname);
            this.Host = true;
            RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 5 };
            PhotonNetwork.CreateRoom(roomname, ro, TypedLobby.Default);
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
            if (PhotonNetwork.isMasterClient)
            {
                GameObject boot = PhotonNetwork.Instantiate("Boat_Mobile_Roeien", this.transform.position, Quaternion.identity, 0);
                this._boot = boot.GetComponent<Roeiboot>();
                PhotonNetwork.Instantiate("AI_Boat_Mobile_Roeien", this.transform.position - new Vector3(0, 0, 10f), Quaternion.identity, 0);
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
            Debug.Log("Player connected");
            if (!PhotonNetwork.isMasterClient) return;
            this.Invoke("WaitForFirstPlayer", 5);
            if (this._boot.Paddles == null || this._boot.Paddles.Count == 0)
            {
                // TODO: Send RPC to client.
                Debug.Log("No Paddles avaiable bro");
                return;
            }
            Debug.Log("Spawn");
            Paddle paddleToAssign = this._boot.NextPaddle;
            GameObject spawnedPlayer = PhotonNetwork.Instantiate("Roeier", paddleToAssign.transform.position, Quaternion.identity, 0);
            spawnedPlayer.transform.SetParent(this._boot.transform);
            this._boot.AssignPlayer(spawnedPlayer.GetComponent<PhotonRoeier>());
            spawnedPlayer.transform.position = paddleToAssign.transform.position;
            PhotonView tempPlayerView = spawnedPlayer.GetPhotonView();
            PhotonView tempPaddleView = paddleToAssign.gameObject.GetPhotonView();
            

            this.photonView.RPC("AssignPaddle", player, tempPlayerView.viewID, tempPaddleView.viewID, (int)tempPaddleView.GetComponent<Paddle>().RowSide);
        }

        private void WaitForFirstPlayer()
        {
            this._aPlayerHasJoined = true;
        }
            
        private void FixedUpdate()
        {
            if (!this._aPlayerHasJoined || !PhotonNetwork.isMasterClient)
                return;
            //this._photonView.RPC("UpdateBoatTransform", PhotonTargets.All, this._boot.transform.position, this._boot.transform.rotation.eulerAngles);
        }


        [PunRPC]
        public void UpdateBoatTransform(Vector3 pos, Vector3 rot)
        {
            if (PhotonNetwork.isMasterClient) return;
            this._boot.transform.position = pos;
            this._boot.transform.eulerAngles = rot;
        }


        /// <summary>
        /// Called on connecting client
        /// </summary>
        [PunRPC]
        public void AssignPaddle(int playerID, int paddleID, int rowside)
        {
            Debug.Log("assigning paddle");
            PhotonRoeier myPlayer = PhotonView.Find(playerID).gameObject.GetComponent<PhotonRoeier>();

            this._roeiButtonHandler.Roeier = myPlayer;
            myPlayer.PaddleViewId = paddleID;
            myPlayer.Side = (RowTiltController.RowSide)rowside;
        }


        [PunRPC]
        public void AddForce(int paddleViewId, float force)
        {
            Paddle paddle = PhotonView.Find(paddleViewId).GetComponent<Paddle>();
            this._boot.AddForce(paddle.transform.position, force);
        }
    }
}
