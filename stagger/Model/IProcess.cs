namespace Stagger.Model
{
    public interface IProcess
    {
        int ID { get; }
        int ArrivalTime { get; }
        int ServiceTime { get; }
        int Priority { get; }
        int Steps { get; }
        int CurrentStep { get; }
        bool Completed { get; }
        void Progress();
    }
}