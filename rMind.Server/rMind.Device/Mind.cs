using System;
using System.Threading;
using System.Collections.Generic;

using Windows.ApplicationModel.AppService;

namespace rMind.Device
{
    using Core;
    using Windows.Foundation;
    using Windows.Foundation.Collections;

    public static class AsyncOperationExtension
    {
        public static T WaitResult<T>(this IAsyncOperation<T> awaiter, long mils = 0)
        {
            while (awaiter.Status != AsyncStatus.Completed)
            { Thread.Sleep(10); }

            return awaiter.GetResults();
        }        
    }

#warning комментарии перевести на английский
    public class Mind : IMindCore
    {        
        Dictionary<Guid, Device> m_devices;
        Timer m_timer;

        public Mind()
        {

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
            return new
            {
                success = false,
                message = "GPIO controller not found"
            };            
        }

        public bool Check()
        {
            return false;
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
            RunSocket();
            MindBuilder.Build(this);

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
            using (var connection = new AppServiceConnection())
            {
                connection.AppServiceName = "com.microsoft.rmind.devicebridgeservice";
                connection.PackageFamilyName = "rMind.Bridge-uwp_1.0.0.0_x86__wga7ry0kns2te";

                var status = connection.OpenAsync().WaitResult();
                Console.WriteLine(status);

                switch (status)
                {
                    case AppServiceConnectionStatus.Success:
                        Console.WriteLine("Try send query");

                        var message = new ValueSet();
                        message.Add("Command", "GPIO");
                        message.Add("Pin", 21);

                        var responce = connection.SendMessageAsync(message).WaitResult();
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
}
