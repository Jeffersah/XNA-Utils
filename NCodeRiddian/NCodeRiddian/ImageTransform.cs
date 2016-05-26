using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public abstract class ImageTransform
    {
        public static void SetData(Texture2D image, Color[] data)
        {
            image.SetData<Color>(data);
        }
        public static void SetData(Texture2D image, Color[,] data)
        {
            Color[] data2 = new Color[image.Width * image.Height];
            for (int i = 0; i < image.Width * image.Height; i++)
            {
                data2[i] = data[i % image.Width, (int)(i / image.Width)];
            }
            image.SetData<Color>(data2);
        }

        public static Color[] GetData(Texture2D image)
        {
            Color[] data = new Color[image.Width * image.Height];
            image.GetData<Color>(data);
            return data;
        }

        public static Color[,] GetData2(Texture2D image)
        {
            Color[] data = new Color[image.Width * image.Height];
            Color[,] data2 = new Color[image.Width, image.Height];
            image.GetData<Color>(data);
            for(int i = 0; i < data.Length; i++)
            {
                data2[i % image.Width, (int)(i / image.Width)] = data[i];
            }
            return data2;
        }
    }
}
