using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    /// <summary>
    /// Class allowing for split screen using multiple cameras
    /// </summary>
    public class MultiCamera
    {
        public Point CameraLocation;
        private Point CameraStore;
        private float myZoom;

        private float storeZoom;
        private Rectangle ScreenLocation;
        private RenderTarget2D personalTarget;
        private static bool isRunningRender;


        private static RenderTarget2D TOTAL;

        /// <summary>
        /// The current zoom of the camera, 1 being standard
        /// </summary>
        public float zoom
        {
            get
            {
                return myZoom;
            }
            set
            {
                if (value > 0)
                    myZoom = value;
            }
        }

        /// <summary>
        /// Must be called before any drawing is done
        /// </summary>
        /// <param name="screenSize">The size of the full screen</param>
        /// <param name="gd">the current graphics device</param>
        public static void SETUP(Point screenSize, GraphicsDevice gd)
        {
            TOTAL = new RenderTarget2D(gd, screenSize.X, screenSize.Y, true, gd.PresentationParameters.BackBufferFormat, DepthFormat.Depth16, 0, RenderTargetUsage.PreserveContents);
        }

        public MultiCamera(Rectangle screenLocation, GraphicsDevice gd)
        {
            ScreenLocation = screenLocation;

            personalTarget = new RenderTarget2D(gd, ScreenLocation.Width, ScreenLocation.Height);
            isRunningRender = false;
            CameraLocation = new Point(0, 0);
            myZoom = 1;
        }

        /// <summary>
        /// Tries to keep the full camera view in rectangle r without changing the zoom level
        /// </summary>
        /// <param name="r"></param>
        public void assure(Rectangle r)
        {
            Rectangle myrec = getView();
            CameraLocation.X = Math.Max(CameraLocation.X, r.X);
            CameraLocation.Y = Math.Max(CameraLocation.Y, r.Y);
            if (CameraLocation.X + myrec.Width > r.X + r.Width)
            {
                CameraLocation.X = r.X + r.Width - myrec.Width;
            }
            if (CameraLocation.Y + myrec.Height > r.Y + r.Height)
            {
                CameraLocation.Y = r.Y + r.Height - myrec.Height;
            }
        }

        /// <summary>
        /// Moves the camera so that it's entire view fits in rectangle r. Zooms only if nessecary
        /// </summary>
        /// <param name="r"></param>
        public void advancedAssure(Rectangle r)
        {
            Rectangle myrec = getView();
            if (myrec.Width > r.Width)
            {
                myZoom = ((float)myrec.Width) / ((float)r.Width);
                myrec = getView();
            }
            if (myrec.Height > r.Height)
            {
                myZoom = ((float)myrec.Height) / ((float)r.Height);
            }
            assure(r);
        }

        /// <summary>
        /// Centers the camera on a given point
        /// </summary>
        /// <param name="p"></param>
        public void Center(Point p)
        {
            CameraLocation.X = (int)(p.X - (ScreenLocation.Width / zoom / 2));
            CameraLocation.Y = (int)(p.Y - (ScreenLocation.Height / zoom / 2));
        }

        /// <summary>
        /// Centers the camera on a given vector
        /// </summary>
        /// <param name="p"></param>
        public void Center(Vector2 p)
        {
            Center(LocationManager.getPointFromVector(p));
        }

        /// <summary>
        /// Get the full view of the camera
        /// </summary>
        /// <returns>Full view space, accounting for zoom</returns>
        public Rectangle getView()
        {
            return MemSave.getr(CameraLocation.X, CameraLocation.Y, (int)Math.Round(ScreenLocation.Width / myZoom), (int)Math.Round(ScreenLocation.Height / myZoom));
        }

        /// <summary>
        /// Get the camera location on screen
        /// </summary>
        /// <returns></returns>
        public Rectangle GetScreenLocation()
        {
            return ScreenLocation;
        }

        /// <summary>
        /// Called immediately after SpriteBatch Begin to tell the multicamera to start drawing
        /// </summary>
        /// <param name="sb">The spritebatch</param>
        public void startRender(SpriteBatch sb)
        {
            startRender(sb, Color.CornflowerBlue);
        }

        /// <summary>
        /// Called immediately after SpriteBatch Begin to tell the multicamera to start drawing
        /// </summary>
        /// <param name="sb">The SpriteBatch</param>
        /// <param name="clearColor">The color to clear the screen</param>
        public void startRender(SpriteBatch sb, Color clearColor)
        {
            if (isRunningRender)
                throw new Exception("startRender called while render still running!");

            CameraStore = Camera.getlocation().Location;
            storeZoom = Camera.getZoom();
            Camera.ZoomTo(myZoom);
            Camera.setLocation(CameraLocation);
            sb.GraphicsDevice.SetRenderTarget(personalTarget);
            sb.GraphicsDevice.Clear(clearColor);
            isRunningRender = true;
            sb.Begin();
        }

        /// <summary>
        /// Called when this camera is done drawing, writes all camera data to the static rendertarget. Must be called before SpriteBatch.End
        /// </summary>
        /// <param name="sb">The spritebatch used to begin this call</param>
        public void endRender(SpriteBatch sb)
        {
            if (!isRunningRender)
                throw new Exception("endRender called without starting render!");
            sb.End();

            Camera.setLocation(CameraStore);
            Camera.ZoomTo(storeZoom);
            sb.GraphicsDevice.SetRenderTarget(TOTAL);
            sb.Begin();
            sb.Draw(personalTarget, ScreenLocation, Color.White);
            sb.End();

            isRunningRender = false;
        }

        /// <summary>
        /// Finishes all multicamera rendering and returns the spritebatch target to the screen. Doesn't actually draw anything.
        /// </summary>
        /// <param name="sb"></param>
        public static void FINISH(SpriteBatch sb)
        {
            sb.GraphicsDevice.SetRenderTarget(null);
        }

        /// <summary>
        /// Called after FINISH, draws all data to the backbuffer
        /// </summary>
        /// <param name="sb"></param>
        public static void RENDER(SpriteBatch sb)
        {
            sb.Draw(TOTAL, MemSave.getr(0, 0, TOTAL.Width, TOTAL.Height), Color.White);
        }
    }
}