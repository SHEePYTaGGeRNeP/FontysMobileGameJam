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
                AddForce(this._achter, args.Strength * args.Efficiency);
            };
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

	public Paddle AssignPlayer(PhotonRoeier player)
	{
		int index = Random.Range(0, this.Paddles.Count);
		Paddle paddle = this.Paddles[index];
		player.Paddle = paddle;
		this.Paddles.Remove(paddle);
		return paddle;
	}

}
