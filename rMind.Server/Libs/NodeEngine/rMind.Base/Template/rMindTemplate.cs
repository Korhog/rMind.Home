using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Template
{
    using Elements;

    /// <summary>
    /// класс, шаблонизатор
    /// </summary>
    public class rMindTemplate
    {
        protected List<rMindTemplatePart> m_parts;
        protected rMindBaseElement m_container;

        public rMindTemplate(rMindBaseElement container)
        {
            m_parts = new List<rMindTemplatePart>();
            m_container = container;
        }

    }
}
