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
        RobotMindGraph robot;
        public Guid Guid { get; set; }
        public virtual object GetInstance()
        {
            return null;
        }

        public TemplateContainer(RobotMindGraph parent) : base(parent)
        {
            robot = parent;
            Build();
        }

        protected virtual void Build()
        {

        }
    }
}
