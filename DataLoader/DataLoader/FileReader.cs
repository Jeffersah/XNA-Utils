using System;
using System.IO;

namespace DataLoader
{
    public class FileReader
    {
        private StreamReader read;

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

        public char readNextChar()
        {
            return (char)read.Read();
        }

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

        public string readLine()
        {
            return read.ReadLine();
        }

        public bool hasNext()
        {
            return !read.EndOfStream;
        }

        public int readInt()
        {
            return Int32.Parse(readNext());
        }

        public double readDouble()
        {
            return Double.Parse(readNext());
        }

        public float readFloat()
        {
            return float.Parse(readNext());
        }
    }
}