using System;
using System.Collections.Generic;

namespace Algorithms
{
    public class Heap : PriorityQueue<int>
    {
        public bool MaxHeap = true;
        public int[] Queue { get; private set; }

        public Heap(int[] array)
        {
            Queue = new int[array.Length];
            Array.Copy(array, Queue, array.Length);

            BuildMaxHeap(Queue);
        }

        public void Insert(int x)
        {
            int[] array = Queue;
            Array.Resize(ref array, Queue.Length + 1);
            array[array.Length - 1] = x;

            int curIndex = array.Length - 1;

            while (curIndex != 0)
            {
                curIndex = Parent(curIndex);
                MaxHeapify(array, curIndex);
            }

            Queue = array;
        }

        public int Max()
        {
            return Queue[0];
        }

        public int ExtractMax()
        {
            int[] array = Queue;
            int max = array[0];
            array[0] = array[array.Length - 1];
            Array.Resize(ref array, array.Length - 1);
            MaxHeapify(array, 0);
            Queue = array;

            return max;
        }

        public void IncreaseKey(int x, int k)
        {
            throw new NotImplementedException();
        }

        public static int[] HeapSort(int[] array)
        {
            int[] sortedArray = new int[array.Length];
            int[] copy = new int[array.Length];
            Array.Copy(array, copy, array.Length);
       
            BuildMaxHeap(copy);

            int endIndex = sortedArray.Length;

            for (int i = 0; i < sortedArray.Length; i++)
            {
                int max = ExtractMax(ref copy, endIndex);
                sortedArray[i] = max;
                endIndex--;
            }

            return sortedArray;
        }

        private static int ExtractMax(ref int[] array, int endIndex)
        {
            int max = array[0];
            array[0] = array[endIndex - 1];
            MaxHeapify(array, 0, endIndex);

            return max;
        }

        private static void BuildMaxHeap(int[] array)
        {
            for (int i = array.Length/2; i >= 0; i--)
            {
                MaxHeapify(array, i);
            }
        }

        private static int[] MaxHeapify(int[] array, int root, int endIndex = -1)
        {
            int left = Left(root);
            int right = Right(root);

            //Is leaf

            int arrayEnd = endIndex == -1 ? array.Length : endIndex;
            if (left >= arrayEnd && right >= arrayEnd)
                return array;

            int largestIndex = root;

            if (left < arrayEnd && array[root] < array[left])
            {
                largestIndex = left;
            }

            if (right < arrayEnd && array[largestIndex] < array[right])
            {
                largestIndex = right;
            }

            if (largestIndex == root)
                return array;
            
            int temp = array[root];
            array[root] = array[largestIndex];
            array[largestIndex] = temp;

            return MaxHeapify(array, largestIndex, endIndex);
        }

        /// <summary>
        /// Get left child index
        /// </summary>
        /// <returns></returns>
        private static int Left(int index)
        {
            return 2*(index + 1) - 1;
        }

        /// <summary>
        /// Get right child index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int Right(int index)
        {
            return 2 * (index + 1);
        }

        private int Parent(int index)
        {
            return (index - 1) / 2;
        }
    }
}
