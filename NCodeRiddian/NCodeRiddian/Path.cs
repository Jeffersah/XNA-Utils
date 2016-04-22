using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    public abstract class A_Path<T>
    {
        protected List<T> path;

        public A_Path()
        {
            path = new List<T>();
        }

        public A_Path(List<T> use)
        {
            path = use;
        }

        public List<T> GetPath()
        {
            return path;
        }

        public T GetLast()
        {
            if (path.Count <= 0)
                throw new IndexOutOfRangeException("Path is Empty - Can't get Last");
            return path[path.Count - 1];
        }

        public virtual T Get(int i)
        {
            if (i < 0 || i > path.Count)
                throw new IndexOutOfRangeException(i + " is out of range of path (length:" + path.Count + ")");
            return path[i];
        }

        public virtual T GetNext(T item)
        {
            int i = path.IndexOf(item) + 1;
            if (i == 0)
                throw new IndexOutOfRangeException(item + " not contained in path");
            if (i == path.Count)
                return path[path.Count - 1];
            return path[i];
        }

        public virtual T GetNext(int i)
        {
            i++;
            if (i <= 0)
                throw new IndexOutOfRangeException(i + " Out of range of path");
            if (i == path.Count)
                return path[path.Count - 1];
            return path[i];
        }

        public virtual void Add(T item)
        {
            path.Add(item);
        }

        public abstract bool isValid(T t);

        public bool isValid(int i)
        {
            return isValid(path[i]);
        }

        public bool VerifyPath()
        {
            foreach (T t in path)
                if (!isValid(t))
                    return false;
            return true;
        }

        public virtual List<T> ReversePath()
        {
            List<T> pth = new List<T>();
            for (int x = path.Count - 1; x >= 0; x--)
                pth.Add(path[x]);
            return pth;
        }
    }

    public class Path : A_Path<Pathable>
    {
        public Rectangle maxRec;

        public Path()
            : base()
        {
            path = new List<Pathable>();
        }

        public Path(Pathable s)
            : base()
        {
            path = new List<Pathable>();
            path.Add(s);
        }

        public Path(Path p, Pathable add)
            : base()
        {
            path = new List<Pathable>();

            foreach (Pathable s in p.path)
                Add(s);
            if (add != null)
            {
                Add(add);
            }
        }

        public override void Add(Pathable s)
        {
            if (maxRec.Width == 0 && maxRec.Height == 0)
            {
                maxRec = new Rectangle(((Pathable)((Pathable)s)).getTrueRectangle().X, ((Pathable)s).getTrueRectangle().Y, ((Pathable)s).getTrueRectangle().Width, ((Pathable)s).getTrueRectangle().Height);
            }
            path.Add(s);
            if (((Pathable)s).getTrueRectangle().X < maxRec.X)
            {
                maxRec.Width += maxRec.X - ((Pathable)s).getTrueRectangle().X;
                maxRec.X = ((Pathable)s).getTrueRectangle().X;
            }
            if (((Pathable)s).getTrueRectangle().Y < maxRec.Y)
            {
                maxRec.Height += maxRec.Y - ((Pathable)s).getTrueRectangle().Y;
                maxRec.Y = ((Pathable)s).getTrueRectangle().Y;
            }
            if (((Pathable)s).getTrueRectangle().X > maxRec.X + maxRec.Width)
            {
                maxRec.Width = ((Pathable)s).getTrueRectangle().X - maxRec.X;
            }
            if (((Pathable)s).getTrueRectangle().Y > maxRec.Y + maxRec.Height)
            {
                maxRec.Height = ((Pathable)s).getTrueRectangle().Y - maxRec.Y;
            }
        }

        public Path reverse()
        {
            Path newPath = new Path();
            for (int x = path.Count - 1; x >= 0; x--)
                newPath.Add(path[x]);

            return newPath;
        }

        public static int S_COMPAIR(Path p1, Path p2)
        {
            return p1.path.Count - p2.path.Count;
        }

        public bool verifyPath()
        {
            foreach (Pathable s in path)
            {
                if (((Pathable)s).isValid())
                {
                    path.Clear();
                    return false;
                }
            }

            return true;
        }

        public bool contain(Pathable s)
        {
            return path.Contains(s);
        }

        public Pathable getLast()
        {
            if (path.Count > 0)
                return path[path.Count - 1];
            else
                return default(Pathable);
        }

        public Pathable getFirst()
        {
            if (path.Count > 0)
                return path[0];
            else
                return default(Pathable);
        }

        public Pathable getNext(Pathable current)
        {
            if (current == null)
                return null;
            for (int x = 0; x < path.Count - 1; x++)
                if (current.Equals(path[x]))
                    return path[x + 1];

            return default(Pathable);
        }

        public override string ToString()
        {
            return "Path-" + path.Count;
        }

        public override bool isValid(Pathable t)
        {
            return t.isValid();
        }
    }
}