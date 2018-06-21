using System.Linq;
using System.Collections.Generic;    

using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace rMind.Nodes
{  
    using Draw;
    using ColorContainer;
    using Elements;
    using rMind.Input;
    using Windows.UI.Xaml;
    using Windows.UI.Text;

    public enum rMindNodeType
    {
        None,
        Input,
        Output
    }

    public enum rMindNodeOriantation
    {
        None,
        Left,
        Right,
        Top,
        Bottom
    }

    public enum rMindNodeAttachMode
    {
        Single,
        Multi
    }

    public enum rMindNodeConnectionType
    {
        None,
        Container,
        Value
    }

    public class rMindNodeTheme
    {
        public SolidColorBrush BaseFill;
        public SolidColorBrush BaseStroke;
        public SolidColorBrush OveredFill;
        public SolidColorBrush OveredStroke;

        public static rMindNodeTheme Theme(Color color)
        {

            var colors = rMindColors.Current();

            return new rMindNodeTheme
            {
                BaseFill = colors.GetSolidBrush(Colors.DarkGray),
                BaseStroke = colors.GetSolidBrush(Colors.Black),
                OveredFill = colors.GetSolidBrush(Colors.DarkGray),
                OveredStroke = colors.GetSolidBrush(Colors.YellowGreen)
            };
        }

        public static rMindNodeTheme Theme()
        {
            var colors = rMindColors.Current();

            return new rMindNodeTheme
            {
                BaseFill = colors.GetSolidBrush(Colors.DarkGray),
                BaseStroke = colors.GetSolidBrush(Colors.Black),
                OveredFill = colors.GetSolidBrush(Colors.DarkGray),
                OveredStroke = colors.GetSolidBrush(Colors.YellowGreen)
            };
        }
    }

    public struct rMindNodeDesc
    {
        public rMindNodeType NodeType;
        public rMindNodeOriantation NodeOrientation;
        public rMindNodeAttachMode AttachMode { get; set; }
        public rMindNodeConnectionType ConnectionType { get; set; }
    }

    public partial class rMindBaseNode : rMindItemUI, IDrawElement, IInteractElement
    {
        public int Index { get { return m_parent.Nodes.IndexOf(this); } }

        rMindNodeTheme m_theme;
        public rMindNodeTheme Theme { 
            get
            {
                if (m_parent.NodeTheme != null && m_use_accent_color)
                    return m_parent.NodeTheme;

                if (m_theme == null)
                    m_theme = rMindNodeTheme.Theme();

                return m_theme;
            }
            set
            {
                m_theme = value;
                UpdateAccentColor();
            }
        }

        int m_row = 0;
        int m_row_span = 1;

        int m_col = 0;

        public int Column {
            get
            {
                return m_col;
            }
            set
            {
                m_col = value;
                Grid.SetColumn(m_template, m_col);
            }
        }

        public int Row
        {
            get
            {
                return m_row;
            }
            set
            {
                m_row = value;
                Grid.SetRow(m_template, m_row);
            }
        }

        public int RowSpan
        {
            get
            {
                return m_row_span;
            }
            set
            {
                m_row_span = value;
                Grid.SetRowSpan(m_template, m_row_span);
            }
        }

        protected bool m_use_accent_color = false;
        public bool UseAccentColor {
            get { return m_use_accent_color; }
            set
            {
                if (m_use_accent_color == value)
                    return;

                m_use_accent_color = value;
                UpdateAccentColor();                
            }
        }

        public rMindNodeType NodeType { get; set; } = rMindNodeType.None;
        rMindNodeOriantation m_node_orientation = rMindNodeOriantation.None;
        rMindNodeAttachMode m_attach_mode = rMindNodeAttachMode.Single;
        rMindNodeConnectionType m_connection_type = rMindNodeConnectionType.Container;

        public rMindNodeAttachMode AttachMode { get { return m_attach_mode; } set { m_attach_mode = value; } }
        public rMindNodeConnectionType ConnectionType {
            get { return m_connection_type; }
            set { SetConnectionType(value); }
        }

        protected virtual void SetConnectionType(rMindNodeConnectionType connectionType)
        {
            if (m_connection_type == connectionType)
                return;

            m_connection_type = connectionType;
            var r = m_connection_type == rMindNodeConnectionType.Container ? 10 : 3;

            m_area.RadiusX = r;
            m_area.RadiusY = r;
        }

        public rMindNodeOriantation NodeOrientation
        {
            get { return m_node_orientation; }
            set { SetNodeOrientation(value); }
        }

        protected virtual void SetNodeOrientation(rMindNodeOriantation orientation)
        {
            if (m_node_orientation == orientation)
                return;

            m_node_orientation = orientation;
            Thickness thickness;

            m_template.ColumnDefinitions.Clear();
            m_template.RowDefinitions.Clear();

            switch(orientation)
            {
                case rMindNodeOriantation.Left:
                    m_template.VerticalAlignment = VerticalAlignment.Center;
                    m_template.HorizontalAlignment = HorizontalAlignment.Left;

                    m_template.ColumnDefinitions.Add(new ColumnDefinition());
                    m_template.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    Grid.SetColumn(m_area, 0);
                    Grid.SetColumn(m_hit_area, 0);
                    Grid.SetColumn(m_label, 1);

                    Grid.SetRow(m_area, 0);
                    Grid.SetRow(m_hit_area, 0);
                    Grid.SetRow(m_label, 0);



                    thickness = new Thickness(2, 6, 2, 6);
                    break;
                case rMindNodeOriantation.Right:
                    m_template.VerticalAlignment = VerticalAlignment.Center;
                    m_template.HorizontalAlignment = HorizontalAlignment.Right;

                    m_template.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                    m_template.ColumnDefinitions.Add(new ColumnDefinition ());

                    Grid.SetColumn(m_area, 1);
                    Grid.SetColumn(m_hit_area, 1);
                    Grid.SetColumn(m_label, 0);

                    Grid.SetRow(m_area, 0);
                    Grid.SetRow(m_hit_area, 0);
                    Grid.SetRow(m_label, 0);

                    thickness = new Thickness(2, 6, 2, 6);
                    break;
                case rMindNodeOriantation.Top:
                    m_template.VerticalAlignment = VerticalAlignment.Top;
                    m_template.HorizontalAlignment = HorizontalAlignment.Center;

                    m_template.RowDefinitions.Add(new RowDefinition());
                    m_template.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    Grid.SetColumn(m_area, 0);
                    Grid.SetColumn(m_hit_area, 0);
                    Grid.SetColumn(m_label, 0);

                    Grid.SetRow(m_area, 0);
                    Grid.SetRow(m_hit_area, 0);
                    Grid.SetRow(m_label, 1);

                    thickness = new Thickness(6, 2, 6, 2);
                    break;
                case rMindNodeOriantation.Bottom:
                    m_template.VerticalAlignment = VerticalAlignment.Bottom;
                    m_template.HorizontalAlignment = HorizontalAlignment.Center;

                    m_template.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    m_template.RowDefinitions.Add(new RowDefinition() );

                    Grid.SetColumn(m_area, 0);
                    Grid.SetColumn(m_hit_area, 0);
                    Grid.SetColumn(m_label, 0);

                    Grid.SetRow(m_area, 1);
                    Grid.SetRow(m_hit_area, 1);
                    Grid.SetRow(m_label, 0);

                    thickness = new Thickness(6, 2, 6, 2);
                    break;
                default:
                    m_template.ColumnDefinitions.Add(new ColumnDefinition());
                    m_template.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                    Grid.SetColumn(m_area, 0);
                    Grid.SetColumn(m_hit_area, 0);
                    Grid.SetColumn(m_label, 1);

                    Grid.SetRow(m_area, 0);
                    Grid.SetRow(m_hit_area, 0);
                    Grid.SetRow(m_label, 0);

                    thickness = new Thickness(2, 6, 2, 6);
                    break;
            }

            Margin = thickness;
        }

        rMindBaseElement m_parent;
        public rMindBaseElement Parent { get { return m_parent; } }

        protected Rectangle m_area;
        protected Rectangle m_hit_area;

        protected TextBlock m_label;
        public string Label {
            get { return m_label.Text; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    m_label.Visibility = Visibility.Collapsed;
                }

                m_label.Visibility = Visibility.Visible;
                m_label.Text = value;
            }
        }

        public Brush Fill {
            get { return m_area.Fill; }
            set
            {
                if (UseAccentColor) return;
                m_area.Fill = value;
            }
        }

        public Brush Stroke
        {
            get { return m_area.Stroke; }
            set
            {
                if (UseAccentColor) return;
                m_area.Stroke = value;
            }
        }

        public rMindBaseNode(rMindBaseElement parent) : base()
        {
            m_parent = parent;
            m_attached_dots = new List<rMindBaseWireDot>();
            Init();
        }

        public virtual void Init()
        {
            var r = m_connection_type == rMindNodeConnectionType.Container ? 10 : 3;

            m_label = new TextBlock
            {
                Visibility = Visibility.Collapsed,
                IsHitTestVisible = false,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(4, 0, 0, 0),
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new FontFamily("Consolas"),
                Foreground = new SolidColorBrush(Colors.White)
            };

            m_template.Children.Add(m_label);

            m_hit_area = new Rectangle
            {
                Fill = new SolidColorBrush(Colors.Transparent)
            };
            m_template.Children.Add(m_hit_area);

            m_area = new Rectangle {
                Width = 20,
                Height = 20,
                RadiusX = r,
                RadiusY = r,
                StrokeThickness = 2,
                IsHitTestVisible = false,
                Fill = new SolidColorBrush(Colors.Black),
                Stroke = new SolidColorBrush(Colors.White)
            };

            UpdateAccentColor();

            Margin = new Windows.UI.Xaml.Thickness(2, 6, 2, 6);
            m_template.Children.Add(m_area);

            SubscribeInput();
        }

        public rMindBaseController GetController()
        {
            return m_parent.GetController();
        }

        public Types.Vector2 GetOffset()
        {
            var localOffset = Parent.GetOffset();

            var cd = Template.ColumnDefinitions;
            var local_w = 0.0;             

            double w = 0.0;

            // горизонтальный оффсет
            switch (m_node_orientation)
            {
                case rMindNodeOriantation.Left:
                    if (Template.Visibility == Visibility.Collapsed)
                        local_w = 0;
                    else 
                        local_w = cd
                            .Where(col => cd.IndexOf(col) < Grid.GetColumn(m_area))
                            .Sum(x => x.ActualWidth) + cd[Grid.GetColumn(m_area)].ActualWidth / 2;

                    cd = Parent.Template.ColumnDefinitions;
                    w = cd.Where(col => cd.IndexOf(col) < Grid.GetColumn(Template))
                        .Select(col => col.ActualWidth)
                        .Sum() + local_w;
                    break;
                    
                case rMindNodeOriantation.Right:
                    if (Template.Visibility == Visibility.Collapsed)
                        local_w = 0;
                    else
                        local_w = cd[Grid.GetColumn(m_area)].ActualWidth / 2;

                    w = Parent.Template.ActualWidth - local_w;
                    break;
            }  

            var h = 0.0;
            if (!Parent.Expanded)
            {
                h = Parent.Template.ActualHeight / 2;
            }
            else
            {
                var rd = Parent.Template.RowDefinitions;
                h = rd.Where(row => rd.IndexOf(row) < Grid.GetRow(Template)).Sum(row => row.ActualHeight);
                var full_h = rd.Where(row => rd.IndexOf(row) < Grid.GetRow(Template) + RowSpan).Sum(row => row.ActualHeight);
                h = h + (full_h - h) / 2;
            }

            return localOffset + new Types.Vector2(w, h);
        }

        #region input        
        private void onPointerEnter(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            GetController().SetOveredNode(this);

            if (m_attach_mode == rMindNodeAttachMode.Single && m_attached_dots.Count > 0)
                return;

            Glow(true);
        }


        private void onPointerExit(object sender, PointerRoutedEventArgs e)
        {
            GetController().SetOveredNode(null);
            Glow(false);
        }

        private void onPointerUp(object sender, PointerRoutedEventArgs e)
        {

        }

        #endregion

        protected override void Glow(bool state)
        {

            m_area.Stroke = state ? Theme.OveredStroke : Theme.BaseStroke;
        }

        #region attached dots

        List<rMindBaseWireDot> m_attached_dots;

        protected virtual bool ValidateAttach(rMindBaseWireDot dot)
        {
            if (m_attach_mode == rMindNodeAttachMode.Single && m_attached_dots.Count > 0)
                return false;

            return true;
        }
        /// <summary> Attach wire dot </summary>
        /// <param name="dot">rMindBaseWireDot</param>
        public void Attach(rMindBaseWireDot dot)
        {
            if (!ValidateAttach(dot))
            {
                dot.Wire.Delete();
                return;
            }

            m_attached_dots.Add(dot.SetNode(this));
            Update();            
        }

        /// <summary> Отсоединение всех узлов </summary>        
        public void Detach()
        {
            while(m_attached_dots.Count > 0)            
                Detach(m_attached_dots[0]);
        }

        /// <summary> Detach wire dot </summary>
        /// <param name="dot">rMindBaseWireDot</param>
        public void Detach(rMindBaseWireDot dot)
        {
            dot.Detach();
            m_attached_dots.Remove(dot);
        }

        /// <summary> Update attached dots </summary>
        public void Update()
        {
            foreach (var dot in m_attached_dots)
            {
                dot.SetPosition(GetOffset());
            }
        }

        #endregion


        public void SetCell(int col, int row)
        {
            Column = col;
            Row = row;
            Update();
        }

        public virtual void UpdateAccentColor()
        {
            var theme = Theme;

            m_area.Fill = theme.BaseFill;
            m_area.Stroke = theme.BaseStroke;            
        }

        Windows.UI.Xaml.Thickness Margin {
            set
            {
                m_area.Margin = value;
            }
        }

        public bool CanAttach { get { return m_attach_mode == rMindNodeAttachMode.Multi || m_attached_dots.Count == 0; } }

        /// <summary>get list of nodes connected by wires </summary>
        public List<rMindBaseNode> GetReverseNodes()
        {
            return m_attached_dots
                .Where(x => x.ReverseDot?.Node != null)
                .Select(x => x.ReverseDot.Node)
                .ToList();
        }
    }

}
