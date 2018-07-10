using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace rMind.Robot
{
    public delegate void LoggerWriteDelegate(string message);

    public class Logger
    {
        static Logger instance;
        static readonly object sync = new object();

        public static Logger Current()
        {
            lock (sync)
            {
                if (instance == null)
                    instance = new Logger();
            }

            return instance;
        }

        public event LoggerWriteDelegate OnMessage;
        public void Write(string message)
        {
            OnMessage?.Invoke(string.Format("{0}: {1}", DateTime.UtcNow, message));
        }
    }
}
