using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Rotate : MonoBehaviour {
	public Vector3 v;
	// Update is called once per frame
	void Update ()
	{
	    this.transform.Rotate(this.v * Time.deltaTime);
	}
}
