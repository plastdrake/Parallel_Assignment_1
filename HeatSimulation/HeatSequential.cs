using System;

namespace HeatSimulation
{
    public class HeatSequential : HeatSimulationBase
    {
        public override string Name {
            get { return nameof(HeatSequential); }
        }

        public override void Compute()
        {
            // Initial plates for previous and current time steps, with
            // heat starting on one side.
            var prevIter = new float[SquarePlateSize, SquarePlateSize];
            var currIter = new float[SquarePlateSize, SquarePlateSize];
            for (int y = 0; y < SquarePlateSize; y++) {
                prevIter[y, 0] = 255.0f;
            }
            // Run simulation
            for (int step = 0; step < MaximumTimeStep; step++) {
                for (int y = 1; y < SquarePlateSize - 1; y++) {
                    for (int x = 1; x < SquarePlateSize - 1; x++) {
                        currIter[y, x] =
                            ((prevIter[y, x - 1] +
                              prevIter[y, x + 1] +
                              prevIter[y - 1, x] +
                              prevIter[y + 1, x]) * 0.25f);
                    }
                }
                Swap(ref prevIter, ref currIter);
            }
            HeatImage = prevIter;
        }
    }
}
