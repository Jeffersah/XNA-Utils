using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian.Menus
{
    public class MenuScreen
    {
        protected List<I_MenuComponent> MenuComponents;

        public MenuScreen()
        {
            MenuComponents = new List<I_MenuComponent>();
        }

        public void update()
        {
            foreach (I_MenuComponent i in MenuComponents)
                i.update();
        }

        public void draw(SpriteBatch sb)
        {
            foreach (I_MenuComponent i in MenuComponents)
                i.draw(sb);
        }

        public void addComponent(I_MenuComponent menuComp)
        {
            MenuComponents.Add(menuComp);
        }
    }

    public interface I_MenuComponent
    {
        void update();

        void draw(SpriteBatch sb);
    }

    public class MC_TextBox : I_MenuComponent
    {
        Point pos;
        WrappedText text;
        Color c;
        SpriteFont sfont;
        float spacing;

        public MC_TextBox(string text, Rectangle position, SpriteFont font, Color color, float spacing)
        {
            this.text = new WrappedText(text, position.Width, font);
            sfont = font;
            c = color;
            pos = position.Location;
            this.spacing = spacing;
        }

        public void update()
        {
        }

        public void draw(SpriteBatch sb)
        {
            text.draw(sb, pos, 1, c);
        }
    }

    public class MC_Button : I_MenuComponent
    {
        public string Id;
        public Image mainImage;
        private Rectangle r;
        private Color tint;
        private OnClickFunction buttonFunction;

        public delegate void OnClickFunction(MC_ButtonClickEvent buttonEvent);

        public MC_Button(Image i, Rectangle location, Color color, OnClickFunction action, string id)
        {
            mainImage = i;
            r = location;
            tint = color;
            buttonFunction = action;
            Id = id;
        }
        public MC_Button(Image i, Rectangle location, Color color, OnClickFunction action) : this(i, location, color, action, "-Generic button-")
        {
        }

        public void update()
        {
            if (Cursor.leftPress)
            {
                if (r.Intersects(new Rectangle(Cursor.worldLoc().X, Cursor.worldLoc().Y, 1, 1)))
                {
                    buttonFunction(new MC_ButtonClickEvent(this));
                }
            }
        }

        public void draw(SpriteBatch sb)
        {
            Camera.draw(sb, mainImage, r, tint);
        }
    }

    public class MC_ButtonClickEvent
    {
        public MC_Button button;

        public MC_ButtonClickEvent(MC_Button inb)
        {
            button = inb;
        }
    }

    public class MC_Image : I_MenuComponent
    {
        public Image mainImage;
        private Rectangle r;
        private Color tint;

        public MC_Image(Image i, Rectangle location, Color color)
        {
            mainImage = i;
            r = location;
            tint = color;
        }

        public void update()
        {
        }

        public void draw(SpriteBatch sb)
        {
            Camera.draw(sb, mainImage, r, tint);
        }
    }
}