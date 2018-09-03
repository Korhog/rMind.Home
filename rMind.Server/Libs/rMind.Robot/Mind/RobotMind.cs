using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot
{
    public class RobotMind : IRobotMind
    {
        /// <summary> Конфигурация умного дома </summary>
        public IRobotConfig Config { get; private set; }

        private static readonly object sync = new object();
        private static RobotMind robotMindInstance;

        public static IRobotMind Current
        {
            get
            {
                lock (sync)
                {
                    if (robotMindInstance == null)
                    {
                        robotMindInstance = new RobotMind();
                    }
                }
                return robotMindInstance;
            }
        }  
        
        private RobotMind()
        {
            Config = new RobotConfig(); 
        }

        public void Add(Guid guid, object obj)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public object Create<T>() where T : IDevice
        {
            throw new NotImplementedException();
        }

        public object Get(Guid guid)
        {
            throw new NotImplementedException();
        }

    }
}
