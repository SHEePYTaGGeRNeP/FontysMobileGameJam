namespace Assets.Scripts.Helpers.Components
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class FadeIn : MonoBehaviour
    {
        [SerializeField]
        private LerpHelper _lerpParameters;

        private Image _image;

        private void Start()
        {
            this._image = this.GetComponent<Image>();
        }

        private void Update()
        {
            this._image.color = this._image.color.WithAlpha(this._lerpParameters.LerpStep(Time.time));
            if (this._lerpParameters.IsLerpReached)
			{
				this.enabled = false;
				return;
			}
        }
    }
}
