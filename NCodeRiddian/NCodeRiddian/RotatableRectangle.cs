using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public class RotatableRectangle
    {
        public float Width, Height;
        public float X, Y, Rotation;

        public Vector2 Location
        {
            get
            {
                return new Vector2(X,Y);
            }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public RotatableRectangle(float X, float Y, float Width, float Height, float Rotation)
        {
            this.Width = Width;
            this.Height = Height;
            this.X = X;
            this.Y = Y;
            this.Rotation = Rotation;
        }

        public void RectifyRotation()
        {
            while (Rotation < 0)
                Rotation += (float)Math.PI * 2;
            Rotation %= (float)Math.PI * 2;
        }

        public Vector2[] GetCorners()
        {
            Vector2[] coords = { new Vector2(- (Width/2), - (Height/2)),
                                 new Vector2(- (Width/2), (Height/2)),
                                new Vector2((Width/2),  (Height/2)),
                                new Vector2((Width/2),  -(Height/2))};
            for (int i = 0; i < 4; i++)
            {
                coords[i] = LocationManager.TransformAround(Vector2.Zero, coords[i], Rotation);
                coords[i].X += X;
                coords[i].Y += Y;
            }

            return coords;
        }

        public bool GetColision(RotatableRectangle r)
        {
            Vector2[] myCoords = GetCorners();
            Vector2[] otherCoords = r.GetCorners();
            if (GetColision_Rough(myCoords, otherCoords))
                return GetColision_Accurate(myCoords, otherCoords);
            return false;
        }

        public bool GetColision_Rough(RotatableRectangle r)
        {
            Vector2[] myCoords = GetCorners();
            Vector2[] otherCoords = r.GetCorners();
            return GetColision_Rough(myCoords, otherCoords);
        }

        public Rectangle GetNoRotation()
        {
            return new Rectangle((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Width), (int)Math.Round(Height));
        }

        public Rectangle GetBounds()
        {
            Vector2[] myCoords = GetCorners();
            Point low = new Point(int.MaxValue, int.MaxValue), high = new Point(int.MinValue, int.MinValue);
            for (int i = 0; i < 4; i++)
            {
                if (myCoords[i].X < low.X)
                    low.X = (int)myCoords[i].X;
                if (myCoords[i].X > high.X)
                    high.X = (int)myCoords[i].X;
                if (myCoords[i].Y < low.Y)
                    low.Y = (int)myCoords[i].Y;
                if (myCoords[i].Y > high.Y)
                    high.Y = (int)myCoords[i].Y;
            }
            return new Rectangle(low.X, low.Y, high.X - low.X, high.Y - low.Y);
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }

        private bool GetColision_Rough(Vector2[] myCoords, Vector2[] otherCoords)
        {
            Point low = new Point(int.MaxValue, int.MaxValue), high = new Point(int.MinValue, int.MinValue);
            for (int i = 0; i < 4; i++)
            {
                if (myCoords[i].X < low.X)
                    low.X = (int)myCoords[i].X;
                if (myCoords[i].X > high.X)
                    high.X = (int)myCoords[i].X;
                if (myCoords[i].Y < low.Y)
                    low.Y = (int)myCoords[i].Y;
                if (myCoords[i].Y > high.Y)
                    high.Y = (int)myCoords[i].Y;
            }
            Rectangle myRect = new Rectangle(low.X, low.Y, high.X - low.X, high.Y - low.Y);
            low = new Point(int.MaxValue, int.MaxValue);
            high = new Point(int.MinValue, int.MinValue);
            for (int i = 0; i < 4; i++)
            {
                if (otherCoords[i].X < low.X)
                    low.X = (int)otherCoords[i].X;
                if (otherCoords[i].X > high.X)
                    high.X = (int)otherCoords[i].X;
                if (otherCoords[i].Y < low.Y)
                    low.Y = (int)otherCoords[i].Y;
                if (otherCoords[i].Y > high.Y)
                    high.Y = (int)otherCoords[i].Y;
            }
            Rectangle other = new Rectangle(low.X, low.Y, high.X - low.X, high.Y - low.Y);
            return myRect.Intersects(other);
        }

        private bool GetColision_Accurate(Vector2[] myCoords, Vector2[] otherCoords)
        {
            Vector2 limit = new Vector2(-10000000, -10000000);
            for (int i = 0; i < 4; i++)
            {
                int ccount1 = 0;
                int ccount2 = 0;
                for (int i2 = 0; i2 < 4; i2++)
                {
                    if (LocationManager.linesIntersect(new Vector2[] { myCoords[i], limit }, new Vector2[] { otherCoords[i2], otherCoords[(i2 + 1) % 4] }))
                        ccount1++;
                    if (LocationManager.linesIntersect(new Vector2[] { otherCoords[i], limit }, new Vector2[] { myCoords[i2], myCoords[(i2 + 1) % 4] }))
                        ccount2++;
                    if (LocationManager.linesIntersect(new Vector2[] { myCoords[i], myCoords[(i + 1) % 4] }, new Vector2[] { otherCoords[i2], otherCoords[(i2 + 1) % 4] }))
                        return true;
                }
                if (ccount1 == 1 || ccount2 == 1)
                    return true;
            }
            return false;
        }
        
        public bool GetColision_Accurate(RotatableRectangle r)
        {
            Vector2[] myCoords = GetCorners();
            Vector2[] otherCoords = r.GetCorners();
            return GetColision_Accurate(myCoords, otherCoords);
        }

        public Polygon ConvertToPoly()
        {
            Vector2[] coords = { new Vector2(- (Width/2), - (Height/2)),
                                 new Vector2(- (Width/2), (Height/2)),
                                new Vector2((Width/2),  (Height/2)),
                                new Vector2((Width/2),  -(Height/2))};
            return new Polygon(coords, new Vector2(X, Y), Rotation);
        }
    }
}
