using System;
using System.Collections.ObjectModel;

namespace rMind.Robot
{
    public interface IRobotMind
    {
        void Clear();
        object Get(Guid guid);
        object Create<T>() where T : IDevice;
        void Add(Guid guid, object obj);

        IRobotConfig Config { get; }
    }
}
