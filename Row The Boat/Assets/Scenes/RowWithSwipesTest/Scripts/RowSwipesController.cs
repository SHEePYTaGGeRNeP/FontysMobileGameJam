using UnityEngine;
using System.Collections;

public class RowSwipesController : MonoBehaviour {


	public float rowForce;

	Rigidbody rb;

	float swipeStartTime;
	bool couldBeSwipe;
	Vector2 startPos;

	float minSwipeDist = 50;
	float maxSwipeTime = 10;

	// Use this for initialization
	void Start () {
	    this.rb = this.transform.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    this.HandleSwipes ();
	}

	void HandleSwipes() 
	{
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch(0);
			
			switch (touch.phase) {
			case TouchPhase.Began:
			        this.couldBeSwipe = true;
			        this.startPos = touch.position;
			        this.swipeStartTime = Time.time;
				break;
				
			case TouchPhase.Ended:
				float swipeTime = Time.time - this.swipeStartTime;
				float swipeDist = (touch.position - this.startPos).magnitude;
				
				if (this.couldBeSwipe && swipeTime < this.maxSwipeTime && swipeDist > this.minSwipeDist) {
					// Swipe detected!

				    this.couldBeSwipe = false; // Set to false so we can detect future swipes
					
					float absHorizontal = Mathf.Abs(touch.position.x - this.startPos.x); 
					float absVertical = Mathf.Abs(touch.position.y - this.startPos.y);
					
					if (absHorizontal > absVertical) //Checking if swipe was horizontal or vertical
					{
						if (Mathf.Sign(touch.position.x - this.startPos.x) == 1f)
						{
							//Swiped Right
							Debug.Log("Swiped RIGHT!");

						}
						else 
						{
							//Swiped Left
							Debug.Log("Swiped LEFT!");
						}
					}
					else 
					{
						if (Mathf.Sign(touch.position.y - this.startPos.y) == 1f)
						{
							// Swiped Up
						}
						else 
						{
							Debug.Log(touch.position.x);
							// Swiped Down
							if (touch.position.x > (Screen.width / 2)) {
							    this.rb.AddForce(Vector3.left * this.rowForce);
							} else {
							    this.rb.AddForce(Vector3.right * this.rowForce);
							}
						}
					}
				}
				break;
			}
		}
	}
}
