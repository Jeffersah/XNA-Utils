using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    public struct PerlinLayer_Old
    {
        public float Degree;
        public int Size;
        public PerlinLayer_Old(float amplitutde, int size)
        {
            Size = size;
            Degree = amplitutde;
        }

        public float[,] AddLayer(float[,] orig)
        {
            for (int x = 0; x < orig.GetLength(0); x += Size)
            {
                for (int y = 0; y < orig.GetLength(1); y += Size)
                {
                    float amt = (float)(GlobalRandom.random.NextDouble() * Degree);
                    for (int tx = x; tx < orig.GetLength(0) && tx < x + Size; tx++)
                    {
                        for (int ty = y; ty < orig.GetLength(1) && ty < y + Size; ty++)
                        {
                            orig[tx, ty] += amt;
                        }
                    }
                }
            }
            return orig;
        }
    }
    
    public class Perlin_Old
    {
        public static float[,] Generate(int xsize, int ysize, PerlinLayer_Old[] Layers)
        {
            float[,] tgt = new float[xsize, ysize];
            foreach (PerlinLayer_Old play in Layers)
            {
                tgt = play.AddLayer(tgt);
            }
            return tgt;
        }

        public static float[,] Generate(int size)
        {
            float[,] flt = new float[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    flt[x, y] = .5f;
                }
            }

            int numLayers = (int)Math.Ceiling(Math.Log(size) / Math.Log(2));
            for (int l = 0; l < numLayers; l++)
            {
                GenerateOctave(flt, (int)Math.Ceiling(size / Math.Pow(2, l)), (float)(numLayers - l) / numLayers);
            }

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    flt[x, y] = Math.Min(1, Math.Max(0, flt[x, y]));
                }
            }

            return flt;
        }

        private static void GenerateOctave(float[,] orig, int size, float limit)
        {
            for (int x = 0; x < orig.GetLength(0); x += size)
            {
                for (int y = 0; y < orig.GetLength(1); y += size)
                {
                    for (int dx = 0; dx < size && dx + x < orig.GetLength(0); dx++)
                    {
                        for (int dy = 0; dy < size && dy + y < orig.GetLength(0); dy++)
                        {
                            orig[x + dx, y + dy] += (float)GlobalRandom.NextBetween(-limit / 2, limit / 2);
                        }
                    }
                }
            }
        }

        public static float[,] Smooth(float[,] flt, int radius)
        {
            for (int x = 0; x < flt.GetLength(0); x++)
            {
                for (int y = 0; y < flt.GetLength(1); y++)
                {
                    float sum = 0;
                    float numChecked = 0;
                    for (int dx = x - radius; dx <= x + radius; dx++)
                    {
                        for (int dy = y - radius; dy <= y + radius; dy++)
                        {
                            if (dx > 0 && dy > 0 && dx < flt.GetLength(0) && dy < flt.GetLength(1))
                            {
                                numChecked++;
                                sum += flt[dx, dy];
                            }
                        }
                    }
                    flt[x, y] = sum / numChecked;
                }
            }
            return flt;
        }

        public static void Emphasize(float[,] arr, float amt)
        {
            amt = 1 / amt;
            for (int x = 0; x < arr.GetLength(0); x++)
            {
                for (int y = 0; y < arr.GetLength(1); y++)
                {
                    float adj = arr[x, y] - .5f;
                    adj /= amt;
                    adj = Math.Min(1, Math.Max(-1, adj));
                    arr[x, y] = adj;
                }
            }
        }
    }
}
