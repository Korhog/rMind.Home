using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot
{
    public class ConfigProperty
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public object Value { get; set; }
    }

    public interface IDeviceConfig
    {
        string Name { get; set; }
        Guid Guid { get; set; }
        Type DeviceType { get; set; }
        List<ConfigProperty> Properties { get; set; }
    }

    public class DeviceConfig : IDeviceConfig
    {
        public string Name { get; set; }
        public Guid Guid { get; set; }
        public Type DeviceType { get; set; }
        public List<ConfigProperty> Properties { get; set; }
    }
}
