using rMind.Elements;
using rMind.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace rMind.RobotEngine
{
    public class EventNode : rMindBaseNode
    {
        /// <summary> Информация о событие </summary>
        public EventInfo EventInfo { get; private set; }

        public EventNode(rMindBaseElement parent, EventInfo info) : base(parent)
        {
            EventInfo = info;
        }
    }
}
