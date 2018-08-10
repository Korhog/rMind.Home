using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot
{
    public interface IDevice : ILogicNode
    {
        IDeviceConfig Config { get; set; }
        /// <summary> update device state </summary>
        void Update();
    }
}
