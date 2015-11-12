using UnityEngine;
using System.Collections;

public partial class Sensor {

	/// <summary>
	/// Use this class to send sensor hardware statistics.
	/// </summary>
	/// <remarks>
	/// Just call <code>Sensor.Statistics.Send();</code> once upon app startup.
	/// The app will automatically check whether a send happened before
	/// and not try it again if it did, so no need to catch anything yourself.
	/// </remarks>
	public static class Statistics {

		// this links to a Google Apps Script
		static string url = "https://script.google.com/macros/s/AKfycbxT0XSNwdMHN20Kn_1zOb6-cQ3QXusAEgh8tyWh3jPrfRe_9XY/exec";

		/// <summary>
		/// This checks whether this device already sent statistics,
		/// if not it sends exactly one POST request and never again afterwards
		/// (unless statistics version changes)
		/// </summary>
		public static void Send() {
			Sensor.Singleton.StartCoroutine(SendInternal());
		}

		private static IEnumerator SendInternal ()
		{
#if !NO_GYRODROID_STATISTICS
			// statistics versioning
			version = PlayerPrefs.GetString("statisticsVersion", "");
			if(version == statisticsVersion || Application.isEditor)
				yield break;

			// wait time before next try, prevents database double write
			if(Time.realtimeSinceStartup - lastTryTime < 15)
				yield break;
			lastTryTime = Time.realtimeSinceStartup;
			

			WWWForm form = new WWWForm();
			form.AddField ("Statistics Version", statisticsVersion);

			form.AddField("Device Model", SystemInfo.deviceModel);
			form.AddField("Device Unique Identifier", SystemInfo.deviceUniqueIdentifier);
			form.AddField("Android ID", GetUniqueHardwareID());
			form.AddField("Operating System", SystemInfo.operatingSystem);
			form.AddField("Graphics Memory Size", SystemInfo.graphicsMemorySize);
			form.AddField("System Memory Size", SystemInfo.systemMemorySize);

			// add sensor data
			for(int i = 1; i <= Sensor.Count; i++)
			{
				var sensor = Sensor.Get ((Sensor.Type) i);
				if(sensor.description == "") continue;

				if(sensor.name != null && sensor.name != "") form.AddField (sensor.description + ".Name", sensor.name);
				form.AddField (sensor.description + ".Available", sensor.available ? "TRUE" : "FALSE");
				if(sensor.vendor != null && sensor.vendor != "") form.AddField (sensor.description + ".Vendor", sensor.vendor);
				form.AddField (sensor.description + ".Resolution", sensor.resolution.ToString());
			}

			// ultra secret secret key
			form.AddField ("dyn", "xwo9fUJnATeVz6UIYR9cNwKIrVlv6w");
			WWW www = new WWW(url, form);

			yield return www;
			if(string.IsNullOrEmpty(www.error))
			{
				version = statisticsVersion;
				PlayerPrefs.SetString("statisticsVersion", version);
				PlayerPrefs.Save ();

				Debug.Log ("Sent GyroDroid statistics (this only happens once).");
			}
			else
			{
				Debug.Log ("Error sending GyroDroid statistics (this only happens once): " + www.error);
			}
			
	#endif
			yield break;
		}

		private static string statisticsVersion = "v20140509";
		private static string version;
		private static float lastTryTime;

		private static string GetUniqueHardwareID() {
			string id = SystemInfo.deviceUniqueIdentifier;
	#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass UnityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");  
			AndroidJavaClass Secure = new AndroidJavaClass ("android.provider.Settings$Secure");
			id = Secure.CallStatic<string> ("getString", contentResolver, "android_id");
	#endif
			return id;
		}
	}
}