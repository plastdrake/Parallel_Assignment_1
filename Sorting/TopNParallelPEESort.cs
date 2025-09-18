using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Algorithms;

namespace Sorting
{
    public class TopNParallelPEESort<T> : ITopNSort<T>
    {
        public string Name { get { return "TopN ParallelPEESort+Take(N)"; } }

        public T[] TopNSort(T[] inputOutput, int n)
        {
            ParallelAlgorithms.Sort(inputOutput);
            return inputOutput.Take(n).ToArray();
        }

        public T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            ParallelAlgorithms.Sort(inputOutput, comparer);
            return inputOutput.Take(n).ToArray();
        }
    }
}
