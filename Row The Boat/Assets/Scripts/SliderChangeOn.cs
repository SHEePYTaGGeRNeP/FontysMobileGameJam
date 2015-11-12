using UnityEngine;
using System.Collections;
using UnityEngine.UI;


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
		this._roeiboot.ForceMultiplier = this._slider.value;
		this._multiplierText.text = this._roeiboot.ForceMultiplier.ToString();
	}

}
