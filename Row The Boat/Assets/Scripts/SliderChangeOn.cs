using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    using System.Globalization;

    using Assets.Scripts.Helpers;

    public class SliderChangeOn : MonoBehaviour
    {
        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private Text _multiplierText;

        [SerializeField]
        private Roeiboot _roeiboot;

        public void OnSliderValueChanged()
        {
            LogHelper.Log(typeof(SliderChangeOn), "SliderValueChanged");
            this._roeiboot.ForceMultiplier = this._slider.value;
            this._multiplierText.text = this._roeiboot.ForceMultiplier.ToString(CultureInfo.InvariantCulture);
        }

    }
}
