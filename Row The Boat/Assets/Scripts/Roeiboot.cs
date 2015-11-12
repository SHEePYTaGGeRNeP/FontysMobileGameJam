using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.PhotonNetworking;

[RequireComponent(typeof(Rigidbody))]
public class Roeiboot : MonoBehaviour
{
	public List<Transform> Paddles;

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
                Debug.Log(string.Format("Side: {0}, Strength: {1}, Efficency: {2}", args.Side, args.Strength, args.Efficiency));
                AddForce(this._achter, args.Strength * args.Efficiency);
            };
        }

	}

	void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this.Paddles[3].position);
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this.Paddles[2].position);
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this.Paddles[1].position);
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this.Paddles[0].position);
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
		}

	}

	public void AddForce(Transform paddle, float force = 1f)
	{
		this._rb.AddForceAtPosition(this.transform.forward * force * this.ForceMultiplier * Time.fixedDeltaTime, paddle.position);
		this._rb.AddForceAtPosition(this.transform.forward * force * this.ForceMultiplier * Time.fixedDeltaTime, this._achter.position);
	}

	public Transform AssignPlayer(PhotonRoeier player)
	{
		int index = Random.Range(0, this.Paddles.Count);
		Transform paddle = this.Paddles[index];
		player.Paddle = paddle;
		this.Paddles.Remove(paddle);
		return paddle;
	}
}
