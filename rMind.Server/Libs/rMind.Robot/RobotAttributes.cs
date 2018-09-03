using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property)]
    public class DisplayName : Attribute
    {
        public DisplayName(string name)
        {
            Name = name;
        }

        public string Name { get; private set; } 
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Setter : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class Config : Attribute
    {
        public Config(string def)
        {
            Default = def;
        }
        public string Default { get; private set; }
    }
}
