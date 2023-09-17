namespace Stagger.Model
{
    public class Process : IProcess
    {
        private static int _currentID = 0;
        public int ID { get; }
        public int ArrivalTime { get; init; }
        public int ServiceTime { get; init; }
        public int Priority { get; init; }
        public int Steps { get; init; }
        public int CurrentStep { get; private set; }
        public bool Completed { get { return CurrentStep.Equals(Steps); } }

        public Process(int arrivalTime, int priority, int steps)
        {
            this.ID = ++_currentID;
            this.ArrivalTime = arrivalTime;
            this.ServiceTime = steps;
            this.Priority = priority;
            this.Steps = steps;
            this.CurrentStep = 0;
        }

        public void Progress() 
        {
            this.CurrentStep++;
        }
    }
}