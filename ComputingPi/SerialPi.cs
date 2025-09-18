using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ComputingPi
{
    public class SerialPi : IComputePi
    {
        public string Name {
            get {
                return nameof(SerialPi);
            }
        }

        /// <summary>Estimates the value of PI using a for loop.</summary>
        public double ComputePi(int numberOfSteps)
        {
            double sum = 0.0;
            double step = 1.0 / (double)numberOfSteps;
            for (int i = 0; i < numberOfSteps; i++)
            {
                double x = (i + 0.5) * step;
                sum += 4.0 / (1.0 + x * x);
            }
            return step * sum;
        }
    }
}
