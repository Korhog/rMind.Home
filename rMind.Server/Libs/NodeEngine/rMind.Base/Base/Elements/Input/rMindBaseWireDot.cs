using Windows.UI;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

namespace rMind.Elements
{
    using rMind.Draw;
    using rMind.Input;
    using rMind.Types;

    /// <summary> Ending of wire </summary>
    public partial class rMindBaseWireDot : rMindItemUI, IDrawElement, IInteractElement
    {
        float zoom = 1;

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            GetController().SetDragWireDot(this, e);
        }

        public void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;
            GetController().DropWireDot();
        }

        public void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;
            var translation = new Vector2(e.Delta.Translation.X, e.Delta.Translation.Y) / zoom;
            GetController().TranslateWireDot(translation);
        }

        public void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            e.Handled = true;
            zoom = GetController()?.CanvasController.Zoom ?? 1;
            if (zoom == 0)
                zoom = 1;
        }

        public void SubscribeInput()
        {
            m_area.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

            m_area.PointerPressed += OnPointerPressed;

            m_area.ManipulationStarting += OnManipulationStarting;
            m_area.ManipulationCompleted += OnManipulationCompleted;
            m_area.ManipulationDelta += OnManipulationDelta;
        }

        public void UnsubscribeInput()
        {
            m_area.PointerPressed -= OnPointerPressed;

            m_area.ManipulationStarting -= OnManipulationStarting;
            m_area.ManipulationCompleted -= OnManipulationCompleted;
            m_area.ManipulationDelta -= OnManipulationDelta;
        }
    }
}
