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

    public abstract class SetterContainer : TemplateContainer
    {
        public SetterContainer(RobotMindGraph parent) : base(parent)
        {
            
        }

        VoidDelegate @delegate;
        public VoidDelegate Delegate
        {
            get
            {
                if (@delegate == null)
                {
                    @delegate = GenerateDelegate();
                }
                return @delegate;
            }
        }

        protected abstract VoidDelegate GenerateDelegate();
    }

    public class SetterContainer<T> : SetterContainer
    {
        protected MethodInfo m_methodInfo;
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

        public SetterContainer(RobotMindGraph parent, MethodInfo info) : base(parent)
        {
            m_methodInfo = info;
            InitArguments(info);
        }

        protected virtual void InitArguments(MethodInfo info)
        {
            Header = info.GetCustomAttribute<DisplayName>()?.Name ?? "Node";
            AccentColor = Colors.LimeGreen;

            var paramList = m_methodInfo.GetParameters();
            foreach(var param in paramList)
            {
                rMindRow row = new rMindRow
                {
                    InputNodeType = rMindNodeConnectionType.Value,
                    OutputNodeType = rMindNodeConnectionType.None,
                    InputNode = new ParameterNode(this, param)
                    {
                        Label = param.Name,

                        ConnectionType = rMindNodeConnectionType.Value,
                        NodeOrientation = rMindNodeOriantation.Left,
                        NodeType = rMindNodeType.Input,
                        AttachMode = rMindNodeAttachMode.Single,
                        Theme = new rMindNodeTheme
                        {
                            BaseFill = new SolidColorBrush(Colors.Black),
                            BaseStroke = new SolidColorBrush(Colors.DarkGoldenrod)
                        }
                    }
                };

                AddRow(row);
            }
        }

        protected override void Build()
        {
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
        }

        protected override VoidDelegate GenerateDelegate()
        {
            var method = m_methodInfo;
            var instance = GetInstance();
            if (instance == null)
                return null;

            var paramList = Nodes.Where(x => x is ParameterNode).Select(x => x as ParameterNode).ToArray();

            // Пока мы не используем ппараметры по умолчанию
            if (paramList.Where(x => !x.GetReverseNodes().Any()).Any())
                return null;

            var funcs = paramList
                .Select(x => x.GetReverseNodes().FirstOrDefault() as PropertyNode)
                .Select(x => x.Func())
                .ToArray();

            VoidDelegate @delegate = () =>
            {
                var props = funcs.Select(x => x.Invoke()).ToArray();
                method.Invoke(instance, props);
            };            
            return @delegate;
        }
    }
}
