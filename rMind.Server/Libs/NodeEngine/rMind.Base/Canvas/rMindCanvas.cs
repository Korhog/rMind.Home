using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace rMind.CanvasEx
{
    using Elements;
    /// <summary>
    /// Extended canvas for rMindBaseController
    /// </summary>
    public sealed class rMindCanvas : Control
    {
        rMindBaseController m_current_controller;
        Canvas m_canvas;
        ScrollViewer m_scroll;
        ScaleTransform m_scale;       

        public rMindCanvas()
        {
            this.DefaultStyleKey = typeof(rMindCanvas);    
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_canvas = GetTemplateChild("PART_CANVAS") as Canvas;
            m_scroll = GetTemplateChild("PART_SCROLL") as ScrollViewer;
            m_scale = GetTemplateChild("PART_SCALE") as ScaleTransform;
        }

        public void SetController(rMindBaseController controller)
        {
            m_current_controller?.Unsubscribe();
        }
    }
}
