using System;
using System.Threading;
using System.Threading.Tasks;

namespace rMind.Robot.ExternalDevices
{
    [DisplayName("Timer")]
    class Timer : IDevice
    {
        public void Update()
        {
            OnTick?.Invoke();
        }

        public Timer()
        {
            SynchronizationContext ctx = SynchronizationContext.Current;
            Thread th = new Thread((state) =>
            {
                while (true)
                {
                    var context = state as SynchronizationContext;
                    context.Post((s) => { Update(); }, null);
                    //OnTick?.Invoke();
                    Thread.Sleep(1000);
                }
            });

            th.Start(ctx);
            //var t = Task.Factory.StartNew(action);
            //t.Start()

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
