using UnityEngine;
using System.Collections;

public class PlayerDiamond : MonoBehaviour
{
    #region Properties
    public Transform HeadTransform;
    public float HeightOffset = 0.5f;
    #endregion

    #region Members
    PhotonView m_PhotonView;
    PhotonView PhotonView
    {
        get
        {
            if(this.m_PhotonView == null )
            {
                this.m_PhotonView = this.transform.parent.GetComponent<PhotonView>();
            }

            return this.m_PhotonView;
        }
    }

    Renderer m_DiamondRenderer;
    Renderer DiamondRenderer
    {
        get
        {
            if(this.m_DiamondRenderer == null )
            {
                this.m_DiamondRenderer = this.GetComponentInChildren<Renderer>();
            }

            return this.m_DiamondRenderer;
        }
    }

    float m_Rotation;
    float m_Height;
    #endregion

    #region Update
    void Start()
    {
        this.m_Height = this.HeightOffset;

        if(this.HeadTransform != null )
        {
            this.m_Height += this.HeadTransform.position.y;
        }
    }

    void Update() 
    {
        this.UpdateDiamondPosition();
        this.UpdateDiamondRotation();
        this.UpdateDiamondVisibility();
    }

    void UpdateDiamondPosition()
    {
        Vector3 targetPosition = Vector3.zero;

        if(this.HeadTransform != null )
        {
            targetPosition = this.HeadTransform.position;
        }

        targetPosition.y = this.m_Height;

        if( float.IsNaN( targetPosition.x ) == false && float.IsNaN( targetPosition.z ) == false )
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 10f );
        }
    }

    void UpdateDiamondRotation()
    {
        this.m_Rotation += Time.deltaTime * 180f;
        this.m_Rotation = this.m_Rotation % 360;

        this.transform.rotation = Quaternion.Euler( 0, this.m_Rotation, 0 );
    }

    void UpdateDiamondVisibility()
    {
        this.DiamondRenderer.enabled = true;

        if(this.PhotonView == null || this.PhotonView.isMine == false )
        {
            this.DiamondRenderer.enabled = false;
        }
    }
    #endregion
}
