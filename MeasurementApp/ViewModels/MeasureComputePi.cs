using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using ComputingPi;
using Utilities;

namespace MeasurementApp.ViewModels
{
    class MeasureComputePi : Measure<IComputePi>
    {
        public int TotalNumberOfSteps{
            get {
                return totalNumberOfSteps;
            }
            set {
                totalNumberOfSteps = value;
                OnPropertyChanged();
            }
        }

        public MeasureComputePi()
        {
            AddAlgorithms(availablePiAlgorithms);
            TotalNumberOfSteps = 1_000_000_000;
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
                IComputePi c = AvailableAlgorithms[i].Algorithm;
                start[i] = new Task(() => {});
                end[i] = start[i].ContinueWith(t => {
                        // Render the initialization to the UI.
                        ResultLog += "Algorithm " + c.Name + ": Started with " + TotalNumberOfSteps + " steps. " +
                            Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(t => {
                        // Run in the background a long computation which generates a result.
                        return Run(c, TotalNumberOfSteps);
                    }).ContinueWith(t => {
                        // Render the result on the UI.
                        ResultLog += "Algorithm " + c.Name + " gave Pi = " +
                            t.Result.Item1.ToString() + ":   " +
                            t.Result.Item2.ToString() + " wall clock sec. " +
                            t.Result.Item3.ToString() + " processor user time sec." +
                            Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            Task realStart = new Task(() => {
                    for (int i = 0; i < start.Length; i++) {
                        if (start[i] != null) {
                            start[i].RunSynchronously();
                            end[i].Wait();
                        }
                    }
                });
            realStart.ContinueWith(t => {
                // Enable the start button.
                RunIsEnabled = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());

            realStart.Start();
        }

        private static Tuple<double, double, double> Run(IComputePi computePi, int totalNumberOfSteps)
        {
            Stopwatch wallClock = new Stopwatch();
            ProcessUserTime processUserTime = new ProcessUserTime();
            // Measure the computation of Pi.
            wallClock.Restart();
            processUserTime.Restart();
            double result = computePi.ComputePi(totalNumberOfSteps);
            processUserTime.Stop();
            wallClock.Stop();
            return
                new Tuple<double, double, double>(result,
                                                  wallClock.Elapsed.TotalSeconds,
                                                  processUserTime.ElapsedTotalSeconds);
        }

        protected readonly IComputePi[] availablePiAlgorithms = {
                new SerialPi(),
                new SerialLinqPi(),
                // Add more here!
            };
        private int totalNumberOfSteps;
    }
}
