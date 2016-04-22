using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    /// <summary>
    /// Class to automatically cycle through array elements
    /// </summary>
    public abstract class ArrayHelper
    {
        /// <summary>
        /// Set every element in an array to the return value of a delegation
        /// </summary>
        /// <typeparam name="T">Type of the Array</typeparam>
        /// <param name="arr">The array</param>
        /// <param name="DO">The delegate to set the values taking input (Key->Value)</param>
        public static void SetForEach<T>(T[] arr, Func<int, T, T> DO)
        {
            for (int x = 0; x < arr.Length; x++)
                arr[x] = DO(x, arr[x]);
        }
        /// <summary>
        /// Perform a delegation with each element in the array
        /// </summary>
        /// <typeparam name="T">Type of the Array</typeparam>
        /// <param name="arr">The array</param>
        /// <param name="DO">The delegate to run taking input (Key->Value)</param>
        public static void DoForEach<T>(T[] arr, Action<int, T> DO)
        {
            for (int x = 0; x < arr.Length; x++)
                DO(x, arr[x]);
        }
        /// <summary>
        /// Set every element in an array to the return value of a delegation
        /// </summary>
        /// <typeparam name="T">Type of the Array</typeparam>
        /// <param name="arr">The array</param>
        /// <param name="DO">The delegate to set the values taking input (X,Y,Value)</param>
        public static void SetForEach<T>(T[,] arr, Func<int, int, T, T> DO)
        {
            for (int x = 0; x < arr.GetLength(0); x++)
                for(int y = 0; y < arr.GetLength(1); y++)
                    arr[x,y] = DO(x,y, arr[x,y]);
        }
        /// <summary>
        /// Perform a delegation with each element in the array
        /// </summary>
        /// <typeparam name="T">Type of the Array</typeparam>
        /// <param name="arr">The array</param>
        /// <param name="DO">The delegate to run taking input (X,Y,Value)</param>
        public static void DoForEach<T>(T[,] arr, Action<int, int, T> DO)
        {
            for (int x = 0; x < arr.GetLength(0); x++)
                for (int y = 0; y < arr.GetLength(1); y++)
                    DO(x, y, arr[x, y]);
        }

        /// <summary>
        /// Set every element in an array to the return value of a delegation
        /// </summary>
        /// <typeparam name="T">Type of the Array</typeparam>
        /// <param name="arr">The array</param>
        /// <param name="DO">The delegate to set the values taking input (X,Y,Z,Value)</param>
        public static void SetForEach<T>(T[,,] arr, Func<int, int, int, T, T> DO)
        {
            for (int x = 0; x < arr.GetLength(0); x++)
                for (int y = 0; y < arr.GetLength(1); y++)
                    for(int z = 0; z < arr.GetLength(2); z++)
                        arr[x, y,z] = DO(x, y,z, arr[x, y,z]);
        }
        /// <summary>
        /// Perform a delegation with each element in the array
        /// </summary>
        /// <typeparam name="T">Type of the Array</typeparam>
        /// <param name="arr">The array</param>
        /// <param name="DO">The delegate to run taking input (X,Y,Z,Value)</param>
        public static void DoForEach<T>(T[,,] arr, Action<int, int, int, T> DO)
        {
            for (int x = 0; x < arr.GetLength(0); x++)
                for (int y = 0; y < arr.GetLength(1); y++)
                    for (int z = 0; z < arr.GetLength(2); z++)
                        DO(x, y, z, arr[x, y, z]);
        }
    }
}
