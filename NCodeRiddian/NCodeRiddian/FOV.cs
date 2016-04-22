using System;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public class FOV
    {
        #region oldFOV

        /*
        static Point[] MainDirections = {new Point(1, 0),       // Issue if < 0 ???
                                         new Point(0, -1),
                                         new Point(0, -1),
                                         new Point(-1, 0),
                                         new Point(-1, 0),
                                         new Point(0, 1),
                                         new Point(0, 1),
                                         new Point(1, 0),
                                        };
        static Point[] SubDirections = {new Point(0, -1),
                                  new Point(1, 0),
                                  new Point(-1, 0),
                                  new Point(0, -1),
                                  new Point(0, 1),
                                  new Point(-1, 0),
                                  new Point(1, 0),
                                  new Point(0, 1),
                                 };
        static Point[] TargetAngles = { new Point(1, -1), new Point(-1, -1), new Point(-1, 1), new Point(1, 1) };
        static Point[] getMax = { new Point(1, 1), //0
                                  new Point(0, 0), //1
                                  new Point(1, 0), //2
                                  new Point(0, 1), //3
                                  new Point(0, 0), //4
                                  new Point(1, 1), //5
                                  new Point(0, 1),
                                  new Point(1, 0) };
        static Point[] getMin = { new Point(0,0), //0
                                  new Point(1,1), //1
                                  new Point(0, 1), //2
                                  new Point(1, 0), //3
                                  new Point(1, 1), //4
                                  new Point(0, 0), //5
                                  new Point(1, 0),
                                  new Point(0, 1) };

        public static void Calculate(Point Source, I_Seeable[,] World, int range)
        {
            World[Source.X, Source.Y].see(1);
            for (int Q = 0; Q < 8; Q++)
            {
                FOVHelper(Source, World, Q, MainDirections[Q], TargetAngles[Q / 2], 1, range);
            }
        }

        private static void FOVHelper(Point source, I_Seeable[,] World, int quad, Point minAngle, Point maxAngle, int distance, int sightRange)
        {
            Point MainDirection = MainDirections[quad];
            Point SubDirection = SubDirections[quad];
            Point Current = new Point( source.X + (MainDirection.X * distance) + Math.Abs(SubDirection.X) * (minAngle.Y == 0 ? 0 : (distance * minAngle.X) / minAngle.Y),
                                       source.Y + (MainDirection.Y * distance) + Math.Abs(SubDirection.Y) * (minAngle.X == 0 ? 0 : (distance * minAngle.Y) / minAngle.X));
            Point Target = new Point(  source.X + (MainDirection.X * distance) + Math.Abs(SubDirection.X) * (maxAngle.Y == 0 ? 0 : (distance * maxAngle.X) / maxAngle.Y),
                                       source.Y + (MainDirection.Y * distance) + Math.Abs(SubDirection.Y) * (maxAngle.X == 0 ? 0 : (distance * maxAngle.Y) / maxAngle.X));
            SubDirection = new Point(Math.Sign(Target.X - Current.X), Math.Sign(Target.Y - Current.Y));
            //if (distance == 1)
              //  Console.Out.WriteLine("Quad {0}, SUB {1}, DIR {2}, ANGLE {3}-{4}", quad, new Point(Target.X - Current.X, Target.Y - Current.Y), SubDirection, minAngle, maxAngle);
            Point newMin = minAngle;
            bool hasSeenOpen = false;
            int cur = 0;
            while (!Current.Equals(Target) && Current.X >= 0 && Current.X < World.GetLength(0) && Current.Y >= 0 && Current.Y < World.GetLength(1))
            {
                I_Seeable CurrentSeeable = World[Current.X, Current.Y];

                CurrentSeeable.see(1);

                CurrentSeeable.debug("" + cur);
                if (CurrentSeeable.isSightBlocking())
                {
                    if (hasSeenOpen && distance < sightRange)
                    {
                        hasSeenOpen = false;
                        Point newMax = new Point(Current.X - source.X + getMax[quad].X, Current.Y - source.Y - getMax[quad].Y);
                        CurrentSeeable.debug("" + newMax.X + "/" + newMax.Y);
                        FOVHelper(source, World, quad, newMin, newMax, distance + 1, sightRange);
                        newMin.X = Current.X - source.X + getMin[quad].X;
                        newMin.Y = Current.Y - source.Y + getMin[quad].Y;
                    }
                    else
                    {
                        newMin.X = Current.X - source.X + getMin[quad].X;
                        newMin.Y = Current.Y - source.Y + getMin[quad].Y;
                        CurrentSeeable.debug("" + newMin.X + "/" + newMin.Y);
                    }
                }
                else
                {
                    hasSeenOpen = true;
                }
                cur++;
                Current.X += SubDirection.X;
                Current.Y += SubDirection.Y;
            }

            if (hasSeenOpen && distance < sightRange)
            {
                FOVHelper(source, World, quad, newMin, maxAngle, distance + 1, sightRange);
            }
        }

        public static void CalculateSimple(Vector2 Source, I_Seeable[,] World, int range)
        {
        }
        */

        #endregion oldFOV

        #region NewFOV

        //  Octant data
        //
        //    \ 1 | 2 /
        //   8 \  |  / 3
        //   -----+-----
        //   7 /  |  \ 4
        //    / 6 | 5 \
        //
        //  1 = NNW, 2 =NNE, 3=ENE, 4=ESE, 5=SSE, 6=SSW, 7=WSW, 8 = WNW
        private static int[] VisibleOctants = { 1, 2, 3, 4, 5, 6, 7, 8 };

        /// <summary>
        /// Start here: go through all the octants which surround the player to
        /// determine which open cells are visible
        /// </summary>
        public static void Calculate(Point Source, I_Seeable[,] World, int range)
        {
            foreach (int o in VisibleOctants)
            {
                ScanOctant(Source, World, 1, o, 1.0, 0.0, range);
                //ScanOctantCompact(Source, World, 1, o, 1, 0, range);
            }
        }

        public static bool[,] GetSightMap(Point Source, I_Seeable[,] World, int range)
        {
            bool[,] ret = new bool[World.GetLength(0), World.GetLength(1)];
            bool[,] worldCol = new bool[World.GetLength(0), World.GetLength(1)];
            ArrayManager.ForAll<bool>(ret, (x, y) => x = false);
            ArrayManager.ForAll<bool>(worldCol, (x, y) => x = World[y[0], y[1]].isSightBlocking());
            foreach (int o in VisibleOctants)
            {
                ret = ScanOctant(Source, worldCol, ret, 1, o, 1.0, 0.0, range);
                //ScanOctantCompact(Source, World, 1, o, 1, 0, range);
            }

            return ret;
        }

        public static bool[,] GetSightMap(Point Source, bool[,] World, int range)
        {
            bool[,] ret = new bool[World.GetLength(0), World.GetLength(1)];

            ArrayManager.ForAll<bool>(ret, (x, y) => x = false);

            foreach (int o in VisibleOctants)
            {
                ret = ScanOctant(Source, World, ret, 1, o, 1.0, 0.0, range);
                //ScanOctantCompact(Source, World, 1, o, 1, 0, range);
            }

            return ret;
        }

        /*

        static Point[] MainMotion = { new Point(0, -1),
                                      new Point(0, -1),
                                      new Point(1, 0),
                                      new Point(1, 0),
                                      new Point(0, 1),
                                      new Point(0, 1),
                                      new Point(-1, 0),
                                      new Point(-1, 0)};
        static Point[] SubMotion = { new Point(-1, 0),
                                      new Point(1, 0),
                                      new Point(0, -1),
                                      new Point(0, 1),
                                      new Point(1, 0),
                                      new Point(-1, 0),
                                      new Point(0, 1),
                                      new Point(0, -1)};

        static bool[] CompareGT = { true, false, false, true, true, false, false, true };

        protected static void ScanOctantCompact(Point Source, I_Seeable[,] World, int pDepth, int pOctant, double pStartSlope, double pEndSlope, int range)
        {
            int x = 0, y = 0;
            Point MainMove = MainMotion[pOctant - 1];
            Point SubMove = SubMotion[pOctant - 1];
            bool mainX = true;
            int sub = 1;
            int main = 1;
            if (MainMove.X != 0)
            {
                x = Source.X + pDepth * MainMove.X;
                main = MainMove.X;
                x = Math.Max(0, Math.Min(World.GetLength(0), x));
                sub = SubMove.Y;
                y = Source.Y + SubMove.Y * Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                mainX = true;
            }
            else if (MainMove.Y != 0)
            {
                mainX = false;
                main = MainMove.Y;
                y = Source.Y + pDepth * MainMove.Y;
                y = Math.Max(0, Math.Min(World.GetLength(1), y));
                sub = SubMove.X;
                x = Source.X + SubMove.X * Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
            }
            bool cgt = CompareGT[pOctant - 1];
            while((cgt && GetSlope(x, y, Source.X, Source.Y, !mainX) >= pEndSlope) ||
                  (!cgt && GetSlope(x, y, Source.X, Source.Y, !mainX) <= pEndSlope))
            {
                if (LocationManager.distanceCheck(LocationManager.getVectorFromPoint(Source), new Vector2(x, y), range))
                {
                    World[x, y].see(1);
                    int useSub = (mainX ? y : x) + sub;
                    if (World[x, y].isSightBlocking()) //current cell blocked
                    {
                        if (useSub >= 0 && useSub < World.GetLength((mainX ? 1 : 0)) && !World[x + (mainX ? 0 : sub), y + (mainX ? sub : 0)].isSightBlocking()) //prior cell within range AND open...
                            //...incremenet the depth, adjust the endslope and recurse
                            ScanOctantCompact(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope((mainX ? x - .5 * main : x + sub * .5), (!mainX ? y + .5 * main : y + sub * .5), Source.X, Source.Y, mainX), range);
                    }
                    else
                    {
                        if (useSub >= 0 && useSub < World.GetLength((mainX ? 1 : 0)) && World[x + (mainX ? 0 : sub), y + (mainX ? sub : 0)].isSightBlocking()) //prior cell within range AND open...
                            //..adjust the startslope
                            pStartSlope = GetSlope((mainX ? x + .5 * main : x + sub * .5), (!mainX ? y + .5 * main : y + sub * .5), Source.X, Source.Y, mainX);
                    }
                }
                if (mainX)
                {
                    y -= sub;
                }
                else
                {
                    x -= sub;
                }
            }
            if (mainX)
            {
                y += sub;
            }
            else
            {
                x += sub;
            }

            if (x < 0)
                x = 0;
            else if (x >= World.GetLength(0))
                x = World.GetLength(0) - 1;

            if (y < 0)
                y = 0;
            else if (y >= World.GetLength(1))
                y = World.GetLength(1) - 1;

            if (pDepth < range & !World[x, y].isSightBlocking())
                ScanOctantCompact(Source, World, pDepth + 1, pOctant, pStartSlope, pEndSlope, range);
        }

        */

        /// <summary>
        /// Examine the provided octant and calculate the visible cells within it.
        /// </summary>
        /// <param name="pDepth">Depth of the scan</param>
        /// <param name="pOctant">Octant being examined</param>
        /// <param name="pStartSlope">Start slope of the octant</param>
        /// <param name="pEndSlope">End slope of the octance</param>
        protected static void ScanOctant(Point Source, I_Seeable[,] World, int pDepth, int pOctant, double pStartSlope, double pEndSlope, int range)
        {
            int visrange2 = range * range;
            int x = 0;
            int y = 0;

            switch (pOctant)
            {
                case 1: //nnw // 0, -1
                    // -1, 0
                    y = Source.Y - pDepth;
                    if (y < 0) return;

                    x = Source.X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x < 0) x = 0;

                    while (GetSlope(x, y, Source.X, Source.Y, false) >= pEndSlope)
                    {
                        if (LocationManager.distanceCheck(LocationManager.getVectorFromPoint(Source), new Vector2(x, y), range))
                        {
                            World[x, y].see(1);
                            if (World[x, y].isSightBlocking()) //current cell blocked
                            {
                                if (x - 1 >= 0 && !World[x - 1, y].isSightBlocking()) //prior cell within range AND open...
                                    //...incremenet the depth, adjust the endslope and recurse
                                    ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Source.X, Source.Y, false), range);
                            }
                            else
                            {
                                if (x - 1 >= 0 && World[x - 1, y].isSightBlocking()) //prior cell within range AND open...
                                    //..adjust the startslope
                                    pStartSlope = GetSlope(x - 0.5, y - 0.5, Source.X, Source.Y, false);
                            }
                        }
                        x++;
                    }
                    x--;
                    break;

                case 2: //nne

                    y = Source.Y - pDepth;
                    if (y < 0) return;

                    x = Source.X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x >= World.GetLength(0)) x = World.GetLength(0) - 1;

                    while (GetSlope(x, y, Source.X, Source.Y, false) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            World[x, y].see(1);
                            if (World[x, y].isSightBlocking())
                            {
                                if (x + 1 < World.GetLength(0) && !World[x + 1, y].isSightBlocking())
                                    ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Source.X, Source.Y, false), range);
                            }
                            else
                            {
                                if (x + 1 < World.GetLength(0) && World[x + 1, y].isSightBlocking())
                                    pStartSlope = -GetSlope(x + 0.5, y - 0.5, Source.X, Source.Y, false);
                            }
                        }
                        x--;
                    }
                    x++;
                    break;

                case 3:

                    x = Source.X + pDepth;
                    if (x >= World.GetLength(0)) return;

                    y = Source.Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y < 0) y = 0;

                    while (GetSlope(x, y, Source.X, Source.Y, true) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            World[x, y].see(1);
                            if (World[x, y].isSightBlocking())
                            {
                                if (y - 1 >= 0 && !World[x, y - 1].isSightBlocking())
                                    ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Source.X, Source.Y, true), range);
                            }
                            else
                            {
                                if (y - 1 >= 0 && World[x, y - 1].isSightBlocking())
                                    pStartSlope = -GetSlope(x + 0.5, y - 0.5, Source.X, Source.Y, true);
                            }
                        }
                        y++;
                    }
                    y--;
                    break;

                case 4:

                    x = Source.X + pDepth;
                    if (x >= World.GetLength(0)) return;

                    y = Source.Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y >= World.GetLength(1)) y = World.GetLength(1) - 1;

                    while (GetSlope(x, y, Source.X, Source.Y, true) >= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            World[x, y].see(1);
                            if (World[x, y].isSightBlocking())
                            {
                                if (y + 1 < World.GetLength(1) && !World[x, y + 1].isSightBlocking())
                                    ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Source.X, Source.Y, true), range);
                            }
                            else
                            {
                                if (y + 1 < World.GetLength(1) && World[x, y + 1].isSightBlocking())
                                    pStartSlope = GetSlope(x + 0.5, y + 0.5, Source.X, Source.Y, true);
                            }
                        }
                        y--;
                    }
                    y++;
                    break;

                case 5:

                    y = Source.Y + pDepth;
                    if (y >= World.GetLength(1)) return;

                    x = Source.X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x >= World.GetLength(0)) x = World.GetLength(0) - 1;

                    while (GetSlope(x, y, Source.X, Source.Y, false) >= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            World[x, y].see(1);
                            if (World[x, y].isSightBlocking())
                            {
                                if (x + 1 < World.GetLength(1) && !World[x + 1, y].isSightBlocking())
                                    ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Source.X, Source.Y, false), range);
                            }
                            else
                            {
                                if (x + 1 < World.GetLength(1)
                                        && World[x + 1, y].isSightBlocking())
                                    pStartSlope = GetSlope(x + 0.5, y + 0.5, Source.X, Source.Y, false);
                            }
                        }
                        x--;
                    }
                    x++;
                    break;

                case 6:

                    y = Source.Y + pDepth;
                    if (y >= World.GetLength(1)) return;

                    x = Source.X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x < 0) x = 0;

                    while (GetSlope(x, y, Source.X, Source.Y, false) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            World[x, y].see(1);
                            if (World[x, y].isSightBlocking())
                            {
                                if (x - 1 >= 0 && !World[x - 1, y].isSightBlocking())
                                    ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Source.X, Source.Y, false), range);
                            }
                            else
                            {
                                if (x - 1 >= 0
                                        && World[x - 1, y].isSightBlocking())
                                    pStartSlope = -GetSlope(x - 0.5, y + 0.5, Source.X, Source.Y, false);
                            }
                        }
                        x++;
                    }
                    x--;
                    break;

                case 7:

                    x = Source.X - pDepth;
                    if (x < 0) return;

                    y = Source.Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y >= World.GetLength(1)) y = World.GetLength(1) - 1;

                    while (GetSlope(x, y, Source.X, Source.Y, true) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            World[x, y].see(1);
                            if (World[x, y].isSightBlocking())
                            {
                                if (y + 1 < World.GetLength(1) && !World[x, y + 1].isSightBlocking())
                                    ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Source.X, Source.Y, true), range);
                            }
                            else
                            {
                                if (y + 1 < World.GetLength(1) && World[x, y + 1].isSightBlocking())
                                    pStartSlope = -GetSlope(x - 0.5, y + 0.5, Source.X, Source.Y, true);
                            }
                        }
                        y--;
                    }
                    y++;
                    break;

                case 8: //wnw

                    x = Source.X - pDepth;
                    if (x < 0) return;

                    y = Source.Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y < 0) y = 0;

                    while (GetSlope(x, y, Source.X, Source.Y, true) >= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            World[x, y].see(1);
                            if (World[x, y].isSightBlocking())
                            {
                                if (y - 1 >= 0 && !World[x, y - 1].isSightBlocking())
                                    ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Source.X, Source.Y, true), range);
                            }
                            else
                            {
                                if (y - 1 >= 0 && World[x, y - 1].isSightBlocking())
                                    pStartSlope = GetSlope(x - 0.5, y - 0.5, Source.X, Source.Y, true);
                            }
                        }
                        y++;
                    }
                    y--;
                    break;
            }

            if (x < 0)
                x = 0;
            else if (x >= World.GetLength(0))
                x = World.GetLength(0) - 1;

            if (y < 0)
                y = 0;
            else if (y >= World.GetLength(1))
                y = World.GetLength(1) - 1;

            if (pDepth < range & !World[x, y].isSightBlocking())
                ScanOctant(Source, World, pDepth + 1, pOctant, pStartSlope, pEndSlope, range);
        }

        protected static bool[,] ScanOctant(Point Source, bool[,] World, bool[,] Ret, int pDepth, int pOctant, double pStartSlope, double pEndSlope, int range)
        {
            int visrange2 = range * range;
            int x = 0;
            int y = 0;

            switch (pOctant)
            {
                case 1: //nnw // 0, -1
                    // -1, 0
                    y = Source.Y - pDepth;
                    if (y < 0) return Ret;

                    x = Source.X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x < 0) x = 0;

                    while (GetSlope(x, y, Source.X, Source.Y, false) >= pEndSlope)
                    {
                        if (LocationManager.distanceCheck(LocationManager.getVectorFromPoint(Source), new Vector2(x, y), range))
                        {
                            Ret[x, y] = true;
                            if (World[x, y]) //current cell blocked
                            {
                                if (x - 1 >= 0 && !World[x - 1, y]) //prior cell within range AND open...
                                    //...incremenet the depth, adjust the endslope and recurse
                                    ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Source.X, Source.Y, false), range);
                            }
                            else
                            {
                                if (x - 1 >= 0 && World[x - 1, y]) //prior cell within range AND open...
                                    //..adjust the startslope
                                    pStartSlope = GetSlope(x - 0.5, y - 0.5, Source.X, Source.Y, false);
                            }
                        }
                        x++;
                    }
                    x--;
                    break;

                case 2: //nne

                    y = Source.Y - pDepth;
                    if (y < 0) return Ret;

                    x = Source.X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x >= World.GetLength(0)) x = World.GetLength(0) - 1;

                    while (GetSlope(x, y, Source.X, Source.Y, false) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            Ret[x, y] = true;
                            if (World[x, y])
                            {
                                if (x + 1 < World.GetLength(0) && !World[x + 1, y])
                                    ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Source.X, Source.Y, false), range);
                            }
                            else
                            {
                                if (x + 1 < World.GetLength(0) && World[x + 1, y])
                                    pStartSlope = -GetSlope(x + 0.5, y - 0.5, Source.X, Source.Y, false);
                            }
                        }
                        x--;
                    }
                    x++;
                    break;

                case 3:

                    x = Source.X + pDepth;
                    if (x >= World.GetLength(0)) return Ret;

                    y = Source.Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y < 0) y = 0;

                    while (GetSlope(x, y, Source.X, Source.Y, true) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            Ret[x, y] = true;
                            if (World[x, y])
                            {
                                if (y - 1 >= 0 && !World[x, y - 1])
                                    ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Source.X, Source.Y, true), range);
                            }
                            else
                            {
                                if (y - 1 >= 0 && World[x, y - 1])
                                    pStartSlope = -GetSlope(x + 0.5, y - 0.5, Source.X, Source.Y, true);
                            }
                        }
                        y++;
                    }
                    y--;
                    break;

                case 4:

                    x = Source.X + pDepth;
                    if (x >= World.GetLength(0)) return Ret;

                    y = Source.Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y >= World.GetLength(1)) y = World.GetLength(1) - 1;

                    while (GetSlope(x, y, Source.X, Source.Y, true) >= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            Ret[x, y] = true;
                            if (World[x, y])
                            {
                                if (y + 1 < World.GetLength(1) && !World[x, y + 1])
                                    ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y + 0.5, Source.X, Source.Y, true), range);
                            }
                            else
                            {
                                if (y + 1 < World.GetLength(1) && World[x, y + 1])
                                    pStartSlope = GetSlope(x + 0.5, y + 0.5, Source.X, Source.Y, true);
                            }
                        }
                        y--;
                    }
                    y++;
                    break;

                case 5:

                    y = Source.Y + pDepth;
                    if (y >= World.GetLength(1)) return Ret;

                    x = Source.X + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x >= World.GetLength(0)) x = World.GetLength(0) - 1;

                    while (GetSlope(x, y, Source.X, Source.Y, false) >= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            Ret[x, y] = true;
                            if (World[x, y])
                            {
                                if (x + 1 < World.GetLength(1) && !World[x + 1, y])
                                    ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Source.X, Source.Y, false), range);
                            }
                            else
                            {
                                if (x + 1 < World.GetLength(1)
                                        && World[x + 1, y])
                                    pStartSlope = GetSlope(x + 0.5, y + 0.5, Source.X, Source.Y, false);
                            }
                        }
                        x--;
                    }
                    x++;
                    break;

                case 6:

                    y = Source.Y + pDepth;
                    if (y >= World.GetLength(1)) return Ret;

                    x = Source.X - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (x < 0) x = 0;

                    while (GetSlope(x, y, Source.X, Source.Y, false) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            Ret[x, y] = true;
                            if (World[x, y])
                            {
                                if (x - 1 >= 0 && !World[x - 1, y])
                                    ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, GetSlope(x - 0.5, y - 0.5, Source.X, Source.Y, false), range);
                            }
                            else
                            {
                                if (x - 1 >= 0
                                        && World[x - 1, y])
                                    pStartSlope = -GetSlope(x - 0.5, y + 0.5, Source.X, Source.Y, false);
                            }
                        }
                        x++;
                    }
                    x--;
                    break;

                case 7:

                    x = Source.X - pDepth;
                    if (x < 0) return Ret;

                    y = Source.Y + Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y >= World.GetLength(1)) y = World.GetLength(1) - 1;

                    while (GetSlope(x, y, Source.X, Source.Y, true) <= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            Ret[x, y] = true;
                            if (World[x, y])
                            {
                                if (y + 1 < World.GetLength(1) && !World[x, y + 1])
                                    ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y + 0.5, Source.X, Source.Y, true), range);
                            }
                            else
                            {
                                if (y + 1 < World.GetLength(1) && World[x, y + 1])
                                    pStartSlope = -GetSlope(x - 0.5, y + 0.5, Source.X, Source.Y, true);
                            }
                        }
                        y--;
                    }
                    y++;
                    break;

                case 8: //wnw

                    x = Source.X - pDepth;
                    if (x < 0) return Ret;

                    y = Source.Y - Convert.ToInt32((pStartSlope * Convert.ToDouble(pDepth)));
                    if (y < 0) y = 0;

                    while (GetSlope(x, y, Source.X, Source.Y, true) >= pEndSlope)
                    {
                        if (GetVisDistance(x, y, Source.X, Source.Y) <= visrange2)
                        {
                            Ret[x, y] = true;
                            if (World[x, y])
                            {
                                if (y - 1 >= 0 && !World[x, y - 1])
                                    ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, GetSlope(x + 0.5, y - 0.5, Source.X, Source.Y, true), range);
                            }
                            else
                            {
                                if (y - 1 >= 0 && World[x, y - 1])
                                    pStartSlope = GetSlope(x - 0.5, y - 0.5, Source.X, Source.Y, true);
                            }
                        }
                        y++;
                    }
                    y--;
                    break;
            }

            if (x < 0)
                x = 0;
            else if (x >= World.GetLength(0))
                x = World.GetLength(0) - 1;

            if (y < 0)
                y = 0;
            else if (y >= World.GetLength(1))
                y = World.GetLength(1) - 1;

            if (pDepth < range & !World[x, y])
                ScanOctant(Source, World, Ret, pDepth + 1, pOctant, pStartSlope, pEndSlope, range);
            return Ret;
        }

        /// <summary>
        /// Get the gradient of the slope formed by the two points
        /// </summary>
        /// <param name="pX1"></param>
        /// <param name="pY1"></param>
        /// <param name="pX2"></param>
        /// <param name="pY2"></param>
        /// <param name="pInvert">Invert slope</param>
        /// <returns></returns>
        private static double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
        {
            if (pInvert)
                return (pY1 - pY2) / (pX1 - pX2);
            else
                return (pX1 - pX2) / (pY1 - pY2);
        }

        /// <summary>
        /// Calculate the distance between the two points
        /// </summary>
        /// <param name="pX1"></param>
        /// <param name="pY1"></param>
        /// <param name="pX2"></param>
        /// <param name="pY2"></param>
        /// <returns>Distance</returns>
        private static int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
        {
            return ((pX1 - pX2) * (pX1 - pX2)) + ((pY1 - pY2) * (pY1 - pY2));
        }

        #endregion NewFOV
    }

    public interface I_Seeable
    {
        bool isSightBlocking();

        void see(float percent);

        void debug(string s);
    }
}