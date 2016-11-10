namespace Assets.Scripts.Helpers.Components
{
    using UnityEngine;

    public class DebugObject : MonoBehaviour
    {
        public static Transform Instance;

        private void Awake()
        {
            Instance = this.transform;
        }
    }
}
