using UnityEngine;

/// <summary>
/// Very basic component to move a GameObject by WASD and Space.
/// </summary>
/// <remarks>
/// Requires a PhotonView. 
/// Disables itself on GameObjects that are not owned on Start.
/// 
/// Speed affects movement-speed. 
/// JumpForce defines how high the object "jumps". 
/// JumpTimeout defines after how many seconds you can jump again.
/// </remarks>
[RequireComponent(typeof (PhotonView))]
public class MoveByKeys : Photon.MonoBehaviour
{
    public float Speed = 10f;
    public float JumpForce = 200f;
    public float JumpTimeout = 0.5f;

    private bool isSprite;
    private float jumpingTime;
    private Rigidbody body;
    private Rigidbody2D body2d;

    public void Start()
    {
        //enabled = photonView.isMine;
        this.isSprite = (this.GetComponent<SpriteRenderer>() != null);

        this.body2d = this.GetComponent<Rigidbody2D>();
        this.body = this.GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    public void FixedUpdate()
    {
        if (!this.photonView.isMine)
        {
            return;
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left*(this.Speed*Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right*(this.Speed*Time.deltaTime);
        }

        // jumping has a simple "cooldown" time but you could also jump in the air
        if (this.jumpingTime <= 0.0f)
        {
            if (this.body != null || this.body2d != null)
            {
                // obj has a Rigidbody and can jump (AddForce)
                if (Input.GetKey(KeyCode.Space))
                {
                    this.jumpingTime = this.JumpTimeout;

                    Vector2 jump = Vector2.up*this.JumpForce;
                    if (this.body2d != null)
                    {
                        this.body2d.AddForce(jump);
                    }
                    else if (this.body != null)
                    {
                        this.body.AddForce(jump);
                    }
                }
            }
        }
        else
        {
            this.jumpingTime -= Time.deltaTime;
        }

        // 2d objects can't be moved in 3d "forward"
        if (!this.isSprite)
        {
            if (Input.GetKey(KeyCode.W))
            {
                this.transform.position += Vector3.forward*(this.Speed*Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.S))
            {
                this.transform.position -= Vector3.forward*(this.Speed*Time.deltaTime);
            }
        }
    }
}
