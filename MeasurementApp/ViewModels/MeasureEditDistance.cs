using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using EditDistance;
using Utilities;

namespace MeasurementApp.ViewModels
{
    class MeasureEditDistance : Measure<IEditDistance>
    {
        public string String1 {
            get {
                return string1;
            }
            set {
                string1 = value;
                LengthOfString1 = string1.Length;
                OnPropertyChanged();
            }
        }

        public int LengthOfString1 {
            get {
                return lengthOfString1;
            }
            set {
                lengthOfString1 = value;
                OnPropertyChanged();
            }
        }

        public string String2 {
            get {
                return string2;
            }
            set {
                string2 = value;
                LengthOfString2 = string2.Length;
                OnPropertyChanged();
            }
        }

        public int LengthOfString2 {
            get {
                return lengthOfString2;
            }
            set {
                lengthOfString2 = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> RandomizeString1Command
        {
            get; protected set;
        }

        public RelayCommand<object> RandomizeString2Command
        {
            get; protected set;
        }

        public MeasureEditDistance()
        {
            AddAlgorithms(availableEditDistanceAlgorithms);
            RandomizeString1Command = new RelayCommand<object>(_ => {
                String1 = random.RandomString(LengthOfString1);
            });
            RandomizeString2Command = new RelayCommand<object>(_ => {
                String2 = random.RandomString(LengthOfString2);
            });
            // Default setup.
            LengthOfString1 = 1000;
            LengthOfString2 = 1100;
            RandomizeString1Command.Execute(null);
            char[] characters = (String1 + random.RandomString(LengthOfString2 - LengthOfString1)).ToCharArray();
            random.Shuffle<char>(characters);
            String2 = new String(characters);
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
                IEditDistance ed = AvailableAlgorithms[i].Algorithm;
                start[i] = new Task(() => {});
                end[i] = start[i].ContinueWith(t => {
                        // Render the initialization to the UI.
                        ResultLog += "Algorithm " + ed.Name + ": Started with (" +
                            String1 + ", " + String2 + ")." + Environment.NewLine;
                    }, TaskScheduler.FromCurrentSynchronizationContext()).ContinueWith(t => {
                        // Run in the background a long computation which generates a result.
                        return Run(ed);
                    }).ContinueWith(t => {
                        // Render the result on the UI.
                        ResultLog += "Algorithm " + ed.Name + ": " +
                            t.Result.Item1.ToString() + " string distance. " +
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

        private Tuple<int, double, double> Run(IEditDistance distance)
        {
            Stopwatch wallClock = new Stopwatch();
            ProcessUserTime processUserTime = new ProcessUserTime();

            // Measure the edit distance computation.
            wallClock.Restart();
            processUserTime.Restart();
            int dist = distance.EditDistance(string1, string2);
            processUserTime.Stop();
            wallClock.Stop();
            return
                new Tuple<int, double, double>(dist,
                                               wallClock.Elapsed.TotalSeconds,
                                               processUserTime.ElapsedTotalSeconds);
        }


        private readonly IEditDistance[] availableEditDistanceAlgorithms = {
                new EditDistanceSequential()
                // Add more here!
            };
        private readonly Random random = new Random();
        private string string1, string2;
        private int lengthOfString1, lengthOfString2;
    }
}
