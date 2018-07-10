using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using rMind.RobotEngine;
using System.Reflection;

namespace Editor
{
    class RobotMenuFlyoutItem : MenuFlyoutItem
    {
        public Guid Guid { get; set; }
        public Type ClassType { get; set; }
        public MethodInfo Method { get; set; }
        public ClassTemplate ClassTemplate { get; set; }
    }
}
