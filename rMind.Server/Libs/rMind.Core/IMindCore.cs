using System;
using System.Threading;

namespace rMind.Core
{
    public interface IMindCore
    {
        object Switch(int pin);
        bool Check();
        bool Load();

        /// <summary> run device scheduller </summary>
        bool RunScheduler();
    }
}
