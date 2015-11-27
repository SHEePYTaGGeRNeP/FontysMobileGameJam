namespace Assets.Scripts.PhotonNetworking
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class LobbiesManager : MonoBehaviour
    {

        private Transform panel;
        private List<GameObject> serverList;
        private GameObject scroll;
        private GameObject selectedObject;
        private Color unselectedColor;


        [SerializeField]
        private InputField _roomnameNameField;
        private bool startUpdating;

        [SerializeField]
        private PhotonManager _photonManager;

        public void StartUpdating()
        {
            this.startUpdating = true;
            this.Enable();
        }

        public void OnEnable()
        {
            if (!this.startUpdating)
                return;
            this.Enable();
        }

        private void Enable()
        {
            if (this.serverList == null)
            {
                this.panel = this.transform.FindChild("Panel");
                this.scroll = this.transform.FindChild("Scrollbar").gameObject;
                this.serverList = new List<GameObject>();
                this.unselectedColor = new Color(171 / 255.0f, 174 / 255.0f, 182 / 255.0f, 1);
            }

            RoomOptions ro = new RoomOptions() { isVisible = true, maxPlayers = 5,isOpen = true};
            PhotonNetwork.CreateRoom("Test1", ro, TypedLobby.Default);
            PhotonNetwork.CreateRoom("Test2", ro, TypedLobby.Default);
            PhotonNetwork.CreateRoom("Test3", ro, TypedLobby.Default);
            PhotonNetwork.CreateRoom("Test4", ro, TypedLobby.Default);
            PhotonNetwork.CreateRoom("Test5", ro, TypedLobby.Default);
            PhotonNetwork.JoinOrCreateRoom("Test31", ro, TypedLobby.Default);
            this.InvokeRepeating("PopulateServerList", 0, 2);

        }

        public void OnDisable()
        {
            this.CancelInvoke();
        }

        public void Update()
        {
            if (Input.GetButton("Fire1") && this.startUpdating)
            {
                GameObject server = EventSystem.current.currentSelectedGameObject;
                if (server != null)
                {
                    if (server.name == "ServerButton")
                    {
                        if (this.selectedObject != null)
                            this.selectedObject.transform.FindChild("Image").GetComponent<Image>().color = this.unselectedColor;

                        this.selectedObject = server.transform.parent.gameObject;
                        this.selectedObject.transform.FindChild("Image").GetComponent<Image>().color = Color.white;
                    }
                }
            }
        }

        public void PopulateServerList()
        {
            int i = 0;
            RoomInfo[] hostData = PhotonNetwork.GetRoomList();

            int selected = this.serverList.IndexOf(this.selectedObject);

            for (int j = 0; j < this.serverList.Count; j++)
            {
                Destroy(this.serverList[j]);
            }
            this.serverList.Clear();

            if (null != hostData)
            {
                for (i = 0; i < hostData.Length; i++)
                {
                    if (!hostData[i].open)
                        continue;

                    GameObject text = (GameObject)Instantiate(Resources.Load("ServerObject"));
                    this.serverList.Add(text);
                    text.transform.SetParent(this.panel, false);
                    text.transform.FindChild("ServerText").GetComponent<Text>().text = hostData[i].name;
                    text.transform.FindChild("PlayerText").GetComponent<Text>().text = hostData[i].playerCount + "/" + hostData[i].maxPlayers;
                    text.transform.FindChild("MapText").GetComponent<Text>().text = hostData[i].customProperties["map"].ToString();
                    text.transform.FindChild("GMText").GetComponent<Text>().text = hostData[i].customProperties["gm"].ToString();
                    text.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, (i * -25), 0);
                }
            }
            if ((i * -25) < -290)
            {
                this.scroll.SetActive(true);
            }
            else
            {
                this.scroll.SetActive(false);
            }
            if (selected >= 0 && selected < this.serverList.Count)
            {
                this.selectedObject = this.serverList[selected];
                this.selectedObject.transform.FindChild("Image").GetComponent<Image>().color = Color.white;
            }
        }

        public void CreateRoomButtonClick()
        {
            this._photonManager.CreateRoom(this._roomnameNameField.text);
        }

    }
}
