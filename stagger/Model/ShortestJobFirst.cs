namespace Stagger.Model
{
    public class ShortestJobFirst : IStagger
    {
        public string Name => "Shortest Job First";
        public List<IProcess> Arriving { get; } = new List<IProcess>();
        public List<IProcess> Waiting { get; } = new List<IProcess>();
        public List<IProcess> Ready { get; } = new List<IProcess>();
        public List<IProcess> Completed { get; } = new List<IProcess>();
        public bool Idle => !Busy;
        public bool Busy =>  Ready.Any() || Waiting.Any() || Arriving.Any() || Current is not null;
        public int Length => 
            (Current?.Steps ?? 0)
              + Ready.Sum(process => process.Steps)
                + Arriving.Sum(process => process.Steps)
                  + Waiting.Sum(process => process.Steps)
                    + Completed.Sum(process => process.Steps);
        public int Progress => 
            (Current?.CurrentStep ?? 0)
              + Ready.Sum(process => process.CurrentStep)
                + Arriving.Sum(process => process.CurrentStep)
                  + Waiting.Sum(process => process.CurrentStep)
                    + Completed.Sum(process => process.CurrentStep);
        public IProcess? Current { get; private set; }
        public int Clock { get; private set; }

        public ShortestJobFirst()
        {
            Clock = 0;
        }

        public ShortestJobFirst(IEnumerable<IProcess> initial) : this()
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
                process.Arrive(Clock);
                Arriving.Remove(process);
                Ready.Add(process);

                log($"-----------------");
                log($"PID {process.ID.ToString().PadLeft(4, '0')} has arrived.");
                log($"");
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
                log($"PID {process.ID.ToString().PadLeft(4, '0')} has received the input it was waiting for.");
                log($"");
                log($"Moved PID {process.ID.ToString().PadLeft(4, '0')} from WAITING to READY queue.");
                log($"-----------------");
                log($"");
            }
        }

        private void HandleReadyQueue(WriteCallback log)
        {
            if (Current is not null)
            {
                HandleProcess(log, Current);
                return;
            }

            if (!Ready.Any())
            {
                log($"-----------------");
                log($"No processes are READY.");
                log($"-----------------");
                log($"");
                return;
            }
            
            IProcess next = Current = GetNext(log);
            HandleProcess(log, next);
        }

        private IProcess GetNext(WriteCallback log)
        {
            IProcess next = Ready
                .OrderBy(process => process.Steps)
                .ThenBy(Ready.IndexOf)
                .First();
            Ready.Remove(next);

            log($"-----------------");
            log($"PID {next.ID.ToString().PadLeft(4, '0')} now owns the CPU.");
            log($"-----------------");
            log($"");

            return next;
        }

        private void HandleProcess(WriteCallback log, IProcess process)
        {           
            if (!process.Progress() && process.Suspended)
            {
                Current = null;
                Ready.Remove(process);
                Waiting.Add(process);
                ReportSuspension(log, process);
                return;
            }
            
            ReportSuccess(log, process);
            
            if (process.Completed)
            {
                Current = null;
                Ready.Remove(process);
                Completed.Add(process);
                process.Complete(Clock);
                ReportCompletion(log, process);
                return;
            }
        }

        private static void ReportSuspension(WriteCallback log, IProcess next)
        {
            log($"-----------------");
            log($"PID {next.ID.ToString().PadLeft(4, '0')} is waiting for input.");
            log($"");
            log($"PID {next.ID.ToString().PadLeft(4, '0')} no longer owns the CPU and has been moved to WAITING queue.");
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

        public void ReportStatistics(WriteCallback log)
        {            
            log($"-----------------");
            log($"Stagger {Name} has executed all processes.");
            log($"");
            log($"Total execution time:");
            log($"  {Clock} time units.");
            log($"");
            log($"Mean execution time:");
            log($"  {1.0 * Completed.Sum(process => process.CompletedAt - process.ArrivedAt) / Completed.Count:N2} time units.");
            log($"");
            log($"Mean waiting time:");
            log($"  {1.0 * Completed.Sum(process => process.CompletedAt - process.ArrivedAt - process.Steps) / Completed.Count:N2} time units.");
            log($"-----------------");
            log($"");
        }
    }
}