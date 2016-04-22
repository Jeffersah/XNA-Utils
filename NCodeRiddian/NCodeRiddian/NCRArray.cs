using System;

namespace NCodeRiddian
{
    public class NCRArray<E>
    {
        private E[] behindArray;
        private int[] dimensions;

        public NCRArray(int dimension)
        {
            behindArray = new E[dimension];
            dimensions = new int[] { dimension };
        }

        public NCRArray(params int[] dimension)
        {
            int totalLength = 1;
            foreach (int i in dimension)
                totalLength *= i;
            behindArray = new E[totalLength];
            dimensions = dimension;
        }

        private int getIdx(int[] pos)
        {
            int idx = 0;
            for (int x = 0; x < pos.Length; x++)
            {
                idx += pos[x] * (x - 1 == -1 ? 1 : dimensions[x - 1]);
            }
            return idx;
        }

        public E this[params int[] i]
        {
            get
            {
                try
                {
                    return behindArray[getIdx(i)];
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException("No index " + i, ex);
                }
            }
            set
            {
                try
                {
                    behindArray[getIdx(i)] = value;
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException("No index " + i, ex);
                }
            }
        }

        public int Length()
        {
            return dimensions[0];
        }

        public int Length(int dimension)
        {
            return dimensions[dimension];
        }

        private void eachhelper(int[] position, ADJ adjustmentFunction)
        {
            adjustmentFunction(behindArray[getIdx(position)], position);
            position[0]++;
            bool done = false;
            for (int x = 0; !done && x < position.Length; x++)
            {
                if (position[x] == dimensions[x])
                {
                    position[x] = 0;
                    if (x + 1 >= dimensions.Length)
                        return;
                    position[x + 1]++;
                    eachhelper(position, adjustmentFunction);
                }
            }
        }

        public void each(ADJ adjustmentFunction)
        {
            int[] pos = new int[dimensions.Length];
            for (int x = 0; x < pos.Length; x++)
                pos[x] = 0;
            eachhelper(pos, adjustmentFunction);
        }

        public void each(ADJ2 adjustmentFunction)
        {
            for (int x = 0; x < behindArray.Length; x++)
            {
                adjustmentFunction(behindArray[x]);
            }
        }

        public delegate void ADJ(E element, int[] index);

        public delegate void ADJ2(E element);
    }
}