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
using System;

public static class GUITools {
	public static bool Enabled = false;
	public static float dpiSimulated = 160.0f;

	public static float DpiScaling {
		get {
			if(Enabled) {
				if(Screen.dpi > 0)
					return Screen.dpi / 160.0f;
				else
					return dpiSimulated / 160.0f;
			}
			else
				return 1;
		}
	}

	public static void SetFontSizes() {
		GUI.skin.label.fontSize = (int)(16 * DpiScaling);
		GUI.skin.button.fontSize = (int)(16 * DpiScaling);

		GUI.skin.horizontalScrollbar.fixedHeight = (int) (32 * DpiScaling);
		GUI.skin.verticalScrollbar.fixedWidth = (int) (32 * DpiScaling);
		GUI.skin.horizontalScrollbarThumb.fixedHeight = (int) (33 * DpiScaling);
		GUI.skin.verticalScrollbarThumb.fixedWidth = (int) (33 * DpiScaling);
	}

	public static Rect Scale(Rect rect) {
		Rect r = rect;
		r.width *= DpiScaling;
		r.height *= DpiScaling;
		return r;
	}
}

public class LoaderGUI : MonoBehaviour {

	[Serializable]
	public class SceneData {
		public string name;
		public string path;
	}
	
	void Awake() {
		GUITools.Enabled = true;
		
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
		Screen.autorotateToPortrait = true;
		Screen.autorotateToPortraitUpsideDown = true;
	}
	
	void Disable() {
		GUITools.Enabled = false;
	}
	
	public SceneData[] scenes;
	public GUISkin guiSkin;

	void Start() {
		Sensor.Statistics.Send ();
	}

	Vector2 scrollVector = Vector2.zero;
	void OnGUI() {
		float dpiScaling = GUITools.DpiScaling;

		GUI.color = Color.white;
		GUI.skin = guiSkin;
		GUITools.SetFontSizes();

		// Screen.height - Screen.height / 5
		scrollVector = GUI.BeginScrollView(new Rect(Screen.width/2 - 100 * dpiScaling, Screen.height/10, 230 * dpiScaling, Screen.height * 0.8f), scrollVector, new Rect(0,0,200 * dpiScaling, (scenes.Length * 50 + 50) * dpiScaling));
		
//		GUILayout.BeginArea(new Rect(0, 0, 300, scenes.Length * 50));
		
		foreach(SceneData s in scenes)
		if(GUILayout.Button(s.name, GUILayout.Height(50 * dpiScaling), GUILayout.Width(230 * dpiScaling)))
		{
			Debug.Log("Loading scene: " + s.name);
			Application.LoadLevel(s.path);
		}
		
//		GUILayout.EndArea();
		
		GUI.EndScrollView();
	}
}
