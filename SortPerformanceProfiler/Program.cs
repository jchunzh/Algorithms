using System;
using System.Diagnostics;
using System.Linq;
using Algorithms;

namespace SortPerformanceProfiler
{
    public class Program
    {
        static void Main(string[] args)
        {
            const int seed = 0;

            int[] sizes =
            {
                1000,
                10000,
                100000,
                1000000
            };

            foreach (var count in sizes)
            {
                Random randNum = new Random(seed);
                int[] random = Enumerable
                    .Repeat(0, count)
                    .Select(i => randNum.Next(0, int.MaxValue))
                    .ToArray();

                int[] randomCopy = new int[random.Length];
                random.CopyTo(randomCopy, 0);
                int[] randomHeapCopy = new int[random.Length];
                random.CopyTo(randomHeapCopy, 0);

                int[] sorted = random.OrderBy(s => s).ToArray();
                TimeSpan orderByTime = MeasureSort(s => s.OrderBy(t => t).ToArray(), random, sorted);
                TimeSpan sortTime = MeasureSort(s =>
                                                {
                                                    Array.Sort(s);
                                                    return s;
                                                }, randomCopy, sorted);
                TimeSpan heapTime = MeasureSort(Heap.HeapSort, random, sorted.Reverse().ToArray());
                TimeSpan inplaceHeapTime = MeasureSort(Heap.InplaceHeapSort, randomHeapCopy, sorted.Reverse().ToArray());
                TimeSpan bstTime = MeasureSort(s =>
                {
                    BinarySearchTree<int> tree = new BinarySearchTree<int>();

                    foreach (int val in s)
                    {
                        tree.Insert(val);
                    }

                    return tree.ToArray();
                }, random, sorted);
                TimeSpan avlTime = MeasureSort(s =>
                {
                    AvlTree<int> tree = new AvlTree<int>();

                    foreach (int val in s)
                        tree.Insert(val);

                    return tree.ToArray();
                }, random, sorted);

                Console.WriteLine("----- {0} -----", count);
                Console.WriteLine("Heap:               {0}", heapTime.TotalSeconds);
                Console.WriteLine("Inplace Heap:       {0}", inplaceHeapTime.TotalSeconds);
                Console.WriteLine("Binary search tree: {0}", bstTime.TotalSeconds);
                Console.WriteLine("AVL tree:           {0}", avlTime.TotalSeconds);
                Console.WriteLine("Sort:               {0}", sortTime.TotalSeconds);
                Console.WriteLine("OrderBy:            {0}", orderByTime.TotalSeconds);
            }
            
            Console.ReadLine();
        }

        static TimeSpan MeasureSort(Func<int[], int[]> sortFunction, int[] random, int[] expected)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int[] actual = sortFunction(random);
            stopwatch.Stop();

            if (!expected.SequenceEqual(actual))
            {
                for (int i = 0; i < expected.Length; i++)
                {
                    if (expected[i] != actual[i])
                        throw new NotImplementedException();
                }
            }
            return stopwatch.Elapsed;
        }
    }
}
