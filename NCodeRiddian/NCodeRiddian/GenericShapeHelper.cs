using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public abstract class GenericShapeHelper
    {
        public static void DrawX(Vector2 Position, float width, SpriteBatch sb, Color c)
        {
            Camera.drawLineGeneric(new Vector2(Position.X - width, Position.Y - width), new Vector2(Position.X + width, Position.Y + width), sb, c);
            Camera.drawLineGeneric(new Vector2(Position.X + width, Position.Y - width), new Vector2(Position.X - width, Position.Y + width), sb, c);
        }
        public static void DrawCross(Vector2 Position, float width, SpriteBatch sb, Color c)
        {
            Camera.drawLineGeneric(new Vector2(Position.X, Position.Y - width), new Vector2(Position.X, Position.Y + width), sb, c);
            Camera.drawLineGeneric(new Vector2(Position.X + width,Position.Y), new Vector2(Position.X - width, Position.Y), sb, c);
        }
    }
}
