using System;
using System.Linq;
using Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlgorithmsTest
{
    [TestClass]
    public class HeapTests
    {
        [TestMethod]
        public void BuildMaxHeap_AlreadyCorrectHeap()
        {
            int[] array = { 16, 14, 10, 8, 7, 9, 3};       

            Heap heap = new Heap(array);

            Assert.IsTrue(IsMaxHeap(heap.Queue, 0));
        }

        /// <summary>
        /// 2^n elements
        /// </summary>
        [TestMethod]
        public void BuildMaxHeap_ScrambledArray_Balance()
        {
            int[] array = { 300, 15, 205, 25, 457, 3, 5 };

            Heap heap = new Heap(array);

            Assert.IsTrue(IsMaxHeap(heap.Queue, 0));
        }

        /// <summary>
        /// Not 2^n elements
        /// </summary>
        [TestMethod]
        public void BuildMaxHeap_ScrambledArray_Unbalanced()
        {
            int[] array = { 39, 58, 31, 56, 38, 70, 43, 69, 8, 2, 5, 6, 9 };

            Heap heap = new Heap(array);

            Assert.IsTrue(IsMaxHeap(heap.Queue, 0));
        }

        [TestMethod]
        public void BuildMaxHeap_AllSameNumber()
        {
            int[] array = {5, 5, 5, 5, 5, 5};
            Heap heap = new Heap(array);

            Assert.IsTrue(IsMaxHeap(heap.Queue, 0));
        }

        [TestMethod]
        public void Max()
        {
            int[] array = { 16, 14, 10, 8, 7, 9, 3 };
            int expected = 16;
            Heap heap = new Heap(array);
            int actual = heap.Max();

            Assert.AreEqual(expected, actual);
            CollectionAssert.AreEqual(array, heap.Queue);
        }

        [TestMethod]
        public void ExtractMax()
        {
            int[] expectArray = { 14, 10, 8, 7, 9, 3 };
            int[] array = { 16, 14, 10, 8, 7, 9, 3 };
            int expected = 16;
            Heap heap = new Heap(array);
            int actual = heap.ExtractMax();

            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectArray.Length, heap.Queue.Length);
            Assert.IsTrue(IsMaxHeap(heap.Queue, 0));
        }

        [TestMethod]
        public void Insert()
        {
            int[] array = {16, 14, 10, 8, 7, 9, 3};
            Heap heap = new Heap(array);
            int insert = 11;
            heap.Insert(insert);

            Assert.AreEqual(array.Length + 1, heap.Queue.Length);
            Assert.IsTrue(heap.Queue.Contains(insert));
            Assert.IsTrue(IsMaxHeap(heap.Queue, 0));
        }

        [TestMethod]
        public void HeapSort()
        {
            int Min = 0;
            int Max = 20;
            Random randNum = new Random(0);
            int[] actualArray = Enumerable
                .Repeat(0, 10)
                .Select(i => randNum.Next(Min, Max))
                .ToArray();

            int[] sortedArray = actualArray.OrderBy(s => s).Reverse().ToArray();
            
            actualArray = Heap.HeapSort(actualArray);

            CollectionAssert.AreEqual(sortedArray, actualArray);
        }

        [TestMethod]
        public void HeapSort_LargeArray()
        {
            Random randNum = new Random(0);
            int[] actualArray = Enumerable
                .Repeat(0, 1000)
                .Select(i => randNum.Next(0, int.MaxValue))
                .ToArray();

            int[] sortedArray = actualArray.OrderBy(s => s).Reverse().ToArray();

            actualArray = Heap.HeapSort(actualArray);

            CollectionAssert.AreEqual(sortedArray, actualArray);
        }

        private bool IsMaxHeap(int[] array, int root)
        {
            //Is leaf
            if (Left(root) >= array.Length && Right(root) >= array.Length)
                return true;

            if (Left(root) < array.Length && array[root] < array[Left(root)])
                return false;
            if (Right(root) < array.Length && array[root] < array[Right(root)])
                return false;

            return IsMaxHeap(array, Left(root)) && IsMaxHeap(array, Right(root));
        }

        private int Left(int index)
        {
            return 2 * (index + 1) - 1;
        }

        private int Right(int index)
        {
            return 2 * (index + 1);
        }
    }
}
