using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    class Pair<E,T>
    {
        public E first;
        public T second;
        public Pair():this(default(E), default(T))
        {
        }
        public Pair(E f, T s)
        {
            first = f;
            second = s;
        }
    }
    class Pair<E, E2, E3>
    {
        public E first;
        public E2 second;
        public E3 third;
        public Pair()
            : this(default(E), default(E2), default(E3))
        {
        }
        public Pair(E f, E2 s, E3 t)
        {
            first = f;
            second = s;
            third = t;
        }
    }
}
