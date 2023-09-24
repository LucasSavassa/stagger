namespace Stagger.Model
{
    public interface IProcess
    {
        int ID { get; }
        int ArrivalTime { get; set; }
        int ServiceTime { get; }
        int Priority { get; }
        int Steps { get; }
        int CurrentStep { get; }
        int RemainingSteps { get; }
        bool Completed { get; }
        bool Suspended { get; }
        int? ArrivedAt { get; }
        int? CompletedAt { get; }
        bool Progress();
        bool CanResume();
        void Arrive(int clock);
        void Complete(int clock);
    }
}