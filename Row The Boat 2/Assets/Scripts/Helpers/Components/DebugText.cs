using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Helpers.Components
{
    [RequireComponent(typeof(Text))]
    public class DebugText : MonoBehaviour
    {


        private static Text _instance;


        void Awake()
        {
            if (_instance == null)
                _instance = this.GetComponent<Text>();
        }

        public static void SetText(string text)
        {
            if (_instance == null) return;
            _instance.text = text;
        }
    }
}