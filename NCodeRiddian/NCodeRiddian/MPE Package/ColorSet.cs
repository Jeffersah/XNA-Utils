using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public class ColorSet
    {
        public static Color[] Fire = { Color.Red, Color.DarkRed, Color.OrangeRed, Color.Orange, Color.DarkOrange, Color.Yellow, Color.LightYellow };
        public static Color[] DirtyFire = { Color.Red, Color.DarkRed, Color.OrangeRed, Color.Orange, Color.DarkOrange, Color.Yellow, Color.LightYellow, Color.Gray, Color.SlateGray, Color.Black };
        public static Color[] Smoke = { Color.White, Color.WhiteSmoke, Color.Gray, Color.LightGray, Color.DarkGray, Color.SlateGray, Color.LightSlateGray };
        public static Color[] Ice = { Color.Blue, Color.LightBlue, Color.DarkBlue, Color.CornflowerBlue, Color.Cyan, Color.DarkCyan, Color.LightCyan };
        public static Color[] Dirt = { new Color(153, 53, 0), new Color(204, 51, 0), new Color(204, 102, 0), new Color(61, 31, 0), new Color(151, 61, 0), new Color(107, 71, 0), new Color(153, 92, 31) };
        public static Color[] Dark = { Color.Black, new Color(10, 10, 10), new Color(20, 20, 20), new Color(5, 5, 15), new Color(15, 5, 5), new Color(25, 25, 25), new Color(30, 15, 30) };

        private List<Color> colors;

        public ColorSet()
        {
            colors = new List<Color>();
        }

        public ColorSet(Color[] c)
        {
            colors = new List<Color>(c);
        }

        public void addColor(Color c)
        {
            colors.Add(c);
        }

        public bool removeColor(Color c)
        {
            return colors.Remove(c);
        }

        public Color getRandomColor()
        {
            return colors[BasicParticle.random.Next(colors.Count)];
        }
    }
}