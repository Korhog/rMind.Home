using rMind.BaseControls.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot
{
    public interface IRobotConfig
    {
        T Create<T>() where T : IDevice;
        ObservableCollection<IDevice> Devices { get; }
        ObservableCollection<IDeviceConfig> DeviceConfigurations { get; }
        ITreeItem Root { get; }

        // Get device
        IDevice this[Guid index] { get; }
        
        // Load config from JSON
        bool Load(string json);
    }
}
