using UnityEngine;
using System.Collections;

public class SetScreenRotation : MonoBehaviour {

	public ScreenOrientation so = ScreenOrientation.Landscape;

	// Use this for initialization
	void Awake () {
        Debug.Log(so);
		Screen.orientation = so;
	}
}
