using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Utilities;

namespace HeatSimulation
{
    public abstract class HeatSimulationBase : ICompute
    {
        public abstract string Name { get; }

        public int SquarePlateSize { get; set; }
        public int MaximumTimeStep { get; set; }

        public float[,] HeatImage { get; protected set; }

        public abstract void Compute();

        protected void Swap<T>(ref T one, ref T two)
        {
            T tmp = one; one = two; two = tmp;
        }
    }
}
