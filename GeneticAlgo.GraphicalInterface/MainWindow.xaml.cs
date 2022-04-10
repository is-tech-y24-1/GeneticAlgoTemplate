using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using GeneticAlgo.GraphicalInterface.Tools;
using GeneticAlgo.Shared;
using Serilog;

namespace GeneticAlgo.GraphicalInterface
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : Window
    {
        //TODO: move to configuration
        private static readonly TimeSpan IterationInterval = TimeSpan.FromMilliseconds(100);

        private bool _isActive;
        private readonly IExecutionContext _executionContext;
        private readonly IPixelDrawer _pd;
        private readonly IStatConsumer _statConsumer;

        public MainWindow()
        {
            InitializeComponent();

            Logger.Init();

            //TODO:
            _executionContext = new DummyExecutionContext();
            //TODO: move to configuration
            _pd = new PixelDrawer(ImageView, 10, 10);
            _statConsumer = new StatConsumer(CountInput, PlotSample);

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

        public void StartSimulator()
        {
            if (!_isActive)
                return;

            Log.Information("Start simulation");
            _executionContext.OnRoundStart();
            bool runNextRound;
            do
            {
                Log.Debug("Round start");
                if (!_isActive)
                    return;

                runNextRound = _executionContext.OnIterationStart();
                Application.Current.Dispatcher.Invoke(() => _executionContext.OnUiRender(_statConsumer, _pd));
                Thread.Sleep(IterationInterval);
            } while (runNextRound);

            _executionContext.OnRoundEnd();
        }
    }
}
