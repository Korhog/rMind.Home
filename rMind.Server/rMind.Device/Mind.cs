using System;
using System.Threading;
using System.Collections.Generic;

using Bifrost.Devices.Gpio;
using Bifrost.Devices.Gpio.Abstractions;
using Bifrost.Devices.Gpio.Core;

using System.Net.WebSockets;

namespace rMind.Device
{
    using Core;
#warning комментарии перевести на английский
    public class Mind : IMindCore
    {        
        IGpioController m_gpioController;
        Dictionary<Guid, Device> m_devices;
        Timer m_timer;

        static int F() { return 4; }
        int f = Mind.F();


        public Mind()
        {
            m_gpioController = null;

            try
            {
                m_gpioController = GpioController.Instance;
            }
            catch (Exception e)
            {
                // ignore
            }
        }

        

        void Execute(object sender)
        {
            foreach(var pair in m_devices)
            {
                Console.WriteLine("Executing device {0}", pair.Key);
                pair.Value.Execute();
            }            
        }


        /// <summary> Переключаем состояние пина </summary>
        public object Switch(int pin)
        {
            if (m_gpioController == null)
            {
                return new
                {
                    success = false,
                    message = "GPIO controller not found"
                };
            }

            using (var gpio = m_gpioController.OpenPin(pin))
            {
                gpio.SetDriveMode(GpioPinDriveMode.Output);

                var state = gpio.Read();
                state = state == GpioPinValue.High ? GpioPinValue.Low : GpioPinValue.High;
                gpio.Write(state);

                return new
                {
                    success = false,
                    state = state,
                };
            }
        }

        public bool Check()
        {
            return m_gpioController != null;
        }

        /// <summary> Load current scheme </summary>
        public bool Load()
        {
            m_devices = new Dictionary<Guid, Device>();
            m_devices[Guid.NewGuid()] = new Blink(this);

            return true;
        }

        public bool RunScheduler()
        {
            int fires = 0;
            m_timer = new Timer(new TimerCallback(Execute), fires, 0, 25000);
            return true;
        }

        public bool StopScheduler()
        {
            return true;
        }

        public async void RunSocket()
        {
            var ws = new ClientWebSocket();
            var uri = new Uri("ws://localhost");

            await ws.ConnectAsync(uri, CancellationToken.None);
        }
    }
    
}
