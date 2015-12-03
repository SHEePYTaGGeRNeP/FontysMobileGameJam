using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.PhotonNetworking;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody))]
public class Roeiboot : MonoBehaviour
{
    public List<Paddle> Paddles;
    public bool Photon;

    [SerializeField]
    private Transform _achter;
    private RowTiltController _rowController;

    private Rigidbody _rb;

    public float ForceMultiplier = 2;

    private Paddle _nextPaddle;
    public Paddle NextPaddle
    {
        get
        {
            if (this._nextPaddle == null)
                this.SetNextPaddle();
            return this._nextPaddle;
        }
    }

    // Use this for initialization
    void Start()
    {
        this._rb = this.GetComponent<Rigidbody>();

        this._rowController = this.GetComponent<RowTiltController>();
        if (this._rowController != null)
        {
            Debug.Log("Added row controls!");
            this._rowController.Row += (sender, args) =>
            {
                //if (args.Side != RowTiltController.RowSide.Left) return;
                this.AddForce(this._achter.position, args.Strength * args.Efficiency);
            };
        }
        if (!PhotonNetwork.isMasterClient)
        {
            PhotonManager.Instance.Boot = this;
        }

    }

    void FixedUpdate()
    {
        if (this.Photon) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
            this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this.Paddles[3].transform.position);
            this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this.Paddles[2].transform.position);
            this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this.Paddles[1].transform.position);
            this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this.Paddles[0].transform.position);
            this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
        }

    }

    public void AddForce(Vector3 paddle, float force = 1f)
    {
        this._rb.AddForceAtPosition(this.transform.forward * force * this.ForceMultiplier, paddle);
        this._rb.AddForceAtPosition(this.transform.forward * force * this.ForceMultiplier, this._achter.position);
    }


    private void SetNextPaddle()
    {
        if (this.Paddles.Count == 0 && this._nextPaddle != null)
            this._nextPaddle = null;
        else if (this.Paddles.Count > 0)
        {
            int index = Random.Range(0, this.Paddles.Count);
            this._nextPaddle = this.Paddles[index];
        }
    }

    public Paddle AssignPlayer(PhotonRoeier player)
    {
        Paddle nextPaddle = this.NextPaddle;
        player.PaddleViewId = nextPaddle.gameObject.GetPhotonView().viewID;
        this.Paddles.Remove(nextPaddle);
        this._nextPaddle = null;
        return nextPaddle;
    }

}
