using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using GeneticAlgo.GraphicalInterface.Tools;
using GeneticAlgo.Shared;

namespace GeneticAlgo.GraphicalInterface
{
    public partial class MainWindow : Window
    {
        //TODO: move to configuration
        private static readonly TimeSpan IterationInterval = TimeSpan.FromMilliseconds(100);

        private bool _isActive = false;
        private readonly IExecutionContext _executionContext;
        private readonly IPixelDrawer _pd;
        private readonly IStatConsumer _statConsumer;

        public MainWindow()
        {
            InitializeComponent();

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

        private void StartSimulation(object sender, DoWorkEventArgs e)
        {
            _isActive = true;
            while (true)
            {
                StartSimulator();
            }
        }

        public void StartSimulator()
        {
            if (!_isActive)
                return;

            _executionContext.OnRoundStart();
            bool runNextRound = false;
            do
            {
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
