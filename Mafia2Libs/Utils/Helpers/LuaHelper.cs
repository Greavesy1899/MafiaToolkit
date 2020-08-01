using System;
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
            bool isAP = (info.Extension.Equals(".AP") ? true : false);
            string name = info.FullName.Remove(info.FullName.Length - (isAP ? 7 : 4));
            name += "_d" + info.Extension;
            var curCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            using (var writer = new StreamWriter(name, false, Encoding.ASCII))
            {
                Output output = new Output(writer);
                decompile.Print(output);
                writer.Flush();
            }
            FixDecompiledLua(name);
            System.Threading.Thread.CurrentThread.CurrentCulture = curCulture;
        }

        private static void FixDecompiledLua(string Name)
        {
            string[] lines = File.ReadAllLines(Name);
            int LastFunctionLine = 0;
            bool HaveTableInsertLine = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("function"))
                {
                    LastFunctionLine = i;
                    HaveTableInsertLine = false;
                }

                if (lines[i].Contains("table") && lines[i].Contains("({}"))
                {
                    if (HaveTableInsertLine == false)
                    {
                        HaveTableInsertLine = true;
                        lines[LastFunctionLine] = lines[LastFunctionLine] + Environment.NewLine + "TableInsertValue = {}";
                    }
                }

                if (lines[i].Contains("{}") && HaveTableInsertLine == true)
                {
                    lines[i] = lines[i].Replace("{}", "TableInsertValue");
                }

                if (lines[i].Contains(":magnitude()") && lines[i].Contains("-"))
                {
                    string Checker = lines[i].Substring(lines[i].IndexOf(":magnitude()") - 2);
                    if (Checker.Substring(0, 1) != "(" && Checker.Substring(1, 1) == ")")
                    {
                        //MessageBox.Show("OK");
                    }
                    else
                    {
                        string CheckerLine = lines[i].Substring(0, lines[i].IndexOf(":magnitude()"));
                        int IndexOfMinuse = CheckerLine.LastIndexOf("-") - 1;
                        int IndexOfSpace = lines[i].Substring(0, IndexOfMinuse).LastIndexOf(" ") + 1;
                        lines[i] = lines[i].Insert(IndexOfSpace, "(").Replace(":magnitude()", "):magnitude()");
                    }
                }

                if (lines[i].Contains(":normalize()") && lines[i].Contains("-"))
                {
                    string Checker = lines[i].Substring(lines[i].IndexOf(":normalize()") - 2);
                    if (Checker.Substring(0, 1) != "(" && Checker.Substring(1, 1) == ")")
                    {
                        //MessageBox.Show("OK");
                    }
                    else
                    {
                        string CheckerLine = lines[i].Substring(0, lines[i].IndexOf(":normalize()"));
                        int IndexOfMinuse = CheckerLine.LastIndexOf("-") - 1;
                        int IndexOfSpace = lines[i].Substring(0, IndexOfMinuse).LastIndexOf(" ") + 1;
                        lines[i] = lines[i].Insert(IndexOfSpace, "(").Replace(":normalize()", "):normalize()");
                    }
                }

                File.WriteAllLines(Name, lines);
            }
        }
    }
}
