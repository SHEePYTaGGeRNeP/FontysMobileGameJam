// prefrontal cortex -- http://prefrontalcortex.de
// Full Android Sensor Access for Unity3D
// Contact:
// 		contact@prefrontalcortex.de

using UnityEngine;
using System.Collections;

public class CheckAvailability : MonoBehaviour {
	
	public Color guiColor = Color.white;
	public string sceneDescription = "";
	
	public string CheckForErrors() {
		string goodOutput = "";
		string badOutput = "";
		
		for(int i = 1; i <= Sensor.Count; i++)
		{
			Sensor.Information info = Sensor.Get((Sensor.Type)i);
			if(info == null)
				continue;
			goodOutput += info.active && info.available ? "\n\t" + info.description  : "";
			badOutput  += info.active && !info.available ? "\n\t" + info.description : "";	
		}
		
		return "From the sensors needed for this scene, your device: \n" +
			"supports: "     + goodOutput + "\n";//  + 
//			"not supports: " + badOutput;
	}
	
	string message;
		
	void Start() {
	    this.message = "";	
	}
	
	void OnGUI() {
		if(this.message == "")
		    this.message = this.CheckForErrors();
		
		GUI.color = this.guiColor;
		
		GUILayout.BeginArea(GUITools.Scale (new Rect(10,10,Screen.width-20, 200)));
		if(this.sceneDescription != "") GUILayout.Label(this.sceneDescription);
		GUILayout.Label(this.message);
		GUILayout.EndArea();
	}
}
