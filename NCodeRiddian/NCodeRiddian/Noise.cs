using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;

namespace NCodeRiddian
{
    /// <summary>
    /// Provides noise and smoothing functions for 2D float arrays
    /// </summary>
    public class Noise
    {
        /// <summary>
        /// Smooths an array by seting each value to the average of values within a square distance. Mutates the original array.
        /// </summary>
        /// <param name="flt">Array to smooth</param>
        /// <param name="radius">Radius of smoothing</param>
        /// <returns>Smoothed Array</returns>
        public static float[,] SimpleSmooth(float[,] flt, int radius)
        {
            for (int x = 0; x < flt.GetLength(0); x++)
            {
                for (int y = 0; y < flt.GetLength(1); y++)
                {
                    float sum = 0;
                    float numChecked = 0;
                    for (int dx = Math.Max(x - radius, 0); dx <= x + radius && dx < flt.GetLength(0); dx++)
                    {
                        for (int dy = Math.Max(y - radius, 0); dy <= y + radius && dy < flt.GetLength(1); dy++)
                        {
                            numChecked++;
                            sum += flt[dx, dy];
                        }
                    }
                    flt[x, y] = sum / numChecked;
                }
            }
            return flt;
        }

        /// <summary>
        /// Multiplies each value by a given amount, centered around .5
        /// </summary>
        /// <param name="flt">Array to alter</param>
        /// <param name="amt">Amount to change. 2 makes all values further from .5, .5 makes all values closer to .5</param>
        /// <returns></returns>
        public static float[,] Emphasize(float[,] flt, float amt)
        {
            for (int x = 0; x < flt.GetLength(0); x++)
            {
                for (int y = 0; y < flt.GetLength(1); y++)
                {
                    float pmamt = flt[x, y] - .5f;
                    pmamt *= amt;
                    flt[x, y] = MathHelper.Clamp(pmamt + .5f, 0, 1);
                }
            }
            return flt;
        }

        /// <summary>
        /// Adjust this array such that its lowest value is 0 and its highest is 1, and all others are interpolated correctly in proportion to the original scaling
        /// </summary>
        /// <param name="flt"></param>
        /// <returns></returns>
        public static float[,] MaximumContrast(float[,] flt)
        {
            float greatest = 0;
            float smallest = 1;
            for (int x = 0; x < flt.GetLength(0); x++)
            {
                for (int y = 0; y < flt.GetLength(1); y++)
                {
                    if (flt[x, y] > greatest)
                        greatest = flt[x, y];
                    if (flt[x, y] < smallest)
                        smallest = flt[x, y];
                }
            }
            ArrayHelper.SetForEach<float>(flt, (x, y, a) => { return a - smallest; });
            greatest -= smallest;
            return Multiply(flt, 1 / greatest);
        }

        /// <summary>
        /// Directly multiply every element in the array
        /// </summary>
        /// <param name="flt"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static float[,] Multiply(float[,] flt, float amt)
        {
            ArrayHelper.SetForEach<float>(flt, (x, y, a) => { return(float)MathHelper.Clamp(a * amt, 0, 1); });
            return flt;
        }

        /// <summary>
        /// Clumps values into smooth groups based on amt.
        /// </summary>
        /// <param name="flt"></param>
        /// <param name="amt"></param>
        /// <returns></returns>
        public static float[,] Clump(float[,] flt, float amt)
        {
            for (int x = 0; x < flt.GetLength(0); x++)
            {
                for (int y = 0; y < flt.GetLength(1); y++)
                {
                    flt[x, y] = amt * ((int)(flt[x, y] / amt));
                }
            }
            return flt;
        }
    }

    public class Perlin
    {
        public static float[,] GenerateOctave(int SizeX, int SizeY, int SegmentSize, float Amplitude)
        {
            float[,] output = new float[SizeX, SizeY];
            float[,] gen = new float[(int)Math.Ceiling((double)SizeX / SegmentSize) + 2, (int)Math.Ceiling((double)SizeY / SegmentSize) + 2];
            ArrayHelper.SetForEach<float>(gen, (x,y,z)=>{return (float)GlobalRandom.random.NextDouble() * Amplitude;});
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    float A = gen[(x / SegmentSize) + 1, (y / SegmentSize) + 1];
                    float B = gen[(x / SegmentSize) + 2, (y / SegmentSize) + 1];
                    float C = gen[(x / SegmentSize) + 1, (y / SegmentSize) + 2];
                    float D = gen[(x / SegmentSize) + 2, (y / SegmentSize) + 2];

                    float XA = MathHelper.SmoothStep(A, B, ((float)x / SegmentSize) % 1);
                    float XB = MathHelper.SmoothStep(C, D, ((float)x / SegmentSize) % 1);
                    output[x,y] = MathHelper.SmoothStep(XA, XB, ((float)y / SegmentSize) % 1);
                }
            }
            return output;
        }

        public static float[,] GenerateNoise_Powers(int SizeX, int SizeY, float Persistance, float initialAmplitude)
        {
            //frequency = 2^i
            //amplitude = persistence^i
            int i = 0;
            bool done = false;
            float[,] output = new float[SizeX, SizeY];
            ArrayHelper.SetForEach<float>(output, (x, y, z) => { return .5f; });
            float[,] NextOctave;
            float amplitude = initialAmplitude;
            int freq = 1;
            while (!done)
            {
                int Size = Math.Min(SizeX, SizeY) / freq;
                if (Size <= 1)
                {
                    Size = 1;
                    done = true;
                }
                NextOctave = GenerateOctave(SizeX, SizeY, Size, amplitude);
                ArrayHelper.SetForEach<float>(output, (x, y, z) => { return MathHelper.Clamp(Add(z, NextOctave[x, y]), 0, 1); });
                freq *= 2;
                amplitude *= Persistance;
            }
            return output;
        }

        public static float Add(float A, float B)
        {
            return (A + B);
        }
    }
}
