using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;
using GeneticAlgo.WpfInterface.Tools;
using OxyPlot;
using OxyPlot.Series;
using Serilog;

namespace GeneticAlgo.WpfInterface
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Window
    {
        //TODO: move to configuration
        private static readonly TimeSpan IterationInterval = TimeSpan.FromMilliseconds(1000);

        private bool _isActive;

        private readonly IStatisticsConsumer _statisticsConsumer;
        private readonly IExecutionContext _executionContext;
        private readonly ExecutionConfiguration _configuration;

        public PlotModel ScatterModel { get; }
        public PlotModel BarModel { get; }

        public MainWindow()
        {
            InitializeComponent();

            Logger.Init();

            var lineSeries = new LineSeries
            {
                StrokeThickness = 0,
                MarkerSize = 3,
                MarkerStroke = OxyColors.ForestGreen,
                MarkerType = MarkerType.Plus,
            };

            ScatterModel = new PlotModel
            {
                Title = "Points",
                Series = { lineSeries },
            };

            var barSeries = new LinearBarSeries
            {
                DataFieldX = nameof(FitnessModel.X),
                DataFieldY = nameof(FitnessModel.Y),
            };

            BarModel = new PlotModel
            {
                Title = "Fitness",
                Series = { barSeries },
            };

            _statisticsConsumer = new PlotStatisticConsumer(lineSeries, barSeries);
            _executionContext = new DummyExecutionContext(100, 10);
            _configuration = new ExecutionConfiguration(IterationInterval, 10, 0);

            PlotSample.Model = ScatterModel;
            PlotSample2.Model = BarModel;

            var worker = new BackgroundWorker();
            worker.DoWork += StartSimulation;
            worker.RunWorkerAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _isActive = true;
        }

        private void StartSimulation(object? sender, DoWorkEventArgs e)
        {
            _isActive = true;
            while (true)
            {
                StartSimulator();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public async void StartSimulator()
        {
            if (!_isActive)
                return;

            Log.Information("Start simulation");
            _isActive = true;
            _executionContext.Reset();

            IterationResult iterationResult;

            do
            {
                iterationResult = await _executionContext.ExecuteIterationAsync();
                _executionContext.ReportStatistics(_statisticsConsumer);

                ScatterModel.InvalidatePlot(true);
                BarModel.InvalidatePlot(true);

                Task.Delay(_configuration.IterationDelay).Wait();
            }
            while (iterationResult == IterationResult.IterationFinished && _isActive);
        }
    }
}
