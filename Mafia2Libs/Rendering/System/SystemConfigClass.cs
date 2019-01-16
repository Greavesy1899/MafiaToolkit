using System.Windows.Forms;

namespace ModelViewer.Programming.SystemClasses
{
    public class SystemConfigClass
    {
        public string Title { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public static FormBorderStyle BorderStyle { get; set; }
        public static bool FullScreen { get; private set; }
        public static bool VerticalSyncEnabled { get; set; }
        public static float ScreenDepth { get; private set; }
        public static float ScreenNear { get; private set; }
        public static string ShaderFilePath { get; private set; }
        public static string DataFilePath { get; private set; }

        public SystemConfigClass(bool Fullscreen, bool Vsync) : this("SharpDX Demo", Fullscreen, Vsync) { }
        public SystemConfigClass(string title, bool FullScreen, bool Vsync) : this(title, 800, 600, FullScreen, Vsync) { }
        public SystemConfigClass(string title, int width, int height, bool Fullscreen, bool Vsync)
        {
            FullScreen = Fullscreen;
            Title = title;
            if (!FullScreen)
            {
                Width = width;
                Height = height;
            }
            else
            {
                Width = Screen.PrimaryScreen.Bounds.Width;
                Height = Screen.PrimaryScreen.Bounds.Height;
            }
        }
        static SystemConfigClass()
        {
            VerticalSyncEnabled = true;
            ScreenDepth = 10000.0f;
            ScreenNear = 1f;
            BorderStyle = FormBorderStyle.FixedSingle;
            ShaderFilePath = @"Shaders\";
            DataFilePath = @"Data\";
        }
    }
}
