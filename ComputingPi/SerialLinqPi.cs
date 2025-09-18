using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ComputingPi
{
    public class SerialLinqPi : IComputePi
    {
        public string Name {
            get {
                return nameof(SerialLinqPi);
            }
        }

        /// <summary>Estimates the value of PI using serial LINQ.</summary>
        public double ComputePi(int numberOfSteps)
        {
            double step = 1.0 / (double)numberOfSteps;
            return (from i in Enumerable.Range(0, numberOfSteps)
                    let x = (i + 0.5) * step
                    select 4.0 / (1.0 + x * x)).Sum() * step;
        }
    }
}
