using System;

namespace Rendering.Sys
{
    public class FPSClass
    {
        private int frameCountPerSecond;
        private TimeSpan start;

       public int FPS { get; private set; }

        public void Init()
        {
            frameCountPerSecond = 0;
            FPS = 0;
            start = DateTime.Now.TimeOfDay;
        }

        public void Frame()
        {
            frameCountPerSecond++;
            int passed = (DateTime.Now.TimeOfDay - start).Seconds;
            if(passed >= 1)
            {
                FPS = frameCountPerSecond;
                frameCountPerSecond = 0;
                start = DateTime.Now.TimeOfDay;
            }
        }
    }
}
