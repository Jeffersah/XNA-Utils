using System;
using System.Collections.Generic;

namespace NCodeRiddian
{
    abstract public class Pather
    {
        private static List<PathNode> closedList = new List<PathNode>();

        public static Path GeneratePath(Pathable s, Pathable endPoint)
        {
            closedList.Clear();
            OpenHeap.openHeap.Clear();

            PathNode start = new PathNode(s, endPoint);
            start.setF(0);
            closedList.Add(start);

            foreach (Pathable s2 in start.element.getConnected())
            {
                OpenHeap.addToHeap(new PathNode(s2, endPoint, start));
            }

            bool done = false;
            while (!done)
            {
                if (OpenHeap.openHeap.Count == 0)
                {
                    return null;
                }

                PathNode next = OpenHeap.removeFirst();

                if (next.element == endPoint)
                {
                    return next.generatePath();
                }

                closedList.Add(next);
                foreach (Pathable s2 in next.element.getConnected())
                {
                    PathNode pnext = new PathNode(s2, endPoint, next);

                    if (!closedList.Contains(pnext))
                    {
                        if (!OpenHeap.contains(pnext))
                        {
                            OpenHeap.addToHeap(pnext);
                        }
                        else
                        {
                            if (OpenHeap.openHeap[OpenHeap.openHeap.IndexOf(pnext)].F > pnext.F)
                            {
                                OpenHeap.openHeap[OpenHeap.openHeap.IndexOf(pnext)].parentNode = next;
                                OpenHeap.openHeap[OpenHeap.openHeap.IndexOf(pnext)].setF(next);
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }

            return null;
        }
    }

    internal class OpenHeap
    {
        public static List<PathNode> openHeap = new List<PathNode>();

        public static bool contains(PathNode pn)
        {
            return openHeap.Contains(pn);
        }

        public static PathNode removeFirst()
        {
            if (openHeap.Count != 0)
            {
                PathNode toRemove = openHeap[0];
                openHeap.RemoveAt(0);
                if (openHeap.Count != 0)
                {
                    openHeap.Insert(0, openHeap[openHeap.Count - 1]);
                    openHeap.RemoveAt(openHeap.Count - 1);
                    sortHeap();
                }

                return toRemove;
            }
            return null;
        }

        public static void addToHeap(PathNode add)
        {
            openHeap.Add(add);
            sortDown(openHeap.Count - 1);
        }

        public static void sortDown(int i)
        {
            bool done = false;
            if (openHeap.Count < 2)
                return;
            while (!done)
            {
                if (i != 0 && openHeap[i].G < openHeap[H_getParent(i)].G)
                {
                    swap(i, H_getParent(i));
                    i = H_getParent(i);
                }
                else
                    done = true;
            }
        }

        public static void sortHeap()
        {
            bool done = false;
            int t1 = 0;
            while (!done)
            {
                if (H_getTeir(t1) == H_getTeir(openHeap.Count - 1))
                    return;

                PathNode child1 = null;
                PathNode child2 = null; ;
                if (H_getChild(t1, false) < openHeap.Count)
                {
                    child2 = openHeap[H_getChild(t1, false)];
                    child1 = openHeap[H_getChild(t1, true)];
                }
                else if (H_getChild(t1, true) < openHeap.Count)
                {
                    child1 = openHeap[H_getChild(t1, true)];
                }

                if (child1 == null)
                {
                    done = true;
                }
                else if (child2 == null)
                {
                    done = true;
                    if (child1.G > openHeap[t1].G)
                        swap(H_getChild(t1, true), t1);
                }
                else
                {
                    bool ChildOneLess = child1.G < openHeap[t1].G;
                    bool ChildTwoLess = child2.G < openHeap[t1].G;

                    if (ChildOneLess && ChildTwoLess)
                    {
                        if (child1.G < child2.G)
                        {
                            swap(H_getChild(t1, true), t1);
                            t1 = H_getChild(t1, true);
                        }
                        else
                        {
                            swap(H_getChild(t1, false), t1);
                            t1 = H_getChild(t1, false);
                        }
                    }
                    else if (ChildOneLess)
                    {
                        swap(H_getChild(t1, true), t1);
                        t1 = H_getChild(t1, true);
                    }
                    else if (ChildTwoLess)
                    {
                        swap(H_getChild(t1, false), t1);
                        t1 = H_getChild(t1, false);
                    }
                    else
                    {
                        done = true;
                    }
                }
            }
        }

        public static void swap(int idx1, int idx2)
        {
            PathNode temp = openHeap[idx1];
            openHeap[idx1] = openHeap[idx2];
            openHeap[idx2] = temp;
        }

        public static int H_getTeir(int idx)
        {
            return (int)(Math.Log((idx % 2 == 0 ? 0 : 1) + idx + 1) / Math.Log(2));
        }

        public static int H_getTeirIdx(int idx)
        {
            return (int)(idx - ((int)Math.Pow(2, H_getTeir(idx)))) + 1;
        }

        public static int H_getTotalIdx(int idx, int tier)
        {
            return (int)(Math.Pow(2, tier) + idx);
        }

        public static int H_getChild(int idx, bool left)
        {
            return (int)(idx * 2) + (left ? 1 : 2);
        }

        public static int H_getParent(int idx)
        {
            return H_getTotalIdx(H_getTeirIdx(idx) / 2, H_getTeir(idx) - 1) - 1;
        }
    }

    internal class PathNode
    {
        public Pathable element;
        public PathNode parentNode;
        public float F; //Pathed score
        public float H; //Heuristic Score
        public float G; //Global Score

        public override bool Equals(object obj)
        {
            return obj is PathNode && ((PathNode)obj).element.Equals(element);
        }

        public PathNode(Pathable s, Pathable hub)
        {
            parentNode = null;
            element = s;
            H = s.getHeuristicScore(hub);
        }

        public PathNode(Pathable s, Pathable hub, PathNode parent)
        {
            parentNode = parent;
            element = s;
            H = s.getHeuristicScore(hub);

            setF(parent.F + s.getMoveScore(parentNode.element));
        }

        public void setF(PathNode pnode)
        {
            setF(pnode.F + pnode.element.getMoveScore(element));
        }

        public void setF(float f)
        {
            F = f;
            G = H + F;
        }

        public Path generatePath()
        {
            return generatePathHelper(new Path()).reverse();
        }

        public Path generatePathHelper(Path p)
        {
            p.Add(element);
            if (parentNode == null)
            {
                return p;
            }
            else
            {
                return parentNode.generatePathHelper(p);
            }
        }
    }
}