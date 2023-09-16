

namespace Stagger.Model
{
    public class FirstComeFirstServed : IStagger
    {
        public string Name => "First Come First Served";
        public Queue<IProcess> Ready { get; } = new Queue<IProcess>();
        public Queue<IProcess> Waiting { get; } = new Queue<IProcess>();
        public Queue<IProcess> Completed { get; } = new Queue<IProcess>();

        public bool Idle => !this.Busy;

        public bool Busy =>  this.Ready.Any() || this.Waiting.Any();

        public FirstComeFirstServed() { }

        public void Work()
        {
            IProcess process = this.GetNext();
            this.Handle(process);
        }

        private IProcess GetNext()
        {
            return this.Ready.Dequeue();
        }

        private void Handle(IProcess process)
        {
            if (process.Completed) return;            
        }
    }
}