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

    public class GetterContainer<T> : TemplateContainer
    {
        public GetterContainer(rMindBaseController parent) : base(parent)
        {
                        
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
                    OutputNode = new rMindBaseNode(this)
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
