using Windows.UI.Xaml.Input;

namespace rMind.Input
{
    /// <summary> Интерактивный элемент </summary>
    public interface IInteractElement
    {
        /// <summary> Подписка на события ввода </summary>
        void SubscribeInput();

        /// <summary> отписка от событий ввода </summary>
        void UnsubscribeInput();

        /// <summary> Начало манипуляций </summary>
        void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e);

        /// <summary> Дельта манипуляций (перемещение, поворот) </summary>
        void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e);

        /// <summary> Завершение манипуляций </summary>
        void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e);
    }
}
