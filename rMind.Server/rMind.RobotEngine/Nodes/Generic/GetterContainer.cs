using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml.Media;

namespace rMind.RobotEngine
{
    using rMind.Nodes;
    using rMind.Content.Row;
    using rMind.Elements;
    using rMind.Robot;
    using Windows.UI;

    public class GetterContainer : TemplateContainer
    {
        public GetterContainer(RobotMindGraph parent) : base(parent)
        {

        }
    }

    public class GetterContainer<T> : GetterContainer
    {
        public GetterContainer(RobotMindGraph parent) : base(parent)
        {
                        
        }

        public override object GetInstance()
        {
            var root = Parent.CanvasController as RobotMindGraphController;
            if (root == null)
                return null;

            var mind = root.Mind;
            if (mind.Get(Guid) == null)
            {
                var instance = Activator.CreateInstance<T>();
                mind.Add(Guid, instance);
            }

            return mind.Get(Guid);
        }

        protected override void Build()
        {
            Header = typeof(T).GetCustomAttribute<DisplayName>()?.Name ?? "Node";
            AccentColor = Colors.DeepSkyBlue;

            foreach(var prop in typeof(T).GetProperties())
            {
                var row = new rMindRow
                {
                    InputNodeType = rMindNodeConnectionType.None,
                    OutputNodeType = rMindNodeConnectionType.Value,
                    OutputNode = new PropertyNode(this, prop)
                    {
                        Label = prop.GetCustomAttribute<DisplayName>()?.Name ?? "Event",

                        ConnectionType = rMindNodeConnectionType.Value,
                        NodeOrientation = rMindNodeOriantation.Right,
                        NodeType = rMindNodeType.Output,
                        AttachMode = rMindNodeAttachMode.Multi,
                        Theme = new rMindNodeTheme
                        {
                            BaseFill = new SolidColorBrush(Colors.Black),
                            BaseStroke = new SolidColorBrush(Colors.DeepSkyBlue)
                        }
                    }
                };

                AddRow(row);
            }
        }
    }
}
