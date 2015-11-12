using UnityEngine;

public class TestScript : MonoBehaviour
{
    private readonly float accelerometerUpdateInterval = 1.0f / 60.0f;

    private float lowPassFilterFactor;
    // The greater the value of LowPassKernelWidthInSeconds, the slower the filtered value will converge towards current input sample (and vice versa).
    private readonly float lowPassKernelWidthInSeconds = 1.0f;
    private Vector3 lowPassValue = Vector3.zero;
// This next parameter is initialized to 2.0 per Apple's recommendation, or at least according to Brady! ;)
    private float shakeDetectionThreshold = 2.0f;

    // Use this for initialization
    private void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
    }

    // Update is called once per frame
    private void Update()
    {
        var acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        var deltaAcceleration = acceleration - lowPassValue;
        if (deltaAcceleration.sqrMagnitude >= 2.0f)
        {
            // Perform your "shaking actions" here, with suitable guards in the if check above, if necessary to not, to not fire again if they're already being performed.
            Debug.Log("Shake event detected at time " + Time.time);
        }
    }
}