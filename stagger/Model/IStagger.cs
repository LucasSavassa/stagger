namespace Stagger.Model
{
    public delegate void WriteCallback(string message);
    public interface IStagger
    {
        string Name { get; }
        List<IProcess> Ready { get; }
        List<IProcess> Waiting { get; }
        List<IProcess> Completed { get; }
        bool Idle { get; }
        bool Busy { get; }
        int Length { get; }
        int Current { get; }
        void Add(IProcess process);
        void Work(WriteCallback log);
    }
}