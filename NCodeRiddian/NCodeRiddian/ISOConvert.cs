using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public abstract class ISOConvert
    {
        public static Point GetPFromIso(int x, int y, int z)
        {
            return new Point(y - x, (int)Math.Round((y + x) / 2f) + z);
        }

        public static Vector2 GetVFromIso(int x, int y, int z)
        {
            return new Vector2(y - x, ((y + x) / 2f) + z);
        }

        public static Vector2 GetVFromIso(float x, float y, float z, int width, int height)
        {
            return new Vector2(width * (y-x), (width/2f) * (x + y) - height * z);
        }

        public static Point GetPFromIso(int x, int y, int z, int width, int height)
        {
            return new Point(width * (y - x), (width / 2) * (x + y) - height * z);
        }

        public static float getDepth(float x, float y, float z, int maxx, int maxy, int maxz)
        {
            return ((x + y + z) / (maxx + maxy + maxz));
        }
    }
}
