using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    [SerializeField]
    private float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    this.transform.Translate(Input.GetAxis("Horizontal") * Time.deltaTime * this.speed, 0, Input.GetAxis("Vertical") * Time.deltaTime * this.speed);
    }
}
