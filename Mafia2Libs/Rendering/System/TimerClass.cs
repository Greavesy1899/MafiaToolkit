using System.Diagnostics;
using System.Threading;

namespace Rendering.Sys
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
        public void Init()
        {
            m_ticksPerMs = (Stopwatch.Frequency / 1000.0f);
            _StopWatch = Stopwatch.StartNew();
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
