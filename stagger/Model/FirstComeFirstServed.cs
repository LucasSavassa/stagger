


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

        public int Length { get; init; }

        public int Current { get; private set; }

        public FirstComeFirstServed(IEnumerable<IProcess> initial)
        {
            foreach(IProcess process in initial) this.Ready.Enqueue(process);
            this.Length = initial.Sum(process => process.Steps);
            this.Current = 0;
        }

        public void Work(WriteCallback log)
        {
            if (this.Idle) return;

            IProcess next = this.Ready.Peek();
            this.Handle(next);
            Report(log, next);
        }

        private void Handle(IProcess process)
        {
            process.Progress();
            this.Progress();
        }

        private void Progress()
        {
            this.Current++;
        }

        private static void Report(WriteCallback log, IProcess next)
        {
            log($"-----------------");
            log($"Step executed.");
            log($"");
            log($"Process {next.ID} progressed {1.0 / next.Steps:p}.");
            log($"And is now {1.0 * next.CurrentStep / next.Steps:p} complete.");
            log($"-----------------");
        }
    }
}