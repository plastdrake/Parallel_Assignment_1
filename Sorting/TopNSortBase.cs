using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Utilities;

namespace Sorting
{
    public abstract class TopNSortBase<T> : ITopNSort<T>
    {
        public abstract string Name { get; }

        public T[] TopNSort(T[] inputOutput, int n)
        {
            return TopNSort(inputOutput, n, Comparer<T>.Default);
        }

        public abstract T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer);
    }
}
