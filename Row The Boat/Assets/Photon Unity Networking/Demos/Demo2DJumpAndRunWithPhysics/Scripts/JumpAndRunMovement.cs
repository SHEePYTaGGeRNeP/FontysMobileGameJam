using UnityEngine;
using System.Collections;

public class JumpAndRunMovement : MonoBehaviour 
{
    public float Speed;
    public float JumpForce;

    Animator m_Animator;
    Rigidbody2D m_Body;
    PhotonView m_PhotonView;

    bool m_IsGrounded;

    void Awake() 
    {
        this.m_Animator = this.GetComponent<Animator>();
        this.m_Body = this.GetComponent<Rigidbody2D>();
        this.m_PhotonView = this.GetComponent<PhotonView>();
    }

    void Update() 
    {
        this.UpdateIsGrounded();
        this.UpdateIsRunning();
        this.UpdateFacingDirection();
    }

    void FixedUpdate()
    {
        if(this.m_PhotonView.isMine == false )
        {
            return;
        }

        this.UpdateMovement();
        this.UpdateJumping();
    }

    void UpdateFacingDirection()
    {
        if(this.m_Body.velocity.x > 0.2f )
        {
            this.transform.localScale = new Vector3( 1, 1, 1 );
        }
        else if(this.m_Body.velocity.x < -0.2f )
        {
            this.transform.localScale = new Vector3( -1, 1, 1 );
        }
    }

    void UpdateJumping()
    {
        if( Input.GetKey( KeyCode.Space ) == true && this.m_IsGrounded == true )
        {
            this.m_Animator.SetTrigger( "IsJumping" );
            this.m_Body.AddForce( Vector2.up * this.JumpForce );
            this.m_PhotonView.RPC( "DoJump", PhotonTargets.Others );
        }
    }

    [PunRPC]
    void DoJump()
    {
        this.m_Animator.SetTrigger( "IsJumping" );
    }

    void UpdateMovement()
    {
        Vector2 movementVelocity = this.m_Body.velocity;

        if( Input.GetAxisRaw( "Horizontal" ) > 0.5f )
        {
            movementVelocity.x = this.Speed;
            
        }
        else if( Input.GetAxisRaw( "Horizontal" ) < -0.5f )
        {
            movementVelocity.x = -this.Speed;
        }
        else
        {
            movementVelocity.x = 0;
        }

        this.m_Body.velocity = movementVelocity;
    }

    void UpdateIsRunning()
    {
        this.m_Animator.SetBool( "IsRunning", Mathf.Abs(this.m_Body.velocity.x ) > 0.1f );
    }

    void UpdateIsGrounded()
    {
        Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y );

        //RaycastHit2D hit = Physics2D.Raycast( position, -Vector2.up, 0.1f, 1 << LayerMask.NameToLayer( "Ground" ) );
        RaycastHit2D hit = Physics2D.Raycast(position, -Vector2.up, 0.1f);

        this.m_IsGrounded = hit.collider != null;
        this.m_Animator.SetBool( "IsGrounded", this.m_IsGrounded );
    }
}
