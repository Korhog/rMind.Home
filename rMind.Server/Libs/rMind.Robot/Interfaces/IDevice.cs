using System;

namespace rMind.Robot
{
    public interface IDevice : ILogicNode
    {
        Guid Guid { get; set; }
        /// <summary> Device config </summary>
        IDeviceConfig Config { get; set; }
        
        /// <summary> Update device state </summary>
        void Update();
    }
}