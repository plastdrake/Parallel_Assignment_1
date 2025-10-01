// MandelbrotParallel
// -----------------
// This class implements a parallelized Mandelbrot set computation.
// The Mandelbrot set is a fractal, visualized by iterating a simple formula for each point in the complex plane.
// Each pixel in the output image represents a point in the complex plane, and its color is determined by how quickly
// the formula escapes to infinity. The computation for each pixel is independent, making it ideal for parallelization.
//
// Steps:
//   1. For each pixel in the image, map it to a point in the complex plane.
//   2. For each point, iterate the Mandelbrot formula to determine if it escapes.
//   3. Color the pixel based on the number of iterations before escape (or if it never escapes).
//   4. Use a parallel loop to compute each row (or pixel) independently, leveraging all CPU cores.
// This approach greatly speeds up rendering, especially for high-resolution images.

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
            // Step 1: Prepare the ranges for the complex plane
            var xRange = new Tuple<double, double>(LowerX, UpperX);
            var yRange = new Tuple<double, double>(LowerY, UpperY);
            int widthPixels = Image.GetLength(0);
            int heightPixels = Image.GetLength(1);
            double stepx = (xRange.Item2 - xRange.Item1) / widthPixels;
            double stepy = (yRange.Item2 - yRange.Item1) / heightPixels;

            // Step 2: Use a parallel loop to compute each row independently
            Parallel.For(0, widthPixels, i =>
            {
                for (int j = 0; j < heightPixels; j++)
                {
                    // Step 3: Map pixel (i, j) to a point (tempx, tempy) in the complex plane
                    double tempx = xRange.Item1 + i * stepx;
                    double tempy = yRange.Item1 + j * stepy;
                    // Step 4: Compute the number of iterations before escape
                    int color = Diverge(tempx, tempy);
                    // Step 5: Store the result in the image array
                    Image[i, j] = MAX_ITERATIONS - color;
                }
            });
        }
    }
}
