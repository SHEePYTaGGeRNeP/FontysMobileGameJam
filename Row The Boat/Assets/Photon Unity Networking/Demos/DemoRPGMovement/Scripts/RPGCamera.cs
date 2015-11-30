using UnityEngine;
using System.Collections;

public class RPGCamera : MonoBehaviour 
{
    public Transform Target;

    public float MaximumDistance;
    public float MinimumDistance;

    public float ScrollModifier;
    public float TurnModifier;

    Transform m_CameraTransform;

    Vector3 m_LookAtPoint;
    Vector3 m_LocalForwardVector;
    float m_Distance;
    
    void Start() 
    {
        this.m_CameraTransform = this.transform.GetChild( 0 );
        this.m_LocalForwardVector = this.m_CameraTransform.forward;

        this.m_Distance = -this.m_CameraTransform.localPosition.z / this.m_CameraTransform.forward.z;
        this.m_Distance = Mathf.Clamp(this.m_Distance, this.MinimumDistance, this.MaximumDistance );
        this.m_LookAtPoint = this.m_CameraTransform.localPosition + this.m_LocalForwardVector * this.m_Distance;
    }

    void LateUpdate() 
    {
        this.UpdateDistance();
        this.UpdateZoom();
        this.UpdatePosition();
        this.UpdateRotation();
    }

    void UpdateDistance()
    {
        this.m_Distance = Mathf.Clamp(this.m_Distance - Input.GetAxis( "Mouse ScrollWheel" ) * this.ScrollModifier, this.MinimumDistance, this.MaximumDistance );
    }

    void UpdateZoom()
    {
        this.m_CameraTransform.localPosition = this.m_LookAtPoint - this.m_LocalForwardVector * this.m_Distance;
    }

    void UpdatePosition()
    {
        if(this.Target == null )
        {
            return;
        }

        this.transform.position = this.Target.transform.position;
    }

    void UpdateRotation()
    {
        if( Input.GetMouseButton( 0 ) == true || Input.GetMouseButton( 1 ) == true )
        {
            this.transform.Rotate( 0, Input.GetAxis( "Mouse X" ) * this.TurnModifier, 0 );
        }

        if( Input.GetMouseButton( 1 ) == true && this.Target != null )
        {
            this.Target.rotation = Quaternion.Euler( 0, this.transform.rotation.eulerAngles.y, 0 );
        }
    }
}
