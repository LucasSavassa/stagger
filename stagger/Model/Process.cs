namespace Stagger.Model
{
    public class Process : IProcess
    {
        private static int _currentID = 0;
        private const double _probability = 0.25;
        public int ID { get; }
        public int ArrivalTime { get; set; }
        public int ServiceTime { get; init; }
        public int Priority { get; init; }
        public int Steps { get; init; }
        public int CurrentStep { get; private set; }
        public int RemainingSteps => Steps - CurrentStep;
        public bool Completed => CurrentStep.Equals(Steps);
        public bool Suspended { get; private set; }
        public int? ArrivedAt { get; private set; }
        public int? CompletedAt { get; private set; }

        public Process(int arrivalTime, int priority, int steps)
        {
            this.ID = ++_currentID;
            this.ArrivalTime = arrivalTime;
            this.ServiceTime = steps;
            this.Priority = priority;
            this.Steps = steps;
            this.CurrentStep = 0;
            this.Suspended = false;
        }

        public bool Progress() 
        {
            if (this.WaitingInput())
            {
                this.Suspended = true;
                return false;
            }

            this.CurrentStep++;
            return true;
        }

        public bool CanResume()
        {
            this.Suspended = this.WaitingInput();
            return !this.Suspended;
        }

        private bool WaitingInput()
        {
            return new Random().NextDouble() <= _probability;
        }

        public void Arrive(int clock)
        {
            this.ArrivedAt ??= clock;
        }

        public void Complete(int clock)
        {
            this.CompletedAt ??= clock;
        }
    }
}