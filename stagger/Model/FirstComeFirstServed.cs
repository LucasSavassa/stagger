



namespace Stagger.Model
{
    public class FirstComeFirstServed : IStagger
    {
        public string Name => "First Come First Served";
        public List<IProcess> Ready { get; } = new List<IProcess>();
        public List<IProcess> Waiting { get; } = new List<IProcess>();
        public List<IProcess> Completed { get; } = new List<IProcess>();
        public bool Idle => !this.Busy;
        public bool Busy =>  this.Ready.Any() || this.Waiting.Any();
        public int Length => 
            this.Ready.Sum(process => process.Steps)
          + this.Waiting.Sum(process => process.Steps)
          + this.Completed.Sum(process => process.Steps);
        public int Current { get; private set; }

        public FirstComeFirstServed()
        {
            
        }
        public FirstComeFirstServed(IEnumerable<IProcess> initial)
        {
            foreach(IProcess process in initial) this.Add(process);
        }

        public void Add(IProcess process)
        {
            this.Ready.Add(process);
        }

        public void Work(WriteCallback log)
        {
            if (this.Idle) return;

            IProcess next = this.GetNext();
            this.Handle(next);
            Report(log, next);
        }

        private IProcess GetNext()
        {
            return this.Ready.OrderBy(process => process.ArrivalTime).First();
        }

        private void Handle(IProcess process)
        {
            process.Progress();
            this.Progress();
            
            if (process.Completed) {
                this.Ready.Remove(process);
                this.Completed.Add(process);
            }
        }

        private void Progress()
        {
            this.Current++;
        }

        private static void Report(WriteCallback log, IProcess next)
        {
            log($"-----------------");
            log($"1 step has been executed for process {next.ID}.");
            log($"");
            log($"This process is {1.0 * next.CurrentStep / next.Steps:p} complete.");
            log($"-----------------");
        }
    }
}