using System;
using System.Linq;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace rMind.Elements
{
    using Types;
    using Nodes;
    using Windows.UI.Xaml;
    using rMind.Storage;
    using rMind.Input;

    public enum rMindManipulationMode
    {
        None,
        Scale,
        Scroll,
        Select
    }

    public struct ManipulationData
    {
        public Vector2 BeginVector;
        public Vector2 CurrentVector;
        // vector from center of canvas to scale center
        // public Vector2 CenterVector;
        public Vector2 CurrentScroll;
        public double BaseScale;
    }

    /// <summary>
    /// Base scheme controller : input
    /// </summary>
    public partial class rMindBaseController : IStorageObject, IInteractElement
    {
        /// <summary>center of canvas</summary>
        protected Vector2 m_canvas_center;
        protected rMindManipulationMode m_manipulation_mode = rMindManipulationMode.None;
        protected ManipulationData m_manipulation_data;

        protected ulong? m_pointer_timestamp = null;
        public void SetPointerTimestamp(PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(m_scroll);
            m_pointer_timestamp = point.Timestamp;
        }

        TextBlock m_test;
        public Visibility ArrowVisibility { get { return m_parent.BreadCrumbs.IndexOf(this) == 0 ? Visibility.Collapsed : Visibility.Visible; } }        

        public void SubscribeInput()
        {
            m_canvas.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;            
            m_test = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            m_canvas.Children.Add(m_test);

            // events 
            m_canvas_center = new Vector2(m_canvas.Width / 2.0, m_canvas.Height / 2.0);

            m_canvas.PointerWheelChanged += onWheel;

            m_canvas.ManipulationStarting += OnManipulationStarting;
            m_canvas.ManipulationCompleted += OnManipulationCompleted;
            m_canvas.ManipulationDelta += OnManipulationDelta;

            if (m_scroll != null) m_scroll.Loaded += onLoad;

            m_canvas.PointerPressed += OnPointerPress;
            m_canvas.PointerReleased += onPointerExit;

            Window.Current.CoreWindow.KeyDown += onKeyDown;
            Window.Current.CoreWindow.KeyUp += onKeyUp;
        }

        void onLoad(object sender, RoutedEventArgs args)
        {
            m_scroll?.ChangeView(
                (m_scroll.ExtentWidth - m_scroll.ViewportWidth) / 2.0,
                (m_scroll.ExtentHeight - m_scroll.ViewportHeight) / 2.0,
                1, true);
        }

        public void UnsubscribeInput()
        {
            // events     
            m_canvas.PointerWheelChanged -= onWheel;

            m_canvas.ManipulationStarting -= OnManipulationStarting;
            m_canvas.ManipulationCompleted -= OnManipulationCompleted;
            m_canvas.ManipulationDelta -= OnManipulationDelta;

            if (m_scroll != null) m_scroll.Loaded -= onLoad;

            m_canvas.PointerPressed -= OnPointerPress;
            m_canvas.PointerReleased -= onPointerExit;

            Window.Current.CoreWindow.KeyDown -= onKeyDown;
            Window.Current.CoreWindow.KeyUp -= onKeyUp;
        }

        protected virtual void onPointerEnter(object sender, PointerRoutedEventArgs e)
        {
            //SetDragItem(null, e);
        }

        protected virtual void onPointerExit(object sender, PointerRoutedEventArgs e)
        {
            var point = e.GetCurrentPoint(m_scroll);
            m_pointer_timestamp = point.Timestamp;
        }

        protected bool CanControll()
        {
            if (m_overed_item == null && m_items_state.OveredNode == null && m_items_state.DragedWireDot == null)
                return true;

            return false;
        }

        private void OnPointerPress(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = true;

            var pointer = e.GetCurrentPoint(m_canvas);
            var doubleClick = m_pointer_timestamp.HasValue && pointer.Timestamp - m_pointer_timestamp.Value < 300000;

            if (pointer.PointerDevice.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse || doubleClick)
            {
                m_manipulation_data.BeginVector = m_manipulation_data.CurrentVector = new Vector2(pointer.Position.X, pointer.Position.Y);
                m_canvas.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
                if (e.KeyModifiers == Windows.System.VirtualKeyModifiers.Control)
                    m_manipulation_mode = rMindManipulationMode.Scroll;
                else
                {
                    m_manipulation_mode = rMindManipulationMode.Select;
                    StartSelection();
                }                
            }
            else
            {
                m_manipulation_mode = rMindManipulationMode.None;
                m_canvas.ManipulationMode = ManipulationModes.System;
            }            
        }

        protected virtual void SetScrollMode(PointerRoutedEventArgs e)
        {
            //m_manipulation_mode = rMindManipulationMode.Scroll;
            //var pointer = e.GetCurrentPoint(m_scroll);
            //m_manipulation_data.BeginVector = new Vector2(pointer);
            //m_manipulation_data.CurrentScroll = new Vector2(
            //    m_scroll.HorizontalOffset,
            //    m_scroll.VerticalOffset
            //);

            //SetManipulation(false, e);
        }

        protected virtual void onWheel(object sender, PointerRoutedEventArgs e)
        {
            if (m_scroll == null) return;
            if (e.KeyModifiers == Windows.System.VirtualKeyModifiers.Shift)
            {
                e.Handled = true;
                var wheelDelta = e.GetCurrentPoint(m_scroll).Properties.MouseWheelDelta;
                m_scroll.ChangeView(m_scroll.HorizontalOffset + wheelDelta, null, null);
                return;
            }
        }

        protected virtual void onKeyDown(CoreWindow window, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Delete)
            {
                DeleteSelection();
            }
        }

        protected virtual void onKeyUp(CoreWindow window, KeyEventArgs e)
        {

        }

        public void OnManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            e.Handled = true;
        }

        public void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            e.Handled = true;
            if (m_manipulation_mode == rMindManipulationMode.Select)
            {
                m_manipulation_data.CurrentVector += new Vector2(e.Delta.Translation.X, e.Delta.Translation.Y) / (CanvasController?.Zoom ?? 1.0f);
                UpdateSelectorRect();
                return;
            }
        }

        public void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;
            if (m_manipulation_mode == rMindManipulationMode.Select)
            {
                StopSelection();
                return;                
            }
        }
    }
}

