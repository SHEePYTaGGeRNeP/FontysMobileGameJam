namespace Assets.Scripts.Helpers.Components
{
    using UnityEngine;

    public class CameraFacingBillboard : MonoBehaviour
    {
        [SerializeField]
        private Camera _cameraToFace;

        private enum Axis { Up, Down, Left, Right, Forward, Back };

        [SerializeField]
        private bool _reverseFace = false;
        [SerializeField]
        private Axis _axis = Axis.Up;

        // return a direction based upon chosen axis
        private static Vector3 GetAxis(Axis refAxis)
        {
            switch (refAxis)
            {
                case Axis.Down:
                    return Vector3.down;
                case Axis.Forward:
                    return Vector3.forward;
                case Axis.Back:
                    return Vector3.back;
                case Axis.Left:
                    return Vector3.left;
                case Axis.Right:
                    return Vector3.right;
                default:
                    return Vector3.up;
            }
        }

        private void Awake()
        {
            // if no camera referenced, grab the main camera
            if (!this._cameraToFace)
                this._cameraToFace = Camera.main;
        }

        private void Update()
        {
            // rotates the object relative to the camera
            Vector3 targetPos = this.transform.position + this._cameraToFace.transform.rotation * (this._reverseFace ? Vector3.forward : Vector3.back);
            Vector3 targetOrientation = this._cameraToFace.transform.rotation * GetAxis(this._axis);
            this.transform.LookAt(targetPos, targetOrientation);
        }
    }
}
