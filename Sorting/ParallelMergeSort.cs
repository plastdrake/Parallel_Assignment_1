// ParallelMergeSort<T>
// -------------------
// This class implements a parallel merge sort algorithm for sorting arrays.
// The algorithm works as follows:
//   Step 1: The input array is recursively divided into two halves.
//   Step 2: Each half is sorted. If the subarray is large enough, the two halves are sorted in parallel
//           using multiple CPU cores. Otherwise, the sort proceeds sequentially to avoid excessive overhead.
//   Step 3: After both halves are sorted, they are merged back together into a single sorted array.
//           The merge operation combines the two sorted halves efficiently.
//   Step 4: The process repeats recursively until the entire array is sorted.
// This approach leverages parallelism for large arrays, providing significant speedup on multi-core systems.
// For small subarrays, it falls back to sequential sorting for efficiency.

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
            // Start the parallel merge sort with a temporary array for merging
            ParallelMergeSortRecursive(inputOutput, new T[inputOutput.Length], 0, inputOutput.Length - 1, comparer);
        }

        private static void ParallelMergeSortRecursive(T[] array, T[] tempArray, int leftStart, int rightEnd, IComparer<T> comparer)
        {
            // Base case: if the subarray has one or zero elements, it is already sorted
            if (leftStart >= rightEnd)
            {
                return;
            }

            int middle = (leftStart + rightEnd) / 2;
            // Step 1: Recursively divide the array into two halves
            // Step 2: Sort each half
            // If the subarray is large, sort both halves in parallel
            // Otherwise, sort sequentially to avoid parallel overhead
            if (rightEnd - leftStart < 2048)
            {
                // Sequential sort for small subarrays
                ParallelMergeSortRecursive(array, tempArray, leftStart, middle, comparer);
                ParallelMergeSortRecursive(array, tempArray, middle + 1, rightEnd, comparer);
            }
            else
            {
                // Parallel sort for large subarrays
                Parallel.Invoke(
                    () => ParallelMergeSortRecursive(array, tempArray, leftStart, middle, comparer),
                    () => ParallelMergeSortRecursive(array, tempArray, middle + 1, rightEnd, comparer)
                );
            }
            // Step 3: Merge the two sorted halves
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
            // Merge the two sorted halves into tempArray
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
            // Copy any remaining elements from the left half
            Array.Copy(array, left, tempArray, index, leftEnd - left + 1);
            // Copy any remaining elements from the right half
            Array.Copy(array, right, tempArray, index, rightEnd - right + 1);
            // Copy the merged result back into the original array
            Array.Copy(tempArray, leftStart, array, leftStart, size);
        }
    }
}