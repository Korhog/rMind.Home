using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot.ExternalDevices
{
    public abstract class ExternalDevice : IDevice
    {
        public Guid Guid { get; set; }
        public IDeviceConfig Config { get; set; }

        public abstract void Update();
    }
}
