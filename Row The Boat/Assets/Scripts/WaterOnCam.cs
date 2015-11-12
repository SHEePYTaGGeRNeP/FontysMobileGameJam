using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaterOnCam : MonoBehaviour {

    public RectTransform canvas;
    public GameObject gameover;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            RaiseWater();
	}

    public void RaiseWater()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 40, transform.position.z);
        if (transform.position.y >= canvas.GetComponent<RectTransform>().sizeDelta.y)
            gameover.SetActive(true);
    }
}
