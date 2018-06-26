using System;
using rMind.Elements;
using rMind.CanvasEx;
using rMind.Storage;

namespace rMind.RobotEngine
{
    public enum ClassTemplate
    {
        None = 0,
        Setter = 1,
        Getter = 2,
        Event = 3
    }

    public class RobotMindGraph : rMindBaseController
    {
        public RobotMindGraph(rMindCanvasController parent) : base(parent)
        {

        }

        /// <summary> Create node from class template  </summary>
        /// <typeparam name="T">IDevice</typeparam>
        /// <param name="silent">Draw if true</param>
        /// <returns></returns>
        public rMindBaseElement Create<T>(ClassTemplate template = ClassTemplate.None) where T : Robot.ILogicNode
        {
            switch(template)
            {
                case ClassTemplate.Event:
                    return CreateEventContainer<T>();
                case ClassTemplate.Getter:
                    return CreateGetterContainer<T>();
                case ClassTemplate.Setter:
                    return CreateSetterContainer<T>();
            }

            return null;

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

        public rMindBaseElement CreateSetterContainer<T>() where T : Robot.ILogicNode
        {
            var container = Activator.CreateInstance(typeof(SetterContainer<T>), new object[] { this }) as SetterContainer<T>;
            container.SetPosition(new Types.Vector2(100, 100));
            AddElement(container);
            return container;
        }
    }
}
