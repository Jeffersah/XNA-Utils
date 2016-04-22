using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    internal class IDrawableSorter : IComparer<IDRAWABLE>
    {
        public int Compare(IDRAWABLE o1, IDRAWABLE o2)
        {
            return ((IDRAWABLE)o1).getDrawLocation().Y - ((IDRAWABLE)o2).getDrawLocation().Y;
        }
    }

    /// <summary>
    /// A simulated isometric camera
    /// </summary>
    public class IsoCamera
    {
        private static Point origCamSize;
        private static Point camSize;
        /// <summary>
        /// Camera attack angle
        /// </summary>
        public static float camAttack;
        private static float topEdge;
        /// <summary>
        /// Camera position
        /// </summary>
        public static Point cameraPosition;
        /// <summary>
        /// Camera zoom level
        /// </summary>
        public static float zoom;
        /// <summary>
        /// Camera Skew
        /// </summary>
        public static int Skew;

        private static List<IDRAWABLE> ThingsToDraw = new List<IDRAWABLE>();

        /// <summary>
        /// Setup the camera
        /// </summary>
        /// <param name="cameraSize">Point representing size of camera FOV</param>
        /// <param name="cameraAngle"></param>
        public static void setupCamera(Point cameraSize, float cameraAngle)
        {
            camSize = cameraSize;
            origCamSize = camSize;
            camAttack = cameraAngle;
            topEdge = camSize.X / camAttack;
            zoom = 1;
        }

        /// <summary>
        /// Add an item to the render queue, so that it will be drawn on screen
        /// </summary>
        /// <param name="item">The item to add</param>
        public static void addToRenderQueue(IDRAWABLE item)
        {
            ThingsToDraw.Add(item);
        }

        /// <summary>
        /// Remove an item from the render queue, so it will no longer be drawn on screen
        /// </summary>
        /// <param name="toRemove">The item to remove</param>
        public static void removeFromRenderQueue(IDRAWABLE toRemove)
        {
            ThingsToDraw.Remove(toRemove);
        }

        /// <summary>
        /// Clears the render queue entirely, so nothing will be drawn
        /// </summary>
        public static void clearRenderQueue()
        {
            ThingsToDraw.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static int shiftDrawLocation(int x, float y)
        {//camSize.X * .75f
            int dif = (int)(x - ((cameraPosition.X + Skew) + (topEdge / 2f)));
            //Console.Out.WriteLine("Adj:" + ((dif * y) / camAttack));
            return (int)Math.Round(x + (((dif * y) / camAttack)) * (1 - camAttack));
        }

        /// <summary>
        /// Draws everything in the render queue
        /// </summary>
        /// <param name="sb"></param>
        public static void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.BackToFront, null);

            foreach (IDRAWABLE i in ThingsToDraw)
            {
                if (i.getDrawLocation().Y - (i.getDrawLocation().Height / 2) < cameraPosition.Y + camSize.Y)
                {
                    sb.Draw(i.getTexture(), new Rectangle(shiftDrawLocation(i.getDrawLocation().X, getHeightMod(i.getDrawLocation().Y)) - cameraPosition.X,
                                                          i.getDrawLocation().Y - cameraPosition.Y,
                                                          (int)Math.Ceiling((float)i.getDrawLocation().Width * getTotalZoom(i.getDrawLocation().Y)),
                                                          (int)Math.Ceiling((float)i.getDrawLocation().Height * getTotalZoom(i.getDrawLocation().Y))
                                                          ), null, i.getTintColor(), i.getRotation(), i.getDrawOrigin(), i.getFlipEffect(),
                                                          1 - getHeightMod(i.getDrawLocation().Y));
                }
            }

            sb.End();
        }

        public static Rectangle getFullScreen()
        {
            return new Rectangle(cameraPosition.X, cameraPosition.Y, (int)Math.Round(topEdge), camSize.Y);
        }

        public static void DrawIndiv(SpriteBatch sb, Image image, Rectangle r, Color c, Rectangle? sourceRectangle, float rotation, Vector2 origin, SpriteEffects effects)
        {
            r.X -= (int)Math.Round(origin.X);
            r.Y -= (int)Math.Round(origin.Y);
            r.X = shiftDrawLocation(r.X, getHeightMod(r.Y + origin.Y));
            if (getFullScreen().Intersects(r))
            {
                r.X += (int)Math.Round(origin.X);
                r.Y += (int)Math.Round(origin.Y);
                sb.Draw(image.getTexture(),
                        new Rectangle(
                            (int)shiftDrawLocation(r.X, getHeightMod(r.Y)) - cameraPosition.X,
                            (int)r.Y - cameraPosition.Y,
                            (int)Math.Ceiling(r.Width * getTotalZoom(r.Y)),
                            (int)Math.Ceiling(r.Height * getTotalZoom(r.Y))),
                            sourceRectangle == null && image is AnimatedImage ? ((AnimatedImage)image).getFrame() : sourceRectangle,
                            c, rotation, origin, effects, 1 - getHeightMod(r.Y));
            }
        }

        private static float getHeightMod(float y)
        {
            return (y - (float)cameraPosition.Y) / (float)camSize.Y;
        }

        private static float getRawZoom(float y)
        {
            return (getHeightMod(y) / camAttack);
        }

        private static float getTotalZoom(float y)
        {
            return (getRawZoom(y) + camAttack) - (getRawZoom(y) * camAttack);
        }
    }

    public interface IDRAWABLE
    {
        Texture2D getTexture();

        Rectangle getDrawLocation();

        Vector2 getDrawOrigin();

        float getRotation();

        Color getTintColor();

        SpriteEffects getFlipEffect();
    }
}