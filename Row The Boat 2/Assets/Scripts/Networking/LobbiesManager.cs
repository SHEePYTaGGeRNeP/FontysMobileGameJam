namespace Assets.Scripts.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class LobbiesManager : MonoBehaviour
    {
        [SerializeField]
        private Dropdown _roomsDropdown;
        [SerializeField]
        private InputField _roomNameField;
        [SerializeField]
        private Slider _maxPlayersSlider;
        [SerializeField]
        private Text _maxPlayersText;
        [SerializeField]
        private string _sceneToLoad = "GameScene";

        private Dictionary<string, string> _roomsDictionary = new Dictionary<string, string>();
        private PhotonManager _photonManager;
        // ReSharper disable once UnusedMember.Local

        private void Awake()
        {
            this._photonManager = GameObject.FindObjectOfType<PhotonManager>();
        }

        private void Start()
        {
            this._photonManager.OnReceivedRoomListUpdateEvent += delegate { this.PopulateServerList(); };
            this._photonManager.OnJoinedRoomEvent += delegate { this.LoadScene(); };
        }

        public void Refresh()
        {
            LogHelper.Log(typeof(LobbiesManager), "Inside lobby: " + PhotonNetwork.insideLobby);
            this.PopulateServerList();
        }
        public void PopulateServerList()
        {
            this._roomsDropdown.options.Clear();
            this._roomsDropdown.options.Add(new Dropdown.OptionData(String.Empty));
            this._roomsDropdown.captionText.text = String.Empty;
            this._roomsDictionary.Clear();

            RoomInfo[] rooms = PhotonNetwork.GetRoomList();
            LogHelper.Log(typeof(LobbiesManager), "Amount of rooms: " + rooms.Length);
            for (int i = 0; i < rooms.Length; i++)
            {
                if (!rooms[i].open)
                    continue;
                string dropDownText = String.Format("{0} : {1}/{2}", rooms[i].name, rooms[i].playerCount, rooms[i].maxPlayers);
                this._roomsDictionary.Add(dropDownText, rooms[i].name);
                this._roomsDropdown.options.Add(new Dropdown.OptionData(dropDownText));
            }
        }


        public void SliderPlayersChanged()
        {
            this._maxPlayersText.text = this._maxPlayersSlider.value.ToString(CultureInfo.InvariantCulture);
        }

        public void JoinGameButtonClicked()
        {
            bool join = true;
            RoomInfo[] rooms = PhotonNetwork.GetRoomList();
            RoomInfo room = null;
            string roomname = this._roomsDictionary[this._roomsDropdown.captionText.text];

            if (rooms == null || rooms.Length == 0)
                join = false;
            else
                foreach (RoomInfo ro in rooms.Where(ro => ro.name == roomname))
                {
                    room = ro;
                    break;
                }

            if (room == null)
            {
                LogHelper.LogError(typeof(LobbiesManager), "JoinGameButtonClicked", "Room does not exist.");
                join = false;
            }
            else if (!room.open)
            {
                LogHelper.LogError(typeof(LobbiesManager), "JoinGameButtonClicked", "Room is not open.");
                join = false;
            }
            else if (room.playerCount >= room.maxPlayers)
            {
                LogHelper.LogError(typeof(LobbiesManager), "JoinGameButtonClicked", "Room is full.");
                join = false;
            }
            if (join)
                PhotonNetwork.JoinRoom(room.name);
            else
                this.PopulateServerList();
        }

        public void CreateRoomButtonClick()
        {
            if (String.IsNullOrEmpty(this._roomNameField.text.Trim()))
            {
                LogHelper.LogError(typeof(LobbiesManager), "JoinGameButtonClicked", "Roomname is empty.");
                return;
            }
            this._photonManager.CreateRoom(this._roomNameField.text, (byte)this._maxPlayersSlider.value);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(this._sceneToLoad);
        }
    }
}
