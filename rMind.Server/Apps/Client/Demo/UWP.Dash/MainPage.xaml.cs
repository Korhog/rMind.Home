using System;
using System.IO;
using System.Net;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWP.Dash
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OnSwitch(object sender, RoutedEventArgs e)
        {
            var address = sLocal.IsOn ? "localhost" : sDestination.IsOn ? "172.30.1.63" : "192.168.0.15";

            WebRequest webRequest = WebRequest.Create(string.Format("http://{0}:5000/api/values/switch/21", address));

            try
            {
                var response = await webRequest.GetResponseAsync();

                using (var stream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var msg = reader.ReadToEnd();
                        textBox.Text += msg + "\n\n";
                    }
                }
            }
            catch(Exception ex)
            {
                textBox.Text = ex.Message;
            }
        }
    }
}
