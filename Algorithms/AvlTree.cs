using System;
using System.Runtime.InteropServices;

namespace Algorithms
{
    public class AvlTree<T> : BinarySearchTree<T> where T: IComparable
    {
        public override void Insert(T value)
        {
            if (RootNode == null)
            {
                RootNode = new Node<T>{ Height = 1, Left = null, Right = null, Parent = null, Value = value};
                Size++;
                return;
            }

            InsertHelper(value, RootNode);
            //Balance(RootNode);
            Size++;
        }

        private void InsertHelper(T value, Node<T> node)
        {
            node.Height++;

            if (value.CompareTo(node.Value) > 0)
            {
                if (node.Right == null)
                {
                    Node<T> newNode = new Node<T> {Height = 1, Left = null, Right = null, Parent = node, Value = value};
                    node.Right = newNode;
                    return;
                }

                InsertHelper(value, node.Right);
            }
            else
            {
                if (node.Left == null)
                {
                    Node<T> newNode = new Node<T> {Height = 1, Left = null, Right = null, Parent = node, Value = value};
                    node.Left = newNode;
                    return;
                }

                InsertHelper(value, node.Left);
            }

            bool isRootNode = RootNode == node;
            Node<T> balancedNode = Balance(node);

            if (isRootNode)
                RootNode = balancedNode;
        }

        private Node<T> Balance(Node<T> node)
        {
            int leftHeight = node.Left == null ? 0 : node.Left.Height;
            int rightHeight = node.Right == null ? 0 : node.Right.Height;

            //left subtree is higher
            if (leftHeight - rightHeight > 2)
            {
                return RightRotate(node);
            }
            if (rightHeight - leftHeight > 2)
            {
                return LeftRotate(node);
            }

            return node;
        }

        /// <summary>
        /// Returns the new base node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node<T> LeftRotate(Node<T> node)
        {
            Node<T> rightChild = node.Right;

            //Make the right subtree's left child to be the current node's right subtree
            if (node.Right.Left != null)
                node.Right.Left.Parent = node;
            node.Right = node.Right.Left;

            //Replace node's place on the tree with its right child
            if (node.Parent != null)
            {
                if (node.Parent.Left == node)
                {
                    node.Parent.Left = rightChild;
                }
                else
                {
                    node.Parent.Right = rightChild;
                }
            }

            rightChild.Parent = node.Parent;
            //Make the node the left child of its former right child
            rightChild.Left = node;
            node.Parent = rightChild;

            node.Height = CalculateHeight(node);
            rightChild.Height = CalculateHeight(rightChild);

            return rightChild;
        }

        private Node<T> RightRotate(Node<T> node)
        {
            Node<T> leftChild = node.Left;

            if (node.Left.Right != null)
                node.Left.Right.Parent = node;
            node.Left = node.Left.Right;

            if (node.Parent != null)
            {
                if (node.Parent.Left == node)
                    node.Parent.Left = leftChild;
                else
                {
                    node.Parent.Right = leftChild;
                }
            }

            leftChild.Parent = node.Parent;
            leftChild.Right = node;
            node.Parent = leftChild;

            node.Height = CalculateHeight(node);
            leftChild.Height = CalculateHeight(leftChild);

            return leftChild;
        }

        private int CalculateHeight(Node<T> node)
        {
            int height = 1;

            if (node.Left != null)
                height += node.Left.Height;

            if (node.Right != null)
                height += node.Right.Height;

            return height;
        }

        public override bool Delete(T value)
        {
            Node<T> node = FindDeleteNode(value, RootNode);

            if (node == null)
                return false;

            DeleteHelper(node);

            Size--;

            return true;
        }

        /// <summary>
        /// Finds the node to delete and updates the heights along the way
        /// </summary>
        /// <param name="value"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        private Node<T> FindDeleteNode(T value, Node<T> node)
        {
            if (value.CompareTo(node.Value) == 0)
                return node;

            if (node.Left == null && node.Right == null)
                return null;

            Node<T> foundNode;

            if (value.CompareTo(node.Value) < 0)
            {
                if (node.Left == null)
                    return null;

                foundNode = FindDeleteNode(value, node.Left);

            }
            else
            {
                if (node.Right == null)
                    return null;

                foundNode = FindDeleteNode(value, node.Right);
            }


            if (foundNode != null)
                node.Height--;

            return foundNode;
        }

        private void DeleteHelper(Node<T> node)
        {
            //Leaf
            if (node.Left == null && node.Right == null)
            {
                if (node.Parent == null)
                {
                    RootNode = null;
                    return;
                }

                if (node.Parent.Left == node)
                    node.Parent.Left = null;
                else
                    node.Parent.Right = null;
                Node<T> returnNode = Balance(node.Parent);

                if (returnNode.Parent == null)
                    RootNode = returnNode;

                return;
            }

            if (node.Left == null && node.Right != null)
            {
                node.Right.Parent = node.Parent;
                //Right subtree is already balanced since we didn't remove anything from it
                if (node.Parent == null)
                {
                    RootNode = node.Right;
                    return;
                }
                
                if (node.Parent.Left == node)
                    node.Parent.Left = node.Right;
                else
                    node.Parent.Right = node.Right;

                Node<T> returnNode = Balance(node.Parent);

                if (returnNode.Parent == null)
                    RootNode = returnNode;

                return;
            }

            if (node.Left != null && node.Right == null)
            {
                node.Left.Parent = node.Parent;

                if (node.Parent == null)
                {
                    RootNode = node.Left;
                    return;
                }

                if (node.Parent.Left == node)
                    node.Parent.Left = node.Left;
                else
                    node.Parent.Right = node.Left;

                Node<T> returnNode = Balance(node.Parent);

                if (returnNode.Parent == null)
                    RootNode = returnNode;

                return;
            }

            Node<T> successorNode;
            //Find the next largest child and swap with current node, then delete node
            //Find successor in the tallest subtree
            if (node.Left.Height > node.Right.Height)
            {
                Node<T> curNode = node.Left;
                curNode.Height--;

                while (curNode.Right != null)
                {
                    curNode = curNode.Right;
                    curNode.Height--;
                }

                successorNode = curNode;
            }
            else
            {
                Node<T> curNode = node.Right;
                curNode.Height--;

                while (curNode.Left != null)
                {
                    curNode = curNode.Left;
                    curNode.Height--;
                }

                successorNode = curNode;
            }

            //Swap node with successor node
            //Have the successors parent point to node
            if (successorNode.Parent.Left == successorNode)
                successorNode.Parent.Left = node;
            else
                successorNode.Parent.Right = node;
            //Node children point to the successor
            node.Left.Parent = successorNode;           
            node.Right.Parent = successorNode;

            //Successor points to nodes children and parent
            Node<T> tempParent = successorNode.Parent;
            Node<T> tempLeft = successorNode.Left;
            Node<T> tempRight = successorNode.Right;
            successorNode.Parent = node.Parent;
            successorNode.Left = node.Left;
            successorNode.Right = node.Right;

            if (node.Parent == null)
                RootNode = successorNode;
            else
            {
                if (node.Parent.Left == node)
                    node.Parent.Left = successorNode;
                else
                    node.Parent.Right = successorNode;
            }

            node.Parent = tempParent;
            node.Left = tempLeft;
            node.Right = tempRight;
            if (tempLeft != null)
                tempLeft.Parent = node;
            if (tempRight != null)
                tempRight.Parent = node;

            node.Height = successorNode.Height;
            successorNode.Height = successorNode.Left.Height + successorNode.Right.Height + 1;

            DeleteHelper(node);
        }
    }
}
