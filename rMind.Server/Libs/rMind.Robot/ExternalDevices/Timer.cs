using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rMind.Robot.ExternalDevices
{
    [DisplayName("Timer")]
    class Timer : IDevice
    {
        public void Update()
        {
        }

        #region Activators

        [DisplayName("Tick")]
        public event VoidDelegate OnTick;

        #endregion

        [DisplayName("Time")]
        public TimeSpan Time
        {
            get
            {
                return new TimeSpan();
            }
        }

        [Setter]
        [DisplayName("Step")]
        public void SetStep(int mills)
        {

        }
    }
}
