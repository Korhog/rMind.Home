using rMind.BaseControls.Entities;
using rMind.Robot.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot
{
    public class RobotConfig : IRobotConfig
    {
        Dictionary<Guid, IDevice> m_deviceDict;
        /// <summary> Устройства </summary>
        public ObservableCollection<IDevice> Devices { get; private set; }
        public ObservableCollection<IDeviceConfig> DeviceConfigurations { get; private set; }
        public ITreeItem Root { get; private set; }


        public RobotConfig()
        {
            m_deviceDict = new Dictionary<Guid, IDevice>();
            Root = new TreeFolder();
            Devices = new ObservableCollection<IDevice>();
            DeviceConfigurations = new ObservableCollection<IDeviceConfig>();
        }

        public IDevice this[Guid index]
        {
            get
            {
                if (m_deviceDict.ContainsKey(index))
                    return m_deviceDict[index];
                return null;
            }
        }

        public T Create<T>() where T : IDevice
        {
            var device = Activator.CreateInstance<T>();
            device.Guid = Guid.NewGuid();

            Devices.Add(device);

            device.Config = new DeviceConfig
            {
                Name = typeof(T).GetCustomAttribute<DisplayName>()?.Name ?? "Device",
                DeviceType = typeof(T),
                Properties = typeof(T)
                    .GetProperties()
                    .Where(x => x.GetCustomAttributes<rMind.Robot.Config>().Any())
                    .Select(x => new ConfigProperty
                    {
                        ClassName = x.Name,
                        Value = x.GetCustomAttribute<Config>()?.Default,
                        Name = x.GetCustomAttribute<DisplayName>()?.Name ?? "Property"
                    }).ToList(),
                Guid = device.Guid
            };

            DeviceConfigurations.Add(device.Config);
            m_deviceDict[device.Guid] = device;            
            return device;
        }

        public bool Load(string json)
        {
            throw new NotImplementedException();
        }
    }
}
