namespace GeneticAlgo.Shared.Tools;

public class GreaterFitnessEvaluator : IFitnessEvaluator
{
    public GreaterFitnessEvaluator(double minValue, double maxValue)
    {
        MinValue = minValue;
        MaxValue = maxValue;
    }

    public double MinValue { get; }
    public double MaxValue { get; }

    public double Evaluate(double fitness)
        => fitness;
}