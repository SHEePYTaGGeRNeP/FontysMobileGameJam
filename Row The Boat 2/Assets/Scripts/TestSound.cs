using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class TestSound : MonoBehaviour
    {
        private AudioSource _audioSource;
        private AudioSource AudioSource
        {
            get
            {
                if (this._audioSource == null)
                    this._audioSource = this.GetComponent<AudioSource>();
                return this._audioSource;
            }
        }

        public void Play()
        {
            this.AudioSource.Play();
        }
    }
}
