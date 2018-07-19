using System.Windows.Forms;

namespace ModelViewer.System
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
        public static string Version { get; private set; }
        //Paths in mod folder.
        public static string ModFolderPath { get; private set; }
        public static string MeshPath { get; private set; }
        public static string TexturePath { get; private set; }
        public static string GamePath { get; private set; }

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
        {            VerticalSyncEnabled = false;
            ScreenDepth = 10000.0f;
            ScreenNear = 10f;
            BorderStyle = FormBorderStyle.FixedSingle;
            ShaderFilePath = @"Shaders\";
            DataFilePath = @"Data\";

            Version = Application.ProductVersion;
        }
    }
}
