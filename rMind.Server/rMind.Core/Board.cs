using Bifrost.Devices.Gpio;
using Bifrost.Devices.Gpio.Core;
using System;

namespace rMind.Core
{
    public class Board
    {
        public static bool Check()
        {
            try
            {
                var gpioController = GpioController.Instance;
                var pin = gpioController.OpenPin(21);
                pin.SetDriveMode(GpioPinDriveMode.Output);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
