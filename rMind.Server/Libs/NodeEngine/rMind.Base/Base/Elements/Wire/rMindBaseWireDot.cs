using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

namespace rMind.Elements
{
    using Draw;
    using Nodes;
    using rMind.Input;
    using Types;
    /// <summary> Ending of wire </summary>
    public partial class rMindBaseWireDot : rMindItemUI, IDrawElement, IInteractElement
    {
        rMindBaseWire m_parent;
        public rMindBaseWire Wire { get { return m_parent; } }

        rMindBaseNode m_node = null;
        public rMindBaseNode Node { get { return m_node; } }

        protected Rectangle m_area;
        protected SolidColorBrush m_area_fill;

        public rMindBaseWireDot(rMindBaseWire parent) 
        {
            m_parent = parent;            
            Init();
        }

        public virtual void Init()
        {
            m_area_fill = new SolidColorBrush(Colors.Black);
            m_area = new Rectangle()
            {
                Fill = m_area_fill,
                Width = 12,
                Height = 12,
                Margin = new Windows.UI.Xaml.Thickness(-6),
                IsHitTestVisible = false,
                RadiusX = 3,
                RadiusY = 3
            };
            m_template.Children.Add(m_area);
            Canvas.SetZIndex(m_template, 10);
            SubscribeInput();
        }

        public rMindBaseController GetController()
        {
            return m_parent?.GetController();
        }

        public Vector2 GetOffset()
        {            
            return new Vector2(0, 0);            
        }

        public override Vector2 SetPosition(Vector2 vector)
        {
            var pos = base.SetPosition(vector);
            Wire.Update();
            return pos;
        }

        public rMindBaseWireDot SetNode(rMindBaseNode node)
        {
            m_node = node;
            if (node != null)
            {
                var r = node.ConnectionType == rMindNodeConnectionType.Container ? 6 : 0;

                m_area.Fill = node.Theme.BaseStroke;

                m_area.RadiusX = r;
                m_area.RadiusY = r;

                Wire.SetWireColor();
                Wire.Update();
            }
            else
            {
                m_area.RadiusX = 3;
                m_area.RadiusY = 3;
                m_area.Fill = m_area_fill;
            }

            return this;
        }

        public void Detach()
        {
            m_area.RadiusX = 3;
            m_area.RadiusY = 3;
            m_area.Fill = m_area_fill;


            if (Node != null)
            {
                var node = Node;
                m_node = null;
                node?.Detach(this);

                if (GetController()?.CheckIsDraggedDot(this) == false)
                {
                    Wire.Delete();
                }
            }
        }

        /// <summary>
        /// Возвращает точку с друго конца нити
        /// </summary>
        public rMindBaseWireDot ReverseDot {
            get
            {
                if (this == Wire.A)
                    return Wire.B;
                return Wire.A;
            }
        }

        public override void SetEnabledHitTest(bool state)
        {
            m_area.IsHitTestVisible = state;
        }
    }
}
