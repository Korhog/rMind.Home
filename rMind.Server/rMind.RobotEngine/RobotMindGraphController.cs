using rMind.Robot;
using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Controls;

namespace rMind.RobotEngine
{
    public class RobotMindGraphController : CanvasEx.rMindCanvasController
    {
        public RobotMindGraphController(Canvas canvas, ScrollViewer scroll) : base (canvas, scroll)
        {

        }

        public IRobotMind BuildMind()
        {
            var controller = CurrentController;

            // Ищем активаторы
            var eventContainerQuery = controller
                .Items
                .Where(x => x.GetType().GetGenericTypeDefinition() == typeof(EventContainer<>));

            foreach(var eventContainer in eventContainerQuery)
            {
                var classType = eventContainer.GetType().GetGenericArguments().FirstOrDefault();
                
                var instance = Activator.CreateInstance(classType, new object[] { }) ;

                var classEvent = classType.GetEvents().FirstOrDefault();

                VoidDelegate @delegate = () => {
                    Console.WriteLine("Line");
                };

                classEvent.AddEventHandler(instance, @delegate);
                (instance as IDevice)?.Update();
            }
                

            return null;
        }
    }
}
