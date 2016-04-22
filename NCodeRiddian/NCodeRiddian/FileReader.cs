using System;
using System.IO;

namespace NCodeRiddian
{
    /// <summary>
    /// Class to assist in reading text files
    /// </summary>
    public class FileReader
    {
        private StreamReader read;

        /// <summary>
        /// Create a new file reader using the specified file
        /// </summary>
        /// <param name="fileloc">String for file location (including extension). Likely begins with "Content\\"</param>
        public FileReader(string fileloc)
        {
            try
            {
                read = new StreamReader(fileloc);
            }
            catch (DirectoryNotFoundException e)
            {
                try
                {
                    read = new StreamReader("Content/" + fileloc);
                }
                catch (DirectoryNotFoundException e2)
                {
                    throw e2;
                }
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Reads a single character from the filestream
        /// </summary>
        /// <returns>The next character</returns>
        public char readNextChar()
        {
            return (char)read.Read();
        }

        /// <summary>
        /// Returns a string of all text until the next whitespace, linebreak, or end of document
        /// </summary>
        /// <returns></returns>
        public string readNext()
        {
            string outString = "";
            if (read.Peek() < 0 || read.EndOfStream)
                return null;
            char nextChar = readNextChar();
            while (nextChar != ' ' && nextChar != '\n' && nextChar != '\r')
            {
                outString += nextChar;
                nextChar = readNextChar();
                if (read.EndOfStream)
                    return outString;
            }
            if (nextChar == '\n' || nextChar == '\r')
                readNextChar();
            return outString;
        }

        /// <summary>
        /// Reads one line of the file
        /// </summary>
        /// <returns></returns>
        public string readLine()
        {
            return read.ReadLine();
        }

        /// <summary>
        /// Returns FALSE when the filereader has reached the end of the file
        /// </summary>
        /// <returns></returns>
        public bool hasNext()
        {
            return !read.EndOfStream;
        }

        /// <summary>
        /// Reads a single int32 value from the file. Crashes if the next string from the file (to the next whitespace) is not parsable as an int
        /// </summary>
        /// <returns></returns>
        public int readInt()
        {
            return Int32.Parse(readNext());
        }

        /// <summary>
        /// Reads a double value from the file. Crashes if the next string from the file (to the next whitespace) is not parsable as a double
        /// </summary>
        /// <returns></returns>
        public double readDouble()
        {
            return Double.Parse(readNext());
        }

        /// <summary>
        /// Reads a single float value from the file. Crashes if the next string from the file (to the next whitespace) is not parsable as a float
        /// </summary>
        /// <returns></returns>
        public float readFloat()
        {
            return float.Parse(readNext());
        }
    }
}