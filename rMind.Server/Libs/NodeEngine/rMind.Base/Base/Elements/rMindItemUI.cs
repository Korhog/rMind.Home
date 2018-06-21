using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace rMind.Elements
{
    using Types;

    [JsonObject(MemberSerialization.OptIn)]
    public class rMindItemUI
    {
        // Не доступна для управления.
        protected bool m_locked = false;       
        [JsonProperty]
        public bool Locked { get { return m_locked; } set { m_locked = value; } }

        protected bool m_has_translate;

        protected bool m_expanded = true;
        [JsonProperty]
        public bool Expanded { get { return m_expanded; } set { m_expanded = value; } }

        protected Vector2 m_position;
        [JsonProperty]
        public Vector2 Position { get { return m_position; } set { SetPosition(value); } }

        // Graphics
        protected Grid m_template; // main container for any elements
        public Grid Template { get { return m_template; } }

        public rMindItemUI()
        {         
            m_template = new Grid()
            {
                UseLayoutRounding = true,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            m_template.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            m_template.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
        }

        public void SetVisibility(bool state)
        {
            m_template.Visibility = state ? Visibility.Visible : Visibility.Collapsed;
        }

        public virtual Vector2 SetPosition(Vector2 newPos)
        {
            var translation = newPos - m_position;

            m_position = newPos;

            Canvas.SetLeft(m_template, newPos.X);
            Canvas.SetTop(m_template, newPos.Y);

            m_has_translate = true;
            return translation;
        }

        public virtual Vector2 SetPosition(double x, double y)
        {
            return SetPosition(new Vector2(x, y));
        }

        public virtual void Translate(Vector2 vector)
        {
            SetPosition(Position + vector);
        }

        protected virtual void Glow(bool state)
        {

        }

        public string IDS;

        public virtual void SetEnabledHitTest(bool state) { }

        public double Width { get { return m_template.ActualWidth; } }
        public double Height { get { return m_template.ActualHeight; } }
    }
}
