using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    public abstract class TextureManager
    {
        private static List<TextureManagerDefinition> definitions = new List<TextureManagerDefinition>();
        private static List<FontManagerDefinition> fontdefinitions = new List<FontManagerDefinition>();
        private static bool debug = false;
        private static Texture2D generic;

        public static Texture2D getGeneric()
        {
            return generic;
        }

        public static void setupGeneric(GraphicsDevice gd)
        {
            generic = new Texture2D(gd, 1, 1);
            generic.SetData<Color>(new Color[] { Color.White });
        }

        public static void setDebug(bool debug)
        {
            TextureManager.debug = debug;
        }

        public static void loadAllImages(ContentManager cm)
        {
            foreach (string s in Directory.GetFiles("Content", "*", SearchOption.AllDirectories))
            {
                try
                {
                    definitions.Add(new TextureManagerDefinition(s, cm));
                    if (debug)
                    {
                        Console.WriteLine("NCodeRiddian.TextureManager Debug:DefinitionAdded:\"" + definitions[definitions.Count - 1].name + "\"");
                    }
                }
                catch (InvalidFileException e)
                {
                    try
                    {
                        fontdefinitions.Add(new FontManagerDefinition(s, cm));
                        if (debug)
                        {
                            Console.WriteLine("NCodeRiddian.TextureManager Debug: DefinitionAdded:\"" + fontdefinitions[fontdefinitions.Count - 1].name + "\"");
                        }
                    }
                    catch (InvalidFileException e2)
                    {
                    }
                }
            }
        }

        public static Texture2D getTexture(string n)
        {
            foreach (TextureManagerDefinition tmd in definitions)
            {
                if (tmd.name.Equals(n))
                    return tmd.image;
            }
            if (debug)
            {
                Console.Out.WriteLine("Failure, No Definition for Name \"{0}\"", n);
            }
            return null;
        }

        public static SpriteFont getFont(string n)
        {
            int idx = fontdefinitions.FindIndex(x => x.name.Equals(n));
            if (idx == -1)
            {
                if (debug)
                {
                    Console.Out.WriteLine("Failure, No Definition for Name \"{0}\"", n);
                }
            }
            return fontdefinitions[idx].font;
        }
    }

    internal class TextureManagerDefinition
    {
        public string name;
        public Texture2D image;

        public TextureManagerDefinition(string fileloc, ContentManager cm)
        {
            try
            {
                image = cm.Load<Texture2D>(fileloc.Substring(8).Split('.')[0]);
                name = fileloc.Substring(8).Split('.')[0];
            }
            catch (Exception e) { throw new InvalidFileException(); }
        }
    }

    internal class FontManagerDefinition
    {
        public string name;
        public SpriteFont font;

        public FontManagerDefinition(string fileloc, ContentManager cm)
        {
            try
            {
                font = cm.Load<SpriteFont>(fileloc.Substring(8).Split('.')[0]);
                name = fileloc.Substring(8).Split('.')[0];
            }
            catch (Exception e) { throw new InvalidFileException(); }
        }
    }

    internal class InvalidFileException : Exception
    {
    }
}