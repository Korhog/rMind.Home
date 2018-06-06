using System;
using System.Collections.Generic;
using System.Text;

namespace rMind.Core
{
    public abstract class Device : IDevice
    {
        protected IMindCore m_board;

        public Device(IMindCore board)
        {
            m_board = board;
        }

        public abstract void Execute();
        public abstract void Update();
    }
}
