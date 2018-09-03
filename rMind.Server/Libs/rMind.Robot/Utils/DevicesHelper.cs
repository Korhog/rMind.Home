using rMind.Robot.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot.Utils
{
    public class DevicesHelper
    {
        public static Task<List<WrapperDto>> Wrappers
        {
            get
            {
                return Task.Run(() => {
                    var wrappers = typeof(IDevice).Assembly.GetTypes()
                        .Where(x => typeof(IDevice).IsAssignableFrom(x))
                        .Where(x => !x.IsAbstract)
                        .Where(x => x.IsClass)
                        .Select(x => new WrapperDto
                        {
                            Name = x.GetCustomAttribute<DisplayName>()?.Name ?? "Name",
                            DeviceType = x
                        })
                        .ToList();

                    return wrappers;
                });                
            }
        }
    }
}
