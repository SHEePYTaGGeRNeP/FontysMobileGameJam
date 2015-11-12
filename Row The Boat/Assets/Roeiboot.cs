using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Roeiboot : MonoBehaviour {

	[SerializeField]
	private Transform _rechtsvoor, _linksvoor, _rechtsachter, _linksachter;
	
	public float ForceMultiplier;

	private Rigidbody _rb;

	// Use this for initialization
	void Start () {
		this._rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._linksachter.position);
		if (Input.GetKeyDown(KeyCode.D))
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._rechtsachter.position);
		if (Input.GetKeyDown(KeyCode.Q))
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._linksvoor.position);
		if (Input.GetKeyDown(KeyCode.E))
			this._rb.AddForceAtPosition(this.transform.forward * this.ForceMultiplier, this._rechtsvoor.position);

	}
}
