using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot.Dto
{
    public class WrapperDto
    {
        public string Name { get; set; }
        public Type DeviceType { get; set; }
        public string Glyph { get { return "\uE950"; } }
    }
}
