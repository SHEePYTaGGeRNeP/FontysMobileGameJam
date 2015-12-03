using UnityEngine;
using System.Collections;

public class OnClickInstantiate : MonoBehaviour
{
    public GameObject Prefab;
    public int InstantiateType;
    private string[] InstantiateTypeNames = {"Mine", "Scene"};

    public bool showGui;

    void OnClick()
    {
        if (PhotonNetwork.connectionStateDetailed != PeerState.Joined)
        {
            // only use PhotonNetwork.Instantiate while in a room.
            return;
        }

        switch (this.InstantiateType)
        {
            case 0:
                PhotonNetwork.Instantiate(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0, 5f, 0), Quaternion.identity, 0);
                break;
            case 1:
                PhotonNetwork.InstantiateSceneObject(this.Prefab.name, InputToEvent.inputHitPos + new Vector3(0, 5f, 0), Quaternion.identity, 0, null);
                break;
        }
    }

    void OnGUI()
    {
        if (this.showGui)
        {
            GUILayout.BeginArea(new Rect(Screen.width - 180, 0, 180, 50));
            this.InstantiateType = GUILayout.Toolbar(this.InstantiateType, this.InstantiateTypeNames);
            GUILayout.EndArea();
        }
    }


}
