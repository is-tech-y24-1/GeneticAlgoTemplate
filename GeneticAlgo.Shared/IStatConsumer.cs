namespace GeneticAlgo.Shared
{
    public interface IStatConsumer
    {
        void NotifyStatUpdate((int X, int Y)[] statistic);
        void UpdatePlot(int[] data);
    }
}