using UnityEngine;
using System.Collections;

public class MoveToPoint : MonoBehaviour {
	
	Vector3 lastPoint;
	RaycastHit hit;
	
	Touch t;
	Touch lastTouch;
	
	bool haveLastTouch = false;
	
	// Update is called once per frame
	void Update () {
		if(this.haveLastTouch) {
			if(Physics.Raycast(Camera.main.ScreenPointToRay(this.lastTouch.position), out this.hit, Mathf.Infinity)) {
			    this.lastPoint = this.hit.point;
			}
		}
		
		if(this.haveLastTouch = (Input.touchCount == 1)) {
		    this.t = Input.GetTouch(0);
			
			if(Physics.Raycast(Camera.main.ScreenPointToRay(this.t.position), out this.hit, Mathf.Infinity)) {
				if(this.t.phase != TouchPhase.Began && this.t.phase != TouchPhase.Ended) {
					// move by distance between last and current point
				    this.transform.position += this.lastPoint - this.hit.point;
				}

			    this.lastPoint = this.hit.point;
			}

		    this.lastTouch = this.t;
		}
	}
}
