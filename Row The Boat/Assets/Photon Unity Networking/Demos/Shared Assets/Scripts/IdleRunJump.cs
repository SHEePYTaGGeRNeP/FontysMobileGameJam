using UnityEngine;
using System.Collections;

public class IdleRunJump : MonoBehaviour 
{
    protected Animator animator;
    public float DirectionDampTime = .25f;
    public bool ApplyGravity = true;
    public float SynchronizedMaxSpeed;
    public float TurnSpeedModifier;
    public float SynchronizedTurnSpeed;
    public float SynchronizedSpeedAcceleration;

    protected PhotonView m_PhotonView;

    PhotonTransformView m_TransformView;

    //Vector3 m_LastPosition;
    float m_SpeedModifier;

    // Use this for initialization
    void Start () 
    {
        this.animator = this.GetComponent<Animator>();
        this.m_PhotonView = this.GetComponent<PhotonView>();
        this.m_TransformView = this.GetComponent<PhotonTransformView>();

        if(this.animator.layerCount >= 2)
            this.animator.SetLayerWeight(1, 1);
    }
        
    // Update is called once per frame
    void Update () 
    {
        if(this.m_PhotonView.isMine == false && PhotonNetwork.connected == true )
        {
            return;
        }

        if (this.animator)
        {
            AnimatorStateInfo stateInfo = this.animator.GetCurrentAnimatorStateInfo(0);			

            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetButton("Fire1")) this.animator.SetBool("Jump", true);                
            }
            else
            {
                this.animator.SetBool("Jump", false);                
            }

            if(Input.GetButtonDown("Fire2") && this.animator.layerCount >= 2)
            {
                this.animator.SetBool("Hi", !this.animator.GetBool("Hi"));
            }
            
        
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if( v < 0 )
            {
                v = 0;
            }

            this.animator.SetFloat( "Speed", h*h+v*v );
            this.animator.SetFloat( "Direction", h, this.DirectionDampTime, Time.deltaTime );

            float direction = this.animator.GetFloat( "Direction" );

            float targetSpeedModifier = Mathf.Abs( v );

            if( Mathf.Abs( direction ) > 0.2f )
            {
                targetSpeedModifier = this.TurnSpeedModifier;
            }

            this.m_SpeedModifier = Mathf.MoveTowards(this.m_SpeedModifier, targetSpeedModifier, Time.deltaTime * 25f );

            Vector3 speed = this.transform.forward * this.SynchronizedMaxSpeed * this.m_SpeedModifier;
            float turnSpeed = direction * this.SynchronizedTurnSpeed;

            /*float moveDistance = Vector3.Distance( transform.position, m_LastPosition ) / Time.deltaTime;

            if( moveDistance < 4f && turnSpeed == 0f )
            {
                speed = transform.forward * moveDistance;
            }*/

            //Debug.Log( moveDistance );
            //Debug.Log( speed + " - " + speed.magnitude + " - " + speedModifier + " - " + h + " - " + v );

            this.m_TransformView.SetSynchronizedValues( speed, turnSpeed );

            //m_LastPosition = transform.position;
         }   		  
    }
}
