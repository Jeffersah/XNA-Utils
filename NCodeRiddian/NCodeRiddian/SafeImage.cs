using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    /// <summary>
    /// Gets an image, or a placeholder if it hasn't been defined
    /// </summary>
    public abstract class SafeImage
    {
        static Image placeholder;

        /// <summary>
        /// Set a global placeholder
        /// </summary>
        /// <param name="i"></param>
        public static void SetPlaceholder(Image i)
        {
            if (i == null || i.getTexture() == null)
                placeholder = null;
            else
                placeholder = i;
        }

        /// <summary>
        /// Get an image from the fileloc, or the placeholder if none is found
        /// </summary>
        /// <param name="fileloc"></param>
        /// <returns></returns>
        public static Image Get(string fileloc)
        {
            Image test = new Image(fileloc);
            if (test.getTexture() == null)
            { 
                if(placeholder == null)
                {
                    throw new Exception("Placeholder never defined");
                }
                return placeholder;
            }
            return test;
        }

        /// <summary>
        /// Get an image from fileloc, or the backup if the file is not found.
        /// </summary>
        /// <param name="fileloc"></param>
        /// <param name="backup"></param>
        /// <returns></returns>
        public static Image Get(string fileloc, Image backup)
        {
            Image test = new Image(fileloc);
            if (test.getTexture() == null)
            {
                if (backup == null)
                {
                    throw new Exception("Backup is null");
                }
                return backup;
            }
            return test;
        }
    }
}
