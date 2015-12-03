using UnityEngine;
using System.Collections;

public class SwitchCamera : MonoBehaviour {
	
	public Camera groundCamera;
	public Camera airCamera;
	
	Rect r = new Rect(Screen.width - 150 * GUITools.DpiScaling,10,140 * GUITools.DpiScaling,50 * GUITools.DpiScaling);
	void OnGUI() {
		if(GUI.Button(this.r, "Switch Camera"))
		    this.SwitchCam();
	}
	
	void SwitchCam() {
#pragma warning disable 0618
	    this.groundCamera.gameObject.active = !this.groundCamera.gameObject.active;
	    this.airCamera.gameObject.active = !this.groundCamera.gameObject.active;
#pragma warning restore
	}
}
