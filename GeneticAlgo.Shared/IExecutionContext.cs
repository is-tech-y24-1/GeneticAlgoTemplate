namespace GeneticAlgo.Shared
{
    public interface IExecutionContext
    {
        void OnRoundStart();
        bool OnIterationStart();
        void OnRoundEnd();
        void OnUiRender(IStatConsumer statConsumer, IPixelDrawer pixelDrawer);
    }

    public class DummyExecutionContext : IExecutionContext
    {
        public void OnRoundStart()
        {
        }

        public bool OnIterationStart()
        {
            return true;
        }

        public void OnRoundEnd()
        {
        }

        public void OnUiRender(IStatConsumer statConsumer, IPixelDrawer pixelDrawer)
        {
        }
    }
}