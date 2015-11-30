using UnityEngine;
using System.Collections;

public class BackToScene : MonoBehaviour {
	
	public string mainMenuName = "LoaderScene";
	public KeyCode key = KeyCode.Escape;
	public bool allowAutoRotation = false;

	public GUISkin skin;

	void Awake() {
		Screen.autorotateToLandscapeLeft = this.allowAutoRotation;
		Screen.autorotateToLandscapeRight = this.allowAutoRotation;
		Screen.autorotateToPortrait = this.allowAutoRotation;
		Screen.autorotateToPortraitUpsideDown = this.allowAutoRotation;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(this.key))
		{
		    this.GoBack();
		}
	}
	
	void GoBack() {
		Debug.Log("Loading: Menu");
		Application.LoadLevel(this.mainMenuName);
	}
	
	void OnGUI() {
		GUI.skin = this.skin;

		float dpiScaling = GUITools.DpiScaling;
		GUITools.SetFontSizes();

		GUILayout.BeginArea (new Rect(10,Screen.height - 60 * dpiScaling,200 * dpiScaling,50 * dpiScaling));
		
		if(GUILayout.Button ("Back To Menu", GUILayout.Height(50 * dpiScaling), GUILayout.Width(200 * dpiScaling)))
		{
		    this.GoBack();
		}
		
		GUILayout.EndArea();
	}
}
