using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class LogHelper
{
    /// <summary>Writes text to Unity Console with a different color for the class.</summary>
    /// <param name="classname">USE: typeof(Class)</param>
    public static void Log(Type classname, string message)
    {
#if UNITY_EDITOR
        Debug.Log(Time.time + " <color=teal>" + classname.Name + ":</color> " + message);
#endif
    }
    /// <summary>Writes text to Unity Console with a different color for the class + method.</summary>
    /// <param name="classname">USE: typeof(Class)</param>
    public static void Log(Type classname, string method, string message)
    {
#if UNITY_EDITOR
        Debug.Log(Time.time + " <color=teal>" + classname.Name + "." + method + "():</color> " + message);
#endif
    }

    /// <summary>
    /// Writes the error message in Debug.Log
    /// </summary>
    /// <param name="classname">USE: typeof(Class)</param>
    public static void LogError(Type classname, string message)
    {

#if UNITY_EDITOR
        Debug.LogError(Time.time + " <color=red>ERROR in </color>" + classname.Name + ":<color=red> " + message + "</color>");
#endif
    }
    /// <summary>
    /// Writes the error message in Debug.Log
    /// </summary>
    /// <param name="classname">USE: typeof(Class)</param>
    public static void LogError(Type classname, Exception exception)
    {
#if UNITY_EDITOR
        Debug.LogError(Time.time + " < color=red>ERROR in </color>" + classname.Name + ":<color=red> " + exception.Message + "stacktrace:" + exception.StackTrace + "</color>");
#endif
    }
    /// <summary>
    /// Writes the error message in Debug.Log
    /// </summary>
    /// <param name="classname">USE: typeof(Class)</param>
    public static void LogError(Type classname, string method, string message)
    {
#if UNITY_EDITOR
        Debug.LogError(Time.time + " <color=red>ERROR in </color>" + classname.Name + "." + method + "():<color=red> " + message + "</color>");
#endif
    }
    /// <summary>
    /// Writes the error message in Debug.Log
    /// </summary>
    /// <param name="classname">USE: typeof(Class)</param>
    public static void LogError(Type classname, string method, Exception exception)
    {
#if UNITY_EDITOR
        Debug.LogError(Time.time + " <color=red>ERROR in </color>" + classname.Name + "." + method + "():<color=red> " + exception.Message + "stacktrace:" + exception.StackTrace + "</color>");
#endif
    }

    /// <summary>
    /// Writes the warning message in Debug.Log
    /// </summary>
    /// <param name="classname">USE: typeof(Class)</param>
    public static void LogWarning(Type classname, string message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(Time.time + " <color=orange>WARNING in </color>" + classname.Name + ":<color=orange> " + message + "</color>");
#endif
    }
    /// <summary>
    /// Writes the warning message in Debug.Log
    /// </summary>
    /// <param name="classname">USE: typeof(Class)</param>
    public static void LogWarning(Type classname, string method, string message)
    {
#if UNITY_EDITOR
        Debug.LogWarning(Time.time + " <color=orange>WARNING in </color>" + classname.Name + "." + method + "():<color=orange> " + message + "</color>");
#endif
    }

}

// port of this JavaScript code with some changes:
//   http://www.kevlindev.com/gui/math/intersection/Intersection.js
// found here:
//   http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect/563240#563240

public class IntersectionHelper
{
    static readonly double MyEpsilon = 0.00001;

    private static float[] OverlapIntervals(float ub1, float ub2)
    {
        float l = Math.Min(ub1, ub2);
        float r = Math.Max(ub1, ub2);
        float a = Math.Max(0, l);
        float b = Math.Min(1, r);
        if (a > b) // no intersection
            return new float[] { };
        return a == b ? new[] { a } : new[] { a, b };
    }

    // IMPORTANT: a1 and a2 cannot be the same, e.g. a1--a2 is a true segment, not a point
    // b1/b2 may be the same (b1--b2 is a point)
    private static Vector2[] OneD_Intersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        //float ua1 = 0.0f; // by definition
        //float ua2 = 1.0f; // by definition
        float ub1, ub2;

        float denomx = a2.x - a1.x;
        float denomy = a2.y - a1.y;

        if (Math.Abs(denomx) > Math.Abs(denomy))
        {
            ub1 = (b1.x - a1.x) / denomx;
            ub2 = (b2.x - a1.x) / denomx;
        }
        else
        {
            ub1 = (b1.y - a1.y) / denomy;
            ub2 = (b2.y - a1.y) / denomy;
        }

        List<Vector2> ret = new List<Vector2>();
        float[] interval = OverlapIntervals(ub1, ub2);
        foreach (float f in interval)
        {
            float x = a2.x * f + a1.x * (1.0f - f);
            float y = a2.y * f + a1.y * (1.0f - f);
            Vector2 p = new Vector2(x, y);
            ret.Add(p);
        }
        return ret.ToArray();
    }

    private static bool PointOnLine(Vector2 p, Vector2 a1, Vector2 a2)
    {
        float dummyU = 0.0f;
        double d = DistFromSeg(p, a1, a2, MyEpsilon, ref dummyU);
        return d < MyEpsilon;
    }

    private static double DistFromSeg(Vector2 p, Vector2 q0, Vector2 q1, double radius, ref float u)
    {
        // formula here:
        //http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
        // where x0,y0 = p
        //       x1,y1 = q0
        //       x2,y2 = q1
        double dx21 = q1.x - q0.x;
        double dy21 = q1.y - q0.y;
        double dx10 = q0.x - p.x;
        double dy10 = q0.y - p.y;
        double segLength = Math.Sqrt(dx21 * dx21 + dy21 * dy21);
        if (segLength < MyEpsilon)
            throw new Exception("Expected line segment, not point.");
        double num = Math.Abs(dx21 * dy10 - dx10 * dy21);
        double d = num / segLength;
        return d;
    }

    // this is the general case. Really really general
    public static Vector2[] Intersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
        if (a1.Equals(a2) && b1.Equals(b2))
            // both "segments" are points, return either point
            return a1.Equals(b1) ? new Vector2[] { a1 } : new Vector2[] { };

        if (b1.Equals(b2)) // b is a point, a is a segment
            return PointOnLine(b1, a1, a2) ? new Vector2[] { b1 } : new Vector2[] { };

        if (a1.Equals(a2)) // a is a point, b is a segment
            return PointOnLine(a1, b1, b2) ? new Vector2[] { a1 } : new Vector2[] { };

        // at this point we know both a and b are actual segments

        float ua_t = (b2.x - b1.x) * (a1.y - b1.y) - (b2.y - b1.y) * (a1.x - b1.x);
        float ub_t = (a2.x - a1.x) * (a1.y - b1.y) - (a2.y - a1.y) * (a1.x - b1.x);
        float u_b = (b2.y - b1.y) * (a2.x - a1.x) - (b2.x - b1.x) * (a2.y - a1.y);

        // Infinite lines intersect somewhere
        if (!(-MyEpsilon < u_b && u_b < MyEpsilon))   // e.g. u_b != 0.0
        {
            float ua = ua_t / u_b;
            float ub = ub_t / u_b;
            if (0.0f <= ua && ua <= 1.0f && 0.0f <= ub && ub <= 1.0f)
            {
                // Intersection
                return new Vector2[] {
                    new Vector2(a1.x + ua * (a2.x - a1.x),
                        a1.y + ua * (a2.y - a1.y)) };
            }
            // No Intersection
            return new Vector2[] { };
        }
        // Coincident
        // find the common overlapping section of the lines
        // first find the distance (squared) from one point (a1) to each point
        if ((-MyEpsilon < ua_t && ua_t < MyEpsilon)
            || (-MyEpsilon < ub_t && ub_t < MyEpsilon))
        {
            return a1.Equals(a2) ? OneD_Intersection(b1, b2, a1, a2) : OneD_Intersection(a1, a2, b1, b2);
        }
        // Parallel
        return new Vector2[] { };
    }

}