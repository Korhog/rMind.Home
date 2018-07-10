using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.RobotEngine
{
    /// <summary>
    /// Узел для получения значение
    /// </summary>
    public interface IPropertyNode
    {
        /// <summary>
        /// Функц
        /// </summary>
        Func<object> Func();
    }
}
