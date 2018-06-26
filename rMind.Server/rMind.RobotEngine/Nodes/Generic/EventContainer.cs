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

    public class EventContainer<T> : TemplateContainer
    {
        public EventContainer(rMindBaseController parent) : base(parent)
        {
                        
        }

        protected override void Build()
        {
            Header = typeof(T).GetCustomAttribute<DisplayName>()?.Name ?? "Node";
            AccentColor = Colors.SteelBlue;

            foreach(var e in typeof(T).GetEvents())
            {
                var row = new rMindRow
                {                  

                    InputNodeType = rMindNodeConnectionType.None,
                    OutputNodeType = rMindNodeConnectionType.Container,
                    OutputNode = new rMindBaseNode(this)
                    {
                        Label = e.GetCustomAttribute<DisplayName>()?.Name ?? "Event",

                        ConnectionType = rMindNodeConnectionType.Container,
                        NodeOrientation = rMindNodeOriantation.Right,
                        NodeType = rMindNodeType.Output,
                        AttachMode = rMindNodeAttachMode.Multi,
                        Theme = new rMindNodeTheme
                        {
                            BaseFill = new SolidColorBrush(Colors.Black),
                            BaseStroke = new SolidColorBrush(Colors.OrangeRed)
                        }
                    }
                };

                AddRow(row);
            }
        }
    }
}
