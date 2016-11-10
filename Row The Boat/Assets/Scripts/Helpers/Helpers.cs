namespace Assets.Scripts.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using Random = UnityEngine.Random;

    internal class LogHelper
    {
        /// <summary>Writes text to Unity Console with a different color for the class.</summary>
        /// <param name="classname">USE: typeof(Class)</param>
        public static void Log(Type classname, string message)
        { Debug.Log("<color=teal>" + classname.FullName + ":</color> " + message); }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        /// <param name="method">USE: System.Reflection.MethodBase.GetCurrentMethod().Name unless in constructor</param>
        public static void WriteErrorMessage(Type classname, string method, string message)
        { Debug.LogError("<color=red>ERROR in </color>" + classname.FullName + "." + method + "() - Message: <color=red> " + message + "</color>"); }

        /// <summary>
        /// Writes the error message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        /// <param name="method">USE: System.Reflection.MethodBase.GetCurrentMethod().Name unless in constructor</param>
        public static void WriteErrorMessage(Type classname, string method, Exception exception)
        { Debug.LogError("<color=red>ERROR in </color>" + classname.FullName + "." + method + "() - Message: <color=red> " + exception.Message + "stacktrace:" + exception.StackTrace + "</color>"); }
        
        /// <summary>
        /// Writes the warning message in Debug.Log
        /// </summary>
        /// <param name="classname">USE: typeof(Class)</param>
        /// <param name="method">USE: System.Reflection.MethodBase.GetCurrentMethod().Name unless in constructor</param>
        public static void WriteWarningMessage(Type classname, string method, string message)
        { Debug.LogWarning("<color=yellow>WARNING in </color>" + classname.FullName + "." + method + "() - Message: <color=yellow> " + message + "</color>"); }

    }

    public static class ExtensionMethods
    {
        // http://stackoverflow.com/questions/6976597/string-isnulloremptystring-vs-string-isnullorwhitespacestring
        public static bool IsNullEmptyOrWhitespace(this string s)
        { return s == null || s.All(char.IsWhiteSpace); }

        public static bool IsNullOrEmpty<T>(this IList<T> list)
        { return (list == null || list.Count == 0); }

        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null) return;
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
