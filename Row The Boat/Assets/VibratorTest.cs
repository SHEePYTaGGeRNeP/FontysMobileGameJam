using UnityEngine;
using System.Collections;

public class VibratorTest : MonoBehaviour {


	public void Vibrate() {
		long[] pattern = { 
			100, 200, 500, 1000, 2000
		};

		Vibrator.Vibrate(pattern, -1);
	}
}
