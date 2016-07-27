using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace NCodeRiddian
{
    /// <summary>
    /// Handles all content loading. Required for Images to work properly.
    /// </summary>
    public abstract class TextureManager
    {
        private static List<TextureManagerDefinition> definitions = new List<TextureManagerDefinition>();
        private static List<FontManagerDefinition> fontdefinitions = new List<FontManagerDefinition>();
        private static bool debug = false;
        private static Texture2D generic;

        /// <summary>
        /// Returns a single white pixel, if setupGeneric has been called.
        /// </summary>
        /// <returns></returns>
        public static Texture2D getGeneric()
        {
            return generic;
        }

        /// <summary>
        /// Creates a generic texture (a single white pixel) to draw rectangles
        /// </summary>
        /// <param name="gd"></param>
        public static void setupGeneric(GraphicsDevice gd)
        {
            generic = new Texture2D(gd, 1, 1);
            generic.SetData<Color>(new Color[] { Color.White });
        }

        /// <summary>
        /// If true, will print debugging information to the output
        /// </summary>
        /// <param name="debug"></param>
        public static void setDebug(bool debug)
        {
            TextureManager.debug = debug;
        }

        /// <summary>
        /// Recursively searches all directories for images and loads them as texturemanagerdefinitions
        /// </summary>
        /// <param name="cm"></param>
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

        /// <summary>
        /// Get a texture with a given file name, or null if none is found
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a font with a given file name, or null if none is found
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static SpriteFont getFont(string n)
        {
            int idx = fontdefinitions.FindIndex(x => x.name.Equals(n));
            if (idx == -1)
            {
                if (debug)
                {
                    Console.Out.WriteLine("Failure, No Definition for Name \"{0}\"", n);
                }
                return null;
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