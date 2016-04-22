using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public class PolyColider
    {
        public Vector2 LastColisionPoint;
        public int polyLine1;
        public int polyLine2;

        Rectangle debug_bounds;
        List<Vector2[]> debug_AllLines;

        public float maxV;

        Vector2[] HITEDGE;

        Vector2[][] Cot;


        public bool VelocityColision(Polygon moving, Polygon stationary, Vector2 velocity)
        {
            Rectangle roughBounds = moving.Bounds;
            roughBounds.Width += (int)Math.Ceiling(Math.Abs(velocity.X));
            roughBounds.Height += (int)Math.Ceiling(Math.Abs(velocity.Y));
            if (velocity.X < 0)
                roughBounds.X += (int)Math.Ceiling(velocity.X);
            if (velocity.Y < 0)
                roughBounds.Y += (int)Math.Ceiling(velocity.Y);
            debug_bounds = roughBounds;
            if (!roughBounds.Intersects(stationary.Bounds))
                return false;

            float MV = Physics.ConvertToMA(velocity).X;
            float MV2 = MV * MV;

            List<Vector2[]> movingLines = new List<Vector2[]>();
            List<Vector2[]> stationaryLines = new List<Vector2[]>();

            for (int i = 0; i < moving.NumberOfSides(); i++)
            {
                movingLines.Add(new Vector2[] {moving.GetCorner(i) , Vector2.Add(moving.GetCorner(i), velocity)});
            }

            for (int i = 0; i < stationary.NumberOfSides(); i++)
            {
                stationaryLines.Add(new Vector2[] { Vector2.Subtract(stationary.GetCorner(i), velocity), stationary.GetCorner(i) });
            }
            float maxMove = (velocity.X * velocity.X) + (velocity.Y * velocity.Y);
            bool colid = false;
            for(int i = 0; i < movingLines.Count; i++)
            {
                foreach(Vector2 [] edge in stationary.GetEdges())
                {
                    Vector2? Colision = LocationManager.getIntersectionPoint(movingLines[i], edge);
                    switch(LocationManager.LinesIntersect_Precise(movingLines[i], edge))
                    {
                        case 1:
                            if (Colision.HasValue)
                            {
                                float newdist = LocationManager.getDistanceSquared(movingLines[i][0], Colision.Value);
                                if (newdist < maxMove)
                                    maxMove = newdist;
                                colid = true;
                                HITEDGE = edge;
                            }
                            else
                                Console.Out.WriteLine("? ? ? ?");
                            break;
                        case 2:
                            Cot = new Vector2[2][] { movingLines[i], edge };
                            Vector2[] edgecheck = new Vector2[] { movingLines[i][0], Vector2.Add(movingLines[i][0], Vector2.Multiply(velocity, 100000)) };
                            int hitcount = 0;
                            foreach (Vector2[] ec2 in stationary.GetEdges())
                            {
                                if (LocationManager.LinesIntersect_Precise(ec2, edgecheck) == 1)
                                    hitcount++;
                            }
                            if(hitcount % 2 == 0)
                            {
                            }
                            else
                            {
                                if (Colision.HasValue)
                                {
                                    float newdist = LocationManager.getDistanceSquared(movingLines[i][0], Colision.Value);
                                    if (newdist < maxMove)
                                        maxMove = newdist;
                                    colid = true;
                                    HITEDGE = edge;
                                }
                                else
                                    Console.Out.WriteLine("? ? ? ?");
                            }
                            break;
                        default:
                        case 0:
                            break;
                    }
                }
            }

            for (int i = 0; i < stationaryLines.Count; i++)
            {
                foreach (Vector2[] edge in moving.GetEdges())
                {
                    Vector2? Colision = LocationManager.getIntersectionPoint(stationaryLines[i], edge);
                    switch (LocationManager.LinesIntersect_Precise(stationaryLines[i], edge))
                    {
                        case 1:
                            if (Colision.HasValue)
                            {
                                float newdist = MV2 - LocationManager.getDistanceSquared(stationaryLines[i][0], Colision.Value);
                                if (newdist < maxMove)
                                    maxMove = newdist;
                                colid = true;
                                HITEDGE = edge;
                            }
                            else
                                Console.Out.WriteLine("? ? ? ?");
                            break;
                        case 2:
                            Cot = new Vector2[2][] { stationaryLines[i], edge };
                            Vector2[] edgecheck = new Vector2[] { stationaryLines[i][0], Vector2.Add(stationaryLines[i][0], Vector2.Multiply(velocity, 100000)) };
                            int hitcount = 0;
                            foreach (Vector2[] ec2 in moving.GetEdges())
                            {
                                if (LocationManager.LinesIntersect_Precise(ec2, edgecheck) == 1)
                                    hitcount++;
                            }
                            if (hitcount % 2 == 0)
                            {
                            }
                            else
                            {
                                if (Colision.HasValue)
                                {
                                    float newdist = MV2 - LocationManager.getDistanceSquared(stationaryLines[i][0], Colision.Value);
                                    if (newdist < maxMove)
                                        maxMove = newdist;
                                    colid = true;
                                    HITEDGE = edge;
                                }
                                else
                                    Console.Out.WriteLine("? ? ? ?");
                            }
                            break;
                        default:
                        case 0:
                            break;
                    }
                }
            }

            if (colid)
            {
                maxV = (float)Math.Sqrt(maxMove) / MV;
                return true;
            }

            debug_AllLines = new List<Vector2[]>();
            debug_AllLines.AddRange(movingLines);
            debug_AllLines.AddRange(stationaryLines);

            return false;
        }

        public void Debug_Draw(SpriteBatch sb)
        {
            Polygon debug_bounds_p = Polygon.GetFromRectangle(debug_bounds);
            for(int i = 0; i < debug_bounds_p.NumberOfSides(); i++)
            {
                Camera.drawLineGeneric(debug_bounds_p.GetCorner(i), debug_bounds_p.GetCorner((i + 1) % 4), sb, Color.Blue);
            }
            if(debug_AllLines != null)
            { 
                foreach(Vector2[] line in debug_AllLines)
                {
                    Camera.drawLineGeneric(line[0], line[1], sb, Color.Orange);
                }
            }
            if (HITEDGE != null)
                Camera.drawLineGeneric(HITEDGE[0], HITEDGE[1], sb, Color.Red);

            if(Cot != null)
            {
                Camera.drawLineGeneric(Cot[0][0], Cot[0][1], sb, Color.Green);
                Camera.drawLineGeneric(Cot[1][0], Cot[1][1], sb, Color.Green);
            }
        }
    }
}
