using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public class Physics
    {
        public static float GetAngleDifference(float A, float B)
        {
            float tmp = SimplifyAngle(B - A);
            if (tmp > MathHelper.Pi)
                tmp -= MathHelper.TwoPi;
            return tmp;
        }

        public static float GetAngleDifferencePercent(float A, float B)
        {
            float tmp = SimplifyAngle(B - A);
            if (tmp > MathHelper.Pi)
                tmp -= MathHelper.TwoPi;
            return tmp / MathHelper.Pi;
        }

        public static float SimplifyAngle(float ang)
        {
            while (ang < 0)
                ang += MathHelper.TwoPi;
            return ang % MathHelper.TwoPi;
        }

        public static Vector2 AddVectors(Vector2 V1, Vector2 V2)
        {
            return (ConvertToMA(Vector2.Add(ConvertToXY(V1), ConvertToXY(V2))));
        }

        public static Vector2 ConvertToXY(Vector2 v)
        {
            return new Vector2(v.X * (float)Math.Cos(v.Y), v.X * (float)Math.Sin(v.Y));
        }

        public static Vector2 ConvertToMA(Vector2 v)
        {
            return new Vector2(LocationManager.getDistance(Vector2.Zero, v), (float)Math.Atan2(v.Y, v.X));
        }
    }
}
