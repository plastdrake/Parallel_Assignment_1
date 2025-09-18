﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MeasurementApp.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            tabComputePi.DataContext = new ViewModels.MeasureComputePi();
            tabSorting.DataContext = new ViewModels.MeasureSorting();
            tabTopN.DataContext = new ViewModels.MeasureTopNSort();
            tabSpeculativeTopN.DataContext = new ViewModels.MeasureSpeculativeTopNSort();
            tabMandelbrot.DataContext = new ViewModels.MeasureMandelbrot();
            tabHeatSimulation.DataContext = new ViewModels.MeasureHeatSimulation();
            tabEditDistance.DataContext = new ViewModels.MeasureEditDistance();
            tabScanAndFold.DataContext = new ViewModels.MeasureScanAndFold();
        }
    }
}
