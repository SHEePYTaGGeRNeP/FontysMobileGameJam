using UnityEngine;
using System.Collections;

public class PaddleSoundController : MonoBehaviour {


	public AudioClip[] clips;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
	    this.audioSource = this.GetComponent<AudioSource> ();
	}

	public void PlayRandomPaddleSound() {
		if (this.clips.Length > 0) {
		    this.audioSource.PlayOneShot (this.clips [Random.Range (0, this.clips.Length)]);
		}
	}
}
