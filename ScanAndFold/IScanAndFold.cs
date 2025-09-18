using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Utilities;

namespace ScanAndFold
{
    public interface IScanAndFold : ICompute
    {
        void InclusiveScanInPlace<T>(T[] arr, Func<T, T, T> function);
    }
}
