namespace Assets.Scripts.Networking
{
    using System;

    using global::Photon;

    using UnityEngine;
    using System.Collections.Generic;

    public class PhotonManager : PunBehaviour
    {
        public event EventHandler OnJoinedRoomEvent;
        public event EventHandler OnReceivedRoomListUpdateEvent;

        /// <summary>PlayerID, PhotonRoeierViewID</summary>
        private readonly Dictionary<int, PhotonView> _playerRoeiers = new Dictionary<int, PhotonView>();


        //ReSharper disable once UnusedMember.Local
        private void Awake()
        {
            PhotonNetwork.logLevel = PhotonLogLevel.Informational;
            PhotonNetwork.ConnectUsingSettings("0.1");
        }

        //ReSharper disable once UnusedMember.Local
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(new Vector2(10, 100), new Vector2(300, 200)));
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
            GUILayout.EndArea();
        }



        public override void OnLeftRoom()
        {
            LogHelper.Log(typeof(PhotonManager), "On left room");
            Helpers.Components.DebugText.SetText("On left room");
        }
        public override void OnLeftLobby()
        {
            LogHelper.Log(typeof(PhotonManager), "On left lobby");
            Helpers.Components.DebugText.SetText("On left lobby");
        }

        #region Fails / errors

        public override void OnConnectionFail(DisconnectCause cause)
        {
            Helpers.Components.DebugText.SetText("Connection failed " + cause);
            LogHelper.Log(typeof(PhotonManager), "Connection failed " + cause);
        }

        public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            Helpers.Components.DebugText.SetText("Create room failed " + codeAndMsg[0] + " " + codeAndMsg[1]);
            LogHelper.LogError(typeof(PhotonManager), "Create room failed " + codeAndMsg[0] + " " + codeAndMsg[1]);
        }

        public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            Helpers.Components.DebugText.SetText("Join room failed " + codeAndMsg[0] + " " + codeAndMsg[1]);
            LogHelper.LogError(typeof(PhotonManager), "Join room failed " + codeAndMsg[0] + " " + codeAndMsg[1]);
        }

        private void OnPhotonRandomJoinFailed()
        {
            Helpers.Components.DebugText.SetText("OnPhotonRandomJoinFailed Can't join random room - Creating room");
            LogHelper.Log(typeof(PhotonManager), "OnPhotonRandomJoinFailed Can't join random room - Creating room");
            PhotonNetwork.CreateRoom(null);
        }
        
        #endregion

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnReceivedRoomListUpdate()
        {
            if (this.OnReceivedRoomListUpdateEvent != null)
                this.OnReceivedRoomListUpdateEvent.Invoke(null, null);
        }

        public void CreateRoom(string roomname, byte maxPlayers)
        {
            LogHelper.Log(typeof(PhotonManager), "Creating room " + roomname);
            RoomOptions ro = new RoomOptions() { IsVisible = true,MaxPlayers = maxPlayers, IsOpen =  true};
            PhotonNetwork.CreateRoom(roomname, ro, TypedLobby.Default);
        }

        //public override void OnJoinedLobby()
        //{
        //    LogHelper.Log(typeof(PhotonManager), "OnJoinedLobby");// Joining random room!");
        //    PhotonNetwork.JoinRandomRoom();
        //}


        public override void OnJoinedRoom()
        {
            LogHelper.Log(typeof(PhotonManager), "OnJoinedRoom : You have joined room : " + PhotonNetwork.room.name);
            if (PhotonNetwork.isMasterClient)
            {
                //PhotonNetwork.Instantiate("AI_Boat_Mobile_Roeien", this.transform.position - new Vector3(0, 0, 10f), Quaternion.identity, 0);
            }

            EventHandler handler = this.OnJoinedRoomEvent;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
        public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (!PhotonNetwork.isMasterClient) return;
            this.photonView.RPC("Disconnect", PhotonTargets.All, this._playerRoeiers[otherPlayer.ID].viewID);
            PhotonNetwork.Destroy(this._playerRoeiers[otherPlayer.ID]);
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer player)
        {
            LogHelper.Log(typeof(PhotonManager), "Player " + player.ID +" connected.");
            Helpers.Components.DebugText.SetText("Player " + player.ID + " connected.");
            if (!PhotonNetwork.isMasterClient) return;
            

            //this.photonView.RPC("AssignPaddle", player, tempPlayerView.viewID, tempPaddleView.viewID, (int)tempPaddleView.GetComponent<Paddle>().RowSide);
        }

        #region RPC calls
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
        #endregion

    }
}
