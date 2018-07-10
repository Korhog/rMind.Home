using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using rMind.Robot;
using rMind.CanvasEx;
using rMind.RobotEngine;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace Editor
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        RobotMindGraphController canvasController;

        public MainPage()
        {
            this.InitializeComponent();
            canvasController = new RobotMindGraphController(canvas, scroll);

            var contoller = new RobotMindGraph(canvasController);
            contoller.Shadow = MainShadow;
            canvasController.SetController(contoller);
            canvasController.Draw();

            MainShadow.Receivers.Add(receiver);

            var logger = Logger.Current();
            logger.OnMessage += (msg) =>
            {
                log.Text = log.Text + "\n" + msg;
            };

            Loaded += GenerateMenu;
        }

        void GenerateMenu(object sender, RoutedEventArgs args)
        {
            var assembly = typeof(ILogicNode).Assembly;
            var nodes = assembly.GetTypes()
                .Where(x => typeof(IDevice).IsAssignableFrom(x))
                .Where(x => x.IsClass)
                .ToList();

            if (nodes.Count > 0)
            {
                

                MenuFlyout menu = new MenuFlyout();
                MenuFlyoutSubItem item;
                foreach (var node in nodes)
                {
                    var guid = Guid.NewGuid();

                    item = new MenuFlyoutSubItem
                    {
                        Text = node.GetCustomAttribute<DisplayName>()?.Name ?? "Node"
                    };

                    // Если есть cобытия
                    var events = node.GetEvents();
                    if (events.Any())
                    {
                        var eventsItem = new RobotMenuFlyoutItem
                        {
                            Text = "Events",
                            ClassTemplate = ClassTemplate.Event,
                            ClassType = node,
                            Guid = guid
                        };

                        eventsItem.Click += OnMenuClick;

                        item.Items.Add(eventsItem);
                    }

                    // Если есть cобытия
                    var props = node.GetProperties();
                    if (props.Any())
                    {
                        var propsItem = new RobotMenuFlyoutItem
                        {
                            Text = "Properties",
                            ClassTemplate = ClassTemplate.Getter,
                            Guid = guid,
                            ClassType = node
                        };

                        propsItem.Click += OnMenuClick;
                        item.Items.Add(propsItem);
                    }

                    // Если есть cобытия
                    var setters = node.GetMethods().Where(x => x.IsPublic && x.GetCustomAttributes<rMind.Robot.Setter>().Any());
                    if (setters.Any())
                    { 
                        var settersItem = new MenuFlyoutSubItem
                        {
                            Text = "Setters",
                        };

                        foreach(var setter in setters)
                        {
                            var setterItem = new RobotMenuFlyoutItem
                            {
                                ClassTemplate = ClassTemplate.Setter,
                                ClassType = node,
                                Guid = guid,
                                Text = setter.GetCustomAttributes<DisplayName>().FirstOrDefault()?.Name ?? "None",
                                Method = setter
                            };

                            setterItem.Click += OnMenuClick;
                            settersItem.Items.Add(setterItem);
                        }

                        
                        item.Items.Add(settersItem);
                    }

                    menu.Items.Add(item);
                }               

                canvas.ContextFlyout = menu;
            }
        }

        private void OnMenuClick(object sender, RoutedEventArgs args) 
        {
            RobotMenuFlyoutItem robotMenu = sender as RobotMenuFlyoutItem;
            var controller = canvasController.CurrentController as RobotMindGraph;
            if (controller == null)
                return;

            var method = controller.GetType().GetMethod("Create")?.MakeGenericMethod(robotMenu.ClassType);
            var instance = method?.Invoke(controller, new object[] {
                robotMenu.ClassTemplate,
                robotMenu.Method
            });

            if (instance is TemplateContainer)
            {
                (instance as TemplateContainer).Guid = robotMenu.Guid;
            }
        }

        private void OnBuild(object sender, RoutedEventArgs e)
        {
            canvasController.BuildMind();
        }
    }
}
