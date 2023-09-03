using System.Collections.Generic;

namespace Stagger.Model
{
    public interface IStagger
    {
        Queue<IProcess> Ready { get; }
        Queue<IProcess> Waiting { get; }
        Queue<IProcess> Completed { get; }
        bool Idle { get; }
        bool Busy { get; }

        void Work();
    }
}