using System;
using System.Threading;
using System.Threading.Algorithms;
using System.Threading.Tasks;

namespace ScanAndFold
{
    public class ScanAndFoldParallel2FromTS : IScanAndFold
    {
        public string Name { get { return "Parallel Scan and Fold 2 from Toub, S. book"; } }

        public void InclusiveScanInPlace<T>(T[] arr, Func<T, T, T> function)
        {
            T[] result = ParallelAlgorithms.Scan<T>(arr, function, loadBalance: true);
            for (int i = 0; i < result.Length; ++i) {
                arr[i] = result[i];
            }
        }
    }
}
