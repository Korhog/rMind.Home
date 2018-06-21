using System;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace rMind.Elements
{
    using Storage;
    using Draw;
    using Types;
    using ColorContainer;
    using Windows.UI;

    /// <summary> Wire to connect nodes </summary>
    public partial class rMindBaseWire : IDrawElement, IStorageObject
    {
        protected bool m_direction_right;

        protected rMindColors colors;
        protected rMindBaseWireDot m_a_dot;
        protected rMindBaseWireDot m_b_dot;

        Path m_bezie;
        PathGeometry m_bezie_geometry;
        PathFigure m_bezie_figure;
        BezierSegment m_bezie_segment;

        Polyline m_line;

        public rMindBaseWireDot A { get { return m_a_dot; } }
        public rMindBaseWireDot B { get { return m_b_dot; } }
        public Path Line { get { return m_bezie; } }

        rMindBaseController m_parent;

        public rMindBaseWire(rMindBaseController parent)
        {
            m_parent = parent;

            m_a_dot = new rMindBaseWireDot(this);
            m_b_dot = new rMindBaseWireDot(this);

            m_line = new Polyline()
            {
                Stroke = new SolidColorBrush(Windows.UI.Colors.WhiteSmoke),
                StrokeThickness = 6,
                IsHitTestVisible = false
            };

            m_bezie_figure = new PathFigure();
            m_bezie_geometry = new PathGeometry();
            m_bezie_geometry.Figures.Add(m_bezie_figure);
            colors = ColorContainer.rMindColors.Current();
            m_bezie = new Path()
            {
                Stroke = colors.GetSolidBrush(Windows.UI.Colors.GhostWhite),
                StrokeThickness = 2,
                IsHitTestVisible = false,
                Data = m_bezie_geometry
            };

            m_bezie_segment = new BezierSegment();
            m_bezie_figure.Segments.Add(m_bezie_segment);
            Canvas.SetZIndex(Line, 10);
        }

        public rMindBaseController GetController()
        {
            return m_parent;         
        }

        public void Init()
        {

        }

        /// <summary> Update line </summary>
        public virtual void Update()
        {
            if (m_direction_right != (A.Position.X > B.Position.X))
            {
                m_direction_right = A.Position.X > B.Position.X;
                SetWireColor();
            }

            m_bezie_figure.StartPoint = new Point(A.Position.X, A.Position.Y);


            var midPoint = Math.Min(A.Position.X, B.Position.X) + Math.Abs(A.Position.X - B.Position.X) / 2;

            m_bezie_segment.Point1 = new Point(midPoint, A.Position.Y);
            m_bezie_segment.Point2 = new Point(midPoint, B.Position.Y);
            m_bezie_segment.Point3 = new Point(B.Position.X, B.Position.Y);

            var points = new PointCollection();
            points.Add(new Point(A.Position.X, A.Position.Y));
            points.Add(new Point(B.Position.X, B.Position.Y));

            m_line.Points = points;
        }

        public virtual void SetWireColor()
        {
            if (m_a_dot.Node == null || m_b_dot.Node == null)
            {
                m_bezie.Stroke = rMindColors.Current().GetSolidBrush(Colors.White);
                return;
            }      

            m_bezie.Stroke = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 0),
                GradientStops = new GradientStopCollection
                    {
                        new GradientStop
                        {
                            Color = m_direction_right ? B.Node.Theme.BaseStroke.Color : A.Node.Theme.BaseStroke.Color,
                            Offset = 0
                        },
                         new GradientStop
                        {
                            Color = m_direction_right ? A.Node.Theme.BaseStroke.Color : B.Node.Theme.BaseStroke.Color,
                            Offset = 1
                        },
                    }
            };           
        }

        
        public Vector2 GetOffset() { return new Vector2(0, 0); }

        public void Delete()
        {
            A.Detach();
            B.Detach();

            GetController()?.RemoveWire(this);
        }   
        
        public void SetEnabledHitTest(bool state)
        {
            Line.IsHitTestVisible = state;
            A.SetEnabledHitTest(state);
            B.SetEnabledHitTest(state);
        }
    }
}
