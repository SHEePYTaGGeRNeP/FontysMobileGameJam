using UnityEngine;

namespace Assets.Scripts.Helpers.Components
{
    public class CustomFollowTarget : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;
        public Transform Target { get { return this._target; } set { this._target = value; } }
        
        [SerializeField]
        private Vector3 _offset = new Vector3(0f, 7.5f, 0f);

        [SerializeField]
        private bool _moveX = true;
        [SerializeField]
        private bool _moveY = true;
        [SerializeField]
        private bool _moveZ = true;


        private void Awake()
        {
            if (this._offset == Vector3.zero && this.Target != null)
                this._offset = this.transform.position - this.Target.position;
        }
        private void LateUpdate()
        {
            if (this.Target == null)
            {
                this.enabled = false;
                return;
            }
            Vector3 newPosition = this.transform.position;
            if (this._moveX)
                newPosition.x = this.Target.position.x + this._offset.x;
            if (this._moveY)
                newPosition.y = this.Target.position.y + this._offset.y;
            if (this._moveZ)
                newPosition.z = this.Target.position.z + this._offset.z;
            this.transform.position = newPosition;
        }
    }
}
