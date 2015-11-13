using UnityEngine;
using System.Collections;

public class PaddleSoundController : MonoBehaviour {


	public AudioClip[] clips;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	public void PlayRandomPaddleSound() {
		if (clips.Length > 0) {
			audioSource.PlayOneShot (clips [Random.Range (0, clips.Length)]);
		}
	}
}
