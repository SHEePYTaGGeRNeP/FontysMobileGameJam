using UnityEngine;

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    // Update is called once per frame
    void Update()
    {
        if (!this.photonView.isMine)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);

            myThirdPersonController myC = this.GetComponent<myThirdPersonController>();
            stream.SendNext((int)myC._characterState);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

            myThirdPersonController myC = this.GetComponent<myThirdPersonController>();
            myC._characterState = (CharacterState)stream.ReceiveNext();
        }
    }
}
