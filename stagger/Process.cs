namespace Stagger.Model
{
    public class Process : IProcess
    {
        public int ID { get; set; }
        public int ArrivalTime { get; set; }
        public int ServiceTime { get; set; }
        public int Priority { get; set; }
        public int Steps { get; set; }
        public int CurrentStep { get; set; }
        public bool Completed { get; set; }

        public Process() 
        {

        }
    }
}