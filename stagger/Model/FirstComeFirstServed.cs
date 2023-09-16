

namespace Stagger.Model
{
    public class FirstComeFirstServed : IStagger
    {
        public Queue<IProcess> Ready { get; init; }
        public Queue<IProcess> Waiting { get; } = new Queue<IProcess>();
        public Queue<IProcess> Completed { get; } = new Queue<IProcess>();

        public bool Idle => !this.Busy;

        public bool Busy =>  this.Ready.Any() || this.Waiting.Any();

        public FirstComeFirstServed(Queue<IProcess> ready)
        {
            this.Ready = ready;
        }

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