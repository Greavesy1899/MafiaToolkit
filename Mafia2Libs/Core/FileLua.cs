using System;
using System.Diagnostics;
using System.IO;
using Utils.Lua;

namespace Core.IO
{
    public class FileLua : FileBase
    {
        private string extension = string.Empty;

        public FileLua(FileInfo info) : base(info)
        {
            extension = info.Extension;
        }

        private bool IsBytecode()
        {
            using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open)))
            {
                var magic = reader.ReadInt32();
                if (magic == 1635077147)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool Open()
        {
            if(IsBytecode())
            {
                LuaHelper.ReadFile(file);
                FixDecompiledLUA();
            }
            else
            {
                Process.Start(file.FullName);
            }

            return true;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return extension;
        }

        private void FixDecompiledLUA()
        {
            string[] lines = File.ReadAllLines(file.FullName);
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

                File.WriteAllLines(file.FullName, lines);
            }

        }
    }
}
