using System.Collections.Generic;

namespace Assets.Scripts.Helpers.Classes
{
    /// <summary>
    /// Dictionary with a list as values.
    /// http://stackoverflow.com/questions/17887407/dictionary-with-list-of-strings-as-value
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class ListDictionary<TKey, T>
    {
        /// <summary>
        /// Use the Add method in the ListDictionary class and not in here.
        /// </summary>
        public readonly Dictionary<TKey, List<T>> Dictionary = new Dictionary<TKey, List<T>>();

        public void Add(TKey key, T value)
        {
            if (this.Dictionary.ContainsKey(key))
            {
                List<T> list = this.Dictionary[key];
                    list.Add(value);
            }
            else
            {
                List<T> list = new List<T> { value };
                this.Dictionary.Add(key, list);
            }
        }

    }
}
