using System.Collections.Generic;

namespace NCodeRiddian
{
    /// <summary>
    /// An index of lists of images
    /// </summary>
    public class ImageSet
    {
        private List<List<Image>> imgSet = new List<List<Image>>();

        /// <summary>
        /// Adds a new set to the imageset list
        /// </summary>
        /// <param name="setNumber">Numerical index of this set</param>
        public void addSet(int setNumber)
        {
            while (imgSet.Count <= setNumber)
                imgSet.Add(new List<Image>());
        }

        /// <summary>
        /// Add an image to the specified set of images
        /// </summary>
        /// <param name="setNumber">The setnumber</param>
        /// <param name="i">The image</param>
        public void AddImage(int setNumber, Image i)
        {
            while (imgSet.Count <= setNumber)
                imgSet.Add(new List<Image>());
            imgSet[setNumber].Add(i);
        }

        /// <summary>
        /// Get a random image from a specified set
        /// </summary>
        /// <param name="setNumber">The number set to pull from</param>
        /// <returns></returns>
        public Image getRandomImage(int setNumber)
        {
            if (imgSet[setNumber].Count == 0)
                return null;
            return imgSet[setNumber][GlobalRandom.random.Next(imgSet[setNumber].Count)];
        }

        /// <summary>
        /// Get a specific image
        /// </summary>
        /// <param name="setNumber">The set the image was in</param>
        /// <param name="imgNumber">the numerical index of the image</param>
        /// <returns></returns>
        public Image getImage(int setNumber, int imgNumber)
        {
            return imgSet[setNumber][imgNumber];
        }

        /// <summary>
        /// Uses getRandomImage or Adds an image to the set
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Image this[int i]
        {
            get
            {
                return getRandomImage(i);
            }
            set
            {
                AddImage(i, value);
            }
        }
    }
}