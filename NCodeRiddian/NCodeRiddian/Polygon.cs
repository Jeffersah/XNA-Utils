using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    /// <summary>
    /// Generic polygon
    /// </summary>
    public class Polygon
    {
        private static float ROOT2 = (float)Math.Sqrt(2);

        /// <summary>
        /// Defines the vertecies of a 2x2 square centered around the origin
        /// </summary>
        /// <returns></returns>
        public static Vector2[] SQUARE() {return new Vector2[] { new Vector2(-1, -1), new Vector2(1, -1), new Vector2(1, 1), new Vector2(-1, 1) };}
        /// <summary>
        /// Defines the vertecies of a unit square centered around the origin
        /// </summary>
        /// <returns></returns>
        public static Vector2[] UNIT_SQUARE() { return new Vector2[] { new Vector2(-.5f, -.5f), new Vector2(.5f, -.5f), new Vector2(.5f, .5f), new Vector2(-.5f, .5f) };}
        /// <summary>
        /// Defines the vertecies of a right triangle centered around the origin
        /// </summary>
        /// <returns></returns>
        public static Vector2[] RIGHT_TRIANGLE() { return new Vector2[] { new Vector2(-1, -1), new Vector2(1, 1), new Vector2(-1, 1) }; }
        /// <summary>
        /// Defines the vertecies of a regular octagon centered around the origin
        /// </summary>
        /// <returns></returns>
        public static Vector2[] OCTAGON() { return new Vector2[] { new Vector2(1 + ROOT2, 1), new Vector2(1, 1 + ROOT2), new Vector2(-1, 1 + ROOT2), new Vector2(-(1 + ROOT2), 1), new Vector2(-(1 + ROOT2), -1), new Vector2(-1, -(1 + ROOT2)), new Vector2(1, -(1 + ROOT2)), new Vector2(1 + ROOT2, -1) }; }

        /// <summary>
        /// Converts a rectangle into an equivalent polygon
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Polygon GetFromRectangle(Rectangle r)
        {
            Polygon tmp = new Polygon(UNIT_SQUARE(), LocationManager.getVectorFromPoint(r.Center), 0);
            tmp.Scale(r.Width, r.Height);
            return tmp;
        }

        /// <summary>
        /// Converts a line into a polygonal approximation, defined as an infinitely thin rectangle
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Polygon GetFromLine(Vector2 A, Vector2 B)
        {
            float ang = LocationManager.getRotation(A, B);
            float dist = LocationManager.getDistance(A, B);
            Polygon tmp = new Polygon(UNIT_SQUARE(), LocationManager.moveByRotation(A, dist / 2, ang),ang );
            tmp.Scale(dist, 0);
            return tmp;
        }

        Vector2[] corners;

        private Vector2 position;
        private Rectangle simrect;
        private float area;
        private bool AreaGood;

        /// <summary>
        /// Gets the area of the polygon
        /// </summary>
        public float Area
        {
            get
            {
                if (!AreaGood)
                    CalculateArea();
                return area;
            }
        }

        /// <summary>
        /// Depricated. Use Bounds instead.
        /// </summary>
        public Rectangle SimpleRectangle
        {
            get
            {
                return simrect;
            }
        }

        /// <summary>
        /// Returns the position of this polygon
        /// </summary>
        public Vector2 Position
        {
            get{
                return position;
            }
            set{
                if (value.X != position.X || value.Y != position.Y)
                {
                    TransformValid = false;
                }
                position = value;
            }
        }
        /// <summary>
        /// Gets or sets the X value of this polygons position
        /// </summary>
        public float X
        {
            get
            {
                return position.X;
            }
            set
            {
                if (value != position.X)
                    TransformValid = false;
                position.X = value;
            }
        }
        /// <summary>
        /// Gets or sets the Y value of this polygons position
        /// </summary>
        public float Y
        {
            get
            {
                return position.Y;
            }
            set
            {
                if (value != position.Y)
                    TransformValid = false;
                position.Y = value;
            }
        }

        private float rotation;
        /// <summary>
        /// Gets or sets polygonal rotation
        /// </summary>
        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                if (value != rotation)
                {
                    TransformValid = false;
                }
                rotation = value;
            }
        }

        Vector2[] RealCorners;
        bool TransformValid;
        private Rectangle bounds;
        /// <summary>
        /// Gets a rectangle that fully contains this polygon
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                if (TransformValid)
                    return bounds;
                else
                    applyTransform();
                return bounds;
            }
        }

        /// <summary>
        /// Returns the number of sides of this polygon
        /// </summary>
        /// <returns></returns>
        public int NumberOfSides()
        {
            return RealCorners.Length;
        }

        public Polygon(Vector2[] Corners, Vector2 pos, float rot)
        {
            corners = Corners;
            position = pos;
            rotation = rot;
            TransformValid = false;
            RealCorners = new Vector2[corners.Length];
            bounds = new Rectangle(0,0,0,0);
            calcSimRect();
            AreaGood = false;
        }

        public void ChangeShape(Vector2[] newCorners)
        {
            corners = newCorners;
            TransformValid = false;
            AreaGood = false;
            calcSimRect();
        }

        private void calcSimRect()
        {
            Vector2 low = new Vector2();
            Vector2 high = new Vector2();

            for (int i = 0; i < corners.Length; i++)
            {
                if (i == 0)
                {
                    low = new Vector2(corners[i].X, corners[i].Y);
                    high = new Vector2(corners[i].X, corners[i].Y);
                }
                else
                {
                    low.X = Math.Min(corners[i].X, low.X);
                    low.Y = Math.Min(corners[i].Y, low.Y);
                    high.X = Math.Max(corners[i].X, high.X);
                    high.Y = Math.Max(corners[i].Y, high.Y);
                }
            }

            simrect = new Rectangle((int)low.X, (int)low.Y, (int)(high.X - low.X), (int)(high.Y - low.Y));
        }

        private void applyTransform()
        {
            TransformValid = true;
            Vector2 low = new Vector2();
            Vector2 high = new Vector2();

            for (int i = 0; i < corners.Length; i++)
            {
                RealCorners[i] = LocationManager.TransformAround(Vector2.Zero, corners[i], rotation);
                RealCorners[i].X += position.X;
                RealCorners[i].Y += position.Y;
                if (i == 0)
                {
                    low = new Vector2(RealCorners[i].X, RealCorners[i].Y);
                    high = new Vector2(RealCorners[i].X, RealCorners[i].Y);
                }
                else
                {
                    low.X = Math.Min(RealCorners[i].X, low.X);
                    low.Y = Math.Min(RealCorners[i].Y, low.Y);
                    high.X = Math.Max(RealCorners[i].X, high.X);
                    high.Y = Math.Max(RealCorners[i].Y, high.Y);
                }
            }
            bounds = new Rectangle((int)low.X, (int)low.Y, (int)(high.X - low.X), (int)(high.Y - low.Y));
            
        }

        /// <summary>
        /// Inflates a polygon by a given amount, ignoring rotation
        /// </summary>
        /// <param name="xamt"></param>
        /// <param name="yamt"></param>
        public void Inflate(float xamt, float yamt)
        {
            for (int i = 0; i < corners.Length; i++)
            {
                corners[i].X *= xamt;
                corners[i].Y *= yamt;
            }
            calcSimRect();
            TransformValid = false;
            AreaGood = false;
        }

        /// <summary>
        /// Get the y value of the indexed corner in relation to polygon center, ignoring rotation
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float CornerX(int i)
        {
            return corners[i].X;
        }

        /// <summary>
        /// Get the y value of the indexed corner in relation to polygon center, ignoring rotation
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float CornerY(int i)
        {
            return corners[i].Y;
        }

        /// <summary>
        /// Sets the X value of a given corner in relation to polygon location, ignoring rotation
        /// </summary>
        /// <param name="i"></param>
        /// <param name="x"></param>
        public void CornerX(int i, float x)
        {
            corners[i].X = x;
            calcSimRect();
            TransformValid = false;
            AreaGood = false;
        }
        /// <summary>
        /// Sets the Y value of a given corner in relation to polygon location, ignoring rotation
        /// </summary>
        /// <param name="i"></param>
        /// <param name="y"></param>
        public void CornerY(int i, float y)
        {
            corners[i].Y = y;
            calcSimRect();
            TransformValid = false;
            AreaGood = false;
        }

        /// <summary>
        /// Return all this polygons real corners
        /// </summary>
        /// <returns></returns>
        public Vector2[] GetCorners()
        {
            if (!TransformValid)
                applyTransform();
            return RealCorners;
        }
        /// <summary>
        /// Return a specific polygonal corner
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vector2 GetCorner(int i)
        {
            if (!TransformValid)
                applyTransform();
            return RealCorners[i];
        }
        /// <summary>
        /// Return a specific polygonal edge
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Vector2[] GetEdge(int i)
        {
            if (!TransformValid)
                applyTransform();
            return new Vector2[]{RealCorners[(i)%RealCorners.Length],RealCorners[(i+1)%RealCorners.Length]} ;
        }

        /// <summary>
        /// Return all this polygons real edges
        /// </summary>
        /// <returns></returns>
        public List<Vector2[]> GetEdges()
        {
            Vector2[] Corns = GetCorners();
            List<Vector2[]> output = new List<Vector2[]>();
            for (int i = 0; i < Corns.Length; i++)
            {
                output.Add(new Vector2[] { Corns[i], Corns[(i + 1) % Corns.Length] });
            }
            return output;
        }

        /// <summary>
        /// Scales a polygon such that it exactly fits in the specified bounds, ignoring rotation
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public void Scale(float X, float Y)
        {
            float cX = simrect.Width;
            float cY = simrect.Height;
            for(int i = 0; i < corners.Length; i++)
            {
                corners[i].X *= X / cX;
                corners[i].Y *= Y / cY;
            }
            TransformValid = false;
            calcSimRect();
            AreaGood = false;
        }

        /// <summary>
        /// Checks if two polygons colide using a rough check followed by a precise check if nessecary
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool CheckColisions(Polygon p)
        {
            return CheckColisions_Rough(p) && CheckColisions_Accurate(p);
        }

        /// <summary>
        /// Check if the bounds of this polygon intersect the bounds of another.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>False if the polygons cannot be coliding, true if colision is possible</returns>
        public bool CheckColisions_Rough(Polygon p)
        {
            return Bounds.Intersects(p.Bounds);
        }

        /// <summary>
        /// Checks if two polygons are coliding (including encapsulation)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool CheckColisions_Accurate(Polygon p)
        {
            Vector2[] myCorners = GetCorners();
            Vector2[] otherCorners = p.GetCorners();
            for (int i = 0; i < myCorners.Length; i++)
            {
                for (int i2 = 0; i2 < otherCorners.Length; i2++)
                {
                    if (LocationManager.linesIntersect(myCorners[i], myCorners[(i + 1) % myCorners.Length], otherCorners[i2], otherCorners[(i2 + 1) % otherCorners.Length]))
                        return true;
                }
            }

            return !CheckContains(p) || !p.CheckContains(this);
        }

        /// <summary>
        /// Gets a point at which two polygons are coliding, or null otherwise
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Vector2? CheckColisions_Accurate_GetPoint(Polygon p)
        {
            Vector2[] myCorners = GetCorners();
            Vector2[] otherCorners = p.GetCorners();
            for (int i = 0; i < myCorners.Length; i++)
            {
                for (int i2 = 0; i2 < otherCorners.Length; i2++)
                {
                    if (LocationManager.linesIntersect(myCorners[i], myCorners[(i + 1) % myCorners.Length], otherCorners[i2], otherCorners[(i2 + 1) % otherCorners.Length]))
                        return LocationManager.getIntersectionPoint(myCorners[i], myCorners[(i + 1) % myCorners.Length], otherCorners[i2], otherCorners[(i2 + 1) % otherCorners.Length]);
                }
            }

            return null;
        }
        /// <summary>
        /// Gets an array of colision data including both polygon lines and the actual intersect point
        /// </summary>
        /// <param name="p"></param>
        /// <returns>[TgtLineStart, TgtLineEnd, MyLineStart, MyLineEnd, IntersectionPoint]</returns>
        public Vector2[] CheckColisions_Accurate_GetLine(Polygon p)
        {
            Vector2[] myCorners = GetCorners();
            Vector2[] otherCorners = p.GetCorners();
            for (int i = 0; i < myCorners.Length; i++)
            {
                for (int i2 = 0; i2 < otherCorners.Length; i2++)
                {
                    if (LocationManager.linesIntersect(myCorners[i], myCorners[(i + 1) % myCorners.Length], otherCorners[i2], otherCorners[(i2 + 1) % otherCorners.Length]))
                        return new Vector2[] { otherCorners[i2], otherCorners[(i2 + 1) % otherCorners.Length],myCorners[i], myCorners[(i + 1) % myCorners.Length], LocationManager.getIntersectionPoint(myCorners[i], myCorners[(i + 1) % myCorners.Length], otherCorners[i2], otherCorners[(i2 + 1) % otherCorners.Length]).Value};
                }
            }

            return null;
        }

        /// <summary>
        /// Check if a polygon encapulates a point
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Contains(Vector2 v)
        {
            Vector2 limit = new Vector2(this.bounds.X - 2, this.bounds.Y - 2);
            int count = 0;
            for (int i = 0; i < GetCorners().Length; i++)
            {
                if (LocationManager.linesIntersect(v, limit, GetCorners()[i], GetCorners()[(i + 1) % GetCorners().Length]))
                    count++;
            }
            return count % 2 == 1;
        }

        /// <summary>
        /// Check if a polygon fully cointains another polygon
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool CheckContains(Polygon p)
        {
            Vector2 mine = GetCorners()[0];
            Vector2[] otherCorners = p.GetCorners();
            Vector2 limit = new Vector2(Math.Min(this.Bounds.X, p.Bounds.X) - 2, Math.Min(this.Bounds.Y, p.Bounds.Y) - 2);
            int count = 0;
            for (int i = 0; i < otherCorners.Length; i++)
            {
                if (LocationManager.linesIntersect(mine, limit, otherCorners[i], otherCorners[(i + 1) % otherCorners.Length]))
                    count++;
            }
            return count % 2 == 0;
        }

        public int CountIntersections(Vector2 A, Vector2 B)
        {
            int count = 0;
            int cornerClips = 0;
            foreach(Vector2 [] edge in GetEdges())
            {
                int type = LineManager.LinesIntersect(A, B, edge[0], edge[1]).Type;
                if (type >= 2)
                {
                    count++;
                }
                else if(type == 1)
                {
                    count++;
                    cornerClips++;
                }
            }
            count -= cornerClips / 2;
            return count;
        }

        private void CalculateArea()
        {
            AreaGood = true;
            area = 0;
            for (int i = 0; i < corners.Length; i++)
            {
                Vector2 corner1 = corners[i];
                Vector2 corner2 = corners[(i + 1) % corners.Length];
                Vector2 center = Vector2.Zero;

                area += (1 / 2f) * LocationManager.getDistance(center, corner1) * LocationManager.getDistance(center, corner2) * (float)Math.Sin(LocationManager.NormalRelitiveAngle_NormalInput(LocationManager.getRotation(center, corner1), LocationManager.getRotation(center, corner2)));
            }
        }

    }
}
