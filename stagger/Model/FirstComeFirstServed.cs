









namespace Stagger.Model
{
    public class FirstComeFirstServed : IStagger
    {
        public string Name => "First Come First Served";
        public List<IProcess> Arriving { get; } = new List<IProcess>();
        public List<IProcess> Waiting { get; } = new List<IProcess>();
        public List<IProcess> Ready { get; } = new List<IProcess>();
        public List<IProcess> Completed { get; } = new List<IProcess>();
        public bool Idle => !Busy;
        public bool Busy =>  Ready.Any() || Waiting.Any() || Arriving.Any();
        public int Length => 
            Ready.Sum(process => process.Steps)
            + Arriving.Sum(process => process.Steps)
              + Waiting.Sum(process => process.Steps)
                + Completed.Sum(process => process.Steps);
        public int Progress => 
            Ready.Sum(process => process.CurrentStep)
            + Arriving.Sum(process => process.CurrentStep)
              + Waiting.Sum(process => process.CurrentStep)
               + Completed.Sum(process => process.CurrentStep);
        public int Clock { get; private set; }

        public FirstComeFirstServed()
        {
            Clock = -1;
        }

        public FirstComeFirstServed(IEnumerable<IProcess> initial) : this()
        {
            foreach(IProcess process in initial) Arrive(process);
        }

        public void Arrive(IProcess process)
        {
            if(process.ArrivalTime < Clock) return;

            Arriving.Add(process);
        }

        public void Work(WriteCallback log)
        {
            if (Idle) return;

            Clock++;

            HandleArrivingQueue(log);
            HandleWaitingQueue(log);
            HandleReadyQueue(log);
        }

        private void HandleArrivingQueue(WriteCallback log)
        {
            List<IProcess> arrived = new ();

            foreach(IProcess process in Arriving)
            {
                if (process.ArrivalTime.Equals(Clock))
                {
                    arrived.Add(process);
                }
            }

            MoveArrivingToReady(arrived, log);
        }

        private void MoveArrivingToReady(List<IProcess> arrived, WriteCallback log)
        {
            foreach (IProcess process in arrived)
            {
                Arriving.Remove(process);
                Ready.Add(process);

                log($"-----------------");
                log($"Moved PID {process.ID.ToString().PadLeft(4, '0')} from ARRIVING to READY queue.");
                log($"-----------------");
                log($"");
            }
        }

        private void HandleWaitingQueue(WriteCallback log)
        {
            List<IProcess> resumed = new ();

            foreach(IProcess process in Waiting)
            {
                if (process.CanResume())
                {
                    resumed.Add(process);
                }
            }

            MoveWaitingToReady(resumed, log);
        }

        private void MoveWaitingToReady(List<IProcess> resumed, WriteCallback log)
        {
            foreach (IProcess process in resumed)
            {
                Waiting.Remove(process);
                process.ArrivalTime = Clock;
                Ready.Add(process);

                log($"-----------------");
                log($"Moved PID {process.ID.ToString().PadLeft(4, '0')} from WAITING to READY queue.");
                log($"-----------------");
                log($"");
            }
        }

        private void HandleReadyQueue(WriteCallback log)
        {
            if (!Ready.Any())
            {
                log($"-----------------");
                log($"No processes are READY.");
                log($"-----------------");
                return;
            }
            IProcess next = GetNext();
            HandleProcess(log, next);
        }

        private IProcess GetNext()
        {
            return Ready
                .OrderBy(process => process.ArrivalTime)
                .ThenBy(Ready.IndexOf)
                .First();
        }

        private void HandleProcess(WriteCallback log, IProcess process)
        {           
            if (!process.Progress() && process.Suspended)
            {
                Ready.Remove(process);
                Waiting.Add(process);
                ReportSuspension(log, process);
                return;
            }
            
            ReportSuccess(log, process);
            
            if (process.Completed)
            {
                Ready.Remove(process);
                Completed.Add(process);
                ReportCompletion(log, process);
                return;
            }
        }

        private static void ReportSuspension(WriteCallback log, IProcess next)
        {
            log($"-----------------");
            log($"PID {next.ID.ToString().PadLeft(4, '0')} is waiting for input.");
            log($"-----------------");
            log($"");
        }

        private static void ReportSuccess(WriteCallback log, IProcess next)
        {
            log($"-----------------");
            log($"1 step has been executed for PID {next.ID.ToString().PadLeft(4, '0')}.");
            log($"");
            log($"This process is {1.0 * next.CurrentStep / next.Steps:p} complete.");
            log($"-----------------");
            log($"");
        }

        private static void ReportCompletion(WriteCallback log, IProcess process)
        {
            log($"-----------------");
            log($"PID {process.ID.ToString().PadLeft(4, '0')} has completed.");
            log($"");
            log($"Moved PID {process.ID.ToString().PadLeft(4, '0')} from READY to COMPLETED queue.");
            log($"-----------------");
            log($"");
        }
    }
}