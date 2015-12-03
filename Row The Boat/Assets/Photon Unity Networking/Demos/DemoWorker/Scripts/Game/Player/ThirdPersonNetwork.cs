using UnityEngine;
using System.Collections;

public class ThirdPersonNetwork : Photon.MonoBehaviour
{
    ThirdPersonCamera cameraScript;
    ThirdPersonController controllerScript;

    void Awake()
    {
        this.cameraScript = this.GetComponent<ThirdPersonCamera>();
        this.controllerScript = this.GetComponent<ThirdPersonController>();

         if (this.photonView.isMine)
        {
            //MINE: local player, simply enable the local scripts
            this.cameraScript.enabled = true;
            this.controllerScript.enabled = true;
        }
        else
        {
            this.cameraScript.enabled = false;

            this.controllerScript.enabled = true;
            this.controllerScript.isControllable = false;
        }

        this.gameObject.name = this.gameObject.name + this.photonView.viewID;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext((int)this.controllerScript._characterState);
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation); 
        }
        else
        {
            //Network player, receive data
            this.controllerScript._characterState = (CharacterState)(int)stream.ReceiveNext();
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

    private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this

    void Update()
    {
        if (!this.photonView.isMine)
        {
            //Update remote player (smooth this, this looks good, at the cost of some accuracy)
            this.transform.position = Vector3.Lerp(this.transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

}