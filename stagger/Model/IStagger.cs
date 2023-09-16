namespace Stagger.Model
{
    public interface IStagger
    {
        string Name { get; }
        Queue<IProcess> Ready { get; }
        Queue<IProcess> Waiting { get; }
        Queue<IProcess> Completed { get; }
        bool Idle { get; }
        bool Busy { get; }
        void Work();
    }
}