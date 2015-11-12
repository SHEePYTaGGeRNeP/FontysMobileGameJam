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

public static class SensorHelper
{
	
	// Uses the best available rotation sensor
	
	// The first one available on this list will be used:
	// 1) RotationVector
	// 3) Orientation (deprecated. but works on many devices)
	// 2) Accelerometer + MagneticField
	// 3) Accelerometer only - no compass function
	
	// usage:
	// (e.g. on a camera)
	// 
	/*
	void Start()
	{
		SensorHelper.ActivateRotation();
	}
	
	void Update()
	{
		transform.rotation = SensorHelper.rotation;
	}
	*/
	
	// ATTENTION! This class is under construction. It may not work on some devices.
	
	#region API
	
	public static void FetchValue()
	{
		if (rotationHelper != null)
		{
			rotationHelper();
		}
	}
	
	// this holds the newest rotation value by the best found sensor
	public static Quaternion rotation
	{
		get
        {
		    return rotationHelper != null ? rotationHelper() : Quaternion.identity;
		}
	}

    public static bool gotFirstValue { get; private set; }

    // Tries to activate a certain rotation fallback.
	// If ActivateRotation is called, the best (in most cases) fallback is used.
	// There might be cases/devices where other approaches yield better results.
	// 
	// Returns true if fallback is available and activated.
	public static bool TryForceRotationFallback(RotationFallbackType fallbackType)
	{
#pragma warning disable 162
		// for platforms other than Android, always use RotationQuaternion
//		if(Application.platform != RuntimePlatform.Android)
//			fallbackType = RotationFallbackType.RotationQuaternion;
		if(Application.platform == RuntimePlatform.IPhonePlayer)
			fallbackType = RotationFallbackType.RotationQuaternion;

		switch(fallbackType)
		{
		    case RotationFallbackType.RotationQuaternion:
			    if (Sensor.Get(Sensor.Type.RotationVector).available)
			    {
				    Sensor.Activate(Sensor.Type.RotationVector);
				    rotationHelper = new GetRotationHelper(RotationQuaternion);
				    current = RotationFallbackType.RotationQuaternion;
					
					Debug.Log("RotationVector is available.");
				    return true;
			    }
		        break;
		    case RotationFallbackType.OrientationAndAcceleration:
			    if (Sensor.Get(Sensor.Type.Orientation).available)
			    {
				    Sensor.Activate(Sensor.Type.Orientation);
				    rotationHelper = new GetRotationHelper(OrientationAndAcceleration);
					current = RotationFallbackType.OrientationAndAcceleration;
				    
					Debug.Log("Orientation/Acceleration is available.");
			        return true;
			    }
		        break;
		    case RotationFallbackType.MagneticField:
			    if (Sensor.Get(Sensor.Type.MagneticField).available)
			    {
				    Sensor.Activate(Sensor.Type.Accelerometer);
				    Sensor.Activate(Sensor.Type.MagneticField);
				    rotationHelper = new GetRotationHelper(MagneticField);
					current = RotationFallbackType.MagneticField;
				
					Debug.Log("Accelerometer/MagneticField is available.");
			        return true;
			    }
		        break;
			case RotationFallbackType.AccelerationOnly:
				{
					rotationHelper = new GetRotationHelper(InputAcceleration);
					Debug.Log("InputAcceleration is available - no compass support.");
					return true;
				}
				break;
		}

		return false;
#pragma warning restore 162
	}
	
	static RotationFallbackType current;

	// Call this to activate the best available rotation sensor
	public static void ActivateRotation()
	{
		// looks which sensors are available
		// and uses the best one for rotation calculation
		
		gotFirstValue = false;

		// try all rotation fallbacks, sorted by quality
		bool haveSensor = false;
		if(!haveSensor) haveSensor = TryForceRotationFallback(RotationFallbackType.RotationQuaternion);
		if(!haveSensor) haveSensor = TryForceRotationFallback(RotationFallbackType.OrientationAndAcceleration);
		if(!haveSensor) haveSensor = TryForceRotationFallback(RotationFallbackType.MagneticField);
		if(!haveSensor) haveSensor = TryForceRotationFallback(RotationFallbackType.AccelerationOnly);
	}
	
	public static void DeactivateRotation()
	{
		switch(current)
		{
		case RotationFallbackType.RotationQuaternion:
			Sensor.Deactivate(Sensor.Type.RotationVector);
			break;
		case RotationFallbackType.OrientationAndAcceleration:
			Sensor.Deactivate(Sensor.Type.Orientation);
			break;
		case RotationFallbackType.MagneticField:
			Sensor.Deactivate(Sensor.Type.Accelerometer);
			Sensor.Deactivate(Sensor.Type.MagneticField);
			break;	
		}
	}
	
	#endregion
	
	#region HelperFunctions
	
	private delegate Quaternion GetRotationHelper();
	private static GetRotationHelper rotationHelper;
	
	private static Quaternion RotationQuaternion()
	{
		if (Sensor.Get(Sensor.Type.RotationVector).gotFirstValue && Sensor.rotation != Quaternion.identity)
		{
			gotFirstValue = true;
		}
		return Sensor.rotation;
	}

    private static Quaternion GetOrientation()
	{
		if (Sensor.Get(Sensor.Type.MagneticField).gotFirstValue && Sensor.Get(Sensor.Type.Accelerometer).gotFirstValue)
		{
			gotFirstValue = true;
		}
		return Quaternion.Euler(Sensor.GetOrientation());
	}
	
	static Quaternion Q1(float k)
	{
		return new Quaternion(Mathf.Sin(k/2),0,0,Mathf.Cos(k/2));
	}
	
	static Quaternion Q2(float k)
	{
		return new Quaternion(0, Mathf.Sin(k/2),0,Mathf.Cos(k/2));
	}
	
	static Quaternion Q3(float k)
	{
		return new Quaternion(0,0, Mathf.Sin(k/2),Mathf.Cos(k/2));
	}

	private static Transform tempT;
	private static readonly AngleFilter OrientationXFilter = new AngleFilter(8);
	private static Quaternion OrientationAndAcceleration()
	{
		// see https://groups.google.com/forum/#!msg/android-developers/GOm9yhTFZaM/eaoNuQEMAb4J
		// for a short explanation by a Google Engineer
		
		var k = Quaternion.identity;

		/*
		var acc  = Sensor.Get (Sensor.Type.Accelerometer).values;
		var gyro = Sensor.Get (Sensor.Type.Gyroscope).values;
		var mag  = Sensor.Get (Sensor.Type.MagneticField).values;

//		Debug.Log("IM HERE");

		// acc goes down, no orientation
		// orientation : only usable value is first one

		if(tempT == null)
			tempT = new GameObject("test").transform;

		tempT.position = Vector3.zero;
		tempT.rotation = Quaternion.identity;
		tempT.rotation = Quaternion.Euler (0, mag.x, 0);
		tempT.LookAt (tempT.forward, new Vector3(-acc.x, acc.y, acc.z));

		return tempT.rotation;
		*/

		// GyroDroid 4.0 behaviour


		var accelerometer = AccelerometerOrientation();
		
		if (Sensor.Get(Sensor.Type.Orientation).gotFirstValue)
		{
			gotFirstValue = true;
		}


		var y = OrientationXFilter.Update(Sensor.orientation.x);
	
		k.eulerAngles = new Vector3(-accelerometer.x, y, accelerometer.z);
		return k;
	}
	
	private static Quaternion Orientation()
	{
		if (Sensor.Get(Sensor.Type.Orientation).gotFirstValue)
		{
			gotFirstValue = true;
		}
		
		var gyro = Sensor.orientation;
//		Debug.Log ("gyro " + Sensor.Get (Sensor.Type.Gyroscope).values);
//		return EulerToQuaternion(gyro);
//		return Quaternion.Euler(gyro);
		
		var k1 = Q2(gyro.x/90*Mathf.PI/2);
		var k2 = Q1((gyro.y+90)/90*Mathf.PI/2);
		var k3 = Q3((gyro.z)/90*Mathf.PI/2);
		
		var rot = k3 * k2 * k1;
		return rot;
		
//		return EulerToQuaternion(gyro);
	}

#region Test1
// http://www.euclideanspace.com/maths/geometry/rotations/conversions/eulerToQuaternion/index.htm
	static Quaternion EulerToQuaternion(Vector3 euler)
	{
		euler *= Mathf.Deg2Rad;
		euler.x *= -1;
		
		float c1 = Mathf.Cos(euler.z / 2);
		float c2 = Mathf.Cos(euler.x / 2);
		float c3 = Mathf.Cos(euler.y / 2);
		
		float s1 = Mathf.Sin(euler.z / 2);
		float s2 = Mathf.Sin(euler.x / 2);
		float s3 = Mathf.Sin(euler.y / 2);
		
		var q = new Quaternion(
			s1 * s2 * c3 + c1 * c2 * s3,
			s1 * c2 * c3 + c1 * s2 * s3,
			c1 * s2 * c3 - s1 * c2 * s3,
			c1 * c2 * c3 - s1 * s2 * s3);
		
		return q;
	}
#endregion
	
	private static Quaternion InputAcceleration()
	{
		var accelerometer = AccelerometerOrientation();
		var k = Quaternion.identity;
		k.eulerAngles = new Vector3(-accelerometer.x, accelerometer.y, accelerometer.z);
		return k;
	}

	static readonly AngleFilter YFilter = new AngleFilter(2);
	private static Quaternion MagneticField()
	{
		var accelerometer = AccelerometerOrientation();
		var k = Quaternion.identity;
		
		if (Sensor.Get(Sensor.Type.MagneticField).gotFirstValue && Sensor.Get(Sensor.Type.Accelerometer).gotFirstValue)
		{
			gotFirstValue = true;
		}
		
		var yOffset = Sensor.accelerometer.z < 0 ? 180 : 0; 
		var y = YFilter.Update(Sensor.GetOrientation().x + yOffset);
		
		k.eulerAngles = new Vector3(-accelerometer.x, y, accelerometer.z);
		return k;
	}
		
	static readonly Vector3Filter AccFilter = new Vector3Filter(8);

    static SensorHelper()
    {
        gotFirstValue = false;
    }

    private static Vector3 AccelerometerOrientation()
	{
		var tilt = Input.acceleration;
		
		// seems to have changed in Unity > 4.0
		tilt = Quaternion.Euler(0,0,-90) * tilt;
		
		var xRot = tilt.z * 90;
		const float yRot = 0f;
		var zRot = tilt.y * 90;
		
		AccFilter.Update(new Vector3(xRot, yRot, zRot));
		
		return AccFilter.Value;
	}
	
	#endregion
}
