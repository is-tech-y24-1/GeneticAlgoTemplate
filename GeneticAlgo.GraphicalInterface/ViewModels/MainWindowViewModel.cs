using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using GeneticAlgo.GraphicalInterface.Models;
using GeneticAlgo.GraphicalInterface.Tools;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using OxyPlot;
using OxyPlot.Series;

namespace GeneticAlgo.GraphicalInterface.ViewModels;

public class MainWindowViewModel : AvaloniaObject
{
    private readonly IStatisticsConsumer _statisticsConsumer;
    private readonly IFitnessEvaluator _fitnessEvaluator;
    private readonly IExecutionContext _executionContext;
    private readonly ExecutionConfiguration _configuration;

    public MainWindowViewModel(
        IExecutionContext executionContext,
        IFitnessEvaluator fitnessEvaluator,
        ExecutionConfiguration configuration)
    {
        _executionContext = executionContext;
        _fitnessEvaluator = fitnessEvaluator;
        _configuration = configuration;

        IsRunning = AvaloniaProperty
            .RegisterAttached<MainWindowViewModel, bool>(nameof(IsRunning), typeof(MainWindowViewModel));

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

        _statisticsConsumer = new PlotStatisticConsumer(lineSeries, barSeries, fitnessEvaluator);
    }

    public PlotModel ScatterModel { get; }
    public PlotModel BarModel { get; }

    public AttachedProperty<bool> IsRunning { get; }

    public async Task RunAsync()
    {
        var lineSeries = (LineSeries)ScatterModel.Series.Single();
        var barSeries = (LinearBarSeries)BarModel.Series.Single();

        lineSeries.XAxis.Maximum = _configuration.MaximumValue;
        lineSeries.XAxis.Minimum = _configuration.MinimumValue;
        lineSeries.YAxis.Maximum = _configuration.MaximumValue;
        lineSeries.YAxis.Minimum = _configuration.MinimumValue;

        barSeries.YAxis.Maximum = _fitnessEvaluator.MaxValue;
        barSeries.YAxis.Minimum = _fitnessEvaluator.MinValue;

        SetValue(IsRunning, true);
        _executionContext.Reset();

        IterationResult iterationResult;

        do
        {
            iterationResult = await _executionContext.ExecuteIterationAsync();
            _executionContext.ReportStatistics(_statisticsConsumer);

            ScatterModel.InvalidatePlot(true);
            BarModel.InvalidatePlot(true);

            await Task.Delay(_configuration.IterationDelay);
        }
        while (iterationResult == IterationResult.IterationFinished && GetValue(IsRunning));
    }

    public void Stop()
    {
        SetValue(IsRunning, false);
    }
}