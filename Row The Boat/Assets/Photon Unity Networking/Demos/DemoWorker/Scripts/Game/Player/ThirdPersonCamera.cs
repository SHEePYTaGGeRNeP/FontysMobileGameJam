using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{

    public Transform cameraTransform;
    private Transform _target;

    // The distance in the x-z plane to the target

    public float distance = 7.0f;

    // the height we want the camera to be above the target
    public float height = 3.0f;

    public float angularSmoothLag = 0.3f;
    public float angularMaxSpeed = 15.0f;

    public float heightSmoothLag = 0.3f;

    public float snapSmoothLag = 0.2f;
    public float snapMaxSpeed = 720.0f;

    public float clampHeadPositionScreenSpace = 0.75f;

    public float lockCameraTimeout = 0.2f;

    private Vector3 headOffset = Vector3.zero;
    private Vector3 centerOffset = Vector3.zero;

    private float heightVelocity = 0.0f;
    private float angleVelocity = 0.0f;
    private bool snap = false;
    private ThirdPersonController controller;
    private float targetHeight = 100000.0f;

    private Camera m_CameraTransformCamera;

    void OnEnable()
    {
        if( !this.cameraTransform && Camera.main )
            this.cameraTransform = Camera.main.transform;
        if( !this.cameraTransform )
        {
            Debug.Log( "Please assign a camera to the ThirdPersonCamera script." );
            this.enabled = false;
        }

        this.m_CameraTransformCamera = this.cameraTransform.GetComponent<Camera>();

        this._target = this.transform;
        if(this._target )
        {
            this.controller = this._target.GetComponent<ThirdPersonController>();
        }

        if(this.controller )
        {
            CharacterController characterController = (CharacterController)this._target.GetComponent<Collider>();
            this.centerOffset = characterController.bounds.center - this._target.position;
            this.headOffset = this.centerOffset;
            this.headOffset.y = characterController.bounds.max.y - this._target.position.y;
        }
        else
            Debug.Log( "Please assign a target to the camera that has a ThirdPersonController script attached." );

        this.Cut(this._target, this.centerOffset );
    }

    void DebugDrawStuff()
    {
        Debug.DrawLine(this._target.position, this._target.position + this.headOffset );

    }

    float AngleDistance( float a, float b )
    {
        a = Mathf.Repeat( a, 360 );
        b = Mathf.Repeat( b, 360 );

        return Mathf.Abs( b - a );
    }

    void Apply( Transform dummyTarget, Vector3 dummyCenter )
    {
        // Early out if we don't have a target
        if( !this.controller )
            return;

        Vector3 targetCenter = this._target.position + this.centerOffset;
        Vector3 targetHead = this._target.position + this.headOffset;

        //	DebugDrawStuff();

        // Calculate the current & target rotation angles
        float originalTargetAngle = this._target.eulerAngles.y;
        float currentAngle = this.cameraTransform.eulerAngles.y;

        // Adjust real target angle when camera is locked
        float targetAngle = originalTargetAngle;

        // When pressing Fire2 (alt) the camera will snap to the target direction real quick.
        // It will stop snapping when it reaches the target
        if( Input.GetButton( "Fire2" ) )
            this.snap = true;

        if(this.snap )
        {
            // We are close to the target, so we can stop snapping now!
            if(this.AngleDistance( currentAngle, originalTargetAngle ) < 3.0f )
                this.snap = false;

            currentAngle = Mathf.SmoothDampAngle( currentAngle, targetAngle, ref this.angleVelocity, this.snapSmoothLag, this.snapMaxSpeed );
        }
        // Normal camera motion
        else
        {
            if(this.controller.GetLockCameraTimer() < this.lockCameraTimeout )
            {
                targetAngle = currentAngle;
            }

            // Lock the camera when moving backwards!
            // * It is really confusing to do 180 degree spins when turning around.
            if(this.AngleDistance( currentAngle, targetAngle ) > 160 && this.controller.IsMovingBackwards() )
                targetAngle += 180;

            currentAngle = Mathf.SmoothDampAngle( currentAngle, targetAngle, ref this.angleVelocity, this.angularSmoothLag, this.angularMaxSpeed );
        }


        // When jumping don't move camera upwards but only down!
        if(this.controller.IsJumping() )
        {
            // We'd be moving the camera upwards, do that only if it's really high
            float newTargetHeight = targetCenter.y + this.height;
            if( newTargetHeight < this.targetHeight || newTargetHeight - this.targetHeight > 5 )
                this.targetHeight = targetCenter.y + this.height;
        }
        // When walking always update the target height
        else
        {
            this.targetHeight = targetCenter.y + this.height;
        }

        // Damp the height
        float currentHeight = this.cameraTransform.position.y;
        currentHeight = Mathf.SmoothDamp( currentHeight, this.targetHeight, ref this.heightVelocity, this.heightSmoothLag );

        // Convert the angle into a rotation, by which we then reposition the camera
        Quaternion currentRotation = Quaternion.Euler( 0, currentAngle, 0 );

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        this.cameraTransform.position = targetCenter;
        this.cameraTransform.position += currentRotation * Vector3.back * this.distance;

        // Set the height of the camera
        this.cameraTransform.position = new Vector3(this.cameraTransform.position.x, currentHeight, this.cameraTransform.position.z );

        // Always look at the target	
        this.SetUpRotation( targetCenter, targetHead );
    }

    void LateUpdate()
    {
        this.Apply(this.transform, Vector3.zero );
    }

    void Cut( Transform dummyTarget, Vector3 dummyCenter )
    {
        float oldHeightSmooth = this.heightSmoothLag;
        float oldSnapMaxSpeed = this.snapMaxSpeed;
        float oldSnapSmooth = this.snapSmoothLag;

        this.snapMaxSpeed = 10000;
        this.snapSmoothLag = 0.001f;
        this.heightSmoothLag = 0.001f;

        this.snap = true;
        this.Apply(this.transform, Vector3.zero );

        this.heightSmoothLag = oldHeightSmooth;
        this.snapMaxSpeed = oldSnapMaxSpeed;
        this.snapSmoothLag = oldSnapSmooth;
    }

    void SetUpRotation( Vector3 centerPos, Vector3 headPos )
    {
        // Now it's getting hairy. The devil is in the details here, the big issue is jumping of course.
        // * When jumping up and down we don't want to center the guy in screen space.
        //  This is important to give a feel for how high you jump and avoiding large camera movements.
        //   
        // * At the same time we dont want him to ever go out of screen and we want all rotations to be totally smooth.
        //
        // So here is what we will do:
        //
        // 1. We first find the rotation around the y axis. Thus he is always centered on the y-axis
        // 2. When grounded we make him be centered
        // 3. When jumping we keep the camera rotation but rotate the camera to get him back into view if his head is above some threshold
        // 4. When landing we smoothly interpolate towards centering him on screen
        Vector3 cameraPos = this.cameraTransform.position;
        Vector3 offsetToCenter = centerPos - cameraPos;

        // Generate base rotation only around y-axis
        Quaternion yRotation = Quaternion.LookRotation( new Vector3( offsetToCenter.x, 0, offsetToCenter.z ) );

        Vector3 relativeOffset = Vector3.forward * this.distance + Vector3.down * this.height;
        this.cameraTransform.rotation = yRotation * Quaternion.LookRotation( relativeOffset );

        // Calculate the projected center position and top position in world space
        Ray centerRay = this.m_CameraTransformCamera.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 1 ) );
        Ray topRay = this.m_CameraTransformCamera.ViewportPointToRay( new Vector3( 0.5f, this.clampHeadPositionScreenSpace, 1 ) );

        Vector3 centerRayPos = centerRay.GetPoint(this.distance );
        Vector3 topRayPos = topRay.GetPoint(this.distance );

        float centerToTopAngle = Vector3.Angle( centerRay.direction, topRay.direction );

        float heightToAngle = centerToTopAngle / ( centerRayPos.y - topRayPos.y );

        float extraLookAngle = heightToAngle * ( centerRayPos.y - centerPos.y );
        if( extraLookAngle < centerToTopAngle )
        {
            extraLookAngle = 0;
        }
        else
        {
            extraLookAngle = extraLookAngle - centerToTopAngle;
            this.cameraTransform.rotation *= Quaternion.Euler( -extraLookAngle, 0, 0 );
        }
    }

    Vector3 GetCenterOffset()
    {
        return this.centerOffset;
    }

}