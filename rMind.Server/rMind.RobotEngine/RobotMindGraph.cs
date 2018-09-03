using System;
using rMind.Elements;
using rMind.CanvasEx;
using rMind.Storage;
using Windows.UI.Xaml.Media;
using System.Reflection;
using rMind.Robot;

namespace rMind.RobotEngine
{
    public class RobotMindGraph : rMindBaseController
    {
        public RobotMindGraph(rMindCanvasController parent) : base(parent)
        {

        }

        /// <summary> Create node from class template  </summary>
        /// <typeparam name="T">IDevice</typeparam>
        /// <param name="silent">Draw if true</param>
        /// <returns></returns>
        public rMindBaseElement Create<T>(ClassTemplate template = ClassTemplate.None, MethodInfo info = null)  where T : Robot.ILogicNode
        {
            rMindBaseElement result = null;
            switch (template)
            {
                case ClassTemplate.Event:
                    result = CreateEventContainer<T>();
                    break;
                case ClassTemplate.Getter:
                    result = CreateGetterContainer<T>();
                    break;
                case ClassTemplate.Setter:
                    result = CreateSetterContainer<T>(info);
                    break;
            }
            return result;
        }

        public rMindBaseElement CreateEventContainer<T>() where T : Robot.ILogicNode
        {
            var container = Activator.CreateInstance(typeof(EventContainer<T>), new object[] { this }) as EventContainer<T>;
            container.SetPosition(new Types.Vector2(100, 100));
            AddElement(container);
            return container;
        }

        public rMindBaseElement CreateGetterContainer<T>() where T : Robot.ILogicNode
        {
            var container = Activator.CreateInstance(typeof(GetterContainer<T>), new object[] { this }) as GetterContainer<T>;
            container.SetPosition(new Types.Vector2(100, 100));
            AddElement(container);
            return container;
        }

        public rMindBaseElement CreateSetterContainer<T>(MethodInfo info) where T : Robot.ILogicNode
        {
            var container = Activator.CreateInstance(typeof(SetterContainer<T>), new object[] { this, info }) as SetterContainer<T>;
            container.SetPosition(new Types.Vector2(100, 100));
            AddElement(container);
            return container;
        }
    }
}
