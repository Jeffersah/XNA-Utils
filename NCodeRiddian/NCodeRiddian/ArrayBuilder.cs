using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    /// <summary>
    /// Allows the building of set-length arrays as though they are lists
    /// </summary>
    /// <typeparam name="E">The type of the array</typeparam>
    class ArrayBuilder<E>
    {
        E[] interior;
        int idx;
        public ArrayBuilder(int amt)
        {
            interior = new E[amt];
            idx = 0;
        }
        /// <summary>
        /// Add an item to the next open index in the array
        /// </summary>
        /// <param name="inp">The item to add</param>
        public void Add(E inp)
        {
            interior[idx] = inp;
            idx++;
        }
        /// <summary>
        /// Return the current array
        /// </summary>
        /// <returns>The array with all elements added</returns>
        public E[] Finish()
        {
            return interior;
        }
    }
}
