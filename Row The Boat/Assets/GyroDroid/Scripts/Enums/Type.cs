public partial class Sensor
{
	// the sensor types usable
	public enum Type 
	{
	    Accelerometer = 1,
	    MagneticField = 2,
		Orientation = 3,
	    Gyroscope = 4,
	    Light = 5,
	    Pressure = 6,
	    Temperature = 7,
	    Proximity = 8,
	    Gravity = 9,
	    LinearAcceleration = 10,
	    RotationVector = 11,
		// added in GyroDroid 4.0
		RelativeHumidity = 12,
		AmbientTemperature = 13,
		// added in GyroDroid 5.0
		MagneticFieldUncalibrated = 14,
		GameRotationVector = 15,
		GyroscopeUncalibrated = 16,
		SignificantMotion = 17,
		StepDetector = 18,
		StepCounter = 19,
		GeomagneticRotationVector = 20
	}
}