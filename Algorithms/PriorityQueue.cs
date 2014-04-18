namespace Algorithms
{
    public interface PriorityQueue<T>
    {
        T[] Queue { get; }
        void Insert(T x);
        T Max();
        T ExtractMax();
        void IncreaseKey(T x, T k);
    }
}
