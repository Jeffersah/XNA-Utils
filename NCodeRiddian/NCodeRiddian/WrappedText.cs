using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    /// <summary>
    /// Formats text to wrap on screen
    /// </summary>
    public class WrappedText
    {
        private List<string> lines;
        private SpriteFont sf;
        private string orig;
        private int wid;

        public WrappedText(string str, int width, SpriteFont font)
        {
            lines = new List<string>();
            orig = str;
            wid = width;
            sf = font;
            generate(str);
        }

        /// <summary>
        /// Changes the full width of the block of text
        /// </summary>
        /// <param name="wid"></param>
        public void changeSize(int wid)
        {
            lines.Clear();
            this.wid = wid;
            generate();
        }

        /// <summary>
        /// Alters the text
        /// </summary>
        /// <param name="str"></param>
        public void changeString(string str)
        {
            lines.Clear();
            orig = str;
            generate(str);
        }

        public int getWidth()
        {
            return wid;
        }

        public float getHeight(float spacing)
        {
            return lines.Count * sf.MeasureString(lines[0]).Y * spacing;
        }

        private void generate(string str)
        {
            string[] words = str.Split(' ');
            string curString = words[0];
            for (int i = 1; i < words.Length; i++)
            {
                if (sf.MeasureString(curString + " " + words[i]).X > wid)
                {
                    lines.Add(curString);
                    curString = words[i];
                }
                else
                    curString += " " + words[i];
            }
            if (curString.Equals(""))
            { }
            else
                lines.Add(curString);
        }

        private void generate()
        {
            generate(orig);
        }

        public void draw(SpriteBatch sb, Point p, float spacing, Color color)
        {
            if (lines.Count == 0)
                return;
            spacing *= (float)sf.MeasureString(lines[0]).Y;
            for (int i = 0; i < lines.Count; i++)
            {
                sb.DrawString(sf, lines[i], new Vector2(p.X, p.Y + (spacing * i)), color);
            }
        }

        public void draw(SpriteBatch sb, Vector2 v, float spacing, Color color)
        {
            if (lines.Count == 0)
                return;
            spacing *= (float)sf.MeasureString(lines[0]).Y;
            for (int i = 0; i < lines.Count; i++)
            {
                sb.DrawString(sf, lines[i], new Vector2(v.X, v.Y + (spacing * i)), color);
            }
        }

        public void CameraDraw(SpriteBatch sb, Vector2 v, float spacing, Color color)
        {
            if (lines.Count == 0)
                return;
            spacing *= (float)sf.MeasureString(lines[0]).Y;
            for (int i = 0; i < lines.Count; i++)
            {
                Camera.drawString(sb, sf, lines[i], Vector2.Add(v, MemSave.getv(0, (spacing * i))), color, 0, Vector2.Zero, SpriteEffects.None, 0);
            }
        }
    }
}