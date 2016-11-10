namespace Assets.Scripts.Helpers.Components
{
    using UnityEngine;

    public class LookAtVelocity : MonoBehaviour
    {
        private Rigidbody2D _rb2D;
        private Rigidbody _rb;

        [SerializeField]
        private Transform _transformToRotate;

        protected void Awake()
        {
            this._rb2D = this.GetComponent<Rigidbody2D>();
            this._rb = this.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Quaternion rotation = new Quaternion(0,0,0,0);
            if (this._rb2D != null && this._rb2D.velocity != Vector2.zero)
                rotation = Quaternion.LookRotation(this._rb2D.velocity, new Vector3(0, 0, -1));
            else if (this._rb != null && this._rb.velocity != Vector3.zero)
                rotation = Quaternion.LookRotation(this._rb.velocity, Vector3.up);
            if (rotation.eulerAngles == Vector3.zero)
                return;

            this._transformToRotate.rotation = Quaternion.Slerp(this._transformToRotate.rotation, rotation, Time.deltaTime * 5f);

        }
    }
}
