
using UnityEngine;

class SensorEditorUnity : Sensor
{
	// for debugging the values in the editor
	public bool accelerometerAvailable = true;
	public bool magneticFieldAvailable = true;
	public bool orientationAvailable = true;
	public bool gyroscopeAvailable = true; 			
	public bool lightAvailable = true; 				
	public bool pressureAvailable = true; 			
	public bool temperatureAvailable = true; 		
	public bool proximityAvailable = true; 		
	public bool gravityAvailable = true; 			
	public bool linearAccelerationAvailable = true;
	public bool rotationVectorAvailable = true;
	public bool ambientTemperatureAvailable = true;
	public bool relativeHumidityAvailable = true;	
	
	// Actual Values
    public Vector3 accelerometerDebugValue = Vector3.zero;
    public Vector3 magneticFieldDebugValue = Vector3.zero;
    public Vector3 orientationDebugValue = Vector3.zero;
    public Vector3 gyroscopeDebugValue = Vector3.zero;
    public float lightDebugValue = 0;
    public float pressureDebugValue = 0;
    public float temperatureDebugValue = 0;
    public float proximityDebugValue = 0;
    public Vector3 gravityDebugValue = Vector3.zero;
    public Vector3 linearAccelerationDebugValue = Vector3.zero;
    public Vector3 rotationVectorDebugValue = new Vector3(0, -270.0f, 0);
    public Vector3 getOrientationDebugValue = Vector3.zero;
	public float ambientTemperatureDebugValue = 0;
	public float relativeHumidityDebugValue = 0;
		
//#if (!UNITY_ANDROID && !UNITY_IPHONE) || UNITY_EDITOR
	
    private const float AltitudeCoef = 1.0f / 5.255f;
	
	protected override void AwakeDevice()
    {
		if(!Application.isEditor) {
			Singleton = null;
			Destroy(this);
		}
		
		for (var i = 1; i <= Sensor.Count; i++) 
		{
			// fill the sensor information array with debug values
			Sensors[i] = new Information(this.GetSensorDebugAvailable(i), 1, 0, "DEBUG", 0, 100, "DEBUG", 0, Description[i]);
		}
	}

	protected override void SetSensorOn(Type sensorID) {
		base.SetSensorOn(sensorID);
		Input.gyro.enabled = true;
		Input.compass.enabled = true;
	}

    protected override void DisableDevice()
    {
        // Nothing to do
    }
	
    protected override bool ActivateDeviceSensor(Type sensorID, Sensor.Delay sensorSpeed)
    {
		Input.gyro.enabled = true;
		Input.compass.enabled = true;
		Get (sensorID).available = this.GetSensorDebugAvailable((int)sensorID);
		Get (sensorID).active = true;
		return true;
	}
	
	protected override bool DeactivateDeviceSensor(Type sensorID)
    {
		Get (sensorID).active = false;
		return true;
	}
	
	public static SensorEditorUnity Instance {
		get {
			return (SensorEditorUnity) SensorEditorUnity.Singleton;
		}
	}
	
	public void PullDeviceSensor(Type sensorID) {
		var val = Get(sensorID).values;
		
		switch (sensorID)
		{
		    case Type.Accelerometer:
		        this.accelerometerDebugValue = val;
				break;
		    case Type.Gravity:
		        this.gravityDebugValue = val;
				break;
		    case Type.Gyroscope:
		        this.gyroscopeDebugValue = val;
				break;
		    case Type.Light:
		        this.lightDebugValue = val.x;
				break;
		    case Type.LinearAcceleration:
		        this.linearAccelerationDebugValue = val;
				break;
		    case Type.MagneticField:
		        this.magneticFieldDebugValue = val;
				break;
		    case Type.Orientation:
		        this.orientationDebugValue = val;
				break;
		    case Type.Pressure:
		        this.pressureDebugValue = val.x;
				break;
		    case Type.Proximity:
		        this.proximityDebugValue = val.x;
				break;
		    case Type.RotationVector:
		        this.rotationVectorDebugValue = val;
				break;
		    case Type.Temperature:
		        this.temperatureDebugValue = val.x;
				break;
			case Type.AmbientTemperature:
		        this.ambientTemperatureDebugValue = val.x;
				break;
			case Type.RelativeHumidity:
		        this.relativeHumidityDebugValue = val.x;
				break;
		}
	}
	Quaternion lastGyroAttitude = Quaternion.identity;
	Vector3 lastAcceleration;

	protected override Vector3 GetDeviceSensor(Type sensorID)
    {
		Get(sensorID).gotFirstValue = true;
			
	    // if not everything is fine, use debug values (can be set in the inspector)
	    switch (sensorID)
	    {
	        case Type.Accelerometer:
				if(Vector3.Distance(Input.acceleration, this.lastAcceleration) > 0.001f )
					return Input.acceleration;
	            this.lastAcceleration = Input.acceleration;
	            return this.accelerometerDebugValue;
	        case Type.Gravity:
	            return this.gravityDebugValue;
	        case Type.Gyroscope:
	            return this.gyroscopeDebugValue;
	        case Type.Light:
	            return new Vector3(this.lightDebugValue, 0, 0);
	        case Type.LinearAcceleration:
	            return this.linearAccelerationDebugValue;
	        case Type.MagneticField:
	            return this.magneticFieldDebugValue;
	        case Type.Orientation:
	            return this.orientationDebugValue;
	        case Type.Pressure:
	            return new Vector3(this.pressureDebugValue, 0, 0);
	        case Type.Proximity:
	            return new Vector3(this.proximityDebugValue, 0, 0);
	        case Type.RotationVector:
				if(Quaternion.Angle (Input.gyro.attitude, this.lastGyroAttitude) > 0.001f)
					return -(Quaternion.Euler (-90,0,0) * Input.gyro.attitude).eulerAngles;
	            this.lastGyroAttitude = Input.gyro.attitude;
	            return this.rotationVectorDebugValue;
	        case Type.Temperature:
	            return new Vector3(this.temperatureDebugValue, 0, 0);
			case Type.AmbientTemperature:
				return new Vector3(this.ambientTemperatureDebugValue, 0, 0);
			case Type.RelativeHumidity:
				return new Vector3(this.relativeHumidityDebugValue, 0, 0);
            default:
	            return Vector3.zero;
	    }
	}
	
	protected override Vector3 _getDeviceOrientation()
	{
		return this.getOrientationDebugValue;
	}
	
	protected override float GetDeviceAltitude(float pressure, float pressureAtSeaLevel = PressureValue.StandardAthmosphere)
    {
        if (pressure == 0)
        {
            return 0;
        }
        return 44330.0f * (1.0f - Mathf.Pow(pressure / pressureAtSeaLevel, AltitudeCoef));
		//	return -7 * Mathf.Log((pressure / 1000) / (pressureAtSeaLevel / 1000)) * 1000;
	}
	
	protected override Sensor.SurfaceRotation GetSurfaceRotation()
	{
		return Sensor.SurfaceRotation.Rotation0;
	}
	
	protected override Quaternion QuaternionFromDeviceRotationVector(Vector3 v)
	{
		return Quaternion.Euler( v ); // Get (Type.RotationVector).values);
	}
	
	protected override void CompensateDeviceOrientation(ref Vector3 k)
    {
	}
	
	protected Quaternion _getSurfaceRotationCompensation()
    {
    	return Quaternion.Euler(0, 0, 0);
	}
	
	// Available-Checkbox for every sensor for testing whether sensor fallback etc. works
	private bool GetSensorDebugAvailable(int id)
	{
		switch ((Type) id)
		{
			case Type.Accelerometer:
				return this.accelerometerAvailable;
			case Type.Gravity:
				return this.gravityAvailable;
			case Type.Gyroscope:
				return this.gyroscopeAvailable;
			case Type.Light:
				return this.lightAvailable;
			case Type.LinearAcceleration:
				return this.linearAccelerationAvailable;
			case Type.MagneticField:
				return this.magneticFieldAvailable;
			case Type.Orientation:
				return this.orientationAvailable;
			case Type.Pressure:
				return this.pressureAvailable;
			case Type.Proximity:
				return this.proximityAvailable;
			case Type.RotationVector:
				return this.rotationVectorAvailable;
			case Type.Temperature:
				return this.temperatureAvailable;
			case Type.AmbientTemperature:
				return this.ambientTemperatureAvailable;
			case Type.RelativeHumidity:
				return this.relativeHumidityAvailable;
			default:
				return false;
		}
	}
	
	protected override ScreenOrientation ScreenOrientationDevice {
		get {
			return Screen.width >= Screen.height ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait;
		}
	}
	
//#endif
}

