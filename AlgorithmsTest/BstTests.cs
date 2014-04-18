using System;
using Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsTest
{
    [TestClass]
    public class BstTests
    {
        [TestMethod]
        public void Insert_OneValue()
        {
            IBinarySearchTree<int> tree = new BinarySearchTree<int>();
            tree.Insert(1);

            Assert.IsTrue(tree.Find(1));
        }

        [TestMethod]
        public void Insert_OneValue_ToArray()
        {
            IBinarySearchTree<int> tree = new BinarySearchTree<int>();
            tree.Insert(1);
            int[] sortedArray = tree.ToArray();

            Assert.IsTrue(tree.Find(1));
            Assert.AreEqual(1, sortedArray.Length);
            Assert.AreEqual(1, sortedArray[0]);
        }

        [TestMethod]
        public void Insert_SmallUnsortedArray()
        {
            int[] data = new[] {5, 2, 6, 3, 1, 4};
            int[] expected = {1, 2, 3, 4, 5, 6};
            IBinarySearchTree<int> tree = new BinarySearchTree<int>();

            foreach (int value in data)
                tree.Insert(value);

            int[] actualArray = tree.ToArray();
            CollectionAssert.AreEqual(expected, actualArray);
        }

        [TestMethod]
        public void Insert_SortedArray()
        {
            int[] data = {1, 2, 3, 4, 5, 6};
            int[] expected = {1, 2, 3, 4, 5, 6};
            IBinarySearchTree<int> tree = new BinarySearchTree<int>();

            foreach (int value in data)
            {
                tree.Insert(value);
            }

            int[] actual = tree.ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Insert_ReverseSortedArray()
        {
            int[] data = {6, 5, 4, 3, 2, 1};
            int[] expected = {1, 2, 3, 4, 5, 6};

            IBinarySearchTree<int> tree = new BinarySearchTree<int>();

            foreach (int value in data)
            {
                tree.Insert(value);
            }

            int[] actual = tree.ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Delete_Single()
        {
            const int insertValue = 1;

            IBinarySearchTree<int> tree = new BinarySearchTree<int>();
            tree.Insert(insertValue);

            Assert.IsTrue(tree.Find(insertValue));
            Assert.IsTrue(tree.Delete(insertValue));
            Assert.IsFalse(tree.Find(insertValue));
        }

        [TestMethod]
        public void Delete_SmallUnsortedArray()
        {
            int[] data = {5, 2, 6, 3, 1, 4};
            int[] expected = {1, 2, 3, 4, 5, 6};
            IBinarySearchTree<int> tree = new BinarySearchTree<int>();

            foreach (int value in data)
                tree.Insert(value);

            int[] actualArray = tree.ToArray();
            CollectionAssert.AreEqual(expected, actualArray);
            Assert.AreEqual(expected.Length, tree.Size);
            Assert.IsTrue(tree.Delete(3));
            Assert.IsFalse(tree.Find(3));
            Assert.AreEqual(expected.Length - 1, tree.Size);
        }

        [TestMethod]
        public void Delete_EntireArray()
        {
            int[] data = { 5, 2, 6, 3, 1, 4 };
            int[] expected = { 1, 2, 3, 4, 5, 6 };
            IBinarySearchTree<int> tree = new BinarySearchTree<int>();

            foreach (int value in data)
                tree.Insert(value);

            int[] actualArray = tree.ToArray();
            CollectionAssert.AreEqual(expected, actualArray);
            Assert.AreEqual(expected.Length, tree.Size);
            Assert.IsTrue(tree.Delete(1));
            Assert.IsFalse(tree.Find(1));
            Assert.IsTrue(tree.Delete(2));
            Assert.IsFalse(tree.Find(2));
            Assert.IsTrue(tree.Delete(3));
            Assert.IsFalse(tree.Find(3));
            Assert.IsTrue(tree.Delete(4));
            Assert.IsFalse(tree.Find(4));
            Assert.IsTrue(tree.Delete(5));
            Assert.IsFalse(tree.Find(5));
            Assert.IsTrue(tree.Delete(6));
            Assert.IsFalse(tree.Find(6));
            Assert.AreEqual(0, tree.Size);
        }

        [TestMethod]
        public void Delete_Unsorted_Check()
        {
            int[] data = { 5, 2, 6, 3, 1, 4 };
            int[] expected_sorted = { 1, 2, 3, 4, 5, 6 };
            int[] expected_delete3 = { 1, 2, 4, 5, 6 };
            int[] expected_delete2 = { 1, 4, 5, 6 };
            int[] expected_delete6 = { 1, 4, 5 };
            int[] expected_delete1 = { 4, 5 };

            IBinarySearchTree<int> tree = new BinarySearchTree<int>();

            foreach (int value in data)
                tree.Insert(value);

            int[] actualArray = tree.ToArray();

            CollectionAssert.AreEqual(expected_sorted, actualArray);

            Assert.IsTrue(tree.Delete(3));
            CollectionAssert.AreEqual(expected_delete3, tree.ToArray());

            Assert.IsTrue(tree.Delete(2));
            CollectionAssert.AreEqual(expected_delete2, tree.ToArray());

            Assert.IsTrue(tree.Delete(6));
            CollectionAssert.AreEqual(expected_delete6, tree.ToArray());

            Assert.IsTrue(tree.Delete(1));
            CollectionAssert.AreEqual(expected_delete1, tree.ToArray());
        }
    }
}
