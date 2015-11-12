//#if !UNITY_ANDROID && !UNITY_EDITOR
#if !UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;
using System.Collections;

public class AndroidJavaObject {

	public void Dispose () {}
	public void Call(string method, params object[] parameters) {}
	public void CallStatic(string method, params object[] parameters) {}
	public T Get<T>(string field) {return default(T);}
	public void Set<T>(string field, T val) {}
	public T GetStatic<T>(string field) { return default(T); }
	public void SetStatic<T>(string field, T val) {}
	public System.IntPtr GetRawObject() { return System.IntPtr.Zero; }
	public System.IntPtr GetRawClass() { return System.IntPtr.Zero; }
	public T Call<T>(string method, params object[] args) { return default(T); }
	public T CallStatic<T>(string method, params object[] args) { return default(T); }

	public AndroidJavaObject(params object[] parameters) {}
}

public class AndroidJavaClass : AndroidJavaObject {
	public AndroidJavaClass(string className) {}
}

public class AndroidJNI {
	public static void AttachCurrentThread() {}
	
	public static System.IntPtr FindClass(params object[] parameters) {
		return System.IntPtr.Zero;
	}
	
	public static System.IntPtr GetStaticFieldID(params object[] parameters) {
		return System.IntPtr.Zero;
	}
	
	public static System.IntPtr GetStaticObjectField(params object[] parameters) {
		return System.IntPtr.Zero;
	}
	
	public static System.IntPtr GetMethodID(params object[] parameters) {
		return System.IntPtr.Zero;
	}
	
	public static System.IntPtr NewObject(params object[] parameters) {
		return System.IntPtr.Zero;
	}
}

public class jvalue {
	public object l;
}

#endif