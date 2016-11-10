namespace Assets.Scripts.Helpers.Components
{
    using System.Linq;

    using UnityEngine;

    public class LookAtCursor : MonoBehaviour
    {
        private LerpHelper _lerpHelper;
        private Vector3 _previousLocation;

        [SerializeField]
        private float _speed = 3f;

        private void Awake()
        {
            this._lerpHelper = new LerpHelper(0, 1, 0.01f, 1f, LerpHelper.LerpType.Hermite) { Speed = this._speed };
        }

        private void Update()
        {
            this.Look();

        }

        private void Look()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// Change the check to your way of checking the floor.
            foreach (RaycastHit raycastHit in Physics.RaycastAll(ray, 1000).Where(raycastHit => raycastHit.transform.GetComponent<Floor>() != null))
            {
                Vector3 lookAtPosition = raycastHit.point.WithY(this.transform.position.y) - this.transform.position;
                // if there is a difference in mousemovement / where to look
                if (Vector3.Distance(lookAtPosition, this._previousLocation) > 0.5f)
                {
                    this._previousLocation = lookAtPosition;
                    this._lerpHelper.Reset();
                }
                this._lerpHelper.LerpStep(Time.deltaTime);
                Quaternion newRot = Quaternion.LookRotation(lookAtPosition);
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, newRot, this._lerpHelper.CurrentValue);
                //this.transform.LookAt(raycastHit.point.WithY(this.transform.position.y));
                break;
            }
        }
    }
}
