using System.Collections.Generic;

namespace NCodeRiddian
{
    /// <summary>
    /// Provides helper classes for arrays
    /// </summary>
    public static class ArrayManager
    {
        /// <summary>
        /// Get a random element from the specified array
        /// </summary>
        /// <typeparam name="E">Type of the array</typeparam>
        /// <param name="arr">The array</param>
        /// <returns>A random value from the array</returns>
        public static E GetRandom<E>(E[] arr)
        {
            return arr[GlobalRandom.random.Next(arr.Length)];
        }
        /// <summary>
        /// Get a random element from the specified list
        /// </summary>
        /// <typeparam name="E">Type of the list</typeparam>
        /// <param name="arr">The list</param>
        /// <returns>A random value from the list</returns>
        public static E GetRandom<E>(List<E> arr)
        {
            return arr[GlobalRandom.random.Next(arr.Count)];
        }
        /// <summary>
        /// Depricated
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="item"></param>
        /// <param name="pos"></param>
        public delegate void UseItem<E>(E item, params int[] pos);
        /// <summary>
        /// Depricated
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="item"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public delegate void UseItemSimple<E>(E item, int x, int y);
        /// <summary>
        /// Depricated - Use ArrayHelper.DoForEach()
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="array"></param>
        /// <param name="function"></param>
        public static void ForAll<E>(E[,] array, UseItem<E> function)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    function(array[x, y], x, y);
                }
            }
        }
        /// <summary>
        /// Depricated - Use ArrayHelper.DoForEach()
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="array"></param>
        /// <param name="function"></param>
        public static void ForAll<E>(E[,] array, UseItemSimple<E> function)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    function(array[x, y], x, y);
                }
            }
        }

        /// <summary>
        /// Depricated - Use ArrayHelper.DoForEach()
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="array"></param>
        /// <param name="function"></param>
        public static void ForAll<E>(E[, ,] array, UseItem<E> function)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    for (int z = 0; z < array.GetLength(2); z++)
                        function(array[x, y, z], x, y, z);
                }
            }
        }

        /// <summary>
        /// Depricated - Use ArrayHelper.DoForEach()
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="array"></param>
        /// <param name="function"></param>
        public static void ForAll<E>(E[] array, UseItem<E> function)
        {
            for (int x = 0; x < array.GetLength(0); x++)
            {
                function(array[x], x);
            }
        }
    }
}