using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    class Toggleable<E>
    {
        E A;
        E B;
        bool isA;

        public Toggleable(E Current, E Next)
        {
            A = Current;
            B = Next;
            isA = true;
        }
        public Toggleable(E Current) : this(Current, default(E)) { }
        public Toggleable() : this(default(E), default(E)) { }

        public E Current
        {
            get
            {
                if (isA)
                    return A;
                else
                    return B;
            }
            set
            {
                if (isA)
                    A = value;
                else
                    B = value;
            }
        }
        public E Other
        {
            get
            {
                if (isA)
                    return B;
                else
                    return A;
            }
            set
            {
                if (isA)
                    B = value;
                else
                    A = value;
            }
        }

        public void Toggle()
        {
            isA = !isA;
        }
    }
}
