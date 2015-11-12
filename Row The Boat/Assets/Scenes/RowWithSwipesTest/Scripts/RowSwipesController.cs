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
		rb = transform.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		HandleSwipes ();
	}

	void HandleSwipes() 
	{
		if (Input.touchCount == 1) {
			Touch touch = Input.GetTouch(0);
			
			switch (touch.phase) {
			case TouchPhase.Began:
				couldBeSwipe = true;
				startPos = touch.position;
				swipeStartTime = Time.time;
				break;
				
			case TouchPhase.Ended:
				float swipeTime = Time.time - swipeStartTime;
				float swipeDist = (touch.position - startPos).magnitude;
				
				if (couldBeSwipe && swipeTime < maxSwipeTime && swipeDist > minSwipeDist) {
					// Swipe detected!
					
					couldBeSwipe = false; // Set to false so we can detect future swipes
					
					float absHorizontal = Mathf.Abs(touch.position.x - startPos.x); 
					float absVertical = Mathf.Abs(touch.position.y - startPos.y);
					
					if (absHorizontal > absVertical) //Checking if swipe was horizontal or vertical
					{
						if (Mathf.Sign(touch.position.x - startPos.x) == 1f)
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
						if (Mathf.Sign(touch.position.y - startPos.y) == 1f)
						{
							// Swiped Up
						}
						else 
						{
							Debug.Log(touch.position.x);
							// Swiped Down
							if (touch.position.x > (Screen.width / 2)) {
								rb.AddForce(Vector3.left * rowForce);
							} else {
								rb.AddForce(Vector3.right * rowForce);
							}
						}
					}
				}
				break;
			}
		}
	}
}
