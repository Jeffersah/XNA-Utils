using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public class FOVCalculator
    {
        private static int[] MainXMovement = { 1, 0, 0, -1, -1, 0, 0, 1 };
        private static int[] MainYMovement = { 0, -1, -1, 0, 0, 1, 1, 0 };
        private static int[] SubXMovement = { 0, 1, -1, 0, 0, -1, 1, 0 };
        private static int[] SubYMovement = { -1, 0, 0, -1, 1, 0, 0, 1 };
        private static SimpleFraction[] StartingFractions = { new SimpleFraction(0, 1), 
                                                                    new SimpleFraction(1, 0), 
                                                                    new SimpleFraction(1, 0), 
                                                                    new SimpleFraction(0, -1), 
                                                                    new SimpleFraction(0, -1), 
                                                                    new SimpleFraction(-1, 0), 
                                                                    new SimpleFraction(-1, 0), 
                                                                    new SimpleFraction(0, 1)};

        private static SimpleFraction[] StartingFractionsMax = {  new SimpleFraction(1, -1),
                                                                        new SimpleFraction(1, 1),
                                                                        new SimpleFraction(-1, 1), 
                                                                        new SimpleFraction(-1, -1)};


        public void RayCast(Point source, I_Seeable[,] World, float range)
        {
        }

        public void CircularFOV(Point source, I_Seeable[,] World, int range, bool topology)
        {
        }

        public static void RecursiveFOV(Point source, I_Seeable[,] World, int range, params object [] args)
        {
            for (int Q = 0; Q < 8; Q++)
            {
                RecursiveFOVHelper(source, World, StartingFractions[Q], StartingFractionsMax[Q / 2], Q, 1, range, args);
            }
        }

        private static void RecursiveFOVHelper(Point source, I_Seeable[,] World, SimpleFraction minAngle, SimpleFraction maxAngle, int quad, int distance, int range, params object [] args)
        {
            if (minAngle.Compair(maxAngle) == 0 || distance >= range)
                return;

            bool hasHitOpen = false;
            SimpleFraction NewMin = minAngle;
            int numberOfTiles = (int)Math.Abs(Math.Floor(maxAngle.Subtract(minAngle).Multiply(distance)));
            //Console.Out.WriteLine("Quad {0}, Distance {1}: {2}->{3}", quad, distance, minAngle, maxAngle);
            Point firstTile = new Point((MainXMovement[quad] * distance + (int)Math.Ceiling(minAngle.MultiplyInverted(SubXMovement[quad]*distance)) + source.X),
                                        (MainYMovement[quad] * distance + (int)Math.Ceiling(minAngle.Multiply(SubYMovement[quad] * distance))) + source.Y);
            //Console.Out.WriteLine("First Tile : {0},{1} tot",firstTile, numberOfTiles);
            for (int i = 0; i < numberOfTiles; i++)
            {
                Point currentPoint = new Point(firstTile.X + SubXMovement[quad] * i, firstTile.Y + SubYMovement[quad] * i);
                if (currentPoint.X >= World.GetLength(0) || currentPoint.X < 0 || currentPoint.Y >= World.GetLength(1) || currentPoint.Y < 0)
                {
                    if (hasHitOpen)
                    {
                        RecursiveFOVHelper(source, World, NewMin, maxAngle, quad, distance + 1, range, args);
                    }
                    return;
                }

                I_Seeable current = World[currentPoint.X, currentPoint.Y];
                //current.debug("" + i);
                current.See(args);

                if (current.isSightBlocking())
                {
                    if (hasHitOpen)
                    {
                       // current.debug("M" + new SimpleFraction(currentPoint.Y, currentPoint.X - SubXMovement[quad]).toString());
                        RecursiveFOVHelper(source, World, NewMin, new SimpleFraction(currentPoint.Y, currentPoint.X - SubXMovement[quad]), quad, distance + 1, range, args);
                        hasHitOpen = false;
                    }
                    else
                    {
                       // current.debug("m" + new SimpleFraction(currentPoint.Y + SubYMovement[quad], currentPoint.X + SubXMovement[quad]).toString());
                        NewMin = new SimpleFraction(currentPoint.Y + SubYMovement[quad], currentPoint.X + SubXMovement[quad]);
                    }
                }
                else
                {
                    if (!hasHitOpen)
                    {
                       // current.debug("" + i + "H");
                    }
                    hasHitOpen = true;
                }
            }

            if (hasHitOpen)
            {
                RecursiveFOVHelper(source, World, NewMin, maxAngle, quad, distance + 1, range, args);
            }
        }
    }

    public interface I_Seeable
    {
        bool isSightBlocking();
        void See(params object [] args);
        void debug(string s);
        void unsee();
    }

    public struct SimpleFraction
    {
        int Numerator;
        int Denominator;

        public SimpleFraction(int Num, int Denom)
        {
            Numerator = Num;
            Denominator = Denom;
            simplify();
        }
        
        public int Compair(SimpleFraction s2)
        {
            return (Numerator * s2.Denominator) - (s2.Numerator * Denominator);
        }

        public SimpleFraction Subtract(SimpleFraction s2)
        {
            int newdenom = Denominator * s2.Denominator;
            int newnum = (Numerator * s2.Denominator) - (Denominator * s2.Numerator);
            SimpleFraction output = new SimpleFraction(newnum, newdenom);

            return output;
        }

        public void simplify()
        {
            int GCD = getGCD(Numerator, Denominator);
            Numerator /= GCD;
            Denominator /= GCD;
        }

        public SimpleFraction MultiplyBy(int i)
        {
            Numerator *= i;
            Denominator *= i;
            simplify();
            return this;
        }

        public double Multiply(int i)
        {
            double d = i * Numerator;
            return Denominator == 0 ? 0 : d / ((double)Denominator);
        }
        public double MultiplyInverted(int i)
        {
            double d = i * Denominator;
            return Numerator == 0 ? 0 : d / ((double)Numerator);
        }

        private int getGCD(int n1, int n2)
        {
            if (n1 == 0)
                return 1;

            int remainder = n2 % n1;
            if (remainder != 0)
                return getGCD(remainder, n1);
            return n1;
        }

        public string toString()
        {
            return Numerator + "/" + Denominator;
        }
        public override string ToString()
        {
            return Numerator + "/" + Denominator;
        }
    }
}
