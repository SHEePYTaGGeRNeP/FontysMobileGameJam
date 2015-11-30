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

public class SensorStatisticsGUI : MonoBehaviour
{
    public GUISkin guiSkin;
	
	void Start () 
	{
	    this.StartCoroutine(this.StartEverythingSlowly());

		for(int i = 0; i < this.W.Length; i++) {
		    this.W[i] = (int) (this.W[i] * GUITools.DpiScaling);
		}
		for(int i = 0; i < this.W2.Length; i++) {
		    this.W2[i] = (int) (this.W2[i] * GUITools.DpiScaling);
		}
	}
	
	IEnumerator StartEverythingSlowly()
	{
		// Some devices seem to crash when activating a lot of sensors immediately.
		// To prevent this for this scene, we start one sensor per frame.

		for(int i = 1; i <= Sensor.Count; i++)
		{
			Sensor.Activate((Sensor.Type) i);
			yield return new WaitForSeconds(0.1f);
		}	
		
		// wait a moment
		yield return new WaitForSeconds(0.2f);

		Debug.Log ("tried to activate all sensors");

		// activate GUI
	    this.showGui = true;
	}

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = SensorHelper.Rotation;
        this.transform.localRotation = Sensor.rotation;
	}
		
	Vector2 _scrollPosition;
    readonly Color _guiColor = new Color(0.8f,0.8f,0.8f); 
	public bool enableGUI = true;
	bool showGui = false;
	int[] W = new int[]{120,50,50,200,60,85,90,130};
	int[] W2 = new int[]{510};
	
	int C = 0;
	void OnGUI()
	{
		if (!this.showGui || !this.enableGUI) return;
		
		GUI.skin = this.guiSkin;
		GUITools.SetFontSizes();
		
		// show all sensors and values in a big, fat table
		// Remember : You can only see on your device what sensors are supported, not in the editor.
		GUI.color = this._guiColor;
		
		GUILayout.BeginArea(new Rect(5,5,Screen.width-10, Screen.height-10));
	    this._scrollPosition = GUILayout.BeginScrollView(this._scrollPosition);		
		
		GUILayout.BeginHorizontal();
		{
		    this.C = 0;
			GUILayout.Label("Sensor", GUILayout.Width(this.W[this.C++]));
			// GUILayout.Label("#", GUILayout.Width(20));
			GUILayout.Label("Exists", GUILayout.Width(this.W[this.C++]));
			GUILayout.Label("Active", GUILayout.Width(this.W[this.C++]));
			GUILayout.Label("Name", GUILayout.Width(this.W[this.C++]));
			GUILayout.Label("Power", GUILayout.Width(this.W[this.C++]));
			GUILayout.Label("Resolution", GUILayout.Width(this.W[this.C++]));
			GUILayout.Label("MaxRange", GUILayout.Width(this.W[this.C++]));
			// GUILayout.Label("MinDelay", GUILayout.Width(60));
			GUILayout.Label("Values", GUILayout.Width(this.W[this.C++]));
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Label("");
		
		for (var i = 1; i <= Sensor.Count; i++)
		{
		    this.C = 0;
			var s = Sensor.Get((Sensor.Type) i);
		    if (s == null)
		    {
		        continue;
		    }
		    GUILayout.BeginHorizontal();
		    {
		        GUILayout.Label("" + s.description, GUILayout.Width(this.W[this.C++]));
		        GUILayout.Label("" + (s.available?"Yes":"No"), GUILayout.Width(this.W[this.C++]));
		        GUILayout.Label(s.active ? "X" : "O", GUILayout.Width(this.W[this.C++]));
		        GUILayout.Label("" + s.name, GUILayout.Width(this.W[this.C++]));
		        GUILayout.Label("" + s.power, GUILayout.Width(this.W[this.C++]));
		        GUILayout.Label("" + s.resolution.ToString("F2"), GUILayout.Width(this.W[this.C++]));
		        GUILayout.Label("" + s.maximumRange, GUILayout.Width(this.W[this.C++]));
		        // GUILayout.Label("" + s.minDelay, GUILayout.Width(60));
		        GUILayout.Label("" + s.values, GUILayout.Width(this.W[this.C++]));
					
		        if (s.available)
		        {
		            if (GUILayout.Button(s.active?"Deactivate":"Activate", GUILayout.Width(this.W[0])))
		            {
		                if (s.active)
		                {
		                    Sensor.Deactivate((Sensor.Type) i);
		                }
		                else
		                {
		                    Sensor.Activate((Sensor.Type) i);
		                }
		            }
		        }
		        else
		        {
		            GUILayout.Label("Not available", GUILayout.Width(this.W[0]));
		        }
		    }
		    GUILayout.EndHorizontal();
		}
		GUILayout.Label("");	
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Best rotation value", GUILayout.Width(this.W[0]));
			GUILayout.Label("(provided by SensorHelper.rotation)", GUILayout.Width(510));
			GUILayout.Label("" + SensorHelper.rotation, GUILayout.Width(this.W[0]));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("getOrientation", GUILayout.Width(this.W[0]));
			GUILayout.Label("Needs MagneticField and Accelerometer to be enabled. Gets fused from the two.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.GetOrientation(), GUILayout.Width(this.W[0]));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Rotation Quaternion", GUILayout.Width(this.W[0]));
			GUILayout.Label("Calculated from rotation vector. Best accuracy.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.rotation, GUILayout.Width(this.W[0]));
			try
			{
				GUILayout.Label("" + Sensor.rotation.eulerAngles, GUILayout.Width(this.W[0]));
			}
			catch
			{
				
			}
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("getAltitude", GUILayout.Width(this.W[0]));
			GUILayout.Label("Calculated from pressure.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.GetAltitude(), GUILayout.Width(this.W[0]));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("SurfaceRotation", GUILayout.Width(this.W[0]));
			GUILayout.Label("Device surface rotation.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.surfaceRotation, GUILayout.Width(this.W[0]));
		}
		GUILayout.EndHorizontal();
		
		if (Input.gyro.enabled)
		{
	        GUILayout.BeginHorizontal();
		    {
	            GUILayout.Label("Gyro", GUILayout.Width(this.W[0]));
	            GUILayout.Label("Attitude", GUILayout.Width(this.W[0]));
		        GUILayout.Label("" + Input.gyro.attitude, GUILayout.Width(this.W[0]));
	            GUILayout.Label("" + Input.gyro.attitude.eulerAngles, GUILayout.Width(this.W[0]));
		    }
	        GUILayout.EndHorizontal();
		}
		
		if (Input.compass.enabled)
		{
	        GUILayout.BeginHorizontal();
		    {
	            GUILayout.Label("Compass", GUILayout.Width(this.W[0]));
	            GUILayout.Label("Raw Vector", GUILayout.Width(this.W[0]));
	            GUILayout.Label("" + Input.compass.rawVector, GUILayout.Width(this.W[0]));
	        }
	        GUILayout.EndHorizontal();
		}
		if (Application.isEditor)
		{
			GUILayout.Label("WARNING: Sensors can only be accessed on an Android device, not in the editor.");
		}

		GUILayout.Space(40);

	    GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
}