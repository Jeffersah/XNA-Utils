using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public abstract class Mapper
    {
        public enum RoundingMode
        {
            Down,
            Up,
            Nearest
        }

        public static void seeTiles(Seeable[,] world, Vector2 location, float angle, float FOV, float distance, RoundingMode rmode, params object[] parameters)
        {
            int topquad = (int)((angle + FOV) / MathHelper.PiOver4);
            int botquad = (int)((angle - FOV) / MathHelper.PiOver4);

            if (topquad == botquad)
            {
                seeTilesQuad(world, location, new Vector2(angle + FOV, angle - FOV), distance, topquad, rmode, parameters);
            }
            else
            {
                seeTilesQuad(world, location, new Vector2(angle + FOV, MathHelper.PiOver4 * Math.Max(topquad, botquad)), distance, topquad, rmode, parameters);
                seeTilesQuad(world, location, new Vector2(MathHelper.PiOver4 * Math.Max(topquad, botquad), angle - FOV), distance, botquad, rmode, parameters);
            }
        }

        private static void seeTilesQuad(Seeable[,] world, Vector2 location, Vector2 angles, float distance, int quad, RoundingMode rmode, params object[] parameters)
        {
            Queue<SightCol> queue = new Queue<SightCol>();
            int tilesize = world[0, 0].getBounds().Width;

            int curDist = 0;

            enqueNext(world, location, tilesize, queue, curDist, angles, quad, rmode);

            while (queue.Count != 0) //Process Row
            {
                curDist++;
                SightCol s = queue.Dequeue();

                Vector2 newAngles = new Vector2(0, 0);
                bool curblock = false;
                bool topused = false;

                bool firstblock = true;

                while (!s.isDone())
                {
                    Seeable seb = s.getNextSeeable();
                    if (LocationManager.distanceCheck(location, new Vector2(seb.getBounds().X, seb.getBounds().Y), distance))
                    {
                        seb.see(parameters);
                        if (firstblock)
                        {
                            firstblock = false;
                            topused = true;
                            newAngles.X = angles.X;
                        }

                        if (seb.isTransparent() && curblock)
                        {
                            topused = true;
                            curblock = false;
                            newAngles.X = getAngleToNessecaryCorner(location, seb.getBounds(), quad, true);
                        }
                        else if (!seb.isTransparent())
                        {
                            if (topused && !curblock)
                            {
                                newAngles.Y = getAngleToNessecaryCorner(location, seb.getBounds(), quad, false);
                                enqueNext(world, location, tilesize, queue, s.cDist + 1, newAngles, quad, rmode);
                            }
                            else if (!curblock)
                            {
                                curblock = true;
                            }
                        }
                    }
                }
                if (curblock)
                {
                    newAngles.Y = s.angles.Y;
                    enqueNext(world, location, tilesize, queue, s.cDist + 1, newAngles, quad, rmode);
                }
            }
        }

        private static float getAngleToNessecaryCorner(Vector2 location, Rectangle r, int quad, bool istop)
        {
            switch (quad)
            {
                case 0:
                    return LocationManager.getRotation(location, istop ? new Vector2(r.X, r.Y + r.Height) : new Vector2(r.X, r.Y));
                case 1:
                    return LocationManager.getRotation(location, istop ? new Vector2(r.X, r.Y + r.Height) : new Vector2(r.X + r.Width, r.Y + r.Height));
                case 2:
                    return LocationManager.getRotation(location, istop ? new Vector2(r.X + r.Width, r.Y + r.Height) : new Vector2(r.X, r.Y + r.Height));
                case 3:
                    return LocationManager.getRotation(location, istop ? new Vector2(r.X + r.Width, r.Y + r.Height) : new Vector2(r.X, r.Y + r.Height));
                case 4:
                    return LocationManager.getRotation(location, istop ? new Vector2(r.X + r.Width, r.Y) : new Vector2(r.X + r.Width, r.Y + r.Height));
                case 5:
                    return LocationManager.getRotation(location, istop ? new Vector2(r.X, r.Y + r.Height) : new Vector2(r.X + r.Width, r.Y));
                case 6:
                    return LocationManager.getRotation(location, istop ? new Vector2(r.X + r.Width, r.Y + r.Height) : new Vector2(r.X, r.Y));
                case 7:
                    return LocationManager.getRotation(location, istop ? new Vector2(r.X, r.Y) : new Vector2(r.X, r.Y + r.Height));
                default:
                    return float.NaN;
            }
        }

        private static void enqueNext(Seeable[,] world, Vector2 location, int tilesize, Queue<SightCol> scols, int curDist, Vector2 angles, int quad, RoundingMode rmode)
        {
            Point sdirs = new Point(getangle(angles.X, quad, curDist, rmode, tilesize), getangle(angles.Y, quad, curDist, rmode, tilesize));
            Point p1dir = new Point((quad == 0 || quad == 7 || quad == 3 || quad == 4) ? (int)((location.X / tilesize) + curDist) : sdirs.X, !(quad == 0 || quad == 7 || quad == 3 || quad == 4) ? (int)((location.Y / tilesize) + curDist) : sdirs.X);
            Point p2dir = new Point((quad == 0 || quad == 7 || quad == 3 || quad == 4) ? (int)((location.X / tilesize) + curDist) : sdirs.Y, !(quad == 0 || quad == 7 || quad == 3 || quad == 4) ? (int)((location.Y / tilesize) + curDist) : sdirs.Y);
            scols.Enqueue(new SightCol(world, p1dir, p2dir, curDist, angles));
        }

        private static int getangle(float angle, int quad, int dist, RoundingMode rmode, int tsize)
        {
            int tgtPoint = tsize * dist;
            double unrounded = 0;
            if (quad == 0 || quad == 7 || quad == 3 || quad == 4)
            {
                unrounded = (double)tgtPoint * Math.Tan(angle);
            }
            else
            {
                unrounded = (double)tgtPoint / Math.Tan(angle);
            }

            switch (rmode)
            {
                case RoundingMode.Up:
                    return (int)Math.Ceiling(unrounded / (double)tsize);
                case RoundingMode.Down:
                    return (int)Math.Floor(unrounded / (double)tsize);
                case RoundingMode.Nearest:
                default:
                    return (int)Math.Round(unrounded / (double)tsize);
            }
        }
    }

    internal struct SightCol
    {
        private Seeable[] seeables;
        public Vector2 angles;
        public int cDist;
        private int cidx;

        public SightCol(Seeable[,] world, Point idx1, Point idx2, int cdist, Vector2 angles)
        {
            seeables = new Seeable[Math.Abs((idx1.X - idx2.X) + (idx1.Y - idx2.Y))];
            for (int x = 0; x < seeables.Length; x++)
            {
                seeables[x] = world[idx1.X, idx1.Y];
                if (idx1.X < idx2.X)
                    idx1.X++;
                else if (idx1.X > idx2.X)
                    idx1.X--;
                else if (idx1.Y > idx2.Y)
                    idx1.Y--;
                else
                    idx1.Y++;
            }
            this.cDist = cdist;
            this.cidx = 0;

            this.angles = angles;
        }

        public Seeable getNextSeeable()
        {
            cidx++;
            return seeables[cidx - 1];
        }

        public Seeable peekNextSeeable()
        {
            return seeables[cidx];
        }

        public bool isDone()
        {
            return cidx >= seeables.Length;
        }
    }

    public interface Seeable
    {
        /// <summary>
        /// Checks if this tile blocks LOS
        /// </summary>
        /// <returns>TRUE if the object is transparent, FALSE otherwise</returns>
        bool isTransparent();

        /// <summary>
        /// Get the "true bounds" (actual position) of the object
        /// </summary>
        /// <returns>Rectangle containing the objects true bounds</returns>
        Rectangle getBounds();

        /// <summary>
        /// Function called when the seeable is seen
        /// </summary>
        /// <param name="param">A list of objects which can be sent to the function</param>
        void see(params object[] parameters);
    }
}