using rMind.Content;
using rMind.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.RobotEngine
{
    public abstract class TemplateContainer : rMindHeaderRowContainer, IEditorNode
    {
        public TemplateContainer(rMindBaseController parent) : base(parent)
        {
            Build();
        }

        protected virtual void Build()
        {

        }
    }
}
