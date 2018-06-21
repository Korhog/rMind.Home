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
    using Elements;
    using rMind.Input;
    using rMind.Types;

    public partial class rMindBaseNode : rMindItemUI, IDrawElement, IInteractElement
    {
        float zoom = 1;
        rMindBaseWireDot dragDot;

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

        private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;
            // Получаем зум фактор
            zoom = GetController()?.CanvasController?.Zoom ?? 1.0f;
            if (zoom == 0)
                zoom = 1;

            var wire = GetController()?.CreateWire();
            if (wire != null)
            {
                Attach(wire.A);

                GetController()?.SetDragWireDot(wire.B, e);
                dragDot = wire.B;                
            }
        }

        public void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {            
            // не взаимодействуем с заблокированным объектом
            if (AttachMode == rMindNodeAttachMode.Single && m_attached_dots.Count > 0)  
                return;

            e.Handled = true;
        }

        public void SubscribeInput()
        {
            m_hit_area.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

            m_hit_area.PointerPressed += OnPointerPressed;

            m_hit_area.ManipulationStarting += OnManipulationStarting;
            m_hit_area.ManipulationCompleted += OnManipulationCompleted;
            m_hit_area.ManipulationDelta += OnManipulationDelta;
        }

        public void UnsubscribeInput()
        {
            m_hit_area.PointerPressed -= OnPointerPressed;

            m_hit_area.ManipulationStarting -= OnManipulationStarting;
            m_hit_area.ManipulationCompleted -= OnManipulationCompleted;
            m_hit_area.ManipulationDelta -= OnManipulationDelta;
        }
    }
}
