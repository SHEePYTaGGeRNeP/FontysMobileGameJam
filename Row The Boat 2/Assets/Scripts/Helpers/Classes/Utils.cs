using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// TimeSpan time = Utils.TimeAction(() =>
    ///{
    ///    // Do some work
    ///});
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static TimeSpan TimeAction(Action action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    /// <summary>
    /// Not all switch cases present
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string PathForDocumentsFile(string filename = "")
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                {
                    string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
                    path = path.Substring(0, path.LastIndexOf('/'));
                    return Path.Combine(Path.Combine(path, "Documents"), filename);
                }
            case RuntimePlatform.Android:
                {
                    string path = Application.persistentDataPath;
                    path = path.Substring(0, path.LastIndexOf('/'));
                    return Path.Combine(path, filename);
                }
            // Not all switch cases present
            default:
                {
                    string path = Application.dataPath;
                    path = path.Substring(0, path.LastIndexOf('/'));
                    return Path.Combine(path, filename);
                }
        }
    }

    /// <summary>
    /// Returns parent.name:parent.name:transform.name
    /// </summary>
    /// <param name="transform"></param>
    /// <returns>parent.name:parent.name:transform.name</returns>
    public static string GetGameObjectFullName(Transform transform)
    {
        string name = String.Empty;
        Transform t = transform;
        while (t.parent != null)
        {
            name += t.parent.name + ":";
            t = t.parent;
        }
        name += transform.name;
        return name;
    }

    public static Vector2 RandomizedVector2(float magnitude = 1f)
    {
        Vector2 v = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        Vector2 returnVector = v * magnitude;
        return returnVector;
    }
    public static Vector2 RandomizedVector2(Vector2 min, Vector2 max)
    {
        if (min == max)
            return new Vector2(min.x, max.y);
        Vector2 newVector2 = new Vector2(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y));
        return newVector2;
    }
    public static Vector3 RandomizedVector3(float magnitude = 1f)
    {
        Vector3 v = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
        Vector3 returnVector = v * magnitude;
        return returnVector;
    }
    public static Vector3 RandomizedVector3(Vector3 min, Vector3 max)
    {
        if (min == max)
            return new Vector3(min.x, min.y);
        Vector3 newVector3 = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y),
            UnityEngine.Random.Range(min.z, max.z));
        return newVector3;
    }
    
}