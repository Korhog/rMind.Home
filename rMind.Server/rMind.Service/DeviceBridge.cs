using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

using Windows.Devices.Gpio;

namespace rMind.Service
{
    public sealed class DeviceBridge : IBackgroundTask
    {
        private BackgroundTaskDeferral m_taskDeferral;
        private AppServiceConnection m_connection;


        public void Run(IBackgroundTaskInstance taskInstance)
        {
            this.m_taskDeferral = taskInstance.GetDeferral();

            taskInstance.Canceled += OnTaskCanceled; 

            // 
            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            m_connection = details.AppServiceConnection;
            m_connection.RequestReceived += OnRequestReceived;
        }

        private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var messageDeferral = args.GetDeferral();
            try
            {
                ValueSet returnData = new ValueSet();

                var controller = GpioController.GetDefault();
                using (var pin = controller.OpenPin(21))
                {
                    pin.SetDriveMode(GpioPinDriveMode.Output);
                    pin.Write(GpioPinValue.High);
                }

                await args.Request.SendResponseAsync(returnData);
            }
            catch (Exception e) {/*ignore*/}
            finally
            {
                messageDeferral.Complete();
            }
        }

        private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (this.m_taskDeferral != null)
            {
                // Complete the service deferral.
                this.m_taskDeferral.Complete();
            }
        }
    }
}
