using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    public class ParallelMergeSort<T> : SortBase<T>
    {
        public override string Name => "Parallel Merge Sort";
        public override void Sort(T[] inputOutput, IComparer<T> comparer)
        {
            ParallelMergeSortRecursive(inputOutput, new T[inputOutput.Length], 0, inputOutput.Length - 1, comparer);
        }

        private static void ParallelMergeSortRecursive(T[] array, T[] tempArray, int leftStart, int rightEnd, IComparer<T> comparer)
        {
            if (leftStart >= rightEnd)
            {
                return;
            }

            int middle = (leftStart + rightEnd) / 2;
            // Threshold to avoid excessive parallelism
            if (rightEnd - leftStart < 2048)
            {
                ParallelMergeSortRecursive(array, tempArray, leftStart, middle, comparer);
                ParallelMergeSortRecursive(array, tempArray, middle + 1, rightEnd, comparer);
            }
            else
            {
                Parallel.Invoke(
                    () => ParallelMergeSortRecursive(array, tempArray, leftStart, middle, comparer),
                    () => ParallelMergeSortRecursive(array, tempArray, middle + 1, rightEnd, comparer)
                );
            }
            MergeHalves(array, tempArray, leftStart, rightEnd, comparer);
        }

        private static void MergeHalves(T[] array, T[] tempArray, int leftStart, int rightEnd, IComparer<T> comparer)
        {
            int leftEnd = (leftStart + rightEnd) / 2;
            int rightStart = leftEnd + 1;
            int size = rightEnd - leftStart + 1;
            int left = leftStart;
            int right = rightStart;
            int index = leftStart;
            while (left <= leftEnd && right <= rightEnd)
            {
                if (comparer.Compare(array[left], array[right]) <= 0)
                {
                    tempArray[index] = array[left];
                    left++;
                }
                else
                {
                    tempArray[index] = array[right];
                    right++;
                }
                index++;
            }
            Array.Copy(array, left, tempArray, index, leftEnd - left + 1);
            Array.Copy(array, right, tempArray, index, rightEnd - right + 1);
            Array.Copy(tempArray, leftStart, array, leftStart, size);
        }
    }
}