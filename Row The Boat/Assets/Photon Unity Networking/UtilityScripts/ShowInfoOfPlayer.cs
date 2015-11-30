using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
/// <summary>
/// Can be attached to a GameObject to show info about the owner of the PhotonView.
/// </summary>
/// <remarks>
/// This is a Photon.Monobehaviour, which adds the property photonView (that's all).
/// </remarks>
[RequireComponent(typeof(PhotonView))]
public class ShowInfoOfPlayer : Photon.MonoBehaviour
{
    private GameObject textGo;
    private TextMesh tm;
    public float CharacterSize = 0;

    public Font font;
    public bool DisableOnOwnObjects;

    void Start()
    {
        if (this.font == null)
        {
            #if UNITY_3_5
            font = (Font)FindObjectsOfTypeIncludingAssets(typeof(Font))[0];
            #else
            this.font = (Font)Resources.FindObjectsOfTypeAll(typeof(Font))[0];
            #endif
            Debug.LogWarning("No font defined. Found font: " + this.font);
        }

        if (this.tm == null)
        {
            this.textGo = new GameObject("3d text");
            //textGo.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            this.textGo.transform.parent = this.gameObject.transform;
            this.textGo.transform.localPosition = Vector3.zero;

            MeshRenderer mr = this.textGo.AddComponent<MeshRenderer>();
            mr.material = this.font.material;
            this.tm = this.textGo.AddComponent<TextMesh>();
            this.tm.font = this.font;
            this.tm.anchor = TextAnchor.MiddleCenter;
            if (this.CharacterSize > 0)
            {
                this.tm.characterSize = this.CharacterSize;
            }
        }
    }

    void Update()
    {
        bool showInfo = !this.DisableOnOwnObjects || this.photonView.isMine;
        if (this.textGo != null)
        {
            this.textGo.SetActive(showInfo);
        }
        if (!showInfo)
        {
            return;
        }

        
        PhotonPlayer owner = this.photonView.owner;
        if (owner != null)
        {
            this.tm.text = (string.IsNullOrEmpty(owner.name)) ? "player"+owner.ID : owner.name;
        }
        else if (this.photonView.isSceneView)
        {
            this.tm.text = "scn";
        }
        else
        {
            this.tm.text = "n/a";
        }
    }
}
