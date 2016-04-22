using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public class PolygonExtensions
    {
        public static bool DoSingleColision(Polygon MovingPoly, Polygon StationaryPoly, ref Vector2 velocity, bool isMA)
        {
            if (!isMA)
            {
                velocity = Physics.ConvertToMA(velocity);
            }
            Vector2[][] MovingLines = new Vector2[MovingPoly.NumberOfSides()][];
            Vector2[][] StationaryLines = new Vector2[StationaryPoly.NumberOfSides()][];

            for (int i = 0; i < MovingLines.Length; i++)
            {
                MovingLines[i] = new Vector2[] { LocationManager.moveByRotation(MovingPoly.GetCorner(i), -velocity.X, velocity.Y), LocationManager.moveByRotation(MovingPoly.GetCorner(i), velocity.X, velocity.Y) };
               
            }
            for (int i = 0; i < StationaryLines.Length; i++)
            {
                StationaryLines[i] = new Vector2[] { /*StationaryPoly.GetCorner(i)*/LocationManager.moveByRotation(StationaryPoly.GetCorner(i), velocity.X, velocity.Y), LocationManager.moveByRotation(StationaryPoly.GetCorner(i), -velocity.X, velocity.Y) };
           
            }

            int lowindex = -1;
            int otherindex = -1;
            float lowdistance = 0;
            bool movingcloser = false;
            Vector2 intersect = new Vector2();
            for (int i = 0; i < MovingLines.Length; i++)
            {
                for (int i2 = 0; i2 < StationaryLines.Length; i2++)
                {
                    Vector2? V2 = LocationManager.getIntersectionPoint(StationaryPoly.GetEdge(i2), MovingLines[i]);
                    if (V2.HasValue)
                    {
                        if (lowindex == -1 || LocationManager.distanceCheck(V2.Value, MovingLines[i][0], lowdistance))
                        {
                            lowdistance = LocationManager.getDistance(V2.Value, MovingLines[i][0]);
                            lowindex = i;
                            otherindex = i2;
                            movingcloser = true;
                            intersect = V2.Value;
                        }
                    }
                }
            }
            for (int i = 0; i < StationaryLines.Length; i++)
            {
                for (int i2 = 0; i2 < MovingLines.Length; i2++)
                {
                    Vector2? V2 = LocationManager.getIntersectionPoint(MovingPoly.GetEdge(i2), StationaryLines[i]);
                    if (V2.HasValue)
                    {
                        //Console.Out.WriteLine("Intersection at {0}::{1} => Point at {2}", MovingPoly.GetEdge(i2)[0] + "," + MovingPoly.GetEdge(i2)[1], StationaryLines[i][0] + "," + StationaryLines[i][1], V2.Value);
                        if (lowindex == -1 || LocationManager.distanceCheck(V2.Value, StationaryLines[i][1], lowdistance))
                        {
                            lowdistance = velocity.X - LocationManager.getDistance(V2.Value, StationaryLines[i][0]);
                            lowindex = i;
                            otherindex = i2;
                            movingcloser = false;
                            intersect = V2.Value;
                        }
                    }
                }
            }

            if (lowindex != -1)
            {
                //Console.Out.WriteLine("Colis=> Lowindex: {0}, distance: {1}, whose:{2}", lowindex, lowdistance, movingcloser);
                Vector2 AddToVelocity = new Vector2();
                if (movingcloser)
                {
                    float angle = LocationManager.getRotation(intersect, LocationManager.getReflectionLine(MovingLines[lowindex], StationaryPoly.GetEdge(otherindex))[1]);
                    AddToVelocity.Y = angle;
                    AddToVelocity.X = velocity.X;
                }
                else
                {
                    float angle = LocationManager.getRotation(intersect, LocationManager.getReflectionLine(StationaryLines[lowindex], MovingPoly.GetEdge(otherindex))[1]);
                    AddToVelocity.Y = angle;
                    AddToVelocity.X = -velocity.X;
                }
                velocity = AddToVelocity;
                MovingPoly.Position = LocationManager.moveByRotation(MovingPoly.Position, lowdistance - .4f, velocity.Y);
                if (!isMA)
                {
                    velocity = Physics.ConvertToXY(velocity);
                }
                return true;
            }
            if (!isMA)
            {
                velocity = Physics.ConvertToXY(velocity);
            }
            return false;
        }

        public static bool TryTurn_Debug(Polygon myPoly, Polygon StationaryPoly, ref float amt, List<Vector2[]>lines, List<Color>linecolors)
        {
            Vector2 CircleCenter = myPoly.Position;
            Pair<float, bool, float>[] distances;
            float greatestMine = 0;
            float lowestOther = -1;
            ArrayBuilder<Pair<float, bool, float>> build = new ArrayBuilder<Pair<float, bool, float>>(myPoly.NumberOfSides() + StationaryPoly.NumberOfSides());
            for (int x = 0; x < myPoly.NumberOfSides(); x++)
            {
                float next = LocationManager.getDistance(CircleCenter, myPoly.GetCorner(x));
                build.Add(new Pair<float,bool,float>(next, true, LocationManager.getRotation(CircleCenter, myPoly.GetCorner(x))));
                if (next > greatestMine)
                    greatestMine = next;
            }
            for (int x = 0; x < StationaryPoly.NumberOfSides(); x++)
            {
                float next = LocationManager.getDistance(CircleCenter, StationaryPoly.GetCorner(x));
                build.Add(new Pair<float, bool,float>(next, false, LocationManager.getRotation(CircleCenter, StationaryPoly.GetCorner(x))));
                if (lowestOther == -1 || next < lowestOther)
                    lowestOther = next;
            }
            distances = build.Finish();
            float currentMin = amt;
            for (int i = 0; i < distances.Length; i++)
            {
                for (int i2 = 0; i2 < (distances[i].second ? StationaryPoly.NumberOfSides() : myPoly.NumberOfSides()); i2++)
                {
                    Vector2[] line = (distances[i].second ? StationaryPoly.GetEdge(i2) : myPoly.GetEdge(i2));
                    float minAngle = distances[i].third;
                    foreach (float f in LocationManager.CircleIntersects_A(line, CircleCenter, distances[i].first))
                    {
                        lines.Add(new Vector2[] { Vector2.Add(new Vector2(-2, 0), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)), Vector2.Add(new Vector2(2, 0), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)) });
                        linecolors.Add(Color.Red);
                        lines.Add(new Vector2[] { Vector2.Add(new Vector2(0, -2), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)), Vector2.Add(new Vector2(0, 2), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)) });
                        linecolors.Add(Color.Red);
                        if (amt > 0)
                        {
                            if (f > minAngle && LocationManager.NormalRelitiveAngle(minAngle, f) < amt)
                            {
                                lines.Add(new Vector2[] { Vector2.Add(new Vector2(-8, 0), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)), Vector2.Add(new Vector2(8, 0), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)) });
                                linecolors.Add(Color.Blue);
                                lines.Add(new Vector2[] { Vector2.Add(new Vector2(0, -8), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)), Vector2.Add(new Vector2(0, 8), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)) });
                                linecolors.Add(Color.Blue);
                            }
                            if (f > (minAngle) && LocationManager.NormalRelitiveAngle(minAngle, f) < currentMin)
                            {
                                currentMin = LocationManager.NormalRelitiveAngle(minAngle, f);
                            }
                        }
                        else if (amt < 0)
                        {
                            if (f < minAngle && LocationManager.NormalRelitiveAngle(f, minAngle) < -amt)
                            {
                                lines.Add(new Vector2[] { Vector2.Add(new Vector2(-8, 0), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)), Vector2.Add(new Vector2(8, 0), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)) });
                                linecolors.Add(Color.Blue);
                                lines.Add(new Vector2[] { Vector2.Add(new Vector2(0, -8), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)), Vector2.Add(new Vector2(0, 8), LocationManager.moveByRotation(CircleCenter, distances[i].first, f)) });
                                linecolors.Add(Color.Blue);
                            }
                            if (f < minAngle && LocationManager.NormalRelitiveAngle(f, minAngle) < -currentMin)
                            {
                                currentMin = LocationManager.NormalRelitiveAngle(f, minAngle);
                            }
                        }
                    }
                }
            }
            if (currentMin == amt)
            {
                myPoly.Rotation += amt;
                return false;
            }
            else
            {
                myPoly.Rotation += currentMin * .9f;
                amt *= -.9f;
                return true;
            }
        }

        public static bool DoSingleColision_Debug(Polygon MovingPoly, Polygon StationaryPoly, Velocity velocity, List<Vector2[]> lines, List<Color> linecolors)
        {
            Vector2[][] MovingLines = new Vector2[MovingPoly.NumberOfSides()][];
            Vector2[][] StationaryLines = new Vector2[StationaryPoly.NumberOfSides()][];
            bool doWork = false;

            for (int i = 0; i < MovingLines.Length; i++)
            {
                MovingLines[i] = new Vector2[] { velocity.Move(MovingPoly.GetCorner(i), -1f), velocity.Move(MovingPoly.GetCorner(i), 1f)};
                lines.Add(MovingLines[i]);
                linecolors.Add(Color.Blue);
                if (!doWork && LocationManager.getBounds(MovingLines[i]).Intersects(StationaryPoly.Bounds))
                    doWork = true;
            }
            for (int i = 0; i < StationaryLines.Length; i++)
            {
                StationaryLines[i] = new Vector2[] { velocity.Move(StationaryPoly.GetCorner(i), -1f), velocity.Move(StationaryPoly.GetCorner(i), 1f) };
                lines.Add(StationaryLines[i]);
                linecolors.Add(Color.Green);
                if (!doWork && LocationManager.getBounds(StationaryLines[i]).Intersects(MovingPoly.Bounds))
                    doWork = true;
            }
            if (!doWork)
            {
                foreach (Vector2[] v in LocationManager.RectangleEdges(MovingPoly.Bounds))
                {
                    lines.Add(v);
                    linecolors.Add(Color.Yellow);
                }
                foreach (Vector2[] v in LocationManager.RectangleEdges(StationaryPoly.Bounds))
                {
                    lines.Add(v);
                    linecolors.Add(Color.Yellow);
                }
                return false;
            }

            int lowindex = -1;
            int otherindex = -1;
            float lowdistance = 0;
            bool movingcloser = false;
            Vector2 intersect = new Vector2();
            for (int i = 0; i < MovingLines.Length; i++)
            {
                for (int i2 = 0; i2 < StationaryLines.Length; i2++)
                {
                    Vector2? V2 = LocationManager.getIntersectionPoint(StationaryPoly.GetEdge(i2), MovingLines[i]);
                    if (V2.HasValue)
                    {
                        if (lowindex == -1 || LocationManager.distanceCheck(V2.Value, MovingLines[i][0], lowdistance))
                        {
                            lowdistance = LocationManager.getDistance(V2.Value, MovingLines[i][0]);
                            lowindex = i;
                            otherindex = i2;
                            movingcloser = true;
                            intersect = V2.Value;
                        }
                    }
                }
            }
            for (int i = 0; i < StationaryLines.Length; i++)
            {
                for (int i2 = 0; i2 < MovingLines.Length; i2++)
                {
                    Vector2? V2 = LocationManager.getIntersectionPoint(MovingPoly.GetEdge(i2), StationaryLines[i]);
                    if (V2.HasValue)
                    {
                        //Console.Out.WriteLine("Intersection at {0}::{1} => Point at {2}", MovingPoly.GetEdge(i2)[0] + "," + MovingPoly.GetEdge(i2)[1], StationaryLines[i][0] + "," + StationaryLines[i][1], V2.Value);
                        if (lowindex == -1 || LocationManager.distanceCheck(V2.Value, StationaryLines[i][1], lowdistance))
                        {
                            lowdistance = velocity.M - LocationManager.getDistance(V2.Value, StationaryLines[i][0]);
                            lowindex = i;
                            otherindex = i2;
                            movingcloser = false;
                            intersect = V2.Value;
                        }
                    }
                }
            }

            if (lowindex != -1)
            {
                Velocity AddToVelocity = new Velocity();
                if (movingcloser)
                {
                    float angle = LocationManager.getRotation(intersect, LocationManager.getReflectionLine(MovingLines[lowindex], StationaryPoly.GetEdge(otherindex))[1]);
                    AddToVelocity.A = angle;
                    AddToVelocity.M = velocity.M;
                    lines.Add(new Vector2[]{MovingLines[lowindex][0], MovingLines[lowindex][1]});
                    linecolors.Add(Color.Red);
                }
                else
                {
                    float angle = LocationManager.getRotation(intersect, LocationManager.getReflectionLine(StationaryLines[lowindex], MovingPoly.GetEdge(otherindex))[1]);
                    AddToVelocity.A = angle;
                    AddToVelocity.M = velocity.M;
                    lines.Add(new Vector2[] { StationaryLines[lowindex][0], StationaryLines[lowindex][1] });
                    linecolors.Add(Color.Red);
                }

                velocity.X = AddToVelocity.X;
                velocity.Y = AddToVelocity.Y;

                MovingPoly.Position = LocationManager.moveByRotation(MovingPoly.Position, lowdistance, velocity.A);
                return true;
            }
            return false;
        }
    }
}
