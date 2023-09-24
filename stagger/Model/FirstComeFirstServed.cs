






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

            this.Refresh();
            IProcess next = this.GetNext();
            this.Handle(log, next);
        }

        private void Refresh()
        {
            List<IProcess> refreshed = new ();

            foreach(IProcess process in this.Waiting)
            {
                if (RefreshProcess(process))
                {
                    refreshed.Add(process);
                }
            }

            this.MoveProcessesToReady(refreshed);
        }

        private bool RefreshProcess(IProcess process)
        {
            process.Refresh();
            return !process.Suspended;
        }

        private void MoveProcessesToReady(List<IProcess> refreshed)
        {
            foreach (IProcess process in refreshed)
            {
                this.Waiting.Remove(process);
                int highestArrivalTime = this.Ready.Count > 0 ? this.Ready.Select(process => process.ArrivalTime).Max() : 0;
                process.ArrivalTime = highestArrivalTime + 1;
                this.Ready.Add(process);
            }
        }

        private IProcess GetNext()
        {
            return this.Ready.OrderBy(process => process.ArrivalTime).First();
        }

        private void Handle(WriteCallback log, IProcess process)
        {
            if (!process.Progress() && process.Suspended)
            {
                this.Ready.Remove(process);
                this.Waiting.Add(process);
                this.ReportSuspension(log, process);
                return;
            }
            
            this.Progress();
            this.ReportSuccess(log, process);
            
            if (process.Completed)
            {
                this.Ready.Remove(process);
                this.Completed.Add(process);
                return;
            }
        }

        private void Progress()
        {
            this.Current++;
        }

        private void ReportSuspension(WriteCallback log, IProcess next)
        {
            log($"-----------------");
            log($"PID {next.ID.ToString().PadLeft(4, '0')} is waiting for input.");
            log($"-----------------");
        }

        private void ReportSuccess(WriteCallback log, IProcess next)
        {
            log($"-----------------");
            log($"1 step has been executed for PID {next.ID.ToString().PadLeft(4, '0')}.");
            log($"");
            log($"This process is {1.0 * next.CurrentStep / next.Steps:p} complete.");
            log($"-----------------");
        }
    }
}