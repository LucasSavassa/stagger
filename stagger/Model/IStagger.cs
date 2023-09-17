namespace Stagger.Model
{
    public delegate void WriteCallback(string message);
    public interface IStagger
    {
        string Name { get; }
        Queue<IProcess> Ready { get; }
        Queue<IProcess> Waiting { get; }
        Queue<IProcess> Completed { get; }
        bool Idle { get; }
        bool Busy { get; }
        int Length { get; }
        int Current { get; }
        void Work(WriteCallback log);
    }
}