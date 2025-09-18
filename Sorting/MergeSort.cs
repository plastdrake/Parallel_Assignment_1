using System;
using System.Collections.Generic;

namespace Sorting
{
    public class MergeSort<T> : SortBase<T>
    {
        public override string Name { get { return "SequentialMergeSort"; } }

        public override void Sort(T[] inputOutput, IComparer<T> comparer)
        {
            SequentialMergeSort(inputOutput, comparer);
        }

        protected void SequentialMergeSort(T[] inputOutput, IComparer<T> comparer)
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
                SequentialMergeSort(inputOutput1, comparer);
                SequentialMergeSort(inputOutput2, comparer);
                Merge(inputOutput1, inputOutput2, inputOutput, comparer);
            }
        }

        protected void Merge(T[] inputOutput1, T[] inputOutput2, T[] inputOutput, IComparer<T> comparer)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            while (i < inputOutput1.Length && j < inputOutput2.Length) {
                if (comparer.Compare(inputOutput1[i], inputOutput2[j]) <= 0) {
                    inputOutput[k] = inputOutput1[i];
                    i++;
                } else {
                    inputOutput[k] = inputOutput2[j];
                    j++;
                }
                k++;
            }
            if (i == inputOutput1.Length) {
                for (; j < inputOutput2.Length; j++, k++) {
                    inputOutput[k] = inputOutput2[j];
                }
            } else {
                for (; i < inputOutput1.Length; i++, k++) {
                    inputOutput[k] = inputOutput1[i];
                }
            }
        }
    }
}
