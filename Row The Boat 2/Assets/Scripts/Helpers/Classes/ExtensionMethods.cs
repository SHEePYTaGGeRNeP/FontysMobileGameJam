using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if (!UNITY_WINRT)
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
#endif
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ExtensionMethods
{
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static bool IsNullOrEmpty<T>(this IList<T> list)
    { return list == null || list.Count == 0; }
    public static void Shuffle<T>(this IList<T> list)
    {
        if (list == null) return;
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int index = Random.Range(0, n + 1);
            T value = list[index];
            list[index] = list[n];
            list[n] = value;
        }
    }
#if (!UNITY_WINRT)
    /// <summary>
    /// Class must be marked [Serialize].
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T DeepClone<T>(T obj)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }
#endif

    /// <summary>
    /// Custom Foreach in ExtensionMethods.cs
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null) throw new ArgumentNullException("source");
        if (action == null) throw new ArgumentNullException("action");

        foreach (T item in source)
        {
            action(item);
        }
    }


    public static void SetEmissionRate(this ParticleSystem particleSystem, float emissionRate)
    {
        ParticleSystem.EmissionModule emission = particleSystem.emission;
        ParticleSystem.MinMaxCurve rate = emission.rate;
        rate.constantMax = emissionRate;
        emission.rate = rate;
    }

    #region MoreLINQ

    #region License and Terms
    // MoreLINQ - Extensions to LINQ to Objects
    // Copyright (c) 2008-2011 Jonathan Skeet. All rights reserved.
    // 
    // Licensed under the Apache License, Version 2.0 (the "License");
    // you may not use this file except in compliance with the License.
    // You may obtain a copy of the License at
    // 
    //     http://www.apache.org/licenses/LICENSE-2.0
    // 
    // Unless required by applicable law or agreed to in writing, software
    // distributed under the License is distributed on an "AS IS" BASIS,
    // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    // See the License for the specific language governing permissions and
    // limitations under the License.
    #endregion

    /// <summary>
    /// Returns the minimal element of the given sequence, based on
    /// the given projection.
    /// </summary>
    /// <remarks>
    /// If more than one element has the minimal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current minimal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <returns>The minimal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector)
    {
        return source.MinBy(selector, Comparer<TKey>.Default);
    }

    /// <summary>
    /// Returns the minimal element of the given sequence, based on
    /// the given projection and the specified comparer for projected values.
    /// </summary>
    /// <remarks>
    /// If more than one element has the minimal projected value, the first
    /// one encountered will be returned. This overload uses the default comparer
    /// for the projected type. This operator uses immediate execution, but
    /// only buffers a single result (the current minimal element).
    /// </remarks>
    /// <typeparam name="TSource">Type of the source sequence</typeparam>
    /// <typeparam name="TKey">Type of the projected element</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="selector">Selector to use to pick the results to compare</param>
    /// <param name="comparer">Comparer to use to compare projected values</param>
    /// <returns>The minimal element, according to the projection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
    /// or <paramref name="comparer"/> is null</exception>
    /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
        Func<TSource, TKey> selector, IComparer<TKey> comparer)
    {
        if (source == null)
            throw new ArgumentNullException("source");
        if (selector == null)
            throw new ArgumentNullException("selector");
        if (comparer == null)
            throw new ArgumentNullException("comparer");
        using (IEnumerator<TSource> sourceIterator = source.GetEnumerator())
        {
            if (!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence was empty");
            }
            TSource min = sourceIterator.Current;
            TKey minKey = selector(min);
            while (sourceIterator.MoveNext())
            {
                TSource candidate = sourceIterator.Current;
                TKey candidateProjected = selector(candidate);
                if (comparer.Compare(candidateProjected, minKey) < 0)
                {
                    min = candidate;
                    minKey = candidateProjected;
                }
            }
            return min;
        }
    }

    #endregion

    public static bool IsNullEmptyOrWhitespace(this string s)
    { return s == null || s.All(char.IsWhiteSpace); }
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    public static string ToStringNewLine<T>(this IEnumerable<T> something)
    {
        StringBuilder builder = new StringBuilder();
        foreach (T t in something)
            builder.Append(t + "\n");
        return builder.ToString();
    }

#if (!UNITY_WINRT)

    /// <summary>
    /// Named format strings from object attributes. Eg:
    /// string blaStr = aPerson.ToString("My name is {FirstName} {LastName}.")
    /// From: http://www.hanselman.com/blog/CommentView.aspx?guid=fde45b51-9d12-46fd-b877-da6172fe1791
    /// </summary>
    /// <param name="anObject"></param>
    /// <param name="aFormat"></param>
    /// <returns></returns>
    public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider = null)
    {
        StringBuilder sb = new StringBuilder();
        Type type = anObject.GetType();
        Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
        MatchCollection mc = reg.Matches(aFormat);
        int startIndex = 0;
        foreach (Match m in mc)
        {
            Group g = m.Groups[2]; //it's second in the match between { and }
            int length = g.Index - startIndex - 1;
            sb.Append(aFormat.Substring(startIndex, length));

            string toGet = string.Empty;
            string toFormat = string.Empty;
            int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
            if (formatIndex == -1) //no formatting, no worries
            {
                toGet = g.Value;
            }
            else //pickup the formatting
            {
                toGet = g.Value.Substring(0, formatIndex);
                toFormat = g.Value.Substring(formatIndex + 1);
            }

            //first try properties
            PropertyInfo retrievedProperty = type.GetProperty(toGet);
            Type retrievedType = null;
            object retrievedObject = null;
            if (retrievedProperty != null)
            {
                retrievedType = retrievedProperty.PropertyType;
                retrievedObject = retrievedProperty.GetValue(anObject, null);
            }
            else //try fields
            {
                FieldInfo retrievedField = type.GetField(toGet);
                if (retrievedField != null)
                {
                    retrievedType = retrievedField.FieldType;
                    retrievedObject = retrievedField.GetValue(anObject);
                }
            }

            if (retrievedType != null) //Cool, we found something
            {
                string result = string.Empty;
                if (toFormat == string.Empty) //no format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, null) as string;
                }
                else //format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
                }
                sb.Append(result);
            }
            else //didn't find a property with that name, so be gracious and put it back
            {
                sb.Append("{");
                sb.Append(g.Value);
                sb.Append("}");
            }
            startIndex = g.Index + g.Length + 1;
        }
        if (startIndex < aFormat.Length) //include the rest (end) of the string
        {
            sb.Append(aFormat.Substring(startIndex));
        }
        return sb.ToString();
    }

#endif
    #region Vector extensions


    public static Vector2 xy(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 WithX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 WithY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 WithYZ(this Vector3 v, float y, float z)
    { return new Vector3(v.x, y, z); }
    public static Vector3 WithXY(this Vector3 v, float x, float y)
    { return new Vector3(x, y, v.z); }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 WithXZ(this Vector3 v, float x, float z)
    {
        return new Vector3(x, v.y, z);
    }
    public static Vector2 WithX(this Vector2 v, float x)
    {
        return new Vector2(x, v.y);
    }

    public static Vector2 WithY(this Vector2 v, float y)
    {
        return new Vector2(v.x, y);
    }

    public static Vector3 WithZ(this Vector2 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }


    public static Vector3 AddX(this Vector3 v, float x)
    { return new Vector3(v.x + x, v.y, v.z); }
    public static Vector3 AddY(this Vector3 v, float y)
    { return new Vector3(v.x, v.y + y, v.z); }
    public static Vector3 AddZ(this Vector3 v, float z)
    { return new Vector3(v.x, v.y, v.z + z); }

    public static Vector3 Abs(this Vector3 a)
    {
        return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
    }

    public static Vector3 Inverse(this Vector3 a)
    {
        return new Vector3(1 / a.x, 1 / a.y, 1 / a.z);
    }
    #endregion

    // Set the layer of this GameObject and all of its children.
    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
            t.gameObject.SetLayerRecursively(layer);
    }
    public static void SetRenderersRecursively(this GameObject gameObject, bool enabled)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
            renderer.enabled = enabled;
    }
    public static void SetCollisionRecursively(this GameObject gameObject, bool tf)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
            collider.enabled = tf;
    }

    public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag)
    where T : Component
    {
        List<T> results = new List<T>();

        if (gameObject.CompareTag(tag))
            results.Add(gameObject.GetComponent<T>());

        foreach (Transform t in gameObject.transform)
            results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));

        return results.ToArray();
    }

    public static T GetComponentInParents<T>(this GameObject gameObject)
    where T : Component
    {
        for (Transform t = gameObject.transform; t != null; t = t.parent)
        {
            T result = t.GetComponent<T>();
            if (result != null)
                return result;
        }

        return null;
    }
    public static T[] GetComponentsInParents<T>(this GameObject gameObject)
    where T : Component
    {
        List<T> results = new List<T>();
        for (Transform t = gameObject.transform; t != null; t = t.parent)
        {
            T result = t.GetComponent<T>();
            if (result != null)
                results.Add(result);
        }

        return results.ToArray();
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
    public static Color WithR(this Color color, float r)
    {
        return new Color(r, color.g, color.b, color.a);
    }
    public static Color WithG(this Color color, float g)
    {
        return new Color(color.r, g, color.b, color.a);
    }
    public static Color WithB(this Color color, float b)
    {
        return new Color(color.r, color.g, b, color.a);
    }

    public static void DebugDraw(this Ray ray, Color color, float duration = 1f, float directionMultiplier = 1f)
    {
        UnityEngine.Debug.DrawRay(ray.origin, ray.direction * directionMultiplier, color, duration);
    }

}
