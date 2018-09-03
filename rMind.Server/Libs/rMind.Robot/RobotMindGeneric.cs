using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot
{
    public class RobotMindGeneric
    {
        Dictionary<Guid, object> m_devices;
        public RobotMindGeneric()
        {
            m_devices = new Dictionary<Guid, object>();
        }

        public void Add(Guid guid, object obj)
        {
            m_devices[guid] = obj;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public object Create<T>()
        {
            throw new NotImplementedException();
        }

        public object Get(Guid guid)
        {
            if (m_devices.ContainsKey(guid))
                return m_devices[guid];

            return null;
        }
    }
}
