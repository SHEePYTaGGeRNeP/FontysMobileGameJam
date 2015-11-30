// #######################################
// ---------------------------------------
// ---------------------------------------
// prefrontal cortex -- http://prefrontalcortex.de
// ---------------------------------------
// Full Android Sensor Access for Unity3D
// ---------------------------------------
// Contact:
// 		contact@prefrontalcortex.de
// ---------------------------------------
// #######################################


using UnityEngine;
using System.Collections;

public class MoveByLinearAcc : MonoBehaviour {

	Rigidbody r;

	// Use this for initialization
	void Start () {
		Sensor.Activate(Sensor.Type.LinearAcceleration);
	    this.r = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame 
	void FixedUpdate () {
		Vector3 linearAcc = this.FilterMax(Sensor.linearAcceleration / 20 * 10);
	    this.r.position = new Vector3( -linearAcc.x, this.r.position.y, -linearAcc.y);
	}
	
	// Decay filter - goes instantly up to higher values, but slowly down back to zero
	// (LinearAcceleration sensor returns one peak and goes immediately back down to zero - this filter preserves the peak)
	
	Vector3 holder = Vector3.zero;
	Vector3 max = Vector3.zero;
	Vector3 velocity = Vector3.zero;
	
	Vector3 FilterMax(Vector3 input)
	{
		if(input.magnitude > this.max.magnitude) this.max = input;

	    this.holder = Vector3.SmoothDamp(this.holder, this.max, ref this.velocity, 0.1f);
		if(Vector3.Distance(this.holder, this.max) < 0.4f) this.max = Vector3.zero;
		return this.holder;
	}
}