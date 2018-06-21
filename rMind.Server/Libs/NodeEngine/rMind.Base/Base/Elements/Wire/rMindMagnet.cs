using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

namespace rMind.Elements
{
    using Types;

    public class rMindMagnet
    {        
        Rectangle m_area;
        Line m_line;

        public rMindMagnet()
        {
            m_area = new Rectangle()
            {
                Width = 12,
                Height = 12,
                RadiusX = 6,
                RadiusY = 6
            };

            Canvas.SetZIndex(m_area, 9);

            m_line = new Line()
            {
                StrokeThickness = 4
            };

            Canvas.SetZIndex(m_line, 9);
            SetColor(Colors.Black);
        }

        public void Draw(Canvas canvas)
        {
            if (!canvas.Children.Contains(m_area))
                canvas.Children.Add(m_area);

            if (!canvas.Children.Contains(m_line))
                canvas.Children.Add(m_line);
        }

        public void SetColor(Color color)
        {
            var colors = ColorContainer.rMindColors.Current();
            m_area.Fill = colors.GetSolidBrush(color);
            m_line.Stroke = colors.GetSolidBrush(color);
        }

        public void Hide()
        {
            m_area.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            m_line.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public void Show()
        {
            m_area.Visibility = Windows.UI.Xaml.Visibility.Visible;
            m_line.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        public void Magnet(Vector2 A, Vector2 B)
        {
            m_line.X1 = A.X;
            m_line.Y1 = A.Y;

            m_line.X2 = B.X;
            m_line.Y2 = B.Y;

            Canvas.SetLeft(m_area, B.X - 6);
            Canvas.SetTop(m_area, B.Y - 6);
        }
    }
}
