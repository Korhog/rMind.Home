using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using rMind.RobotEngine;

namespace Editor
{
    class RobotMenuFlyoutItem : MenuFlyoutItem
    {
        public Type ClassType { get; set; }
        public ClassTemplate ClassTemplate { get; set; }
    }
}
