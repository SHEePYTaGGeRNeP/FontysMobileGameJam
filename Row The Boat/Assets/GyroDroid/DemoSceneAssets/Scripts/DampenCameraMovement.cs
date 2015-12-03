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

public class DampenCameraMovement : MonoBehaviour {
	
	public Transform target;
	public Transform lookAt;
	public float movementSpeed = 3;
	public float rotationSpeed = 3;
	
	// Update is called once per frame
	void Update () {
	    this.transform.position = Vector3.Lerp(this.transform.position, this.target.position, Time.deltaTime * this.movementSpeed);
	    this.transform.LookAt(this.lookAt.position, this.target.up);
	}
}
