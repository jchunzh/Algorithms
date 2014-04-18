using System;
using System.Dynamic;
using System.Globalization;
using System.Net.NetworkInformation;

namespace Algorithms
{
    public class BinarySearchTree<T> : IBinarySearchTree<T> where T : IComparable
    {
        protected Node<T> RootNode { get; set; }
        public int Size { get; protected set; }

        public virtual void Insert(T value)
        {
            if (RootNode == null)
            {
                RootNode = new Node<T> {Left = null, Right = null, Parent = null, Value = value, Height = 1};
                Size++;
                return;
            }

            InsertHelper(value, RootNode);
            Size++;
        }

        private void InsertHelper(T value, Node<T> curNode)
        {
            curNode.Height++;
            //Left
            if (value.CompareTo(curNode.Value) <= 0)
            {
                if (curNode.Left == null)
                {
                    curNode.Left = new Node<T> {Left = null, Right = null, Parent = curNode, Value = value, Height = 1};
                    return;
                }

                InsertHelper(value, curNode.Left);
                return;
            }

            if (curNode.Right == null)
            {
                curNode.Right = new Node<T> {Left = null, Right = null, Parent = curNode, Value = value, Height = 1};
                return;
            }
            
            InsertHelper(value, curNode.Right);
        }

        /// <summary>
        /// Returns true if the value exists in the BST
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Find(T value)
        {
            if (value == null)
                throw new ArgumentNullException();
            return FindHelper(value, RootNode) != null;
        }

        protected Node<T> FindHelper(T value, Node<T> node)
        {
            if (node == null)
                return null;

            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                    return null;

                return FindHelper(value, node.Left);
            }

            if (value.CompareTo(node.Value) > 0)
            {
                if (node.Right == null)
                    return null;

                return FindHelper(value, node.Right);
            }

            //Equal
            return node;
        }

        public virtual bool Delete(T value)
        {
            Node<T> nodeToDelete = FindHelper(value, RootNode);
            if (nodeToDelete == null)
                return false;

            Size--;
            DeleteNode(nodeToDelete);

            return true;
        }

        private void DeleteNode(Node<T> node)
        {
            //Is leaf, just remove
            if (node.Left == null && node.Right == null)
            {
                if (node.Parent == null)
                    RootNode = null;
                else
                {
                    if (node.Parent.Left == node)
                    {
                        node.Parent.Left = null;
                    }
                    else if (node.Parent.Right == node)
                        node.Parent.Right = null;
                }

                return;
            }

            //No left tree, just replace node with right child
            if (node.Left == null && node.Right != null)
            {
                if (node.Parent == null)
                {
                    RootNode = node.Right;
                    RootNode.Parent = null;
                }
                else
                {
                    if (node.Parent.Left == node)
                    {
                        node.Parent.Left = node.Right;
                        node.Right.Parent = node.Parent;
                    }
                    else if (node.Parent.Right == node)
                    {
                        node.Parent.Right = node.Right;
                        node.Right.Parent = node.Parent;
                    }
                }

                return;
            }

            //No right tree, just replace node with left child
            if (node.Left != null && node.Right == null)
            {
                if (node.Parent == node)
                {
                    RootNode = node.Left;
                    RootNode.Parent = null;
                }
                else
                {
                    if (node.Parent.Left == node)
                    {
                        node.Parent.Left = node.Left;
                        node.Left.Parent = node.Parent;
                    }
                    else if (node.Parent.Right == node)
                    {
                        node.Parent.Right = node.Left;
                        node.Left.Parent = node.Parent;
                    }
                }

                return;
            }

            //Swap node to be deleted with it's successor node
            Node<T> successor = FindSuccessor(node);

            if (node.Left != null)
                node.Left.Parent = successor;
            if (node.Right != null)
                node.Right.Parent = successor;

            if (successor.Left != null)
                successor.Left.Parent = node;
            if (successor.Right != null)
                successor.Right.Parent = node;

            Node<T> tempParent = successor.Parent;
            Node<T> tempLeft = successor.Left;
            Node<T> tempRight = successor.Right;

            successor.Left = node.Left;
            successor.Right = node.Right;

            if (successor.Parent.Left == successor)
                successor.Parent.Left = node;
            else
                successor.Parent.Right = node;

            successor.Parent = node.Parent;

            if (node.Parent == null)
                RootNode = successor;
            else
            {
                if (node.Parent.Left == node)
                    node.Parent.Left = successor;
                else
                {
                    node.Parent.Right = successor;
                }
            }

            node.Parent = tempParent;
            node.Left = tempLeft;
            node.Right = tempRight;

            DeleteNode(node);
        }

        private Node<T> FindSuccessor(Node<T> node)
        {
            if (node.Left == null && node.Right == null)
                return node;

            //Get right subtree's left most value
            if (node.Left == null)
            {
                Node<T> curNode = node.Right;
                while (curNode.Left != null)
                {
                    curNode = curNode.Left;
                }

                return curNode;
            }
            else
            {
                //Get left subtree's right most value
                Node<T> curNode = node.Left;
                while (curNode.Right != null)
                {
                    curNode = curNode.Right;
                }

                return curNode;
            }
        }
        public T[] ToArray()
        {
            if (RootNode == null)
                return new T[0];

            Node<T> curNode = FindMin(RootNode);

            return Traverse(curNode, Size);
        }

        /// <summary>
        /// Traverses the tree in sorted order
        /// </summary>
        /// <param name="smallestNode"></param>
        /// <param name="nodeCount"></param>
        /// <returns></returns>
        private T[] Traverse(Node<T> smallestNode, int nodeCount)
        {
            T[] values = new T[nodeCount];
            Node<T> curNode = smallestNode;
            int index = 0;

            while (curNode != null)
            {
                values[index] = curNode.Value;
                curNode = NextLarger(curNode);
                index++;
            }

            return values;
        }

        /// <summary>
        /// Returns the next largest node compared to the current node or null if the current node is the largest.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node<T> NextLarger(Node<T> node)
        {
            if (node.Right != null)
                return FindMin(node.Right);

            Node<T> parentNode = node.Parent;
            Node<T> curNode = node;

            //Keep on traversing up the tree until the root is reached (parentNode == null)
            //or we find a parent for which this is the left subtree (parentNode.Right != curNode)
            while (parentNode != null && parentNode.Right == curNode)
            {
                curNode = parentNode;
                parentNode = parentNode.Parent;
            }

            return parentNode;
        }

        /// <summary>
        /// Returns the smallest child node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node<T> FindMin(Node<T> node)
        {
            if (node == null)
                return null;

            Node<T> curNode = node;

            while (curNode.Left != null)
            {
                curNode = curNode.Left;
            }

            return curNode;
        }
    }
}
