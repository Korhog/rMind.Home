using System;

namespace rMind.Theme
{
    public class rMindScheme
    {
        static Color.IColorScheme m_scheme;
        static object sync = new Object();

        public static Color.IColorScheme Get()
        {
            if (m_scheme == null)
            {
                lock (sync)
                {
                    if (m_scheme == null)
                    {
                        m_scheme = new Color.rMindSchemeDark();
                    }
                }
            }

            return m_scheme;
        }
    }
}
