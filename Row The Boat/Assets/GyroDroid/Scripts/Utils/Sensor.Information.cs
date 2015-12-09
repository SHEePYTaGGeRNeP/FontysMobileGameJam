
using UnityEngine;

// this class holds all information available on a sensor

public partial class Sensor
{
	[System.Serializable]
	public class Information
	{
	    public Information(bool available, float maximumRange, int minDelay, string name, float power, float resolution, string vendor, int version, string description)
	    {
	        this.gotFirstValue = false;
	        this.available = available;
	        this.maximumRange = maximumRange;
	        this.minDelay = minDelay;
	        this.name = name;
	        this.power = power;
	        this.resolution = resolution;
	        this.vendor = vendor;
	        this.version = version;
	        this.description = description;
	    }
	
	    // describes the sensor
	    public string description { get; private set; }
	
	    // is the sensor available on the device?
	    public bool available { get; /*private*/ set; }
	
	    // is the sensor currently registered and active?
	    // (has to be active to provide values)
		bool _active;
	    public bool active {
			get {
				return this._active;
			}
			internal set {
			    this._active = value;
			    this.gotFirstValue = false;
			    this.SetValue(Vector3.zero);
			}
		}
	
	    public bool gotFirstValue { get; /*internal*/ set; }
	
	    // if suspended is set, OnApplicationPause will active this sensor
	    internal bool suspended;
	
	    // last fetched value
	    public Vector3 values { get; private set; }
	
	    // for internal use		
	    public void SetValue(Vector3 v)
	    {
	        this.values = v;
	    }
	
	    // maximumRange as reported by the device
	    public float maximumRange { get; private set; }
	
	    // minDelay as reported by the device
	    public int minDelay { get; private set; }
	
	    // name as reported by the device
	    public string name { get; private set; }
	
	    // power consumption as reported by the device.
	    // always deactivate sensors you don't need by calling Sensor.Deactivate(SensorType.<yourType>);
	    public float power { get; private set; }
	
	    // sensor resolution as reported by the device
	    public float resolution { get; private set; }
	
	    // sensor vendor as reported by the device
	    public string vendor { get; private set; }
	
	    // sensor version as reported by the device
	    public int version { get; private set; }
	
	    public override string ToString()
	    {
	        return string.Format("Vendor: {0}, Resolution: {1}, MinDelay: {2}, MaxRange: {3}, Value: {4}", this.vendor, this.resolution, this.minDelay, this.maximumRange, this.values.ToString());
	    }


		//public void Serialize(BitStream stream)
		//{
		//	bool _gotFirstValue = false;
		//	bool _available = false;
		//	Vector3 _values = Vector3.zero;

		//	if(stream.isWriting)
		//	{
		//		_available = this.available;
		//		_gotFirstValue = this.gotFirstValue;
		//		_values = this.values;
				
		//		stream.Serialize (ref _gotFirstValue);
		//		stream.Serialize(ref _available);
		//		stream.Serialize(ref _values);
		//	}
		//	else if(stream.isReading)
		//	{
		//		stream.Serialize (ref _gotFirstValue);
		//		stream.Serialize(ref _available);
		//		stream.Serialize(ref _values);

		//	    this.gotFirstValue = _gotFirstValue;
		//	    this.available = _available;
		//	    this.values = _values;
		//	}
		//}
	}
}