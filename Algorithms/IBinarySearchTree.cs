using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public interface IBinarySearchTree<T> where T : IComparable
    {
        int Size { get; }
        void Insert(T value);
        bool Find(T value);
        bool Delete(T value);
        T[] ToArray();
    }
}
