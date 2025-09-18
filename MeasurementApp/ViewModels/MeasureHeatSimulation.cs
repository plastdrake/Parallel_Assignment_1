using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using HeatSimulation;
using Utilities;

namespace MeasurementApp.ViewModels
{
    class MeasureHeatSimulation : Measure<HeatSimulationBase>
    {
        public int SquareSize {
            get {
                return squareSize;
            }
            set {
                squareSize = value;
                OnPropertyChanged();
            }
        }
        public int MaximumTimeStep {
            get {
                return maximumTimeStep;
            }
            set {
                maximumTimeStep = value;
                OnPropertyChanged();
            }
        }

        public BitmapSource HeatImage {
            get {
                return heatImage;
            }
            private set {
                heatImage = value;
                OnPropertyChanged();
            }
        }

        public MeasureHeatSimulation()
        {
            AddAlgorithms(availableHeatSimulationAlgorithms);
            SquareSize = 250;
            MaximumTimeStep = 2000;
        }

        protected override void ToRun()
        {
            RunIsEnabled = false;
            Task[] start = new Task[AvailableAlgorithms.Count];
            Task[] end = new Task[AvailableAlgorithms.Count];
            for (int i = 0; i < AvailableAlgorithms.Count; i++) {
                if (!AvailableAlgorithms[i].Run) {
                    continue;
                }
                HeatSimulationBase h = AvailableAlgorithms[i].Algorithm;
                h.SquarePlateSize = SquareSize;
                h.MaximumTimeStep = MaximumTimeStep;
                start[i] = new Task(() => {});
                end[i] = start[i].ContinueWith(t => {
                        // Render the initialization to the UI.
                        ResultLog += "Algorithm " + h.Name + ": Started with (" +
                            SquareSize + ", " + MaximumTimeStep + ")." + Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(t => {
                        // Run in the background a long computation which generates a result.
                        return Run(h);
                    }).ContinueWith(t => {
                        // Render the result on the UI.
                        ResultLog += "Algorithm " + h.Name + ": " +
                            t.Result.Item1.ToString() + " wall clock sec. " +
                            t.Result.Item2.ToString() + " processor user time sec." +
                            Environment.NewLine;

                        // Ugly conversion to image.
                        byte[] image = new byte[h.HeatImage.GetLength(0) * h.HeatImage.GetLength(1)];
                        for (int x = 0; x < h.HeatImage.GetLength(0); x++) {
                            for(int y = 0; y < h.HeatImage.GetLength(1); y++) {
                                if (!(0 <= h.HeatImage[x, y] && h.HeatImage[x, y] <= 255)) {
                                    System.Console.Write('.');
                                }
                                image[x + y*h.HeatImage.GetLength(1)] = (byte)h.HeatImage[x, y];
                            }
                        }
                        HeatImage =
                            BitmapSource.Create(h.HeatImage.GetLength(0),
                                                h.HeatImage.GetLength(1),
                                                96, 96,
                                                PixelFormats.Gray8,
                                                null,
                                                image,
                                                h.HeatImage.GetLength(0));
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            Task beginning = new Task(() => {
                    for (int i = 0; i < start.Length; i++) {
                        if (AvailableAlgorithms[i].Run) {
                            start[i].RunSynchronously();
                            end[i].Wait();
                        }
                    }
                });
            beginning.ContinueWith(t => {
                // Enable the start button.
                RunIsEnabled = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());

            beginning.Start();
        }

        private static Tuple<double, double> Run(HeatSimulationBase heat)
        {
            Stopwatch wallClock = new Stopwatch();
            ProcessUserTime processUserTime = new ProcessUserTime();

            // Measure the heat simulation computation.
            wallClock.Restart();
            processUserTime.Restart();
            heat.Compute();
            processUserTime.Stop();
            wallClock.Stop();
            return
                new Tuple<double, double>(wallClock.Elapsed.TotalSeconds,
                                          processUserTime.ElapsedTotalSeconds);
        }


        private readonly HeatSimulationBase[] availableHeatSimulationAlgorithms = {
                new HeatSequential()
                // Add more here!
            };
        private int squareSize, maximumTimeStep;
        private BitmapSource heatImage;
    }
}
