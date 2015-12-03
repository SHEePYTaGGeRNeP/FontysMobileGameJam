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

public class SpaceShipControl : MonoBehaviour {

	void Start() {
		SensorHelper.ActivateRotation();
	    this.initialSensorValue = SensorHelper.rotation;
	    this.gotFirstValue = false;

	    this.StartCoroutine(this.Calibration());
	}
	
	IEnumerator Calibration() {
		while(! SensorHelper.gotFirstValue) {
			SensorHelper.FetchValue();
			yield return null;
		}
		
		// wait some frames
		yield return new WaitForSeconds(0.1f);
		
		// set initial rotation
	    this.initialSensorValue = SensorHelper.rotation;
		
		// allow updates
	    this.gotFirstValue = true;
	}
	
	bool gotFirstValue = false;
	Quaternion initialSensorValue;
	
	Quaternion differenceRotation;
	Vector3 differenceEuler;
	
	public float strength = 1;
	public float movementStrength = 10;
		
	void Update() {
		if(!this.gotFirstValue) return;
		
		// calculate difference between current rotation and initial rotation
	    this.differenceRotation = FromToRotation(this.initialSensorValue, SensorHelper.rotation);
		
		// differenceEuler is the difference in degrees between the current SensorHelper.rotation and the initial value
	    this.differenceEuler = this.differenceRotation.eulerAngles;
		
		if(this.differenceEuler.x > 180) this.differenceEuler.x -= 360;
		if(this.differenceEuler.y > 180) this.differenceEuler.y -= 360;
		if(this.differenceEuler.z > 180) this.differenceEuler.z -= 360;
		
		// for an airplane: disable yaw,
		// only use roll and pitch
	    this.differenceEuler.y = 0;
		
		// rotate us
	    this.transform.Rotate(this.differenceEuler * Time.deltaTime * this.strength);
		// move forward all the time (no speed control)
	    this.transform.Translate(Vector3.forward * this.movementStrength * Time.deltaTime, Space.Self);
	}
	
	/// <summary>
	/// Calculates the rotation C needed to rotate from A to B.
	/// </summary>
	public static Quaternion FromToRotation(Quaternion a, Quaternion b) {
		return Quaternion.Inverse(a) * b;
	}

	public bool showGUI = false;
	public void OnGUI() {
		if(!this.showGUI) return;
		GUI.Label(new Rect(10,10,200,25), "Relative rotation to start in degrees:");
		GUI.Label(new Rect(10,40,200,25), ""+ this.differenceEuler);
	}
}
