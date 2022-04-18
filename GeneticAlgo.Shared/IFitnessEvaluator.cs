namespace GeneticAlgo.Shared;

/*
 * Used to abstract on 'quality' of fitness value.
 * Fitness value could be a 'less is better' or 'more is better'.
 */
public interface IFitnessEvaluator
{
    double MinValue { get; }
    double MaxValue { get; }
    double Evaluate(double fitness);
}