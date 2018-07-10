using rMind.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace rMind.RobotEngine
{
    class PropertyNode : rMindBaseNode, IPropertyNode
    {
        /// <summary> Информация о событие </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        TemplateContainer templateContainer;
        public PropertyNode(TemplateContainer parent, PropertyInfo info) : base(parent)
        {
            templateContainer = parent;
            PropertyInfo = info;
        }

        public Func<object> Func()
        {
            var instance = templateContainer.GetInstance();
            Func<object> func = () => { return PropertyInfo.GetMethod.Invoke(instance, new object[] { }); };
            return func;
        }
    }
}
