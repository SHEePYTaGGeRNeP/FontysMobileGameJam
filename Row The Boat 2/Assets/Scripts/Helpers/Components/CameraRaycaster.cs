namespace Assets.Scripts.Helpers.Components
{
    using System.Linq;

    using UnityEngine;

    // ReSharper disable once UnusedMember.Global
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField]
        private Vector3 _offset = new Vector3(0,0,-0.5f);

        [SerializeField]
        private float _duration = 0.1f;

        [SerializeField]
        private float _distance = 5f;

        [SerializeField]
        private Color _colorSquare = Color.magenta;

        [SerializeField]
        private Color _colorCameraRay = Color.white;
        
#if UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        private void LateUpdate()
        {
            Vector3[] positions = new Vector3[4];
            Vector2[] viewPoints = { Vector2.zero, Vector2.right, Vector2.up, Vector2.one };
            for (int i = 0; i < viewPoints.Length; i++)
            {
                Ray ray = this.GetComponent<Camera>().ViewportPointToRay(viewPoints[i]);
                DebugExtension.DebugDrawRay(ray, this._colorCameraRay,this._duration, this._distance);
                foreach (
                    RaycastHit hit in Physics.RaycastAll(ray).Where(hit => hit.transform.GetComponent<Floor>() != null))
                    positions[i] = hit.point + this._offset;
            }
            Debug.DrawLine(positions[0], positions[1], this._colorSquare);
            Debug.DrawLine(positions[0], positions[2], this._colorSquare);
            Debug.DrawLine(positions[1], positions[3], this._colorSquare);
            Debug.DrawLine(positions[2], positions[3], this._colorSquare);
        }
#endif
    }

}
