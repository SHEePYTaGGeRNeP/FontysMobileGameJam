using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.PhotonNetworking;

[RequireComponent(typeof(Rigidbody))]
public class Roeiboot : MonoBehaviour
{
	public List<Transform> Paddles;
	
	[SerializeField]
	private Transform _achter;


	public float ForceMultiplier = 2;

	private Rigidbody _rb;
	
	void Start()
	{
		this._rb = this.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
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

	public void AddForce(Transform paddle)
	{
		this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, paddle.position);
		this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
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
