using System;
using System.Collections.Generic;
using System.Threading.Algorithms;

using Utilities;

namespace Sorting
{
    public class SpeculativeTopNParallelPEESort<T> : ISpeculativeTopNSort<T>
    {
        public string Name { get { return "PEE SpeculativeTopNSort"; } }

        public bool[] SpeculativeTopNSort(T[] inputOutput, int n, ITopNSort<T>[] algorithms)
        {
            return SpeculativeTopNSort(inputOutput, n,
                                       Comparer<T>.Default, algorithms);
        }

        public bool[] SpeculativeTopNSort(T[] inputOutput,
                                          int n, IComparer<T> comparer,
                                          ITopNSort<T>[] algorithms)
        {
            bool[] whoWon = new bool[algorithms.Length];
            // I cannot use the inputOutput directly since we have several algorithms active.
            T[][] inputOutputs = new T[algorithms.Length][];
            for (int a = 0; a < algorithms.Length; ++a) {
                inputOutputs[a] = new T[inputOutput.Length];
                inputOutput.CopyTo(inputOutputs[a], 0);
            }
            bool done = false;
            ParallelAlgorithms.SpeculativeFor<T[]>(0, algorithms.Length, i => {
                T[] r = algorithms[i].TopNSort(inputOutputs[i], n, comparer);
                if (!done) {
                    whoWon[i] = true;
                    done = true;
                }
                return r;
            });
            return whoWon;
        }
    }
}
