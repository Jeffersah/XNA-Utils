using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public class P_Entity
    {
        public Vector2 loc = new Vector2(0, 0);
        private Rectangle sourceRect = new Rectangle();
        public Texture2D img;

        private bool isAnimated;
        private bool loop;
        public Rectangle[,] animFrames;
        private int curFrame;
        private int maxFrame;
        private int changeTime;
        private int mChangeTime;
        private int curAnim;
        public bool done;

        public P_Entity()
        {
        }

        public P_Entity(Vector2 loc)
        {
            this.loc = loc;
            isAnimated = false;
            sourceRect.X = 0; sourceRect.Y = 0;
            done = false;
        }

        public void setupAnimation(int frameHeight, int frameWidth, int FrameDelay, bool loop)
        {
            if (img == null)
                throw new NoImageException("No Image Loaded!");

            done = false;

            animFrames = new Rectangle[img.Height / frameHeight, img.Width / frameWidth];
            for (int x = 0; x < animFrames.GetLength(0); x++)
            {
                for (int y = 0; y < animFrames.GetLength(1); y++)
                {
                    animFrames[x, y] = new Rectangle(frameWidth * y, frameHeight * x, frameHeight, frameWidth);
                }
            }

            mChangeTime = FrameDelay;
            curFrame = 0; curAnim = 0;
            maxFrame = animFrames.GetLength(1);
            this.loop = loop;
            isAnimated = true;
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            img = theContentManager.Load<Texture2D>(theAssetName);
            sourceRect.X = 0;
            sourceRect.Y = 0;
            sourceRect.Height = img.Height;
            sourceRect.Width = img.Width;
        }

        public void setImg(Texture2D newTexture)
        {
            this.img = newTexture;
        }

        public void Draw(SpriteBatch sb, int Animation)
        {
            Draw(sb, 0, 1, new Vector2(0, 0), Animation);
        }

        public void Draw(SpriteBatch sb, float r, float s, int Animation)
        {
            Draw(sb, r, s, new Vector2(0, 0), Animation);
        }

        public void Draw(SpriteBatch theSpriteBatch, float rotation, float scale, Vector2 ssource, int Animation)
        {
            if (!isAnimated)
                DrawAll(theSpriteBatch, rotation, scale, ssource, new Rectangle(0, 0, img.Width, img.Height));
            else
            {
                if (Animation > animFrames.GetLength(0) || Animation < 0)
                    throw new InvalidFrameException("Selected Animation/Start Frame is nonexistant");
                if (curAnim != Animation)
                {
                    curAnim = Animation;
                    curFrame = 0;
                }

                drawFrame(theSpriteBatch, Animation, curFrame, rotation, scale, ssource);

                if (changeTime == 0)
                {
                    curFrame++;
                    if (loop && curFrame == maxFrame)
                        curFrame = 0;
                    else if (!loop && curFrame == maxFrame)
                        done = true;
                    changeTime = mChangeTime;
                }
                else
                    changeTime--;
            }
        }

        public void DrawTint(SpriteBatch theSpriteBatch, float rotation, float scale, Vector2 ssource, int Animation, Color color)
        {
            if (!isAnimated)
                DrawAllTint(theSpriteBatch, rotation, scale, ssource, color);
            else
            {
                if (Animation > animFrames.GetLength(0) || Animation < 0)
                    throw new InvalidFrameException("Selected Animation/Start Frame is nonexistant");
                if (curAnim != Animation)
                {
                    curAnim = Animation;
                    curFrame = 0;
                }

                drawFrameTint(theSpriteBatch, Animation, curFrame, rotation, scale, ssource, color);

                if (changeTime == 0)
                {
                    curFrame++;
                    if (loop && curFrame == maxFrame)
                        curFrame = 0;
                    else if (!loop && curFrame == maxFrame)
                        done = true;
                    changeTime = mChangeTime;
                }
                else
                    changeTime--;
            }
        }

        public void drawFrame(SpriteBatch sb, int animNumber, int frameNumber, float rotation, float scale, Vector2 ssource)
        {
            DrawAll(sb, rotation, scale, ssource, animFrames[animNumber, frameNumber]);
        }

        public void drawFrameTint(SpriteBatch sb, int animNumber, int frameNumber, float rotation, float scale, Vector2 ssource, Color c)
        {
            DrawAllTint(sb, rotation, scale, ssource, animFrames[animNumber, frameNumber], c);
        }

        public void DrawAll(SpriteBatch sb)
        {
            DrawAll(sb, 0, 1, new Vector2(0, 0), new Rectangle(0, 0, img.Width, img.Height));
        }

        public void DrawAll(SpriteBatch sb, float rotation, float scale)
        {
            DrawAll(sb, rotation, scale, new Vector2(0, 0), new Rectangle(0, 0, img.Width, img.Height));
        }

        public void DrawAll(SpriteBatch theSpriteBatch, float rotation, float scale, Vector2 ssource)
        {
            DrawAll(theSpriteBatch, rotation, scale, ssource, new Rectangle(0, 0, img.Width, img.Height));
        }

        public void DrawAll(SpriteBatch theSpriteBatch, float rotation, float scale, Vector2 ssource, Rectangle imgSource)
        {
            if (img == null)
                throw new NoImageException("No Image Loaded!");

            try
            {
                theSpriteBatch.Draw(this.img, this.loc, imgSource, Color.White, rotation, ssource, scale, SpriteEffects.None, 0f);
            }
            catch (ArgumentNullException) { }
        }

        public void DrawAllTint(SpriteBatch theSpriteBatch, float rotation, float scale, Vector2 ssource, Color c)
        {
            theSpriteBatch.Draw(this.img, this.loc, null, Color.White, rotation, ssource, scale, SpriteEffects.None, 0f);
        }

        public void DrawAllTint(SpriteBatch theSpriteBatch, float rotation, float scale, Vector2 ssource, Rectangle r, Color c)
        {
            theSpriteBatch.Draw(this.img, this.loc, r, c, rotation, ssource, scale, SpriteEffects.None, 0f);
        }

        public Texture2D getImage()
        {
            return img;
        }
    }

    internal class NoImageException : Exception
    {
        public NoImageException(string s)
            : base(s)
        {
        }
    }

    internal class InvalidFrameException : Exception
    {
        public InvalidFrameException(string s)
            : base(s)
        {
        }
    }
}