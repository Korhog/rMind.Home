using System;
using System.Reflection;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

using rMind.Controls;
using rMind.Robot;
using rMind.Robot.Utils;
using rMind.BaseControls.Entities;
using System.Linq;
using rMind.RobotEngine;
using rMind.Robot.Entities;

namespace rMind.Config.Pages
{
    public sealed partial class Devices : Page
    {
        IRobotMind mind;

        public Devices()
        {
            InitializeComponent();
            mind = RobotMind.Current;

            Loaded += async (sender, args) =>
            {
                var wrappers = await DevicesHelper.Wrappers;
                MenuFlyout menu = new MenuFlyout();
                menu.Items.Clear();

                var folder = new MenuFlyoutItem
                {
                    Text = "Folder"
                };
                folder.Click += (s, e) => AddConfig();
                menu.Items.Add(folder);

                foreach (var wrapper in wrappers)
                {
                    var item = new MenuFlyoutItem
                    {
                        Text = wrapper.Name
                    };

                    item.Click += (s, e) =>
                    {
                        AddConfig(wrapper.DeviceType);
                    };

                    menu.Items.Add(item);
                }

                tree.DataContext = mind.Config.Root;
                tree.SetRoot(mind.Config.Root);
                tree.AddButton.Flyout = menu;
            };
        }

        /// <summary> Добавление конфига </summary>
        /// <param name="device"></param>
        public void AddConfig(Type device = null)
        {
            if (device == null)
            {
                (tree.DataContext as TreeFolder)?.AddFolder(new TreeFolder()
                {
                    Name = "Folder"
                });

                return;
            }

            var method = mind.Config
                .GetType()
                .GetMethod("Create")
                .MakeGenericMethod(device);

            var cfg = method.Invoke(
                mind.Config, 
                new object[] { }
                );


            var root = (tree.DataContext as TreeFolder);
            if (root == null)
                return;

            var drvFolder = root.AddFolder(new TreeFolder {
                Name = device.GetCustomAttribute<DisplayName>()?.Name ?? "Driver"
            });

            var setters = device.GetMethods().Where(x => x.IsPublic && x.GetCustomAttributes<rMind.Robot.Setter>().Any());
            if (setters.Any())
            {
                var fold = drvFolder.AddFolder(new TreeFolder
                {
                    Name = "Methods"
                });

                foreach(var setter in setters)
                {
                    fold.AddItem(new TreeItem
                    {
                        Name = setter.GetCustomAttribute<DisplayName>()?.Name,
                        ClassTemplate = ClassTemplate.Setter, 
                        MethodInfo = setter,
                        ClassType = device,
                        Guid = device.GUID
                    });
                }
             }

            //
            var events = device.GetEvents();
            // Если есть cобытия
            if (events.Any())
            {
                drvFolder.AddItem(new TreeItem {
                    Name = "Events",
                    ClassTemplate = ClassTemplate.Event,
                    ClassType = device,
                    Guid = device.GUID
                });
            }

            var props = device.GetProperties();
            // Если есть cобытия
            if (props.Any())
            {
                drvFolder.AddItem(new TreeItem
                {
                    Name = "Props",
                    ClassTemplate = ClassTemplate.Getter,
                    ClassType = device,
                    Guid = device.GUID
                });
            }            
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
