using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    /// <summary>
    /// A simple class that allows acces to a global rectangle or vector to prevent new ones from being made where they aren't nessecary
    /// </summary>
    public abstract class MemSave
    {
        private static Rectangle save = new Rectangle(0, 0, 0, 0);
        private static Vector2 savev = new Vector2(0, 0);

        public static Vector2 getv(float x, float y)
        {
            savev.X = x;
            savev.Y = y;
            return savev;
        }

        public static Rectangle getr(int x, int y, int wid, int hei)
        {
            save.X = x;
            save.Y = y;
            save.Width = wid;
            save.Height = hei;
            return save;
        }
    }
}