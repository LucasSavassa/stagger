using System.Reflection;

namespace Stagger.Model
{
    public abstract class Stagger : IStagger
    {
        public IEnumerable<IProcess> Ready { get; init; }
        public IEnumerable<IProcess> Waiting { get; init; }
        public IEnumerable<IProcess> Completed { get; init; }
        public bool Idle { get { return !this.Busy; } }
        public bool Busy { get { return Waiting.Any() || Ready.Any(); } }

        public Stagger(IEnumerable<IProcess> ready)
        {
            this.Ready = ready;
            this.Waiting = (IEnumerable<IProcess>) Activator.CreateInstance(ready.GetType());
            this.Completed = (IEnumerable<IProcess>) Activator.CreateInstance(ready.GetType());
        }

        public abstract void Work();
    }
}