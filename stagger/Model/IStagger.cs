namespace Stagger.Model
{
    public delegate void WriteCallback(string message);
    public interface IStagger
    {
        string Name { get; }
        List<IProcess> Arriving { get; }
        List<IProcess> Waiting { get; }
        List<IProcess> Ready { get; }
        List<IProcess> Completed { get; }
        bool Idle { get; }
        bool Busy { get; }
        int Length { get; }
        int Progress { get; }
        int Clock { get; }
        void Arrive(IProcess process);
        void Work(WriteCallback log);
        void ReportStatistics(WriteCallback log);
    }
}