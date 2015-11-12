using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaterOnCam : MonoBehaviour {

    public RectTransform canvas;
    public GameObject gameover;
    private bool moving;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float smoothTime = 0.3f;
    private Vector3 originalPosition;

	// Use this for initialization
	void Start () {
        moving = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            RaiseWater();

        if (transform.position.y >= canvas.GetComponent<RectTransform>().sizeDelta.y)
            gameover.SetActive(true);

        if (moving)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, transform.position.y + 40, transform.position.z), ref velocity, smoothTime);
            if (transform.position.y >= originalPosition.y + 40)
                moving = false;
        }
	}

    public void RaiseWater()
    {
        originalPosition = transform.position;
        moving = true;
        //transform.position = new Vector3(transform.position.x, transform.position.y + 40, transform.position.z);
        //if (transform.position.y >= canvas.GetComponent<RectTransform>().sizeDelta.y)
        //    gameover.SetActive(true);
    }
}
