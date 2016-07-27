using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    class VManager
    {
        public static Vector2 convertToMA(Vector2 XY)
        {
            return new Vector2(Vector2.Distance(Vector2.Zero, XY), (float)Math.Atan2(XY.Y, XY.X));
        }

        public static Vector2 convertToXY(Vector2 MA)
        {
            return new Vector2(MA.X * (float)Math.Cos(MA.Y), MA.X * (float)Math.Sin(MA.Y));
        }
    }
}
