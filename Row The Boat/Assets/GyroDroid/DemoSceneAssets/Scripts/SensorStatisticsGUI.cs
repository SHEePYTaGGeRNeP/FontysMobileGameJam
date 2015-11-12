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
		StartCoroutine(StartEverythingSlowly());

		for(int i = 0; i < W.Length; i++) {
			W[i] = (int) (W[i] * GUITools.DpiScaling);
		}
		for(int i = 0; i < W2.Length; i++) {
			W2[i] = (int) (W2[i] * GUITools.DpiScaling);
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
		showGui = true;
	}

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = SensorHelper.Rotation;
        transform.localRotation = Sensor.rotation;
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
		if (!showGui || !enableGUI) return;
		
		GUI.skin = guiSkin;
		GUITools.SetFontSizes();
		
		// show all sensors and values in a big, fat table
		// Remember : You can only see on your device what sensors are supported, not in the editor.
		GUI.color = _guiColor;
		
		GUILayout.BeginArea(new Rect(5,5,Screen.width-10, Screen.height-10));
		_scrollPosition = GUILayout.BeginScrollView(_scrollPosition);		
		
		GUILayout.BeginHorizontal();
		{
			C = 0;
			GUILayout.Label("Sensor", GUILayout.Width(W[C++]));
			// GUILayout.Label("#", GUILayout.Width(20));
			GUILayout.Label("Exists", GUILayout.Width(W[C++]));
			GUILayout.Label("Active", GUILayout.Width(W[C++]));
			GUILayout.Label("Name", GUILayout.Width(W[C++]));
			GUILayout.Label("Power", GUILayout.Width(W[C++]));
			GUILayout.Label("Resolution", GUILayout.Width(W[C++]));
			GUILayout.Label("MaxRange", GUILayout.Width(W[C++]));
			// GUILayout.Label("MinDelay", GUILayout.Width(60));
			GUILayout.Label("Values", GUILayout.Width(W[C++]));
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Label("");
		
		for (var i = 1; i <= Sensor.Count; i++)
		{
			C = 0;
			var s = Sensor.Get((Sensor.Type) i);
		    if (s == null)
		    {
		        continue;
		    }
		    GUILayout.BeginHorizontal();
		    {
		        GUILayout.Label("" + s.description, GUILayout.Width(W[C++]));
		        GUILayout.Label("" + (s.available?"Yes":"No"), GUILayout.Width(W[C++]));
		        GUILayout.Label(s.active ? "X" : "O", GUILayout.Width(W[C++]));
		        GUILayout.Label("" + s.name, GUILayout.Width(W[C++]));
		        GUILayout.Label("" + s.power, GUILayout.Width(W[C++]));
		        GUILayout.Label("" + s.resolution.ToString("F2"), GUILayout.Width(W[C++]));
		        GUILayout.Label("" + s.maximumRange, GUILayout.Width(W[C++]));
		        // GUILayout.Label("" + s.minDelay, GUILayout.Width(60));
		        GUILayout.Label("" + s.values, GUILayout.Width(W[C++]));
					
		        if (s.available)
		        {
		            if (GUILayout.Button(s.active?"Deactivate":"Activate", GUILayout.Width(W[0])))
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
		            GUILayout.Label("Not available", GUILayout.Width(W[0]));
		        }
		    }
		    GUILayout.EndHorizontal();
		}
		GUILayout.Label("");	
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Best rotation value", GUILayout.Width(W[0]));
			GUILayout.Label("(provided by SensorHelper.rotation)", GUILayout.Width(510));
			GUILayout.Label("" + SensorHelper.rotation, GUILayout.Width(W[0]));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("getOrientation", GUILayout.Width(W[0]));
			GUILayout.Label("Needs MagneticField and Accelerometer to be enabled. Gets fused from the two.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.GetOrientation(), GUILayout.Width(W[0]));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("Rotation Quaternion", GUILayout.Width(W[0]));
			GUILayout.Label("Calculated from rotation vector. Best accuracy.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.rotation, GUILayout.Width(W[0]));
			try
			{
				GUILayout.Label("" + Sensor.rotation.eulerAngles, GUILayout.Width(W[0]));
			}
			catch
			{
				
			}
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("getAltitude", GUILayout.Width(W[0]));
			GUILayout.Label("Calculated from pressure.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.GetAltitude(), GUILayout.Width(W[0]));
		}
		GUILayout.EndHorizontal();
	
		GUILayout.BeginHorizontal();
		{
			GUILayout.Label("SurfaceRotation", GUILayout.Width(W[0]));
			GUILayout.Label("Device surface rotation.", GUILayout.Width(510));
			GUILayout.Label("" + Sensor.surfaceRotation, GUILayout.Width(W[0]));
		}
		GUILayout.EndHorizontal();
		
		if (Input.gyro.enabled)
		{
	        GUILayout.BeginHorizontal();
		    {
	            GUILayout.Label("Gyro", GUILayout.Width(W[0]));
	            GUILayout.Label("Attitude", GUILayout.Width(W[0]));
		        GUILayout.Label("" + Input.gyro.attitude, GUILayout.Width(W[0]));
	            GUILayout.Label("" + Input.gyro.attitude.eulerAngles, GUILayout.Width(W[0]));
		    }
	        GUILayout.EndHorizontal();
		}
		
		if (Input.compass.enabled)
		{
	        GUILayout.BeginHorizontal();
		    {
	            GUILayout.Label("Compass", GUILayout.Width(W[0]));
	            GUILayout.Label("Raw Vector", GUILayout.Width(W[0]));
	            GUILayout.Label("" + Input.compass.rawVector, GUILayout.Width(W[0]));
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