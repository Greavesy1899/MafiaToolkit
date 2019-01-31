using System.Diagnostics;
using System.Threading;

namespace Rendering.Utils
{
    public class TimerClass
    {
        //Vars
        private Stopwatch _StopWatch;
        private float m_ticksPerMs;
        private long m_LastFrameTime = 0;
        //Properties
        public float FrameTime { get; private set; }
        public float CumulativeFrameTime { get; private set; }
        //Methods
        public bool Init()
        {
            if (!Stopwatch.IsHighResolution)
            {
                return false;
            }
            if (Stopwatch.Frequency == 0)
            {
                return false;
            }

            _StopWatch = Stopwatch.StartNew();
            m_ticksPerMs = (Stopwatch.Frequency / 1000.0f);

            return true;
        }
        public void Frame2()
        {
            long currentTime = _StopWatch.ElapsedTicks;
            float TimeDifference = currentTime - m_LastFrameTime;
            FrameTime = TimeDifference / m_ticksPerMs;
            CumulativeFrameTime += FrameTime;
            m_LastFrameTime = currentTime;
        }
    }
}
