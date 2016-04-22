using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public abstract class PostProcessing
    {
        protected static GraphicsDeviceManager graphics;
        protected static SpriteBatch spriteBatch;
        protected static RenderTarget2D screenDump;

        public static void Enable(GraphicsDeviceManager gm, SpriteBatch sb)
        {
            graphics = gm;
            spriteBatch = sb;
            screenDump = new RenderTarget2D(gm.GraphicsDevice, gm.PreferredBackBufferWidth, gm.PreferredBackBufferHeight);
        }

        public static void startDrawing()
        {
            graphics.GraphicsDevice.SetRenderTarget(screenDump);
            graphics.GraphicsDevice.Clear(new Color(0, 0, 0, 0));
            spriteBatch.Begin();
        }

        protected static Color[] applyEffect(Color[] colors)
        {
            return colors;
        }

        public static void endDrawing()
        {
            spriteBatch.End();

            graphics.GraphicsDevice.SetRenderTarget(null);
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            Color[] colorArray = new Color[screenDump.Width * screenDump.Height];
            screenDump.GetData<Color>(colorArray);
            colorArray = applyEffect(colorArray);
            screenDump.SetData<Color>(colorArray);
            spriteBatch.Begin();

            spriteBatch.Draw(screenDump, new Vector2(0, 0), Color.White);

            spriteBatch.End();
        }
    }
}