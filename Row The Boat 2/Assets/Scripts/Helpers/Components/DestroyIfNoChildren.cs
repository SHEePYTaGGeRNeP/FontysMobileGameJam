namespace Assets.Scripts.Helpers.Components
{
    using UnityEngine;

    public class EmptyChildDestroyer : MonoBehaviour
    {
        [SerializeField]
        private float _checkInterval = 1f;


        private void Awake()
        {
            if (this._checkInterval > 0)
                this.Invoke("CheckChildren", this._checkInterval);
        }

        private void Update()
        {
            if (this._checkInterval == 0)
                this.CheckChildren();
            else
                this.enabled = false;
        }

        private void CheckChildren()
        {
            if (this.transform.childCount == 0)
                Destroy(this.gameObject);
        }

    }
}
