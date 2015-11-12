#if UNITY_ANDROID

using UnityEngine;

class SensorDeviceAndroid : SensorDeviceUnity
{
    // the static reference to access the gyro values
    private AndroidJavaObject ao;

    protected override void AwakeDevice()
    {
        if (SystemInfo.supportsGyroscope)
        {
			Input.gyro.enabled = true;
		}
		
#if !UNITY_EDITOR
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject obj = jc.GetStatic<AndroidJavaObject>("currentActivity");
#else
		AndroidJavaObject obj = null;
#endif
		ao = new AndroidJavaObject("com.pfc.sensors.SensorClass", obj);

        // get all sensor informations (including whether they area available)
        for (var i = 1; i <= Sensor.Count; i++)
        {
            // fill the sensor information array
            Sensors[i] = new Information(
				ao.Call<bool>("isSensorAvailable", i),
				ao.Call<float>("getMaximumRange", i), 
				ao.Call<int>("getMinDelay", i),
                ao.Call<string>("getName", i), 
				ao.Call<float>("getPower", i), 
				ao.Call<float>("getResolution", i),
				ao.Call<string>("getVendor", i),
				ao.Call<int>("getVersion", i), 
				Description[i]
			);
        }
		
		Debug.Log("test");
		
        base.AwakeDevice();
    }
	
	protected Quaternion CompensateSurfaceRotation;
	protected override void DeviceUpdate()
	{
		CompensateSurfaceRotation = _getSurfaceRotationCompensation();
	}
	
    protected override bool ActivateDeviceSensor(Type sensorID, Sensor.Delay sensorSpeed)
    {
        if (!base.ActivateDeviceSensor(sensorID, sensorSpeed))
		{
			if (ao.Call<bool>("ActivateSensor", (int) sensorID, (int) sensorSpeed))
			{
				Sensors[(int)sensorID].active = true;
				return true;
			}
		}
		return false;
    }

    protected override bool DeactivateDeviceSensor(Type sensorID)
    {
        if (!base.DeactivateDeviceSensor(sensorID))
		{
			if (ao.Call<bool>("DeactivateSensor", (int) sensorID))
			{
				Sensors[(int)sensorID].active = false;
				return true;
			}
		}
		return false;
    }

    protected override Vector3 GetDeviceSensor(Type sensorID)
    {
		AndroidJNI.AttachCurrentThread();
		
        Get(sensorID).gotFirstValue = ao.Call<bool>("gotFirstValue", (int)sensorID);
		
		var ret = Vector3.zero;
		ret.x = ao.Call<float>("getValueX", (int)sensorID);
        ret.y = ao.Call<float>("getValueY", (int)sensorID);
        ret.z = ao.Call<float>("getValueZ", (int)sensorID);
		
		if (sensorID == Type.Orientation && (surfaceRotation == Sensor.SurfaceRotation.Rotation90 || surfaceRotation == Sensor.SurfaceRotation.Rotation270))
		{
			_swapXY(ref ret);
		}
		
		if(sensorID == Type.LinearAcceleration) {
			ret = Quaternion.Inverse(CompensateSurfaceRotation) * ret;
		}
	    return ret;
	}
	
    // Device sensors on Android obey the following relation:
	// accelerometer = gravity + linearAcceleration
	
	protected override Vector3 _getDeviceOrientation()
	{
//		if (!((Sensors.Length > 0) && Get(Type.MagneticField).active && Get(Type.Accelerometer).active))
//		{
//			Debug.Log("To use getOrientation, MagneticField and Accelerometer have to be active, because getOrientation internally fuses the two.\n " +
//				      "Magnetic Field: " + Get(Type.MagneticField).active + ", Accelerometer: " + Get(Type.Accelerometer).active);
//		}

	    var k = Vector3.zero;
		AndroidJNI.AttachCurrentThread();
        k.x = ao.Call<float>("getOrientationX") * Mathf.Rad2Deg;
        k.y = ao.Call<float>("getOrientationY") * Mathf.Rad2Deg;
        k.z = ao.Call<float>("getOrientationZ") * Mathf.Rad2Deg;
		
		if (surfaceRotation == Sensor.SurfaceRotation.Rotation90 || surfaceRotation == Sensor.SurfaceRotation.Rotation270)
		{
			// switch y and z
		    _swapYZ(ref k);
		}
		
		// compensate surface rotation
	    CompensateDeviceOrientation(ref k);
		
		return k;
	}

    protected override float GetDeviceAltitude(float pressure, float pressureAtSeaLevel = PressureValue.StandardAthmosphere)
    {
		const float coef = 1.0f / 5.255f;
        return 44330.0f * (1.0f - Mathf.Pow(pressure / pressureAtSeaLevel, coef)); // HACK switch them?
    }

    protected override Sensor.SurfaceRotation GetSurfaceRotation()
    {
        return (Sensor.SurfaceRotation)ao.Call<int>("getWindowRotation");
    }

    protected override Quaternion QuaternionFromDeviceRotationVector(Vector3 v)
    {
        var r = new Quaternion(-v.x, -v.y, v.z, Mathf.Sqrt(1 - v.sqrMagnitude));
		
        // switch axis 
        r = Quaternion.Euler(90, 0, 0) * r * CompensateSurfaceRotation;

        return r;
    }

    protected override void CompensateDeviceOrientation(ref Vector3 k)
    {
        // add or subtract x
        switch (surfaceRotation)
        {
            case Sensor.SurfaceRotation.Rotation90:
                k.x += 90;
                break;
            case Sensor.SurfaceRotation.Rotation270:
                k.x -= 90;
                break;
            case Sensor.SurfaceRotation.Rotation180:
                k.x += 180;
                break;
        }
    }
	
	protected override ScreenOrientation ScreenOrientationDevice {
		get {
			if(
				(surfaceRotation == Sensor.SurfaceRotation.Rotation0 && Screen.orientation == ScreenOrientation.LandscapeLeft) ||
				(surfaceRotation == Sensor.SurfaceRotation.Rotation270 && Screen.orientation == ScreenOrientation.Portrait) ||
				(surfaceRotation == Sensor.SurfaceRotation.Rotation180 && Screen.orientation == ScreenOrientation.LandscapeRight) ||
				(surfaceRotation == Sensor.SurfaceRotation.Rotation90 && Screen.orientation == ScreenOrientation.PortraitUpsideDown)
			)
				return ScreenOrientation.LandscapeLeft;
			else
				return ScreenOrientation.Portrait;
		}
	}
	
	private static void _swapXY(ref Vector3 k)
	{
		var temp = k.y;
		k.y = -k.z;
		k.z = temp;
	}

    private static void _swapYZ(ref Vector3 k)
    {
        var temp = k.y;
        k.y = k.z;
        k.z = temp;
    }
	
	private Quaternion _getSurfaceRotationCompensation()
    {
        switch (surfaceRotation)
        {
            case Sensor.SurfaceRotation.Rotation90:
                return Quaternion.Euler(0, 0, -90);
            case Sensor.SurfaceRotation.Rotation270:
                return Quaternion.Euler(0, 0, 90);
            case Sensor.SurfaceRotation.Rotation180:
                return Quaternion.Euler(0, 0, 180);
            default:
                return Quaternion.Euler(0, 0, 0);
        }
    }
}
#endif