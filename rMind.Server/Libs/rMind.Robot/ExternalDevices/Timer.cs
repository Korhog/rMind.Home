using System;

namespace rMind.Robot.ExternalDevices
{
    [DisplayName("Timer")]
    class Timer : IDevice
    {
        public void Update()
        {
            OnTick?.Invoke();
        }

        #region Activators

        [DisplayName("Tick")]
        public event VoidDelegate OnTick;

        #endregion

        [DisplayName("Time")]
        public DateTime Time
        {
            get
            {
                return DateTime.Now;
            }
        }

        [Setter]
        [DisplayName("Step")]
        public void SetStep(int mills)
        {

        }

        [Setter]
        [DisplayName("Time")]
        public void SetTime(DateTime time)
        {
            Logger.Current().Write(time.Millisecond.ToString());
        }

        [Setter]
        [DisplayName("Log")]
        public void Log()
        {
            Logger.Current().Write("Log");
        }
    }
}
