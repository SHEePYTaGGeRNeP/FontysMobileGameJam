using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Scripts;

[RequireComponent(typeof(Rigidbody))]
public class Roeiboot : MonoBehaviour
{

	[SerializeField]
	private Transform _rechtsvoor, _linksvoor, _rechtsachter, _linksachter, _achter;
	public float ForceMultiplier = 2;
	private Rigidbody _rb;
    private RowTiltController _rowController;


	// Use this for initialization
	void Start()
	{
		this._rb = this.GetComponent<Rigidbody>();
	    this._rowController = this.GetComponent<RowTiltController>();
	    this._rowController.Row += (sender, args) =>
	    {
	        if (args.Side != RowTiltController.RowSide.Left) return;
            AddForce(this._linksvoor.position, args.Strength * args.Efficiency);
        };
	}

	void FixedUpdate()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
            AddForce(this._linksachter.position);
            AddForce(this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			AddForce(this._rechtsachter.position);
			AddForce(this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			AddForce(this._linksvoor.position);
			AddForce(this._achter.position);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			AddForce(this._rechtsvoor.position);
			AddForce(this._achter.position);
		}

	}

    void AddForce(Vector3 position, float force = 1)
    {
        this._rb.AddForceAtPosition(this.transform.forward * force * this.ForceMultiplier * Time.fixedDeltaTime, position);
    }
}
