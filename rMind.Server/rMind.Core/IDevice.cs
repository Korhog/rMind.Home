using System;
using System.Collections.Generic;
using System.Text;

namespace rMind.Core
{
#warning написать корректное описание на английском
    /// <summary> 
    /// Интерфейс базового устройства. Устройство это некий Wrapper для IoT устройств. 
    /// На текущей стадии устройства являются виртуальным GPIO 
    /// </summary>
    public interface IDevice
    {
        void Execute();
        void Update();
    }
}
