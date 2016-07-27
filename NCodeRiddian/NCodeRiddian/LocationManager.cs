using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace NCodeRiddian
{
    public abstract class LocationManager
    {
        /// <summary>
        /// Gets the rotation in radians between two points
        /// </summary>
        /// <param name="source">The source point</param>
        /// <param name="target">The target point</param>
        /// <returns>Radian value, the rotation between these points.</returns>
        public static float getRotation(Vector2 source, Vector2 target)
        {
            return (float)Math.Atan2(target.Y - source.Y, target.X - source.X);
        }

        public static float getDistance(Vector2 source, Vector2 target)
        {
            return (float)Math.Sqrt(Math.Pow(source.X - target.X, 2) + Math.Pow(source.Y - target.Y, 2));
        }

        public static float getDistanceSquared(Vector2 src, Vector2 tgt)
        {
            return (float)(Math.Pow(src.X - tgt.X, 2) + Math.Pow(src.Y - tgt.Y, 2));
        }

        /// <summary>
        /// Returns the new location of a Vector2 moving based on a rotation and a speed.
        /// </summary>
        /// <param name="source">The start position</param>
        /// <param name="speed">The distance to move</param>
        /// <param name="rotation">The direction to move</param>
        /// <returns>The Point of the new location</returns>
        public static Vector2 moveByRotation(Vector2 source, float speed, float rotation)
        {
            return Vector2.Add(source, new Vector2((float)(speed * Math.Cos(rotation)), (float)(speed * Math.Sin(rotation))));
        }

        public static bool distanceCheck(Vector2 point1, Vector2 point2, float maximum)
        {
            float a = point1.X - point2.X;
            float b = point1.Y - point2.Y;

            return (a * a) + (b * b) <= (maximum * maximum);
        }

        public static Vector2 getVectorFromPoint(Point p)
        {
            return new Vector2(p.X, p.Y);
        }
        /// <summary>
        /// Returns weather or not two lines intersect
        /// </summary>
        /// <param name="A">Line A</param>
        /// <param name="B">Line B</param>
        /// <returns>0 if lines do not intersect, 1 if they intersect, 2 if they share one point</returns>
        public static int LinesIntersect_Precise(Vector2[] A, Vector2[] B)
        {
            return LinesIntersect_Precise(A[0], A[1], B[0], B[1]);
        }
        /// <summary>
        /// Returns weather or not two lines intersect
        /// </summary>
        /// <param name="A">Line A, Point 0</param>
        /// <param name="B">Line A, Point 1</param>
        /// <param name="C">Line B, Point 0</param>
        /// <param name="D">Line B, Point 1</param>
        /// <returns>0 if lines do not intersect, 1 if they intersect, 2 if they share one point</returns>
        public static int LinesIntersect_Precise(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            int opA = MiniIntersect_Precise(A, B, C, D);
            int opB = MiniIntersect_Precise(C, D, A, B);
            if (opA == 2 || opB == 2)
                return 2;
            else if (opA == 1 && opB == 1)
                return 1;
            else
                return 0;
        } 
        public static bool linesIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            return MiniIntersect(A, B, C, D) && MiniIntersect(C, D, A, B);
        }

        private static int MiniIntersect_Precise(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            float var1 = (B.X - A.X) * (C.Y - B.Y) - (B.Y - A.Y) * (C.X - B.X);
            float var2 = (B.X - A.X) * (D.Y - B.Y) - (B.Y - A.Y) * (D.X - B.X);

            if (var1 == 0 || var2 == 0)
                return 2;
            else if ((var1 > 0 && var2 < 0)||(var1 < 0 && var2 > 0))
                return 1;
            else
                return 0;
        }

        private static bool MiniIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            float var1 = (B.X - A.X) * (C.Y - B.Y) - (B.Y - A.Y) * (C.X - B.X);
            float var2 = (B.X - A.X) * (D.Y - B.Y) - (B.Y - A.Y) * (D.X - B.X);

            return (var1 < 0 && var2 > 0) || (var1 > 0 && var2 < 0);
        }

        public static bool linesIntersect(Vector2[] line1, Vector2[] line2)
        {
            return linesIntersect(line1[0], line1[1], line2[0], line2[1]);
        }

        public static Vector2[][] RectangleEdges(Rectangle r) // T, R, B, L
        {
            Vector2[][] edge = new Vector2[4][];
            edge[0] = new Vector2[2];
            edge[1] = new Vector2[2];
            edge[2] = new Vector2[2];
            edge[3] = new Vector2[2];
            edge[0][0] = getVectorFromPoint(new Point(r.X, r.Y));
            edge[0][1] = getVectorFromPoint(new Point(r.X + r.Width, r.Y));

            edge[1][0] = getVectorFromPoint(new Point(r.X + r.Width, r.Y));
            edge[1][1] = getVectorFromPoint(new Point(r.X + r.Width, r.Y + r.Height));

            edge[2][0] = getVectorFromPoint(new Point(r.X + r.Width, r.Y + r.Height));
            edge[2][1] = getVectorFromPoint(new Point(r.X, r.Y + r.Height));

            edge[3][0] = getVectorFromPoint(new Point(r.X, r.Y + r.Height));
            edge[3][1] = getVectorFromPoint(new Point(r.X, r.Y));

            return edge;
        }

        public static Vector2[] RectangeCorners(Rectangle r) // TL TR BR BL
        {
            return new Vector2[] { new Vector2(r.X, r.Y), new Vector2(r.X + r.Width, r.Y), new Vector2(r.X + r.Width, r.Y + r.Height), new Vector2(r.X, r.Y + r.Height) };
        }

        public static bool FOVCheck(Vector2 pos1, float facing, float FOV, float distance, Vector2 pos2)
        {
            if (!distanceCheck(pos1, pos2, distance))
                return false;
            float dif = getRotation(pos1, pos2) - facing;
            return Math.Abs(dif) < FOV;
        }

        public static Point getPointFromVector(Vector2 v)
        {
            return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
        }
        public static Vector2? getIntersectionPoint(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            return getIntersectionPoint(new Vector2[] { A, B }, new Vector2[] { C, D });
        }
        public static Vector2? getIntersectionPoint(Vector2[] src, Vector2[] tgt)
        {
            if (LinesIntersect_Precise(src, tgt) == 0)
            {
                return null;
            }
            Vector2 si1 = convertLineToSI(src);
            Vector2 si2 = convertLineToSI(tgt);
            Vector2 pointOfIntersection = new Vector2();

            if (!float.IsInfinity(si1.X) && !float.IsInfinity(si2.X))
            {
                pointOfIntersection.X = (si2.Y - si1.Y) / (si1.X - si2.X);
                pointOfIntersection.Y = (si1.X * pointOfIntersection.X) + si1.Y;
            }
            else if (!float.IsInfinity(si2.X))
            {
                pointOfIntersection.X = src[0].X;
                pointOfIntersection.Y = (si2.X * pointOfIntersection.X) + si2.Y;
            }
            else if (!float.IsInfinity(si1.X))
            {
                pointOfIntersection.X = tgt[0].X;
                pointOfIntersection.Y = (si1.X * pointOfIntersection.X) + si1.Y;
            }
            return pointOfIntersection;
        }
        public static Vector2? getIntersectionPointRaw(Vector2[] src, Vector2[] tgt)
        {
            Vector2 si1 = convertLineToSI(src);
            Vector2 si2 = convertLineToSI(tgt);
            Vector2 pointOfIntersection = new Vector2();

            if (!float.IsInfinity(si1.X) && !float.IsInfinity(si2.X))
            {
                pointOfIntersection.X = (si2.Y - si1.Y) / (si1.X - si2.X);
                pointOfIntersection.Y = (si1.X * pointOfIntersection.X) + si1.Y;
            }
            else if (!float.IsInfinity(si2.X))
            {
                pointOfIntersection.X = src[0].X;
                pointOfIntersection.Y = (si2.X * pointOfIntersection.X) + si2.Y;
            }
            else if (!float.IsInfinity(si1.X))
            {
                pointOfIntersection.X = tgt[0].X;
                pointOfIntersection.Y = (si1.X * pointOfIntersection.X) + si1.Y;
            }
            return pointOfIntersection;
        }
        public static bool Coterminal(Vector2 [] src, Vector2[] tgt)
        {
            Vector2 si1 = convertLineToSI(src);
            Vector2 si2 = convertLineToSI(tgt);
            if(si1.Equals(si2))
            {
                return true;
            }
            return false;
        }

        public static Vector2? CoterminalFirst(Vector2[] Nonhit, Vector2[] Hit)
        {
            if (!Coterminal(Nonhit, Hit) || Nonhit[0] == Nonhit[1] || Hit[0] == Hit[1])
                return null;

            if(Nonhit[0].X < Nonhit[1].X)
            {
                if(Hit[0].X < Hit[1].X)
                {
                    return Hit[0];
                }
                else
                {
                    return Hit[1];
                }
            }
            else if(Nonhit[0].X > Nonhit[1].X)
            {
                if (Hit[0].X > Hit[1].X)
                {
                    return Hit[0];
                }
                else
                {
                    return Hit[1];
                }
            }
            else
            {
                if (Nonhit[0].Y < Nonhit[1].Y)
                {
                    if (Hit[0].Y < Hit[1].Y)
                    {
                        return Hit[0];
                    }
                    else
                    {
                        return Hit[1];
                    }
                }
                else
                {
                    if (Hit[0].Y > Hit[1].Y)
                    {
                        return Hit[0];
                    }
                    else
                    {
                        return Hit[1];
                    }
                }
            }
        }

        public static bool HalfCoterminal(Vector2 [] src, Vector2[] tgt)
        {
            Vector2 si1 = convertLineToSI(src);
            Vector2 si2 = convertLineToSI(tgt);
            return (    src[0].Y == si2.Y + si2.X * src[0].X ||
                        src[1].Y == si2.Y + si2.X * src[1].X ||
                        tgt[0].Y == si1.Y + si1.X * tgt[0].X ||
                        tgt[1].Y == si1.Y + si1.X * tgt[1].X);
        }

        public static Vector2[] getReflectionLine(Vector2[] src, Vector2[] tgt)
        {
            if (LinesIntersect_Precise(src, tgt) == 0)
            {
                return null;
            }
            Vector2 si1 = convertLineToSI(src);
            Vector2 si2 = convertLineToSI(tgt);

            Vector2 pointOfIntersection = new Vector2();

            if (!float.IsInfinity(si1.X) && !float.IsInfinity(si2.X))
            {
                pointOfIntersection.X = (si2.Y - si1.Y) / (si1.X - si2.X);
                pointOfIntersection.Y = (si1.X * pointOfIntersection.X) + si1.Y;
            }
            else if (!float.IsInfinity(si2.X))
            {
                pointOfIntersection.X = src[0].X;
                pointOfIntersection.Y = (si2.X * pointOfIntersection.X) + si2.Y;
            }
            else if (!float.IsInfinity(si1.X))
            {
                pointOfIntersection.X = tgt[0].X;
                pointOfIntersection.Y = (si1.X * pointOfIntersection.X) + si1.Y;
            }

            float Angle1 = (float)Math.Atan2(src[1].Y - src[0].Y, src[1].X - src[0].X);
            float Angle2 = (float)Math.Atan2(tgt[1].Y - tgt[0].Y, tgt[1].X - tgt[0].X);

            float newAngle = Angle2 + (Angle2 - Angle1);

            float distanceRemaining = LocationManager.getDistance(src[0], src[1]) - LocationManager.getDistance(src[0], pointOfIntersection);

            Vector2[] returnLine = new Vector2[] { pointOfIntersection, moveByRotation(pointOfIntersection, distanceRemaining, newAngle) };

            return returnLine;
        }

        public static Vector2 convertLineToSI(Vector2[] line)
        {
            float slope = (line[1].Y - line[0].Y) / (line[1].X - line[0].X);
            float intersect = line[0].Y + (-line[0].X * slope);

            return new Vector2(slope, intersect);
        }

        public static Rectangle RectangleAroundLine(Vector2[] line)
        {
            return new Rectangle((int)Math.Min(line[0].X, line[1].X), 
                                 (int)Math.Min(line[0].Y, line[1].Y), 
                                 (int)Math.Ceiling(Math.Abs(line[0].X - line[1].X)), 
                                 (int)Math.Ceiling(Math.Abs(line[0].Y - line[1].Y)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1">Source Point</param>
        /// <param name="p2">Rotating Point</param>
        /// <param name="theta">Angle</param>
        /// <returns></returns>
        public static Vector2 TransformAround(Vector2 p1, Vector2 p2, float theta)
        {
            float rot = LocationManager.getRotation(p1, p2) + theta;
            float dist = LocationManager.getDistance(p1, p2);
            return new Vector2(p1.X + (dist * (float)Math.Cos(rot)), p1.Y + (dist * (float)Math.Sin(rot)));
        }
        public static float NormalizeAngle(float angle)
        {
            while (angle < 0) angle += MathHelper.TwoPi;
            return angle % MathHelper.TwoPi;
        }

        public static float RelativeAngle(float source, float target)
        {
            float diff = target - source;
            diff = NormalizeAngle(diff);
            if (diff > MathHelper.Pi)
            {
                diff -= MathHelper.TwoPi;
            }
            return diff;
        }
        public static float NormalRelitiveAngle(float source, float target)
        {
            return NormalizeAngle(target - source);
        }
        public static float NormalRelitiveAngle_NormalInput(float source, float target)
        {
            return NormalRelitiveAngle(NormalizeAngle(source), NormalizeAngle(target));
        }
        public static Rectangle getBounds(Vector2 []A)
        {
            return getBounds(A[0], A[1]);
        }
        public static Rectangle getBounds(Vector2 A, Vector2 B)
        {
            int x = (int)Math.Min(A.X, B.X);
            int y = (int)Math.Min(A.Y, B.Y);
            int wid = (int)Math.Abs(A.X - B.X);
            int hei = (int)Math.Abs(A.Y - B.Y);
            return new Rectangle(x, y, wid, hei);
        }

        /// <summary>
        /// Checks if a line intersects a circle and returns all intersection positions
        /// </summary>
        /// <param name="line">The line to check</param>
        /// <param name="CirclePosition">The position of the circle</param>
        /// <param name="CircleRadius">The radius of the circle</param>
        /// <returns>A list of intersection points</returns>
        public static List<Vector2> CircleIntersects_V(Vector2[] line, Vector2 CirclePosition, float CircleRadius)
        {
            List<Vector2> outp = new List<Vector2>();
            Vector2 d = Vector2.Subtract(line[1], line[0]);
            Vector2 f = Vector2.Subtract(line[0], CirclePosition);
            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - CircleRadius * CircleRadius;

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
            }
            else
            {
                discriminant = (float)Math.Sqrt(discriminant);

                float t1 = (-b - discriminant) / (2 * a);
                float t2 = (-b + discriminant) / (2 * a);

                if (t1 >= 0 && t1 <= 1)
                {
                    outp.Add(LocationManager.moveByRotation(line[0], t1 * LocationManager.getDistance(line[0], line[1]), LocationManager.getRotation(line[0], line[1])));
                    if (t2 >= 0 && t2 <= 1)
                        outp.Add(LocationManager.moveByRotation(line[0], t2 * LocationManager.getDistance(line[0], line[1]), LocationManager.getRotation(line[0], line[1])));
                }

                if (t2 >= 0 && t2 <= 1)
                {
                    outp.Add(LocationManager.moveByRotation(line[0], t2 * LocationManager.getDistance(line[0], line[1]), LocationManager.getRotation(line[0], line[1])));
                }
            }
            return outp;
        }
        /// <summary>
        /// Checks if a line intersects a circle and returns the first intersection point
        /// </summary>
        /// <param name="line">The line to check</param>
        /// <param name="CirclePosition">The position of the circle</param>
        /// <param name="CircleRadius">The radius of the circle</param>
        /// <returns>A list of intersection points</returns>
        public static Vector2? CircleIntersects_CV(Vector2[] line, Vector2 CirclePosition, float CircleRadius)
        {
            Vector2 d = Vector2.Subtract(line[1], line[0]);
            Vector2 f = Vector2.Subtract(line[0], CirclePosition);
            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - CircleRadius * CircleRadius;

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
            }
            else
            {
                discriminant = (float)Math.Sqrt(discriminant);

                float t1 = (-b - discriminant) / (2 * a);
                float t2 = (-b + discriminant) / (2 * a);

                if (t1 >= 0 && t1 <= 1)
                {
                    return (LocationManager.moveByRotation(line[0], t1 * LocationManager.getDistance(line[0], line[1]), LocationManager.getRotation(line[0], line[1])));
                }

                if (t2 >= 0 && t2 <= 1)
                {
                    return (LocationManager.moveByRotation(line[0], t2 * LocationManager.getDistance(line[0], line[1]), LocationManager.getRotation(line[0], line[1])));
                }
            }
            return null;
        }

        public static List<float> CircleIntersects_A(Vector2[] line, Vector2 CirclePosition, float CircleRadius)
        {
            List<float> outp = new List<float>();
            Vector2 d = Vector2.Subtract(line[1], line[0]);
            Vector2 f = Vector2.Subtract(line[0], CirclePosition);
            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - CircleRadius * CircleRadius;

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                // no intersection
            }
            else
            {
                // ray didn't totally miss sphere,
                // so there is a solution to
                // the equation.

                discriminant = (float)Math.Sqrt(discriminant);

                // either solution may be on or off the ray so need to test both
                // t1 is always the smaller value, because BOTH discriminant and
                // a are nonnegative.
                float t1 = (-b - discriminant) / (2 * a);
                float t2 = (-b + discriminant) / (2 * a);

                // 3x HIT cases:
                //          -o->             --|-->  |            |  --|->
                // Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

                // 3x MISS cases:
                //       ->  o                     o ->              | -> |
                // FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

                if (t1 >= 0 && t1 <= 1)
                {
                    // t1 is the intersection, and it's closer than t2
                    // (since t1 uses -b - discriminant)
                    // Impale, Poke
                    outp.Add(getRotation(CirclePosition, LocationManager.moveByRotation(line[0], t1 * LocationManager.getDistance(line[0], line[1]), LocationManager.getRotation(line[0], line[1]))));
                    if (t2 >= 0 && t2 <= 1)
                        outp.Add(getRotation(CirclePosition, LocationManager.moveByRotation(line[0], t2 * LocationManager.getDistance(line[0], line[1]), LocationManager.getRotation(line[0], line[1]))));
                    //return true;
                }

                // here t1 didn't intersect so we are either started
                // inside the sphere or completely past it
                if (t2 >= 0 && t2 <= 1)
                {
                    outp.Add(getRotation(CirclePosition, LocationManager.moveByRotation(line[0], t2 * LocationManager.getDistance(line[0], line[1]), LocationManager.getRotation(line[0], line[1]))));
                }

                // no intn: FallShort, Past, CompletelyInside
            }
            return outp;
        }
        public static List<float> CircleIntersects_P(Vector2[] line, Vector2 CirclePosition, float CircleRadius)
        {
            List<float> outp = new List<float>();
            Vector2 d = Vector2.Subtract(line[1], line[0]);
            Vector2 f = Vector2.Subtract(line[0], CirclePosition);
            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - CircleRadius * CircleRadius;

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                // no intersection
            }
            else
            {
                // ray didn't totally miss sphere,
                // so there is a solution to
                // the equation.

                discriminant = (float)Math.Sqrt(discriminant);

                // either solution may be on or off the ray so need to test both
                // t1 is always the smaller value, because BOTH discriminant and
                // a are nonnegative.
                float t1 = (-b - discriminant) / (2 * a);
                float t2 = (-b + discriminant) / (2 * a);

                // 3x HIT cases:
                //          -o->             --|-->  |            |  --|->
                // Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

                // 3x MISS cases:
                //       ->  o                     o ->              | -> |
                // FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

                if (t1 >= 0 && t1 <= 1)
                {
                    // t1 is the intersection, and it's closer than t2
                    // (since t1 uses -b - discriminant)
                    // Impale, Poke
                    outp.Add(t1);
                    if (t2 >= 0 && t2 <= 1)
                        outp.Add(t2);
                    //return true;
                }

                // here t1 didn't intersect so we are either started
                // inside the sphere or completely past it
                if (t2 >= 0 && t2 <= 1)
                {
                    outp.Add(t2);
                }

                // no intn: FallShort, Past, CompletelyInside
            }
            return outp;
        }
        public static bool CircleIntersects_B(Vector2[] line, Vector2 CirclePosition, float CircleRadius)
        {
            Vector2 d = Vector2.Subtract(line[1], line[0]);
            Vector2 f = Vector2.Subtract(line[0], CirclePosition);
            float a = Vector2.Dot(d, d);
            float b = 2 * Vector2.Dot(f, d);
            float c = Vector2.Dot(f, f) - CircleRadius * CircleRadius;

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                // no intersection
            }
            else
            {
                // ray didn't totally miss sphere,
                // so there is a solution to
                // the equation.

                discriminant = (float)Math.Sqrt(discriminant);

                // either solution may be on or off the ray so need to test both
                // t1 is always the smaller value, because BOTH discriminant and
                // a are nonnegative.
                float t1 = (-b - discriminant) / (2 * a);
                float t2 = (-b + discriminant) / (2 * a);

                // 3x HIT cases:
                //          -o->             --|-->  |            |  --|->
                // Impale(t1 hit,t2 hit), Poke(t1 hit,t2>1), ExitWound(t1<0, t2 hit), 

                // 3x MISS cases:
                //       ->  o                     o ->              | -> |
                // FallShort (t1>1,t2>1), Past (t1<0,t2<0), CompletelyInside(t1<0, t2>1)

                if (t1 >= 0 && t1 <= 1)
                {
                    return true;
                }

                // here t1 didn't intersect so we are either started
                // inside the sphere or completely past it
                if (t2 >= 0 && t2 <= 1)
                {
                    return true;
                }

                // no intn: FallShort, Past, CompletelyInside
            }
            return false;
        }
        public static List<Point> AproxLineThroughGrid(Vector2[] line, int gridSize)
        {
            List<Point> output = new List<Point>();
            float gridsize = gridSize;

            float stepX = line[1].X - line[0].X;
            float stepY = line[1].Y - line[0].Y;
            bool minX = Math.Abs(stepX) < Math.Abs(stepY);

            if (!minX)
            {
                stepY = stepY / stepX * gridsize;
                stepX = gridsize;
            }
            else
            {
                stepX = stepX / stepY * gridsize;
                stepY = gridsize;
            }
            if (Math.Sign(line[1].X - line[0].X) != Math.Sign(stepX))
                stepX *= -1;
            if (Math.Sign(line[1].Y - line[0].Y) != Math.Sign(stepY))
                stepY *= -1;


            Vector2 current = line[0];
            float gs = 0;
            while (!minX ? (gs < line[1].X - line[0].X && stepX > 0) || (gs > line[1].X - line[0].X && stepX < 0) : (gs < line[1].Y - line[0].Y && stepY > 0) || (gs > line[1].Y - line[0].Y && stepY < 0))
            {
                output.Add(new Point((int)(current.X / gridsize), (int)(current.Y / gridsize)));
                current.X += stepX;
                current.Y += stepY;
                gs += !minX ? stepX : stepY;
            }
            return output;
        }
    }
}