using UnityEngine;
using System.Collections;

[RequireComponent( typeof( CharacterController ) )]
public class RPGMovement : MonoBehaviour 
{
    public float ForwardSpeed;
    public float BackwardSpeed;
    public float StrafeSpeed;
    public float RotateSpeed;

    CharacterController m_CharacterController;
    Vector3 m_LastPosition;
    Animator m_Animator;
    PhotonView m_PhotonView;
    PhotonTransformView m_TransformView;

    float m_AnimatorSpeed;
    Vector3 m_CurrentMovement;
    float m_CurrentTurnSpeed;

    void Start() 
    {
        this.m_CharacterController = this.GetComponent<CharacterController>();
        this.m_Animator = this.GetComponent<Animator>();
        this.m_PhotonView = this.GetComponent<PhotonView>();
        this.m_TransformView = this.GetComponent<PhotonTransformView>();
    }

    void Update() 
    {
        if(this.m_PhotonView.isMine == true )
        {
            this.ResetSpeedValues();

            this.UpdateRotateMovement();

            this.UpdateForwardMovement();
            this.UpdateBackwardMovement();
            this.UpdateStrafeMovement();

            this.MoveCharacterController();
            this.ApplyGravityToCharacterController();

            this.ApplySynchronizedValues();
        }

        this.UpdateAnimation();
    }

    void UpdateAnimation()
    {
        Vector3 movementVector = this.transform.position - this.m_LastPosition;

        float speed = Vector3.Dot( movementVector.normalized, this.transform.forward );
        float direction = Vector3.Dot( movementVector.normalized, this.transform.right );

        if( Mathf.Abs( speed ) < 0.2f )
        {
            speed = 0f;
        }

        if( speed > 0.6f )
        {
            speed = 1f;
            direction = 0f;
        }

        if( speed >= 0f )
        {
            if( Mathf.Abs( direction ) > 0.7f )
            {
                speed = 1f;
            }
        }

        this.m_AnimatorSpeed = Mathf.MoveTowards(this.m_AnimatorSpeed, speed, Time.deltaTime * 5f );

        this.m_Animator.SetFloat( "Speed", this.m_AnimatorSpeed );
        this.m_Animator.SetFloat( "Direction", direction );

        this.m_LastPosition = this.transform.position;
    }

    void ResetSpeedValues()
    {
        this.m_CurrentMovement = Vector3.zero;
        this.m_CurrentTurnSpeed = 0;
    }

    void ApplySynchronizedValues()
    {
        this.m_TransformView.SetSynchronizedValues(this.m_CurrentMovement, this.m_CurrentTurnSpeed );
    }

    void ApplyGravityToCharacterController()
    {
        this.m_CharacterController.Move(this.transform.up * Time.deltaTime * -9.81f );
    }

    void MoveCharacterController()
    {
        this.m_CharacterController.Move(this.m_CurrentMovement * Time.deltaTime );
    }

    void UpdateForwardMovement()
    {
        if( Input.GetKey( KeyCode.W ) == true )
        {
            this.m_CurrentMovement = this.transform.forward * this.ForwardSpeed;
        }
    }

    void UpdateBackwardMovement()
    {
        if( Input.GetKey( KeyCode.S ) == true )
        {
            this.m_CurrentMovement = -this.transform.forward * this.BackwardSpeed;
        }
    }

    void UpdateStrafeMovement()
    {
        if( Input.GetKey( KeyCode.Q ) == true )
        {
            this.m_CurrentMovement = -this.transform.right * this.StrafeSpeed;            
        }

        if( Input.GetKey( KeyCode.E ) == true )
        {
            this.m_CurrentMovement = this.transform.right * this.StrafeSpeed;
        }
    }

    void UpdateRotateMovement()
    {
        if( Input.GetKey( KeyCode.A ) == true )
        {
            this.m_CurrentTurnSpeed = -this.RotateSpeed;
            this.transform.Rotate( 0, -this.RotateSpeed * Time.deltaTime, 0 );
        }

        if( Input.GetKey( KeyCode.D ) == true )
        {
            this.m_CurrentTurnSpeed = this.RotateSpeed;
            this.transform.Rotate( 0, this.RotateSpeed * Time.deltaTime, 0 );
        }
    }
}
