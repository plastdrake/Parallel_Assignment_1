using System;
using System.Collections.Generic;
using System.Linq;

namespace Sorting
{
    public class TopNMergeSort<T> : TopNSortBase<T>
    {
        public override string Name { get { return "TopN SequentialMergeSort"; } }

        public override T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            SequentialTopNMergeSort(inputOutput, n, comparer);
            return inputOutput.Take<T>(n).ToArray();
        }

        protected void SequentialTopNMergeSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            if (inputOutput.Length > 1) {
                int halfN = inputOutput.Length / 2;
                T[] inputOutput1 = new T[halfN];
                T[] inputOutput2 = new T[inputOutput.Length - halfN];
                for (int i = 0; i < halfN; i++) {
                    inputOutput1[i] = inputOutput[i];
                }
                for (int i = 0; i < inputOutput2.Length; i++) {
                    inputOutput2[i] = inputOutput[halfN + i];
                }
                SequentialTopNMergeSort(inputOutput1, n, comparer);
                SequentialTopNMergeSort(inputOutput2, n, comparer);
                TopNMerge(inputOutput1, inputOutput2, inputOutput, n, comparer);
            }
        }

        protected void TopNMerge(T[] inputOutput1, T[] inputOutput2, T[] inputOutput, int n, IComparer<T> comparer)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            while (k < n && i < inputOutput1.Length && j < inputOutput2.Length) {
                if (comparer.Compare(inputOutput1[i], inputOutput2[j]) <= 0) {
                    inputOutput[k] = inputOutput1[i];
                    i++;
                } else {
                    inputOutput[k] = inputOutput2[j];
                    j++;
                }
                k++;
            }
            if (k < n && i == inputOutput1.Length) {
                for (; k < n && j < inputOutput2.Length; j++, k++) {
                    inputOutput[k] = inputOutput2[j];
                }
            } else if (k < n) {
                for (; k < n && i < inputOutput1.Length; i++, k++) {
                    inputOutput[k] = inputOutput1[i];
                }
            }
        }
    }
}
