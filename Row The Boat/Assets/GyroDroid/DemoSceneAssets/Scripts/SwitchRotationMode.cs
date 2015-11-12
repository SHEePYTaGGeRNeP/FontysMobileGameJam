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

public class SwitchRotationMode : MonoBehaviour {
	
	void Start() {
		useGUILayout = true;
	}
	
	void OnGUI () {
		GUITools.SetFontSizes();

		GUILayout.BeginArea(GUITools.Scale (new Rect(10,150 * GUITools.DpiScaling, 240, 500)));

		GUI.color = Color.black;
		GUILayout.Label("Different modes:");
		GUI.color = Color.white;
		if(GUILayout.Button ("Rotation Vector", GUILayout.Height (GUITools.DpiScaling * 40)))
//		if(GUI.Button(GUITools.Scale (new Rect(10,140,180,20)), "Rotation Vector"))
			SensorHelper.TryForceRotationFallback(RotationFallbackType.RotationQuaternion);
		
		if(GUILayout.Button ("Orientation/Acceleration", GUILayout.Height (GUITools.DpiScaling * 40)))
//		if(GUI.Button(GUITools.Scale (new Rect(10,165,180,20)), "Orientation/Acceleration"))
			SensorHelper.TryForceRotationFallback(RotationFallbackType.OrientationAndAcceleration);
		
		if(GUILayout.Button ("Magnetic Field", GUILayout.Height (GUITools.DpiScaling * 40)))
//		if(GUI.Button(GUITools.Scale (new Rect(10,190,180,20)), "Magnetic Field"))
			SensorHelper.TryForceRotationFallback(RotationFallbackType.MagneticField);
		GUI.color = Color.black;

//		GUILayout.Label("orientation: " + Sensor.orientation);
//		GUILayout.Label("sum: " + (Sensor.orientation.x + Sensor.orientation.y + Sensor.orientation.z));
//		GUILayout.Label("quat: " + Quaternion.Euler(Sensor.orientation).eulerAngles);
		
//		GUILayout.Space(20 * GUITools.DpiScaling);
//		GUI.color = Color.white;
//		if(GUILayout.Button("Orientation: LandscapeLeft", GUILayout.Height (GUITools.DpiScaling * 40))) Screen.orientation = ScreenOrientation.LandscapeLeft;
//		if(GUILayout.Button("Orientation: LandscapeRight", GUILayout.Height (GUITools.DpiScaling * 40))) Screen.orientation = ScreenOrientation.LandscapeRight;
//		if(GUILayout.Button("Orientation: Portrait", GUILayout.Height (GUITools.DpiScaling * 40))) Screen.orientation = ScreenOrientation.Portrait;
//		if(GUILayout.Button("Orientation: PortraitUpsideDown", GUILayout.Height (GUITools.DpiScaling * 40))) Screen.orientation = ScreenOrientation.PortraitUpsideDown;
		
		GUILayout.EndArea();
	}
}
