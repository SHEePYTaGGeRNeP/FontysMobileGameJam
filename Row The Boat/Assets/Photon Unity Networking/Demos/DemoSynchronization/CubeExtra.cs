using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class CubeExtra : Photon.MonoBehaviour
{
    Vector3 latestCorrectPos = Vector3.zero;
    Vector3 lastMovement = Vector3.zero;
    float lastTime = 0;

    public void Awake()
    {
        if (this.photonView.isMine)
        {
            this.enabled = false;   // Only enable inter/extrapol for remote players
        }

        this.latestCorrectPos = this.transform.position;
    }

    // this method is called by PUN when this script is being "observed" by a PhotonView (setup in inspector)
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // Always send transform (depending on reliability of the network view)
        if (stream.isWriting)
        {
            Vector3 pos = this.transform.localPosition;
            Quaternion rot = this.transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        // When receiving, buffer the information
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);

            this.lastMovement = (pos - this.latestCorrectPos) / (Time.time - this.lastTime);

            this.lastTime = Time.time;
            this.latestCorrectPos = pos;

            this.transform.position = this.latestCorrectPos;
        }
    }

    // This only runs where the component is enabled, which is only on remote peers (server/clients)
    public void Update()
    {
        this.transform.localPosition += this.lastMovement * Time.deltaTime;
    }
}
