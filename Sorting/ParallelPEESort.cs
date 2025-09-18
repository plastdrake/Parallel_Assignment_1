using System;
using System.Collections.Generic;
using System.Threading.Algorithms;

namespace Sorting
{
    public class ParallelPEESort<T> : ISort<T>
    {
        public string Name { get { return "ParallelPEESort"; } }

        public void Sort(T[] inputOutput)
        {
            ParallelAlgorithms.Sort(inputOutput);
        }

        public void Sort(T[] inputOutput, IComparer<T> comparer)
        {
            ParallelAlgorithms.Sort(inputOutput, comparer);
        }
    }
}
