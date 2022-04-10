using System.Linq;
using System.Windows.Controls;
using GeneticAlgo.Shared;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;

namespace GeneticAlgo.GraphicalInterface.Tools;

public class StatConsumer : IStatConsumer
{
    private readonly ListBox _textBox;
    private readonly PlotView _plot;

    public StatConsumer(ListBox textBox, PlotView plot)
    {
        _textBox = textBox;
        _plot = plot;
    }

    public void NotifyStatUpdate((int X, int Y)[] statistic)
    {
        _textBox.ItemsSource = statistic;
    }

    public void UpdatePlot(int[] data)
    {
        PlotModel plotModel = new PlotModel();
        plotModel.Series.Clear();

        var series = new LineSeries
        {
            ItemsSource = data.Select((v, i) => new DataPoint(i, v))
        };

        plotModel.Series.Add(series);

        _plot.Model = plotModel;
        _plot.InvalidatePlot();
    }
}