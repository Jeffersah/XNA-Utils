using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCodeRiddian
{
    /// <summary>
    /// A Binary search tree used for searching algorithms
    /// </summary>
    /// <typeparam name="E">The type of the search tree</typeparam>
    public class BinarySearchTree <E> : IEnumerable<E>
    {
        List<TreeNode<E>> tree;
        Comparer<E> DefaultComparer;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DefaultComparer">The default method of comparing values</param>
        public BinarySearchTree(Comparer<E> DefaultComparer)
        {
            tree = new List<TreeNode<E>>();
            this.DefaultComparer = DefaultComparer;
        }

        public BinarySearchTree():this(Comparer<E>.Default)
        {
        }

        /// <summary>
        /// Change the default method of comparison
        /// </summary>
        /// <param name="NewComparer">The new comparer</param>
        public void SetDefaultComparer(Comparer<E> NewComparer)
        {
            DefaultComparer = NewComparer;
        }

        /// <summary>
        /// Returns the number of elements in the tree
        /// </summary>
        /// <returns>[int] number of elements in the tree</returns>
        public int Count()
        {
            return tree.Count;
        }

        /// <summary>
        /// Adds an item to the search tree
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <param name="comp">Comparer to use</param>
        public void Add(E item, Comparer<E> comp)
        {
            if (tree.Count == 0)
            {
                tree.Add(new TreeNode<E>(item, null));
            }
            else
                tree[0].AddNode(item, comp);
        }
        /// <summary>
        /// Adds an item to the search tree using the default comparer
        /// </summary>
        /// <param name="item">Item to add</param>
        public void Add(E item)
        {
            Add(item, DefaultComparer);
        }

        /// <summary>
        /// Finds an element matching the compareTo element using the specified comparer
        /// </summary>
        /// <param name="compareTo">The element to compare to</param>
        /// <param name="compare">The comparer</param>
        /// <returns></returns>
        public E Find(E compareTo, Comparer<E> compare)
        {
            return tree[0].SearchFor(compareTo, compare);
        }

        /// <summary>
        /// Returns a sorted (least-greatest) array of all elements
        /// </summary>
        /// <returns>Least-greatest sorted array</returns>
        public List<E> SortedArray()
        {
            return tree[0].GetSortedList(new List<E>());
        }

        public IEnumerator<E> GetEnumerator()
        {
            return (IEnumerator<E>)tree.Take(tree.Count).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return tree.GetEnumerator();
        }
    }
    /// <summary>
    /// Node in the binary search tree
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public class TreeNode<E>
    {
        E myNode;
        int Count;
        TreeNode<E> Parent;
        TreeNode<E> LeftChild;
        TreeNode<E> RightChild;

        public static implicit operator E(TreeNode<E> tn)
        {
            return tn.GetElement();
        }

        public TreeNode(E elem, TreeNode<E> parent)
        {
            myNode = elem;
            LeftChild = null;
            RightChild = null;
            Parent = parent;
            Count = 0;
        }

        public E GetElement()
        {
            return myNode;
        }

        public List<E> GetSortedList(List<E> cur)
        {
            if (LeftChild != null)
                LeftChild.GetSortedList(cur);
            cur.Add(GetElement());
            if (RightChild != null)
                RightChild.GetSortedList(cur);
            return cur;
        }

        public void AddNode(E n, Comparer<E> comp)
        {
            Count++;
            if (comp.Compare(myNode, n) < 0)
            {
                if (RightChild == null)
                    RightChild = new TreeNode<E>(n, this);
                else
                    RightChild.AddNode(n, comp);
            }
            else
            {
                if (LeftChild == null)
                    LeftChild = new TreeNode<E>(n, this);
                else
                    LeftChild.AddNode(n, comp);
            }
        }

        public TreeNode<E> SearchFor(E n, Comparer<E> comp)
        {
            var v = comp.Compare(myNode, n);
            if (v == 0)
                return this;
            if (v < 0)
            {
                if (RightChild == null)
                    return null;
                return RightChild.SearchFor(n, comp);
            }
            else
            {
                if (LeftChild == null)
                    return null;
                return LeftChild.SearchFor(n, comp);
            }
        }

        private void TreeUpDecrimentCount()
        {
            Count--;
            if (Parent != null)
                Parent.TreeUpDecrimentCount();
        }

    }
}
