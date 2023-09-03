namespace Stagger.Model
{
    public abstract class Stagger : IStagger
    {
        public Queue<IProcess> Ready { get; init; } = new Queue<IProcess>();
        public Queue<IProcess> Waiting { get; } = new Queue<IProcess>();
        public Queue<IProcess> Completed { get; } = new Queue<IProcess>();
        public bool Idle { get { return !this.Busy; } }
        public bool Busy { get { return Waiting.Any() || Ready.Any(); } }

        public Stagger(Queue<IProcess> ready)
        {
            this.Ready = ready;
        }

        public abstract void Work();
    }
}