using System.Collections.Generic;
using System.Linq;
using GeneticAlgo.GraphicalInterface.Models;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using OxyPlot;
using OxyPlot.Series;

namespace GeneticAlgo.GraphicalInterface.Tools;

public class PlotStatisticConsumer : IStatisticsConsumer
{
    private readonly LineSeries _scatterSeries;
    private readonly LinearBarSeries _linearBarSeries;

    public PlotStatisticConsumer(LineSeries scatterSeries, LinearBarSeries linearBarSeries)
    {
        _scatterSeries = scatterSeries;
        _linearBarSeries = linearBarSeries;
    }

    public void Consume(IReadOnlyCollection<Statistic> statistics)
    {
        _scatterSeries.Points.Clear();

        foreach (var statistic in statistics)
        {
            var point = statistic.Point;
            _scatterSeries.Points.Add(new DataPoint(point.X, point.Y));
        }

        _linearBarSeries.ItemsSource = statistics
            .Select(s => new FitnessModel(s.Id, s.Fitness))
            .ToArray();
    }
}