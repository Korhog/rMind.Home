using Windows.UI.Xaml.Input;

namespace rMind.Elements
{
    using Draw;
    using Types;
    using Storage;
    using Input;

    /// <summary>
    /// Base controller element 
    /// </summary>   
    public partial class rMindBaseElement : rMindBaseItem, IDrawContainer, IStorageObject, IInteractElement
    {
        float zoom = 1;

        public void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            // не взаимодействуем с заблокированным объектом
            if (Locked)
                return;

            e.Handled = true;
        }

        public void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            // не взаимодействуем с заблокированным объектом
            if (Locked)
                return;

            e.Handled = true;

            GetController()?.TranslateContainer(
                this,
                new Vector2(e.Delta.Translation.X, e.Delta.Translation.Y) / zoom
            );
        }

        public void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            // не взаимодействуем с заблокированным объектом
            if (Locked)
                return;

            e.Handled = true; 

            zoom = GetController()?.CanvasController?.Zoom ?? 1.0f;
            if (zoom == 0)
                zoom = 1;
        }

        protected ulong? timeStamp;

        protected void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // не взаимодействуем с заблокированным объектом
            if (Locked)
                return;

            e.Handled = true;

            var doubleTap = false;
            // Если касаемся пальцем или пером, то нужно нужно проверить двойное касание
            if (e.Pointer.PointerDeviceType != Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                var pointer = e.GetCurrentPoint(m_base);
                doubleTap = m_pointer_timestamp.HasValue && pointer.Timestamp - m_pointer_timestamp.Value < 300000;

                if (doubleTap)
                {
                    // двойное касание добавляет объект в выделение.
                    GetController()?.SetSelectedItem(this, true);
                    return;
                }
            }

            GetController()?.SetSelectedItem(
                this,
                e.KeyModifiers == Windows.System.VirtualKeyModifiers.Shift
            );
        }

        protected void OnPointerRealesed(object sender, PointerRoutedEventArgs e)
        {
            // не взаимодействуем с заблокированным объектом
            if (Locked)
                return;

            e.Handled = true;
            timeStamp = e.GetCurrentPoint(m_base).Timestamp;
        }

        public void SubscribeInput()
        {
            m_base.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;

            m_base.PointerPressed += OnPointerPressed;
            m_base.PointerReleased += OnPointerRealesed;

            m_base.ManipulationStarting += OnManipulationStarting;
            m_base.ManipulationCompleted += OnManipulationCompleted;
            m_base.ManipulationDelta += OnManipulationDelta;   
        }

        public void UnsubscribeInput()
        {
            m_base.PointerPressed -= OnPointerPressed;
            m_base.PointerReleased -= OnPointerRealesed;

            m_base.ManipulationStarting -= OnManipulationStarting;
            m_base.ManipulationCompleted -= OnManipulationCompleted;
            m_base.ManipulationDelta -= OnManipulationDelta;
        }
    }
}
