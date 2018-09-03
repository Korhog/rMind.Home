using System;

namespace rMind.Robot.ExternalDevices
{
    [DisplayName("Timer")]
    public class Timer : ExternalDevice
    {
        public override void Update()
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

        [Config("Time")]
        [DisplayName("Display name")]
        public long DisplayName { get; set; }

        [Config("500")]
        [DisplayName("Delta time, ms")]
        public long DeltaTime { get; set; }        
    }
}
