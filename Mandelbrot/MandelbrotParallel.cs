// MandelbrotParallel
// -----------------
// This class implements a parallelized Mandelbrot set computation.
// Each pixel is computed independently, making it ideal for parallelization.
// The Compute method will use parallel loops to speed up the rendering of the fractal.

using System;
using System.Threading.Tasks;

namespace Mandelbrot
{
    public class MandelbrotParallel : MandelbrotBase
    {
        public MandelbrotParallel(int width, int height) : base(width, height) { }

        public override string Name => "Parallel Mandelbrot";

        public override void Compute()
        {
            // Parallel computation will be implemented here
            throw new NotImplementedException();
        }
    }
}
