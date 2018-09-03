using rMind.BaseControls.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot.Entities
{
    public class TreeItem : TreeItemBase
    {        
        public object Tag { get; set; }
        public ClassTemplate ClassTemplate { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public Type ClassType { get; set; }
        public Guid Guid { get; set; }
    }
}
