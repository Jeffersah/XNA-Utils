using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    public class ComparerWeb<T>
    {
        List<WebNode<T>> MyNodes;
        IComparer<T> DefaultComparer;
        public ComparerWeb()
        {
            MyNodes = new List<WebNode<T>>();
        }
        public ComparerWeb(IComparer<T> Comparer)
        {
            MyNodes = new List<WebNode<T>>();
            DefaultComparer = Comparer;
        }

        public void Clear()
        {
            MyNodes.Clear();
        }

        public void AddNode(T input)
        {
            WebNode<T> Node = new WebNode<T>(input);
            foreach (WebNode<T> node in MyNodes)
            {
                int i = DefaultComparer.Compare(Node.getElement(), node.getElement());
                if (i > 0)
                    Node.AddChild(node);
                else if(i < 0)
                    node.AddChild(Node);
            }
            MyNodes.Add(Node);
        }

        public T GetParentless()
        {
            for (int i = 0; i < MyNodes.Count; i++)
            {
                if (MyNodes[i].ParentCount == 0)
                {
                    T tmp = MyNodes[i].getElement();
                    MyNodes[i].BreakTies();
                    MyNodes.RemoveAt(i);
                    return tmp;
                }
            }
            return default(T);
        }
        public T GetChildless()
        {
            for (int i = 0; i < MyNodes.Count; i++)
            {
                if (MyNodes[i].ChildCount == 0)
                {
                    T tmp = MyNodes[i].getElement();
                    MyNodes.RemoveAt(i);
                    return tmp;
                }
            }
            return default(T);
        }

        public int Count{get { return MyNodes.Count; }}
    }
    internal class WebNode<T>
    {
        List<WebNode<T>> Parents;
        List<WebNode<T>> Children;
        T Element;

        public int ParentCount
        {
            get
            {
                return Parents.Count;
            }
            set
            {
            }
        }
        public int ChildCount
        {
            get
            {
                return Children.Count;
            }
            set
            {
            }
        }
        public WebNode(T element)
        {
            Element = element;
            Parents = new List<WebNode<T>>();
            Children = new List<WebNode<T>>();
        }

        public T getElement()
        {
            return Element;
        }

        public void AddChild(WebNode<T> other)
        {
            other.Parents.Add(this);
            Children.Add(other);
        }
        public void BreakTies()
        {
            foreach (WebNode<T> t in Parents)
                t.Children.Remove(this);
            foreach (WebNode<T> t in Children)
                t.Parents.Remove(this);
            Parents.Clear();
            Children.Clear();
        }
    }
}
