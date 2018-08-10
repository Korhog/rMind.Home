using rMind.Controls;
using rMind.Robot;
using rMind.Robot.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace rMind.Config.Pages
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class Devices : Page
    {
        public Devices()
        {
            InitializeComponent();
            Loaded += async (sender, args) =>
            {
                var wrappers = await DevicesHelper.Wrappers;
                Menu.Items.Clear();
                foreach (var wrapper in wrappers)
                {
                    var item = new MenuFlyoutItem
                    {
                        Text = wrapper.GetType().GetCustomAttribute<DisplayName>()?.Name ?? "Item"
                    };

                    item.Click += (s, e) =>
                    {
                        AddConfig(wrapper.DeviceType);
                    };

                    Menu.Items.Add(item);
                }
            };
        }

        public void AddConfig(Type device)
        {
            DeviceList.Items.Add(new DeviceConfig
            {
                Name = device.GetCustomAttribute<DisplayName>()?.Name ?? "Device",
                Properties = device
                    .GetProperties()
                    .Where(x => x.GetCustomAttributes<rMind.Robot.Config>().Any())
                    .Select(x => new ConfigProperty {
                        ClassName = x.Name,
                        Name = x.GetCustomAttribute<DisplayName>()?.Name ?? "Property"
                    }).ToList()
            });
        }

        private void OnDeviceClick(object sender, ItemClickEventArgs e)
        {

            var config = e.ClickedItem as DeviceConfig;
            DataContext = config;

            Properties.Children.Clear();
            Properties.RowDefinitions.Clear();
            var row = 0;
            foreach (var prop in config.Properties)
            {
                Properties.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });

                var field = new TextBox {
                    DataContext = prop,
                    Margin = new Thickness(10, 0, 0, 0)
                };

                var bind = new Binding
                {
                    Path = new PropertyPath("Value"),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };


                var label = new TextBlock { Text = prop.Name, VerticalAlignment = VerticalAlignment.Center };

                Grid.SetColumn(field, 1);
                Grid.SetRow(field, row);
                Grid.SetRow(label, row);

                BindingOperations.SetBinding(field, TextFieldBase.TextProperty, bind);
                
                Properties.Children.Add(label);
                Properties.Children.Add(field);

                row++;
            }            
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {

        }
    }
}
