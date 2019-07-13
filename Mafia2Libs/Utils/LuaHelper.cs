using System.Globalization;
using System.IO;
using System.Text;
using UnluacNET;

namespace Utils.Lua
{
    public class LuaHelper
    {
        private static LFunction FileToFunction(string fn)
        {
            using (var fs = File.Open(fn, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BHeader header = new BHeader(fs);
                return header.Function.Parse(fs, header);
            }
        }

        public static void ReadFile(FileInfo info)
        {
            LFunction main = null;
            main = FileToFunction(info.FullName);
            Decompiler decompile = new Decompiler(main);
            decompile.Decompile();
            bool isAP = (info.Extension == ".AP" ? true : false);
            string name = info.FullName.Remove(info.FullName.Length - (isAP ? 7 : 4));
            name += "_d." + info.Extension/* + (isAP ? ".ap " : "")*/;
            var curCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            using (var writer = new StreamWriter(name, false, Encoding.ASCII))
            {
                Output output = new Output(writer);
                decompile.Print(output);
                writer.Flush();
            }
            System.Threading.Thread.CurrentThread.CurrentCulture = curCulture;
        }
    }
}
