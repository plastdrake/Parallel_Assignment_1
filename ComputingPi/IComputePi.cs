using System;

using Utilities;

namespace ComputingPi
{
    public interface IComputePi : ICompute
    {
        double ComputePi(int numberOfSteps);
    }
}
