using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot
{
    public interface IRobotMind
    {
        void Clear();
        object Get(Guid guid);
        object Create<T>();
        void Add(Guid guid, object obj);
    }
}
