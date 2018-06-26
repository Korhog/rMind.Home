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

    public class SetterContainer<T> : TemplateContainer
    {
        public SetterContainer(rMindBaseController parent) : base(parent)
        {
                        
        }

        protected override void Build()
        {
            Header = typeof(T).GetCustomAttribute<DisplayName>()?.Name ?? "Node";
            AccentColor = Colors.DarkRed;

            rMindRow row = new rMindRow
            {
                InputNodeType = rMindNodeConnectionType.Container,
                OutputNodeType = rMindNodeConnectionType.None,
                InputNode = new rMindBaseNode(this)
                {
                    Label = "Input",

                    ConnectionType = rMindNodeConnectionType.Container,
                    NodeOrientation = rMindNodeOriantation.Left,
                    NodeType = rMindNodeType.Input,
                    AttachMode = rMindNodeAttachMode.Single,
                    Theme = new rMindNodeTheme
                    {
                        BaseFill = new SolidColorBrush(Colors.Black),
                        BaseStroke = new SolidColorBrush(Colors.LightGreen)
                    }
                }
            };

            AddRow(row);

            var methods = typeof(T).GetMethods()
                .Where(x => x.IsPublic && x.GetCustomAttributes<rMind.Robot.Setter>().Any())               
                .ToList();

            foreach (var met in methods)
            {
                row = new rMindRow
                {
                    InputNodeType = rMindNodeConnectionType.Value,
                    OutputNodeType = rMindNodeConnectionType.None,
                    InputNode = new rMindBaseNode(this)
                    {
                        Label = met.GetCustomAttribute<DisplayName>()?.Name ?? "Setter",

                        ConnectionType = rMindNodeConnectionType.Value,
                        NodeOrientation = rMindNodeOriantation.Left,
                        NodeType = rMindNodeType.Input,
                        AttachMode = rMindNodeAttachMode.Single,
                        Theme = new rMindNodeTheme
                        {
                            BaseFill = new SolidColorBrush(Colors.Black),
                            BaseStroke = new SolidColorBrush(Colors.Red)
                        }
                    }
                };

                AddRow(row);
            }
        }
    }
}
