using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NCodeRiddian
{
    /// <summary>
    /// Paths through a square array
    /// </summary>
    public abstract class ArrayPather
    {
        private class PathNode : IComparable
        {
            public float hscore;
            public float fscore;
            public PathNode parent;
            public Point cell;
            public bool closed;

            public PathNode(PathNode pt, Point c, Point end)
            {
                cell = c;
                parent = pt;
                hscore = PointM.Sum(PointM.Add(end, -c.X, -c.Y));
                closed = false;
            }

            public GenericPath GeneratePath()
            {
                GenericPath gp = new GenericPath();
                return _GeneratePath(gp);
            }

            public GenericPath _GeneratePath(GenericPath gp)
            {
                gp.Add(cell);
                if (parent != null)
                    return parent._GeneratePath(gp);
                return gp;
            }

            public int CompareTo(object obj)
            {
                PathNode other = (PathNode)obj;
                return (int)((hscore + fscore) - (other.hscore + other.fscore));
            }
        }

        public static Point[] increments = new Point[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1) };
        public static Point[] dincrements = new Point[] { new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1), new Point(1, 1), new Point(1, -1), new Point(-1, 1), new Point(-1, -1) };

        /// <summary>
        /// Generates a path through the array
        /// </summary>
        /// <typeparam name="E">Type of the array</typeparam>
        /// <param name="arr">The array</param>
        /// <param name="start">The starting position</param>
        /// <param name="end">The target position</param>
        /// <param name="isPathable">Delegate to check pathability using input (fromPoint, toPoint)</param>
        /// <param name="GetFScore">Delegate to get movement score using input (V1, V2, P1, P2)</param>
        /// <param name="diagonal">Boolean to determine ability to move diagonally</param>
        /// <returns>Returns the shortest path from start to end</returns>
        public static GenericPath GeneratePath<E>(E[,] arr, Point start, Point end, Func<Point, Point, bool> isPathable, Func<E, E, Point, Point, float> GetFScore, bool diagonal)
        {
            PathNode[,] nodes = new PathNode[arr.GetLength(0), arr.GetLength(1)];
            MinHeap<PathNode> openHeap = new MinHeap<PathNode>(Comparer<PathNode>.Default);
            nodes[start.X, start.Y] = new PathNode(null, start, end);
            nodes[start.X, start.Y].fscore = 0;
            openHeap.Add(nodes[start.X, start.Y]);
            bool done = false;
            do
            {
                PathNode current = openHeap.ExtractDominating();
                foreach (Point p in diagonal ? dincrements : increments)
                {
                    Point next = PointM.Add(current.cell, p.X, p.Y);
                    if (PointM.Assure(next, 0, arr.GetLength(0), 0, arr.GetLength(1)))
                    {
                        if (isPathable(current.cell, next))
                        {
                            float tfscore = current.fscore + GetFScore(arr[current.cell.X, current.cell.Y], arr[next.X, next.Y], current.cell, next);
                            if ((nodes[next.X, next.Y] != null && (!nodes[next.X, next.Y].closed && nodes[next.X, next.Y].fscore > tfscore)) || nodes[next.X, next.Y] == null)
                            {
                                nodes[next.X, next.Y] = new PathNode(current, next, end);
                                nodes[next.X, next.Y].fscore = tfscore;
                                openHeap.Add(nodes[next.X, next.Y]);
                                if (next.Equals(end))
                                    done = true;
                            }
                        }
                    }
                }
            } while (!done && openHeap.Count > 0);
            if (openHeap.Count != 0)
                return nodes[end.X, end.Y].GeneratePath();
            return null;
        }
    }

    /// <summary>
    /// A path of Points
    /// </summary>
    public class GenericPath : A_Path<Point>
    {
        public GenericPath()
            : base()
        {
        }

        public GenericPath(List<Point> Path)
            : base(Path)
        {
        }

        /// <summary>
        /// Return a reversed version of this path
        /// </summary>
        /// <returns>Reversed path</returns>
        public GenericPath Reverse()
        {
            List<Point> npath = new List<Point>();
            for (int x = path.Count - 1; x > 0; x--)
                npath.Add(path[x]);
            return new GenericPath(npath);
        }

        /// <summary>
        /// DO NOT USE -- Required override
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override bool isValid(Point t)
        {
            return true;
        }
    }
}