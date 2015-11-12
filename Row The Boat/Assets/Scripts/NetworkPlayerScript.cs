using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkPlayerScript : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis("Horizontal") * Time.deltaTime;
		this.transform.position = new Vector3(this.transform.position.x + x , this.transform.position.y, this.transform.position.z);

	}



}
