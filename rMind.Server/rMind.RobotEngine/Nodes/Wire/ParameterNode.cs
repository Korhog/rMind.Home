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
    public class ParameterNode : rMindBaseNode
    {
        /// <summary> Информация о событие </summary>
        public ParameterInfo ParameterInfo { get; private set; }

        public ParameterNode(rMindBaseElement parent, ParameterInfo info) : base(parent)
        {
            ParameterInfo = info;
        }
    }
}
