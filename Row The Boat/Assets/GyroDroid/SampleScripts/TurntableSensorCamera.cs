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

public class TurntableSensorCamera : MonoBehaviour {
	
	public Transform target;
	public float distance;
	public bool useRelativeCameraRotation = true;
	
	// initial camera and sensor value
	private Quaternion initialCameraRotation = Quaternion.identity;
	private bool gotFirstValue = false;
	
	// Use this for initialization
	void Start ()
	{
		// for distance calculation --> its much easier to make adjusments in the editor, just put
		// your camera where you want it to be
		if(this.target == null) {Debug.LogWarning("Warning! Target for TurntableSensorCamera is null."); return;}
		
		// if distance is set to zero, use current camera position --> easier setup
		if(this.distance == 0)
		    this.distance = (this.transform.position - this.target.position).magnitude;
		
		// if you start the app, you will be viewing in the same direction your unity camera looks right now
		if(this.useRelativeCameraRotation)
		    this.initialCameraRotation = Quaternion.Euler(0, this.transform.rotation.eulerAngles.y,0);
		else
		    this.initialCameraRotation = Quaternion.identity;
		// direct call
		// Sensor.Activate(Sensor.Type.RotationVector);
		
		// SensorHelper call with fallback
		SensorHelper.ActivateRotation();
//		SensorHelper.TryForceRotationFallback(RotationFallbackType.OrientationAndAcceleration);

	    this.StartCoroutine(this.Calibration());
	}
	
	IEnumerator Calibration()
	{
	    this.gotFirstValue = false;
		
		while(! SensorHelper.gotFirstValue) {
			SensorHelper.FetchValue();
			yield return null;
		}
		
		SensorHelper.FetchValue();
		
		// wait some frames
		yield return new WaitForSeconds(0.1f);
		
		// Initialize rotation values
		Quaternion initialSensorRotation = SensorHelper.rotation;
	    this.initialCameraRotation *= Quaternion.Euler(0,-initialSensorRotation.eulerAngles.y,0);
		
		// allow updates
	    this.gotFirstValue = true;
	}
	
	// Update is called once per frame
	void LateUpdate()
	{
		// first value gotten from sensor is the offset value for further processing
		if(this.useRelativeCameraRotation)
		if(!this.gotFirstValue) return;
	
		// do nothing if there is no target
		if(this.target == null) return;

	    this.transform.rotation = this.initialCameraRotation * SensorHelper.rotation; // Sensor.rotationQuaternion;
	    this.transform.position = this.target.position - this.transform.forward * this.distance;		
	}
}
