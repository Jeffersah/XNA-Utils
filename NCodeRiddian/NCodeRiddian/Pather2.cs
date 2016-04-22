using System.Collections.Generic;

namespace NCodeRiddian
{
    public class Pather2
    {
        public static Path GeneratePath(Pathable start, Pathable end)
        {
            OpenHeap2 openList = new OpenHeap2();
            List<PathNode> closedList = new List<PathNode>();
            Pathable next = start;
            PathNode thefirst = new PathNode(next, end);
            thefirst.setF(0);
            closedList.Add(thefirst);

            foreach (Pathable s2 in thefirst.element.getConnected())
            {
                openList.openHeap.Add(new PathNode(s2, end, thefirst));
            }

            while (openList.openHeap.Count > 0)
            {
                PathNode nextpathnode = openList.getFirst();
                closedList.Add(nextpathnode);

                if (nextpathnode.element.Equals( end ))
                {
                    return nextpathnode.generatePath();
                }

                foreach (Pathable s2 in nextpathnode.element.getConnected())
                {
                    PathNode pn = new PathNode(s2, end, nextpathnode);
                    if (closedList.Contains(pn)) { }
                    else if (openList.contains(pn))
                    {
                        int index = openList.openHeap.IndexOf(pn);
                        if (openList.openHeap[index].F > nextpathnode.F)
                        {
                            openList.openHeap[index].parentNode = nextpathnode;
                            openList.openHeap[index].setF(nextpathnode);
                        }
                        else { }
                    }
                    else openList.add(pn);
                }
            }

            return null;
        }
    }

    internal class OpenHeap2
    {
        public List<PathNode> openHeap;

        public OpenHeap2()
        {
            openHeap = new List<PathNode>();
        }

        public bool contains(PathNode pn)
        {
            return openHeap.Contains(pn);
        }

        public PathNode getFirst()
        {
            PathNode toreturn = openHeap[0];
            swap(0, openHeap.Count - 1);
            openHeap.RemoveAt(openHeap.Count - 1);
            sortDown();
            return toreturn;
        }

        public void add(PathNode item)
        {
            openHeap.Add(item);
            sortUp();
        }

        private void swap(int i, int i2)
        {
            PathNode temp = openHeap[i];
            openHeap[i] = openHeap[i2];
            openHeap[i2] = temp;
        }

        private void sortUp(int i)
        {
            PathNode current = openHeap[i];
            PathNode parent = getParent(i) >= 0 ? openHeap[getParent(i)] : null;
            if (parent == null || current.G <= parent.G)
                return;

            swap(i, getParent(i));
            sortUp(getParent(i));
        }

        private void sortUp()
        {
            sortUp(openHeap.Count - 1);
        }

        private void sortDown(int i)
        {
            int[] childIndecies = getChildren(i);

            if (childIndecies[0] >= openHeap.Count)
                return;

            PathNode[] children = new PathNode[] { openHeap[childIndecies[0]], childIndecies[1] < openHeap.Count ? openHeap[childIndecies[1]] : null };
            int greaterIndex;

            if (children[1] == null || children[0].G > children[1].G)
                greaterIndex = 0;
            else
                greaterIndex = 1;

            if (openHeap[i].G < children[greaterIndex].G)
            {
                swap(i, childIndecies[greaterIndex]);
                sortDown(childIndecies[greaterIndex]);
            }
        }

        private void sortDown()
        {
            sortDown(0);
        }

        private static int[] getChildren(int i)
        {
            return new int[] { (i + 1) * 2 - 1, ((i + 1) * 2) };
        }

        private static int getParent(int i)
        {
            return (int)(i / 2);
        }
    }
}