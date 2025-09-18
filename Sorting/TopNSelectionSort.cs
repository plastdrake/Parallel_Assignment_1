using System;
using System.Collections.Generic;
using System.Linq;

namespace Sorting
{
    public class TopNSelectionSort<T> : TopNSortBase<T>
    {
        public override string Name { get { return "TopN SelectionSort"; } }

        public override T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            int m = inputOutput.Length;
            for (int i = 0; i < n; i++) {
                int min = i;
                for (int j = i + 1; j < m; j++) {
                    if (comparer.Compare(inputOutput[j], inputOutput[min]) < 0) {
                        T tmp = inputOutput[j];
                        inputOutput[j] = inputOutput[min];
                        inputOutput[min] = tmp;
                    }
                }
            }
            return inputOutput.Take<T>(n).ToArray();
        }
    }
}
