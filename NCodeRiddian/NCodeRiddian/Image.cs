using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    /// <summary>
    /// An NCodeRiddian equivalent of Texture2D for use with cameras. Loaded by calling TextureManager.loadAllImages()
    /// </summary>
    public class Image
    {
        protected Texture2D image;
        private string TextureManagerDefinition;

        public static explicit operator Texture2D(Image i)
        {
            return i.getTexture();
        }

        public Image(Texture2D imageArt)
        {
            image = imageArt;
        }

        /// <summary>
        /// Create an image using it's filename so long as TextureManager.loadAllImages had been called.
        /// </summary>
        /// <param name="TextureManagerName">Filename (without extension, without "Content\\")</param>
        public Image(string TextureManagerName)
        {
            TextureManagerDefinition = TextureManagerName;
            image = TextureManager.getTexture(TextureManagerDefinition);
        }

        /// <summary>
        /// Returns the texture affiliated with this object
        /// </summary>
        /// <returns>Texture2D image data</returns>
        public Texture2D getTexture()
        {
            return image;
        }

        /// <summary>
        /// Changes this images contents
        /// </summary>
        /// <param name="newImage"></param>
        public void setImage(Texture2D newImage)
        {
            image = newImage;
        }
    }

    /// <summary>
    /// An image class for loading spritesheets
    /// </summary>
    public class AnimatedImage : Image
    {
        protected Point frameS;
        protected Rectangle currentFrame;
        protected int framesleep;
        protected int maxFrameSleep;
        protected int NumberOfFrames;
        public onFinish onFinishAction;
        public onFinishParams parameterFinish;

        /// <summary>
        /// Creates a new animated image
        /// </summary>
        /// <param name="animationArt">The texture2d of this spritesheet</param>
        /// <param name="frameSize">the size (in pixels) of every frame</param>
        /// <param name="frameSleepTime">The number of updates to wait between changing frames</param>
        /// <param name="frameNum">The total number of frames in this image</param>
        public AnimatedImage(Texture2D animationArt, Point frameSize, int frameSleepTime, int frameNum)
            : base(animationArt)
        {
            frameS = frameSize;
            currentFrame = new Rectangle(0, 0, frameSize.X, frameSize.Y);
            maxFrameSleep = frameSleepTime;
            framesleep = maxFrameSleep;

            NumberOfFrames = frameNum;
        }

        /// <summary>
        /// Creates a new animated image
        /// </summary>
        /// <param name="fileloc">The file location of this image</param>
        /// <param name="frameSize">the size (in pixels) of every frame</param>
        /// <param name="frameSleepTime">The number of updates to wait between changing frames</param>
        /// <param name="frameNum">The total number of frames in this image</param>
        public AnimatedImage(string fileloc, Point frameSize, int frameSleepTime, int frameNum)
            : base(fileloc)
        {
            frameS = frameSize;
            currentFrame = new Rectangle(0, 0, frameSize.X, frameSize.Y);
            maxFrameSleep = frameSleepTime;
            framesleep = maxFrameSleep;

            NumberOfFrames = frameNum;
        }

        /// <summary>
        /// Returns the size of a single frame
        /// </summary>
        /// <returns></returns>
        public Point getFrameSize()
        {
            return frameS;
        }

        /// <summary>
        /// Resets the animation to the beginning
        /// </summary>
        public void reset()
        {
            currentFrame = new Rectangle(0, 0, currentFrame.Width, currentFrame.Height);
            framesleep = maxFrameSleep;
        }

        /// <summary>
        /// Returns the time to wait between frames
        /// </summary>
        /// <returns></returns>
        public int getFrameSleep()
        {
            return maxFrameSleep;
        }

        /// <summary>
        /// Returns the number of frames
        /// </summary>
        /// <returns></returns>
        public int getNumberOfFrames()
        {
            return NumberOfFrames;
        }

        /// <summary>
        /// Changes the amount of time to wait between frames
        /// </summary>
        /// <param name="i"></param>
        public void setFrameDelay(int i)
        {
            maxFrameSleep = i;
        }

        /// <summary>
        /// Runs one tick of animation
        /// </summary>
        public virtual void applyStep()
        {
            framesleep--;
            if (framesleep <= 0)
            {
                framesleep = maxFrameSleep;
                currentFrame.X += frameS.X;

                if (currentFrame.X >= base.image.Width || (currentFrame.X / frameS.X) + ((getTexture().Width / frameS.X) * (currentFrame.Y / frameS.Y)) >= NumberOfFrames)
                {
                    if (onFinishAction != null)
                        onFinishAction(this);
                    currentFrame.X = 0;
                    currentFrame.Y += frameS.Y;
                    currentFrame.Y %= getTexture().Height;
                }
            }
        }

        /// <summary>
        /// Runs one tick of animation, and uses parameters to launch this images "OnFinishParams" if nessecary
        /// </summary>
        /// <param name="onfinishparam"></param>
        public virtual void applyStep(params object[] onfinishparam)
        {
            framesleep--;
            if (framesleep <= 0)
            {
                framesleep = maxFrameSleep;
                currentFrame.X += frameS.X;

                if (currentFrame.X >= base.image.Width || (currentFrame.X / frameS.X) + ((getTexture().Width / frameS.X) * (currentFrame.Y / frameS.Y)) >= NumberOfFrames)
                {
                    if (parameterFinish != null)
                        parameterFinish(this, onfinishparam);
                    currentFrame.X = 0;
                    currentFrame.Y += frameS.Y;
                    currentFrame.Y %= getTexture().Height;
                }
            }
        }

        public delegate void onFinish(AnimatedImage thisImage);

        public delegate void onFinishParams(AnimatedImage thisImage, params object[] onfinishparam);

        /// <summary>
        /// Skips to a certain frame of animation
        /// </summary>
        /// <param name="animationStep">The frame number to skip to</param>
        /// <param name="resetTime">Boolean indicating wether or not to reset the timer till the next frame</param>
        public void skipToStep(int animationStep, bool resetTime)
        {
            currentFrame.X = frameS.X * animationStep;
            currentFrame.X %= getTexture().Width;

            if (resetTime)
                framesleep = maxFrameSleep;
        }


        /// <summary>
        /// Returns a rectangle referring to the current source frame of animation
        /// </summary>
        /// <returns></returns>
        public Rectangle getFrame()
        {
            return currentFrame;
        }
    }
}