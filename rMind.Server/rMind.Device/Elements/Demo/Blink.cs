using System;
using System.Collections.Generic;
using System.Text;

namespace rMind.Device
{
    using Core;

    public class Blink : Device
    {
        public Blink(IMindCore board) : base(board) { }

        /// <summary> Выполнение логики </summary>
        public override void Execute()
        {
            var result = m_board.Switch(21);
            Console.WriteLine(result);
        }

        public override void Update()
        {
        }
    }
}
