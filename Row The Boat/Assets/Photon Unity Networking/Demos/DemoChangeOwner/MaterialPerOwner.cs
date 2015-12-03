using UnityEngine;
using System.Collections;


[RequireComponent( typeof( PhotonView ) )]
public class MaterialPerOwner : Photon.MonoBehaviour
{
    private int assignedColorForUserId;

    Renderer m_Renderer;

    void Start()
    {
        this.m_Renderer = this.GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        if( this.photonView.ownerId != this.assignedColorForUserId )
        {
            this.m_Renderer.material = PlayerVariables.GetMaterial(this.m_Renderer.material, this.photonView.ownerId );
            this.assignedColorForUserId = this.photonView.ownerId;
            //Debug.Log("Switched Material to: " + this.assignedColorForUserId + " " + this.renderer.material.GetInstanceID());
        }
    }
}
