using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaterOnCam : MonoBehaviour {

    public RectTransform canvas;
    public GameObject gameover;
    private bool moving;
    private bool alive;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private float smoothTime = 0.3f;
    private Vector3 originalPosition;
    [SerializeField]
    private AudioSource creaking;
    [SerializeField]
    private AudioSource sinking;

	// Use this for initialization
	void Start () {
	    this.moving = false;
	    this.alive = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.C) && this.alive)
            this.RaiseWater();

        if (this.transform.position.y >= this.canvas.GetComponent<RectTransform>().sizeDelta.y)
        {
            this.sinking.Play();
            //gameover.SetActive(true);
            this.alive = false;
            Application.LoadLevel("GameOverMenu");
        }
            

        if (this.moving)
        {
            this.transform.position = Vector3.SmoothDamp(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y + 40, this.transform.position.z), ref this.velocity, this.smoothTime);
            if (this.transform.position.y >= this.originalPosition.y + 40)
                this.moving = false;
        }            
	}

    public void RaiseWater()
    {
        this.creaking.Play();
        this.originalPosition = this.transform.position;
        this.moving = true;
        //transform.position = new Vector3(transform.position.x, transform.position.y + 40, transform.position.z);
        //if (transform.position.y >= canvas.GetComponent<RectTransform>().sizeDelta.y)
        //    gameover.SetActive(true);
    }
}
