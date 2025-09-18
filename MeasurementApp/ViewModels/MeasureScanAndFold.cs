using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ScanAndFold;
using Utilities;

namespace MeasurementApp.ViewModels
{
    class MeasureScanAndFold : Measure<IScanAndFold>
    {
        public long[] Array {
            get {
                return array;
            }
            set {
                array = value;
                LengthOfArray = array.Length;
                OnPropertyChanged();
            }
        }

        public int LengthOfArray {
            get {
                return lengthOfArray;
            }
            set {
                lengthOfArray = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> OrderedArrayCommand
        {
            get; protected set;
        }

        public RelayCommand<object> RandomizeArrayCommand
        {
            get; protected set;
        }

        public MeasureScanAndFold()
        {
            AddAlgorithms(availableScanAndFoldAlgorithms);
            OrderedArrayCommand = new RelayCommand<object>(_ => {
                type = "ordered";
                Array = new long[lengthOfArray];
                for (int i = 0; i < Array.Length; i++) {
                    Array[i] = i + 1;
                }
            });
            RandomizeArrayCommand = new RelayCommand<object>(_ => {
                type = "random";
                Array = new long[lengthOfArray];
                for (int i = 0; i < Array.Length; i++) {
                    Array[i] = random.Next(0, 2 ^ 20);
                }
            });
            // Default setup.
            LengthOfArray = 20_000_000;
            OrderedArrayCommand.Execute(null);
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
                IScanAndFold saf = AvailableAlgorithms[i].Algorithm;
                start[i] = new Task(() => {});
                end[i] = start[i].ContinueWith(t => {
                        // Render the initialization to the UI.
                        ResultLog += "Algorithm " + saf.Name + ": Started with (Array[" +
                            Array.Length + "] " + type + ")." + Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(t => {
                        // Run in the background a long computation which generates a result.
                        return Run(saf);
                    }).ContinueWith(t => {
                        // Render the result on the UI.
                        ResultLog += "Algorithm " + saf.Name + ": " +
                            t.Result.Item1.ToString() + " is the last value. " +
                            t.Result.Item2.ToString() + " wall clock sec. " +
                            t.Result.Item3.ToString() + " processor user time sec." +
                            Environment.NewLine;
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

        private Tuple<long, double, double> Run(IScanAndFold saf)
        {
            Stopwatch wallClock = new Stopwatch();
            ProcessUserTime processUserTime = new ProcessUserTime();

            // Measure the Scan and Fold computation.
            long[] arr = new long[array.Length];
            array.CopyTo(arr, 0);
            wallClock.Restart();
            processUserTime.Restart();
            saf.InclusiveScanInPlace<long>(arr, (i, j) => {
                return i + j;
            });
            processUserTime.Stop();
            wallClock.Stop();
            return
                new Tuple<long, double, double>(arr[arr.Length - 1],
                                                wallClock.Elapsed.TotalSeconds,
                                                processUserTime.ElapsedTotalSeconds);
        }


        private readonly IScanAndFold[] availableScanAndFoldAlgorithms = {
                new ScanAndFoldSequential()
                // Add more here!
            };
        private Random random = new Random();
        private long[] array;
        private int lengthOfArray;
        private string type;
    }
}
