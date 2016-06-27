using System;
using System.Collections.Generic;

namespace NCodeRiddian
{
    /// <summary>
    /// Class for pulling random numbers from anywhere in the program
    /// </summary>
    public abstract class GlobalRandom
    {
        /// <summary>
        /// A static random object to be used to generate random numbers
        /// </summary>
        public static Random random = new Random();

        /// <summary>
        /// Returns a random value between two specified values
        /// </summary>
        /// <param name="d1">The lower bound</param>
        /// <param name="d2">The upper bound</param>
        /// <returns>A random number x such that d1&lt;x&lt;d2 </returns>
        public static double NextBetween(double d1, double d2)
        {
            return d1 + (random.NextDouble() * (d2 - d1));
        }

        public static T RandomFrom<T>(T[] array)
        {
            return array[random.Next(array.Length)];
        }
        public static T RandomFrom<T>(List<T> array)
        {
            return array[random.Next(array.Count)];
        }
    }
}