using rMind.BaseControls.Entities;
using rMind.Elements;
using rMind.Robot;
using rMind.Robot.Entities;
using rMind.RobotEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class MindPage : Page
    {
        RobotMindGraphController mindGraphController;
        RobotMindGraph mindGraph;

        public MindPage()
        {
            this.InitializeComponent();

            var mind = RobotMind.Current;
            tree.SetRoot(mind.Config.Root);
            tree.DataContext = mind.Config.Root;

            mindGraphController = new RobotMindGraphController(canvas, scroll);
            mindGraph = new RobotMindGraph(mindGraphController);

            mindGraphController.SetController(mindGraph);
            mindGraphController.Draw();

            var a = mindGraph.CreateGetterContainer<rMind.Robot.ExternalDevices.Timer>();
            a.Translate(new Types.Vector2(100, 100));

            canvas.DragOver += (s, e) =>
            {
                e.AcceptedOperation = DataPackageOperation.Move;
            };            
        }

        private void OnCanvasDrop(object sender, DragEventArgs e)
        {
            var dropItem = (tree.Buffer as TreeItem);
            if (dropItem == null)
                return;


            var current = mindGraphController.CurrentController as RobotMindGraph;
            if (current == null)
                return;

            var pos = e.GetPosition(canvas);
           
            var method = current
                        .GetType()
                        .GetMethod("Create")
                        .MakeGenericMethod(dropItem.ClassType);

            var container = method.Invoke(current, new object[] { dropItem.ClassTemplate, dropItem.MethodInfo });
            (container as rMindBaseElement)?.SetPosition(new Types.Vector2(pos.X, pos.Y));
        }
    }
}
