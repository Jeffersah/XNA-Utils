using System.Collections.Generic;

namespace NCodeRiddian
{
    /// <summary>
    /// An association between keys and their values
    /// </summary>
    /// <typeparam name="E">The Key type</typeparam>
    /// <typeparam name="E2">The Value type</typeparam>
    public class Association<E, E2>
    {
        private List<E> keys;
        private List<E2> values;

        public Association()
        {
            keys = new List<E>();
            values = new List<E2>();
        }

        /// <summary>
        /// Get/Set the value from the specified key. If no such key exists, returns default(valueType)
        /// </summary>
        /// <param name="e">the key</param>
        /// <returns>the value at the defined key</returns>
        public E2 this[E e]
        {
            get
            {
                int i = keys.IndexOf(e);
                if (i == -1)
                    return default(E2);
                return values[keys.IndexOf(e)];
            }
            set
            {
                values[keys.IndexOf(e)] = value;
            }
        }

        /// <summary>
        /// Adds a key-value pair to the association
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The Value</param>
        public void Add(E key, E2 value)
        {
            keys.Add(key);
            values.Add(value);
        }

        /// <summary>
        /// Removes a key-value pair from the association
        /// </summary>
        /// <param name="key">The key</param>
        public void Remove(E key)
        {
            values.RemoveAt(keys.IndexOf(key));
            keys.Remove(key);
        }
    }
}