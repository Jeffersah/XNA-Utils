using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
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

        public void Center(Point p)
        {
            CameraLocation.X = (int)(p.X - (ScreenLocation.Width / zoom / 2));
            CameraLocation.Y = (int)(p.Y - (ScreenLocation.Height / zoom / 2));
        }

        public void Center(Vector2 p)
        {
            Center(LocationManager.getPointFromVector(p));
        }

        public Rectangle getView()
        {
            return MemSave.getr(CameraLocation.X, CameraLocation.Y, (int)Math.Round(ScreenLocation.Width / myZoom), (int)Math.Round(ScreenLocation.Height / myZoom));
        }

        public Rectangle GetScreenLocation()
        {
            return ScreenLocation;
        }

        public void startRender(SpriteBatch sb)
        {
            startRender(sb, Color.CornflowerBlue);
        }

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

        public static void FINISH(SpriteBatch sb)
        {
            sb.GraphicsDevice.SetRenderTarget(null);
        }

        public static void RENDER(SpriteBatch sb)
        {
            sb.Draw(TOTAL, MemSave.getr(0, 0, TOTAL.Width, TOTAL.Height), Color.White);
        }
    }
}