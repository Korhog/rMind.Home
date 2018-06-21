using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace rMind.Theme.Color
{
    public class rMindSchemeDark : IColorScheme
    {
        static SolidColorBrush m_mainContainerBrush;
        static object sync = new Object();

        public SolidColorBrush MainContainerBrush()
        {
            if (m_mainContainerBrush == null)
            {
                lock(sync)
                {
                    if (m_mainContainerBrush == null)
                    {
                        m_mainContainerBrush = new SolidColorBrush(Windows.UI.Colors.DarkGray);
                    }
                }
            }
            return m_mainContainerBrush;
        }
    }
}
