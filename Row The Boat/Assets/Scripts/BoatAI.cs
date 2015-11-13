using UnityEngine;
using System.Collections;

public class BoatAI : MonoBehaviour {

    [SerializeField]
    private Transform _rechtsvoor, _linksvoor, _rechtsachter, _linksachter, _achter;
    [SerializeField]
    private Transform _rayLeft, _rayCenter, _rayRight;
    [SerializeField]
    private float _forceMultiplier;

    public float _paddleInterval;

    public float _randomPercentage = 25;
    public float _rayInterval = 1;
    public float _rayLength = 10;
    public float _maxAngle = 40;

    private float nextPaddle;
    private float nextRaycast;
    private bool angleCorrect = false;
    
    private Rigidbody _rb;


    // Use this for initialization
    void Start()
    {
        this._rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool skipLeft = false;
        bool skipRight = false;

        float yRotation = transform.rotation.eulerAngles.y;
        //Debug.Log("Y Rotation: " + transform.rotation.eulerAngles);

        angleCorrect = false;

        if(yRotation > _maxAngle && yRotation < 180)
        {
            angleCorrect = true;
            skipLeft = true;
        }

        else if(yRotation > 180 && yRotation < (360 -_maxAngle))
        {
            angleCorrect = true;
            skipRight = true;
        }

        else if(Time.time > nextPaddle)
        {
            RaycastHit rayHitLeft;
            RaycastHit rayHitCenter;
            RaycastHit rayHitRight;

            nextRaycast = Time.time + _rayInterval;

            Ray leftRay = new Ray(_rayLeft.position, transform.forward);
            Ray centerRay = new Ray(_rayCenter.position, transform.forward);
            Ray rightRay = new Ray(_rayRight.position, transform.forward);

            Physics.Raycast(leftRay, out rayHitLeft, _rayLength);
            Physics.Raycast(centerRay, out rayHitCenter, _rayLength);
            Physics.Raycast(rightRay, out rayHitRight, _rayLength);
            

            if (rayHitCenter.distance > 0)
            {
                Debug.Log("Center start");
                Debug.DrawLine(_rayCenter.position, rayHitCenter.point, Color.green, _rayInterval);

                    RaycastHit hitLeft;
                    RaycastHit hitRight;

                    Ray rayLeft = new Ray(_rayLeft.position, transform.forward);
                    Ray rayRight = new Ray(_rayRight.position, transform.forward);

                    Physics.Raycast(rayLeft, out hitLeft, _rayLength * 1.5f);
                    Physics.Raycast(rayRight, out hitRight, _rayLength * 1.5f);

                    
                    if (hitLeft.distance > 0 && hitRight.distance > 0)
                    {
                        if (hitLeft.distance > hitRight.distance)
                        {
                            skipLeft = true;
                            Debug.DrawLine(_rayRight.position, hitRight.point, Color.blue, _rayInterval);
                        }
                        else
                        {
                            skipRight = true;
                            Debug.DrawLine(_rayLeft.position, hitLeft.point, Color.red, _rayInterval);
                        }
                    }

                    else if (hitLeft.distance > 0)
                    {
                        skipRight = true;
                        Debug.DrawLine(_rayLeft.position, hitLeft.point, Color.white, _rayInterval);
                    }
                    else if (hitRight.distance > 0)
                    {
                        skipLeft = true;
                        Debug.DrawLine(_rayRight.position, hitRight.point, Color.yellow, _rayInterval);
                    }
                        
                    else
                    {
                        if (Mathf.Round(Random.Range(0, 1)) == 0)
                            skipLeft = true;
                        else
                            skipRight = true;
                    }

                //else if (rayHitLeft.distance > 0)
                //{
                //    Debug.DrawLine(_rayLeft.position, rayHitLeft.point, Color.red, _rayInterval);
                //    skipRight = true;
                //}

                //else if (rayHitRight.distance > 0)
                //{
                //    Debug.DrawLine(_rayRight.position, rayHitRight.point, Color.blue, _rayInterval);
                //    skipLeft = true;
                //}
            }

            else if (rayHitLeft.distance > 0)
            {
                Debug.DrawLine(_rayLeft.position, rayHitLeft.point, Color.red, _rayInterval);
                skipRight = true;
            }

            else if(rayHitRight.distance > 0 )
            {
                Debug.DrawLine(_rayRight.position, rayHitRight.point, Color.blue, _rayInterval);
                skipLeft = true;
            }

            //else if( rayHitLeft.distance > 0 && rayHitCenter.distance > 0 && rayHitRight.distance > 0)
            //{
            //    if (Mathf.Round(Random.Range(0, 1)) == 0)
            //        skipLeft = true;
            //    else
            //        skipRight = true;
            //}
        }

        if(Time.time > nextPaddle)
        {
            float forceMultiplier = _forceMultiplier;

            if (angleCorrect)
                forceMultiplier /= 2;

            Debug.Log("Skip Left Side: " + skipLeft);
            Debug.Log("Skip Right Side: " + skipRight);
            nextPaddle = Time.time + _paddleInterval;

            int r = Random.Range(0, 100);

            if (r <= _randomPercentage && !skipLeft)
                paddleLeftFront(forceMultiplier);

            r = Random.Range(0, 100);

            if (r <= _randomPercentage && !skipRight)
                paddleRightFront(forceMultiplier);

            r = Random.Range(0, 100);

            if (r <= _randomPercentage && !skipLeft)
                paddleLeftBack(forceMultiplier);

            r = Random.Range(0, 100);

            if (r <= _randomPercentage && !skipRight)
                paddleRightBack(forceMultiplier);
        }
    }

    private void paddleLeftFront(float forceMultiplier)
    {
        this._rb.AddForceAtPosition(this.transform.forward * forceMultiplier, this._linksvoor.position);
        this._rb.AddForceAtPosition(this.transform.forward * forceMultiplier, this._achter.position);
    }

    private void paddleRightFront(float forceMultiplier)
    {
        this._rb.AddForceAtPosition(this.transform.forward * forceMultiplier, this._rechtsvoor.position);
        this._rb.AddForceAtPosition(this.transform.forward * forceMultiplier, this._achter.position);
    }

    private void paddleLeftBack(float forceMultiplier)
    {
        this._rb.AddForceAtPosition(this.transform.forward * forceMultiplier, this._linksachter.position);
        this._rb.AddForceAtPosition(this.transform.forward * forceMultiplier, this._achter.position);
    }

    private void paddleRightBack(float forceMultiplier)
    {
        this._rb.AddForceAtPosition(this.transform.forward * forceMultiplier, this._rechtsachter.position);
        this._rb.AddForceAtPosition(this.transform.forward * forceMultiplier, this._achter.position);
    }
}
