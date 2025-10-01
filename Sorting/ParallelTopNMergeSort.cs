// ParallelTopNMergeSort<T>
// ------------------------
// This class implements a parallel algorithm to find the top N (smallest or largest) elements in an array.
// The algorithm works as follows:
//   1. The input array is split into chunks, one per processor core.
//   2. Each chunk is processed in parallel: it is sorted, and the top N elements are selected from it.
//   3. All local top N results are merged into a single list.
//   4. The merged list is sorted, and the global top N elements are returned.
// This approach is much faster than sorting the entire array when N is much smaller than the array size,
// and it leverages multiple CPU cores for better performance on large datasets.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sorting
{
    public class ParallelTopNMergeSort<T> : TopNSortBase<T>
    {
        public override string Name => "Parallel Top-N MergeSort";

        public override T[] TopNSort(T[] inputOutput, int n, IComparer<T> comparer)
        {
            // Step 1: Decide how many chunks to split the array into.
            // We use one chunk per processor core to maximize parallelism.
            int processorCount = Environment.ProcessorCount;
            int length = inputOutput.Length;
            int chunkSize = (length + processorCount - 1) / processorCount; // ensures all elements are covered
            var localTopN = new List<T>[processorCount];

            // Step 2: Process each chunk in parallel.
            // For each chunk:
            //   a. Copy the chunk from the original array.
            //   b. Sort the chunk (so the smallest elements are at the front).
            //   c. Take the top N elements from the sorted chunk (these are the smallest N in that chunk).
            Parallel.For(0, processorCount, i =>
            {
                int start = i * chunkSize;
                int end = Math.Min(start + chunkSize, length);
                if (start >= end) {
                    // This chunk is empty (can happen if there are more processors than data)
                    localTopN[i] = new List<T>();
                    return;
                }
                // Copy the chunk
                T[] chunk = new T[end - start];
                Array.Copy(inputOutput, start, chunk, 0, end - start);
                // Sort the chunk
                Array.Sort(chunk, comparer);
                // Take the top N elements from this chunk
                int take = Math.Min(n, chunk.Length);
                localTopN[i] = chunk.Take(take).ToList();
            });

            // Step 3: Merge all local top N results into a single list.
            // Now we have (processorCount * N) or fewer elements, containing the best from each chunk.
            var merged = localTopN.SelectMany(x => x).ToList();

            // Step 4: Sort the merged list and take the global top N.
            // This final sort gives us the true top N elements from the whole array.
            merged.Sort(comparer);
            return merged.Take(n).ToArray();
        }
    }
}
