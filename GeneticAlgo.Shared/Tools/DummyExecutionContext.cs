using GeneticAlgo.Shared.Models;

namespace GeneticAlgo.Shared.Tools;

public class DummyExecutionContext : IExecutionContext
{
    private readonly int _pointCount;
    private readonly int _maximumValue;
    private readonly Random _random;

    public DummyExecutionContext(int pointCount, int maximumValue)
    {
        _pointCount = pointCount;
        _maximumValue = maximumValue;
        _random = Random.Shared;
    }
    
    private double Next => _random.NextDouble() * _random.Next(_maximumValue);

    public void Reset() { }

    public Task<IterationResult> ExecuteIterationAsync()
    {
        return Task.FromResult(IterationResult.IterationFinished);
    }

    public void ReportStatistics(IStatisticsConsumer statisticsConsumer)
    {
        Statistic[] statistics = Enumerable.Range(0, _pointCount)
            .Select(i => new Statistic(i, new Point(Next, Next), Next))
            .ToArray();

        statisticsConsumer.Consume(statistics);
    }
}