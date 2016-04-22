using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    /// <summary>
    /// Giant class to perform render operations including zoom level and camera position in 2D
    /// </summary>
    public abstract class Camera
    {
        private static Rectangle cameraLocation;
        private static float zoom;
        private static Point origin;

        private static Texture2D spix;
        private static GraphicsDevice gd;

        /// <summary>
        /// Allows the camera to use the generic texture methods
        /// </summary>
        /// <param name="gd">The GraphicsDevice this game is using</param>
        public static void setupGenericTexture(GraphicsDevice gd)
        {
            Camera.gd = gd;
            spix = new Texture2D(gd, 1, 1);
            spix.SetData<Color>(new Color[] { Color.White });
        }

        /// <summary>
        /// Sets up the camera for rendering
        /// </summary>
        /// <param name="camSize">The size of the display screen in pixels</param>
        public static void setupCamera(Point camSize)
        {
            setupCamera(camSize, new Point(0, 0));
        }

        /// <summary>
        /// Sets up the camera for rendering
        /// </summary>
        /// <param name="graphics">The GraphicsDeviceManager used to determine display size</param>
        public static void setupCamera(GraphicsDeviceManager graphics)
        {
            setupCamera(new Point(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
        }

        /// <summary>
        /// Sets up the camera for rendering
        /// </summary>
        /// <param name="camSize">The size of the display screen in pixels</param>
        /// <param name="origin">The origin point for the camera</param>
        public static void setupCamera(Point camSize, Point origin)
        {
            cameraLocation = new Rectangle(0, 0, camSize.X, camSize.Y);
            zoom = 1;
            Camera.origin = origin;
        }

        /// <summary>
        /// Get Camera origin point
        /// </summary>
        /// <returns></returns>
        public static Point getOrigin()
        {
            return origin;
        }

        /// <summary>
        /// Returns a rectangle determining Camera location (without zoom)
        /// </summary>
        /// <returns></returns>
        public static Rectangle getCamera()
        {
            return cameraLocation;
        }

        /// <summary>
        /// Add X to the cameras current X position
        /// </summary>
        /// <param name="x">Value to Add</param>
        public static void addX(int x)
        {
            cameraLocation.X += x;
        }

        /// <summary>
        /// Add Y to the cameras current Y position
        /// </summary>
        /// <param name="y">Value to Add</param>
        public static void addY(int y)
        {
            cameraLocation.Y += y;
        }

        /// <summary>
        /// Move camera by point
        /// </summary>
        /// <param name="p">Adds point to camera location</param>
        public static void incrementCamera(Point p)
        {
            cameraLocation.X += p.X;
            cameraLocation.Y += p.Y;
        }

        /// <summary>
        /// Get camera location (Not including Scaling)
        /// Use getFullCamera for scaling included.
        /// </summary>
        /// <returns></returns>
        public static Rectangle getlocation()
        {
            return cameraLocation;
        }

        /// <summary>
        /// Set camera location
        /// </summary>
        /// <param name="p"></param>
        public static void setLocation(Point p)
        {
            cameraLocation.Location = p;
        }

        /// <summary>
        /// Center the camera on this point
        /// </summary>
        /// <param name="v"> Point on which to center </param>
        public static void Center(Vector2 v)
        {
            Point p = new Point((int)Math.Round(v.X), (int)Math.Round(v.Y));
            Center(p);
        }

        /// <summary>
        /// Center the camera on this point
        /// </summary>
        /// <param name="p"> Point on which to center </param>
        public static void Center(Point p)
        {
            cameraLocation.X = p.X - (getFullScreen().Width / 2);
            cameraLocation.Y = p.Y - (getFullScreen().Height / 2);
        }

        /// <summary>
        /// Zoom by an ammount, focusing on the center.
        /// </summary>
        /// <param name="amt">The ammount by which to zoom</param>
        public static void Zoom(float amt)
        {
            Point zoomCenter = new Point(getFullScreen().X + (getFullScreen().Width / 2), getFullScreen().Y + (getFullScreen().Height / 2));

            zoom += amt;
            setLocation(new Point(zoomCenter.X - (int)(getFullScreen().Width / 2f), zoomCenter.Y - (int)(getFullScreen().Height / 2f)));
        }

        /// <summary>
        /// Zooms the camera to a specified zoom value.
        /// </summary>
        /// <param name="amt">The value to zoom to</param>
        public static void ZoomTo(float amt)
        {
            Zoom(amt - zoom);
        }

        /// <summary>
        /// Get current zoom level
        /// </summary>
        /// <returns>Zoom level</returns>
        public static float getZoom()
        {
            return zoom;
        }

        /// <summary>
        /// Get the full area of the screen.
        /// </summary>
        /// <returns> Rectangle full area of the screen</returns>
        public static Rectangle getFullScreen()
        {
            return new Rectangle(cameraLocation.X, cameraLocation.Y, (int)Math.Ceiling(cameraLocation.Width / zoom), (int)Math.Ceiling(cameraLocation.Height / zoom));
        }

        /// <summary>
        /// Assures this camera is within these bounds.
        /// </summary>
        /// <param name="r"> Maximum camera bounds</param>
        public static void assure(Rectangle r)
        {
            if (cameraLocation.X < r.X)
            {
                cameraLocation.X = r.X;
            }
            if (cameraLocation.Y < r.Y)
            {
                cameraLocation.Y = r.Y;
            }
            if (cameraLocation.X + Math.Round(cameraLocation.Width / zoom) > r.X + r.Width)
                cameraLocation.X = (int)Math.Round(r.X + r.Width - (cameraLocation.Width / zoom));

            if (cameraLocation.Y + Math.Round(cameraLocation.Height / zoom) > r.Y + r.Height)
                cameraLocation.Y = (int)Math.Round(r.Y + r.Height - (cameraLocation.Height / zoom));
        }

        /// <summary>
        /// Draw an image to the screen
        /// </summary>
        /// <param name="sb">The spriteBatch to use</param>
        /// <param name="image">The NCodeRiddian.Image to draw</param>
        /// <param name="r">The rectangle determining image world location</param>
        public static void draw(SpriteBatch sb, Image image, Rectangle r)
        {
            draw(sb, image, r, Color.White, null, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw an image to the screen
        /// </summary>
        /// <param name="sb">The spriteBatch to use</param>
        /// <param name="image">The NCodeRiddian.Image to draw</param>
        /// <param name="r">The rectangle determining image world location</param>
        /// <param name="c">Color to tint the image</param>
        public static void draw(SpriteBatch sb, Image image, Rectangle r, Color c)
        {
            draw(sb, image, r, c, null, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draw an image to the screen
        /// </summary>
        /// <param name="sb">The spriteBatch to use</param>
        /// <param name="image">The NCodeRiddian.Image to draw</param>
        /// <param name="c">Color to tint the image</param>
        /// <param name="r">The rectangle determining image world location</param>
        /// <param name="rotation">The angle of image rotation</param>
        /// <param name="origin">The origin point to rotate the image around</param>
        public static void draw(SpriteBatch sb, Image image, Color c, Rectangle r, float rotation, Vector2 origin)
        {
            draw(sb, image, r, c, null, rotation, origin, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Return the generic texture
        /// </summary>
        /// <returns>A 1x1 image of pure white</returns>
        public static Texture2D getGeneric()
        {
            return spix;
        }

        /// <summary>
        /// Draws an image to the screen using this camera.
        /// </summary>
        /// <param name="sb"> The Sprite Batch </param>
        /// <param name="image"> The Image </param>
        /// <param name="r"> The image location and size</param>
        /// <param name="c"> Color tinting</param>
        /// <param name="sourceRectangle"> Source of image, use Null for animated Image</param>
        /// <param name="rotation"> Rotation of image </param>
        /// <param name="origin"> Origin of image </param>
        /// <param name="effects"> sprite effect for image</param>
        /// <param name="layerDepth"> Layerdepth of image</param>
        public static void draw(SpriteBatch sb, Image image, Rectangle r, Color c, Rectangle? sourceRectangle, float rotation, Vector2 origin, SpriteEffects effects, int layerDepth)
        {
            r.X -= (int)Math.Round(origin.X);
            r.Y -= (int)Math.Round(origin.Y);
            if (getFullScreen().Intersects(r))
            {
                r.X += (int)Math.Round(origin.X);
                r.Y += (int)Math.Round(origin.Y);
                sb.Draw(image.getTexture(),
                        new Rectangle(
                            (int)((Math.Floor((r.X - cameraLocation.X) * zoom)) + Camera.origin.X),
                            (int)((Math.Floor((r.Y - cameraLocation.Y) * zoom)) + Camera.origin.Y),
                            (int)Math.Ceiling(r.Width * zoom),
                            (int)Math.Ceiling(r.Height * zoom)),
                            sourceRectangle == null && image is AnimatedImage ? ((AnimatedImage)image).getFrame() : sourceRectangle,
                            c, rotation, origin, effects, layerDepth);
            }
        }

        /// <summary>
        /// Draws an image to the screen using this camera without checking if it is within the render area - Faster for small images or images that are known to be in the current viewport
        /// </summary>
        /// <param name="sb"> The Sprite Batch </param>
        /// <param name="image"> The Image </param>
        /// <param name="r"> The image location and size</param>
        public static void drawNoCheck(SpriteBatch sb, Image image, Rectangle r)
        {
            drawNoCheck(sb, image, r, Color.White, null, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws an image to the screen using this camera without checking if it is within the render area - Faster for small images or images that are known to be in the current viewport
        /// </summary>
        /// <param name="sb"> The Sprite Batch </param>
        /// <param name="image"> The Image </param>
        /// <param name="r"> The image location and size</param>
        /// <param name="c"> Color tinting</param>
        public static void drawNoCheck(SpriteBatch sb, Image image, Rectangle r, Color c)
        {
            drawNoCheck(sb, image, r, c, null, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
        /// <summary>
        /// Draws an image to the screen using this camera without checking if it is within the render area - Faster for small images or images that are known to be in the current viewport
        /// </summary>
        /// <param name="sb"> The Sprite Batch </param>
        /// <param name="image"> The Image </param>
        /// <param name="r"> The image location and size</param>
        /// <param name="c"> Color tinting</param>
        /// <param name="sourceRectangle"> Source of image, use Null for animated Image</param>
        /// <param name="rotation"> Rotation of image </param>
        /// <param name="origin"> Origin of image </param>
        /// <param name="effects"> sprite effect for image</param>
        /// <param name="layerDepth"> Layerdepth of image</param>
        public static void drawNoCheck(SpriteBatch sb, Image image, Rectangle r, Color c, Rectangle? sourceRectangle, float rotation, Vector2 origin, SpriteEffects effects, int layerDepth)
        {
            sb.Draw(image.getTexture(),
                    new Rectangle(
                        (int)((Math.Floor((r.X - cameraLocation.X) * zoom)) + Camera.origin.X),
                        (int)((Math.Floor((r.Y - cameraLocation.Y) * zoom)) + Camera.origin.Y),
                        (int)Math.Ceiling(r.Width * zoom),
                        (int)Math.Ceiling(r.Height * zoom)),
                        sourceRectangle == null && image is AnimatedImage ? ((AnimatedImage)image).getFrame() : sourceRectangle,
                        c, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        /// Draw a generic texture (solid color) to the screen using this camera
        /// </summary>
        /// <param name="sb">SpriteBatch to use</param>
        /// <param name="r">Rectangle world position to draw</param>
        /// <param name="c">Color of the texture</param>
        /// <param name="sourceRectangle">--Should be null--</param>
        /// <param name="rotation">Rotation of the rectangle</param>
        /// <param name="origin">Origin point to rotate around</param>
        /// <param name="effects">Any spriteeffects to use</param>
        /// <param name="layerDepth">layer of the image</param>
        public static void drawGeneric(SpriteBatch sb, Rectangle r, Color c, Rectangle? sourceRectangle, float rotation, Vector2 origin, SpriteEffects effects, int layerDepth)
        {
            r.X -= (int)Math.Round(origin.X);
            r.Y -= (int)Math.Round(origin.Y);
            if (getFullScreen().Intersects(r))
            {
                r.X += (int)Math.Round(origin.X);
                r.Y += (int)Math.Round(origin.Y);
                sb.Draw(spix,
                        new Rectangle(
                            (int)((Math.Floor((r.X - cameraLocation.X) * zoom)) + Camera.origin.X),
                            (int)((Math.Floor((r.Y - cameraLocation.Y) * zoom)) + Camera.origin.Y),
                            (int)Math.Ceiling(r.Width * zoom),
                            (int)Math.Ceiling(r.Height * zoom)),
                            sourceRectangle,
                            c, rotation, origin, effects, layerDepth);
            }
        }

        /// <summary>
        /// Draw a generic texture (solid color) to the screen using this camera
        /// </summary>
        /// <param name="sb">SpriteBatch to use</param>
        /// <param name="r">Rectangle world position to draw</param>
        /// <param name="c">Color of the texture</param>
        public static void drawGeneric(SpriteBatch sb, Rectangle r, Color c)
        {
            if (getFullScreen().Intersects(r))
            {
                sb.Draw(spix,
                        new Rectangle(
                            (int)((Math.Floor((r.X - cameraLocation.X) * zoom)) + Camera.origin.X),
                            (int)((Math.Floor((r.Y - cameraLocation.Y) * zoom)) + Camera.origin.Y),
                            (int)Math.Ceiling(r.Width * zoom),
                            (int)Math.Ceiling(r.Height * zoom)),
                            null,
                            c, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
        }

        /// <summary>
        /// DEPRICATED - DO NOT USE - INEFFICIENT AS ALL HELL
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        public static void drawGenericPlusTransparency(SpriteBatch sb, Rectangle r, Color c, Rectangle? sourceRectangle, float rotation, Vector2 origin, SpriteEffects effects, int layerDepth)
        {
            Texture2D spix = new Texture2D(gd, 1, 1);
            spix.SetData<Color>(new Color[] { c });
            r.X -= (int)Math.Round(origin.X);
            r.Y -= (int)Math.Round(origin.Y);
            if (getFullScreen().Intersects(r))
            {
                r.X += (int)Math.Round(origin.X);
                r.Y += (int)Math.Round(origin.Y);
                sb.Draw(spix,
                        new Rectangle(
                            (int)((Math.Floor((r.X - cameraLocation.X) * zoom)) + Camera.origin.X),
                            (int)((Math.Floor((r.Y - cameraLocation.Y) * zoom)) + Camera.origin.Y),
                            (int)Math.Ceiling(r.Width * zoom),
                            (int)Math.Ceiling(r.Height * zoom)),
                            sourceRectangle,
                            Color.White, rotation, origin, effects, layerDepth);
            }
        }

        /// <summary>
        /// Draw a generic texture (solid color) to the screen using this camera without first checking if the object is in the renderable area
        /// </summary>
        /// <param name="sb">SpriteBatch to use</param>
        /// <param name="r">Rectangle world position to draw</param>
        /// <param name="c">Color of the texture</param>
        /// <param name="sourceRectangle">--Should be null--</param>
        /// <param name="rotation">Rotation of the rectangle</param>
        /// <param name="origin">Origin point to rotate around</param>
        /// <param name="effects">Any spriteeffects to use</param>
        /// <param name="layerDepth">layer of the image</param>
        public static void drawGenericNoCheck(SpriteBatch sb, Rectangle r, Color c, Rectangle? sourceRectangle, float rotation, Vector2 origin, SpriteEffects effects, int layerDepth)
        {
            sb.Draw(spix,
                    new Rectangle(
                        (int)((Math.Floor((r.X - cameraLocation.X) * zoom)) + Camera.origin.X),
                        (int)((Math.Floor((r.Y - cameraLocation.Y) * zoom)) + Camera.origin.Y),
                        (int)Math.Ceiling(r.Width * zoom),
                        (int)Math.Ceiling(r.Height * zoom)),
                        sourceRectangle,
                        c, rotation, origin, effects, layerDepth);
        }

        /// <summary>
        /// Draws an image stretched along it's X axis to form a line between two points
        /// </summary>
        /// <param name="sb">Spritebatch to use</param>
        /// <param name="i">The image to use</param>
        /// <param name="p1">Start point</param>
        /// <param name="p2">End point</param>
        /// <param name="forcedWidth">Width of the line</param>
        /// <param name="c">Tint color</param>
        public static void drawAsLine(SpriteBatch sb, Image i, Vector2 p1, Vector2 p2, int forcedWidth, Color c)
        {
            float distance = LocationManager.getDistance(p1, p2);
            float rotation = LocationManager.getRotation(p1, p2);

            Vector2 placementPoint = LocationManager.moveByRotation(p1, distance / 2, rotation);

            Rectangle colisCheck = new Rectangle((int)Math.Ceiling(Math.Min(p1.X, p2.X)), (int)Math.Ceiling(Math.Min(p1.Y, p2.Y)), (int)Math.Ceiling(Math.Abs(p1.X - p2.X)), (int)Math.Ceiling(Math.Abs(p1.Y - p2.Y)));

            Rectangle drawLocation = new Rectangle((int)placementPoint.X, (int)placementPoint.Y, (int)Math.Ceiling(distance), forcedWidth);

            drawLocation = new Rectangle(
                            (int)((Math.Round((drawLocation.X - cameraLocation.X) * zoom)) + Camera.origin.X),
                            (int)((Math.Round((drawLocation.Y - cameraLocation.Y) * zoom)) + Camera.origin.Y),
                            (int)Math.Round(drawLocation.Width * zoom),
                            (int)Math.Round(drawLocation.Height * zoom));

            if (colisCheck.Intersects(getFullScreen()))
                sb.Draw(i.getTexture(), drawLocation, null, c, rotation, new Vector2(((float)i.getTexture().Width) / 2f, ((float)i.getTexture().Height) / 2f), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws a string to the screen
        /// </summary>
        /// <param name="sb">SpriteBatch to use</param>
        /// <param name="sf">SpriteFont to use</param>
        /// <param name="text">Text to write</param>
        /// <param name="location">Position to write</param>
        /// <param name="c">Color of text</param>
        /// <param name="rot">Rotation</param>
        /// <param name="origin">Origin of Rotation</param>
        /// <param name="seffect">SpriteEffect</param>
        /// <param name="layerDep">LayerDepth</param>
        public static void drawString(SpriteBatch sb, SpriteFont sf, string text, Vector2 location, Color c, float rot, Vector2 origin, SpriteEffects seffect, int layerDep)
        {
            Rectangle stringmeasure = new Rectangle((int)Math.Round(location.X), (int)Math.Round(location.Y), (int)Math.Round(sf.MeasureString(text).X), (int)Math.Round(sf.MeasureString(text).Y));

            stringmeasure.X -= (int)Math.Round(origin.X);
            stringmeasure.Y -= (int)Math.Round(origin.Y);

            if (stringmeasure.Intersects(getFullScreen()))
            {
                stringmeasure.X += (int)Math.Round(origin.X);
                stringmeasure.Y += (int)Math.Round(origin.Y);
                sb.DrawString(sf, text, new Vector2(((location.X - cameraLocation.X) * zoom) + Camera.origin.X, ((location.Y - cameraLocation.Y) * zoom) + Camera.origin.Y), c, rot, origin, zoom, seffect, layerDep);
            }
        }

        /// <summary>
        /// Draws a generic texture (solid color) line between two points
        /// </summary>
        /// <param name="v1">Start point of the line</param>
        /// <param name="v2">End point of the line</param>
        /// <param name="sb">SpriteBatch to use</param>
        /// <param name="c">Color of the line</param>
        /// <param name="width">Width of the line</param>
        public static void drawLineGeneric(Vector2 v1, Vector2 v2, SpriteBatch sb, Color c, int width)
        {
            float betweenAngle = LocationManager.getRotation(v1, v2);
            float distance = LocationManager.getDistance(v1, v2);
            drawGenericNoCheck(sb, new Rectangle((int)Math.Round(LocationManager.moveByRotation(v1, distance / 2f, betweenAngle).X), (int)Math.Round(LocationManager.moveByRotation(v1, distance / 2f, betweenAngle).Y), (int)Math.Round(distance), width), c, null, betweenAngle, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
        }
        /// <summary>
        /// Draws a generic texture (solid color) line between two points
        /// </summary>
        /// <param name="v1">Start point of the line</param>
        /// <param name="v2">End point of the line</param>
        /// <param name="sb">SpriteBatch to use</param>
        /// <param name="c">Color of the line</param>
        public static void drawLineGeneric(Vector2 v1, Vector2 v2, SpriteBatch sb, Color c)
        {
            float betweenAngle = LocationManager.getRotation(v1, v2);
            float distance = LocationManager.getDistance(v1, v2);
            drawGenericNoCheck(sb, new Rectangle((int)Math.Round(LocationManager.moveByRotation(v1, distance / 2f, betweenAngle).X), (int)Math.Round(LocationManager.moveByRotation(v1, distance / 2f, betweenAngle).Y), (int)Math.Round(distance), 1), c, null, betweenAngle, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
        }

        public static void drawLineGeneric_NonRelative(Vector2 v1, Vector2 v2, SpriteBatch sb, Color c)
        {
            float betweenAngle = LocationManager.getRotation(v1, v2);
            float distance = LocationManager.getDistance(v1, v2);
            sb.Draw(spix, new Rectangle((int)Math.Round(LocationManager.moveByRotation(v1, distance / 2f, betweenAngle).X),
                    (int)Math.Round(LocationManager.moveByRotation(v1, distance / 2f, betweenAngle).Y),
                    (int)Math.Round(distance), 1), null, c, betweenAngle, new Vector2(0.5f, 0.5f), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Displays a string to the screen at its regular size, ignoring zoom level
        /// </summary>
        /// <param name="sb">SpriteBatch to use</param>
        /// <param name="sf">SpriteFont to use</param>
        /// <param name="text">Text to write</param>
        /// <param name="location">Position to write</param>
        /// <param name="c">Color of text</param>
        /// <param name="rot">Rotation</param>
        /// <param name="origin">Origin of Rotation</param>
        /// <param name="seffect">SpriteEffect</param>
        /// <param name="layerDep">LayerDepth</param>
        public static void drawStringNoZoom(SpriteBatch sb, SpriteFont sf, string text, Vector2 location, Color c, float rot, Vector2 origin, SpriteEffects seffect, int layerDep)
        {
            Rectangle stringmeasure = new Rectangle((int)Math.Round(location.X), (int)Math.Round(location.Y), (int)Math.Round(sf.MeasureString(text).X), (int)Math.Round(sf.MeasureString(text).Y));

            stringmeasure.X -= (int)Math.Round(origin.X);
            stringmeasure.Y -= (int)Math.Round(origin.Y);

            if (stringmeasure.Intersects(getFullScreen()))
            {
                stringmeasure.X += (int)Math.Round(origin.X);
                stringmeasure.Y += (int)Math.Round(origin.Y);
                sb.DrawString(sf, text, new Vector2(((location.X - cameraLocation.X) * zoom) + Camera.origin.X, ((location.Y - cameraLocation.Y) * zoom) + Camera.origin.Y), c, rot, origin, 1, seffect, layerDep);
            }
        }


        /// <summary>
        /// Draws a NCodeRiddian.WrappedText object to the screen
        /// </summary>
        /// <param name="sb">the SpriteBatch to use</param>
        /// <param name="tex">WrappedText to write</param>
        /// <param name="p">world location</param>
        /// <param name="spacing">Ammount of space (vertically) between lines of text</param>
        /// <param name="c"> Color of text to draw</param>
        public static void drawWrappedText(SpriteBatch sb, WrappedText tex, Point p, float spacing, Color c)
        {
            Rectangle measure = new Rectangle(p.X, p.Y, tex.getWidth(), (int)tex.getHeight(spacing));

            if (measure.Intersects(getFullScreen()))
            {
                tex.draw(sb, new Vector2(((p.X - cameraLocation.X) * zoom) + Camera.origin.X, ((p.Y - cameraLocation.Y) * zoom) + Camera.origin.Y), spacing, c);
            }
        }
    }
}