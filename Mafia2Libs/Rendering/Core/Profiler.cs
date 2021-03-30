using System;
using System.Diagnostics;

namespace Rendering.Core
{
    public class Profiler
    {
        public static uint NumDrawCallsThisFrame;

        public float FramesPerSecond { get; set; }
        public float FrameTime { get; set; }
        public float DeltaTime { get; set; }

        private Stopwatch Timer;
        private int NumFrames;
        private TimeSpan FrameStart;
        private float TickPerMs;
        private long LastFrameTime;
        private float CumulativeFrameTime;

        private uint LastFramesNumDrawCalls;

        public Profiler()
        {
            Timer = new Stopwatch();
        }
        public void Init()
        {
            NumFrames = 0;
            TickPerMs = Stopwatch.Frequency / 1000.0f;
            Timer.Start();
        }

        public void Update()
        {
            CalculateFPS();
            CalculateFrameTime();
            DeltaTime = 1.0f / FramesPerSecond;

            LastFramesNumDrawCalls = NumDrawCallsThisFrame;
            NumDrawCallsThisFrame = 0;
        }

        private void CalculateFrameTime()
        {
            long currentTime = Timer.ElapsedTicks;
            float TimeDifference = currentTime - LastFrameTime;
            FrameTime = TimeDifference / TickPerMs;
            CumulativeFrameTime += FrameTime;
            LastFrameTime = currentTime;
        }
        private void CalculateFPS()
        {
            NumFrames++;
            int passed = (DateTime.Now.TimeOfDay - FrameStart).Seconds;
            if (passed >= 1)
            {
                FramesPerSecond = NumFrames;
                NumFrames = 0;
                FrameStart = DateTime.Now.TimeOfDay;
            }
        }

        public override string ToString()
        {
            return string.Format("FPS: {0} FrameTime: {1} DrawCalls: {2}", FramesPerSecond, FrameTime, LastFramesNumDrawCalls);
        }
    }
}
