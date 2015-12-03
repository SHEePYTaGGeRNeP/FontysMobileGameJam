using UnityEngine;
using System.Collections;

public class SmoothSyncMovement : Photon.MonoBehaviour
{
    public float SmoothingDelay = 5;
    public void Awake()
    {
        if (this.photonView == null || this.photonView.observed != this)
        {
            Debug.LogWarning(this + " is not observed by this object's photonView! OnPhotonSerializeView() in this class won't be used.");
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation); 
        }
        else
        {
            //Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this

    public void Update()
    {
        if (!this.photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            this.transform.position = Vector3.Lerp(this.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
        }
    }

}