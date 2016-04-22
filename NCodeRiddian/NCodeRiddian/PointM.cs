using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public abstract class PointM
    {
        public static Point Add(Point p1, int x, int y)
        {
            return new Point(p1.X + x, p1.Y + y);
        }

        public static int Sum(Point p1)
        {
            return p1.X + p1.Y;
        }

        public static void Add(ref Point p1, int x, int y)
        {
            p1.X += x;
            p1.Y += y;
        }

        public static Point Multiply(Point p1, int x, int y)
        {
            return new Point(p1.X * x, p1.Y * y);
        }

        public static void Multiply(ref Point p1, int x, int y)
        {
            p1.X *= x;
            p1.Y *= y;
        }

        public static Point Divide(Point p1, int x, int y)
        {
            return new Point(p1.X / x, p1.Y / y);
        }

        public static void Divide(ref Point p1, int x, int y)
        {
            p1.X /= x;
            p1.Y /= y;
        }

        public static bool Assure(Point p, int lowx, int highx, int lowy, int highy)
        {
            return p.X >= lowx && p.X < highx && p.Y >= lowy && p.Y < highy;
        }
    }
}