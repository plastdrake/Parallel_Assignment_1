using System;
using System.Collections.Generic;

namespace Sorting
{
    public abstract class SortBase<T> : ISort<T>
    {
        public abstract string Name { get; }

        public void Sort(T[] inputOutput)
        {
            Sort(inputOutput, Comparer<T>.Default);
        }

        public abstract void Sort(T[] inputOutput, IComparer<T> comparer);
    }
}
