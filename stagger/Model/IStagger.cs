namespace Stagger.Model
{
    public interface IStagger
    {
        IEnumerable<IProcess> Ready { get; }
        IEnumerable<IProcess> Waiting { get; }
        IEnumerable<IProcess> Completed { get; }
        bool Idle { get; }
        bool Busy { get; }
        void Work();
    }
}