using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Roeiboot : MonoBehaviour
{

	[SerializeField]
	private Transform _rechtsvoor, _linksvoor, _rechtsachter, _linksachter, _achter;

	public float ForceMultiplier = 2;

	private Rigidbody _rb;

	// Use this for initialization
	void Start()
	{
		this._rb = this.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._linksachter.position);
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._rechtsachter.position);
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._linksvoor.position);
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._rechtsvoor.position);
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._achter.position);
		}

	}

}
