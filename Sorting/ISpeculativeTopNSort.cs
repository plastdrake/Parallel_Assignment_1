using System.Collections.Generic;

using Utilities;

namespace Sorting
{
    public interface ISpeculativeTopNSort<T> : ICompute
    {
        bool[] SpeculativeTopNSort(T[] inputOutput, int n, ITopNSort<T>[] algorithms);

        bool[] SpeculativeTopNSort(T[] inputOutput, int n, IComparer<T> comparer, ITopNSort<T>[] algorithms);
    }
}
