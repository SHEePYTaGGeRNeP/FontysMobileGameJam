using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class MoveCam : MonoBehaviour
{
    private Vector3 originalPos;
    private Vector3 randomPos;
    private Transform camTransform;
    public Transform lookAt;

	// Use this for initialization
	void Start () 
    {
	    this.camTransform = this.GetComponent<Camera>().transform;
	    this.originalPos = this.camTransform.position;

	    this.randomPos = this.originalPos + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-1, 1));
    }
	
	// Update is called once per frame
    private void Update()
    {
        this.camTransform.position = Vector3.Slerp(this.camTransform.position, this.randomPos, Time.deltaTime);
        this.camTransform.LookAt(this.lookAt);
        if (Vector3.Distance(this.camTransform.position, this.randomPos) < 0.5f)
        {
            this.randomPos = this.originalPos + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), Random.Range(-1, 1));
        }
    }
}
