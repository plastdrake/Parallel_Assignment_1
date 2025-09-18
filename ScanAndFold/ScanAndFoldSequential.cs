using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanAndFold
{
    public class ScanAndFoldSequential : IScanAndFold
    {
        public string Name { get { return "Sequential Scan and Fold"; } }

        public void InclusiveScanInPlace<T>(T[] arr, Func<T, T, T> function)
        {
            for (int i = 1; i < arr.Length; i++) {
                arr[i] = function(arr[i - 1], arr[i]);
            }
        }
    }
}
