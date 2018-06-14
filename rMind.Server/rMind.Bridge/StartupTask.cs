using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.AppService;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace rMind.Bridge
{
    public sealed class StartupTask : IBackgroundTask
    {
        private Guid m_connectionGuid;
        private static readonly Dictionary<Guid, AppServiceConnection> Connections = new Dictionary<Guid, AppServiceConnection>();

        BackgroundTaskDeferral deferral;
        AppServiceConnection connection;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            deferral = taskInstance.GetDeferral();

            m_connectionGuid = Guid.NewGuid();

            var triggerDetails = (AppServiceTriggerDetails)taskInstance.TriggerDetails;
            var connection = triggerDetails.AppServiceConnection;
            Connections.Add(m_connectionGuid, connection);
            connection.RequestReceived += ConnectionRequestReceived;

            deferral.Complete();
        }

        private async void ConnectionRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // Take out a deferral since we use await
            var appServiceDeferral = args.GetDeferral();
            try
            {
                foreach (var connection in Connections)
                {
                    await SendMessage(connection, args.Request.Message);
                }
            }
            finally
            {
                appServiceDeferral.Complete();
            }
        }

        private async Task SendMessage(KeyValuePair<Guid, AppServiceConnection> connection, ValueSet set)
        {
            var result = await connection.Value.SendMessageAsync(set);
            if (result.Status == AppServiceResponseStatus.Failure)
            {
                return;
            }
        }
    }
}
