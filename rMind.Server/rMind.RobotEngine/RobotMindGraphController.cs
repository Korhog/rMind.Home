using rMind.Robot;
using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Controls;

namespace rMind.RobotEngine
{
    public class RobotMindGraphController : CanvasEx.rMindCanvasController
    {
        public IRobotMind Mind { get; private set; }

        public RobotMindGraphController(Canvas canvas, ScrollViewer scroll) : base (canvas, scroll)
        {
            Mind = new RobotMindGeneric();
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
                var instance = (eventContainer as EventContainer)?.GetInstance();
                if (instance == null)
                    break;

                // Получаем список всех событий контейнера
                var query = eventContainer.Nodes
                    .Where(x => x.GetReverseNodes().Any())
                    .Where(x => x is EventNode)
                    .Select(x => x as EventNode);

                foreach (var eventNode in query)
                {
                    var delegateQuery = eventNode
                        .GetReverseNodes()
                        .Where(x => x.Parent is SetterContainer)
                        .Select(x => x.Parent as SetterContainer);
                    var classEvent = eventNode.EventInfo;
                    foreach (var delegateNode in delegateQuery)
                    {
                        VoidDelegate @delegate = delegateNode.Delegate;
                        if (@delegate != null)
                        {
                            classEvent.RemoveEventHandler(instance, @delegate);
                            classEvent.AddEventHandler(instance, @delegate);
                        }
                    }
                }

                (instance as IDevice)?.Update();
            }
                

            return null;
        }
    }
}
