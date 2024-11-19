using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using UnluacNET;
using Utils.Settings;

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
            var curCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            using (var writer = new StreamWriter(name, false, Encoding.ASCII))
            {
                Output output = new Output(writer);
                decompile.Print(output);
                writer.Flush();
            }
            FixDecompiledLua(name);
            Thread.CurrentThread.CurrentCulture = curCulture;
        }

        private static void FixDecompiledLua(string Name)
        {
            if (ToolkitSettings.EnableLuaHelper == false)
            {
                // Lua helper is disabled, do not make changes
                return;
            }

            // LuaHelper enabled, make changes
            string[] lines = File.ReadAllLines(Name);
            int LastFunctionLine = 0;
            int FirstListLine = 0;
            bool HaveTableInsertLine = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("function"))
                {
                    LastFunctionLine = i;
                    HaveTableInsertLine = false;
                }
                if (lines[i].Contains("{"))
                {
                    FirstListLine = i;
                }
                //empty table list fix
                if (lines[i].Contains("table") && lines[i].Contains("({}"))
                {
                    if (HaveTableInsertLine == false)
                    {
                        HaveTableInsertLine = true;
                        lines[LastFunctionLine] = lines[LastFunctionLine] + Environment.NewLine + "LuaHelperTableInsertValue = {}";
                    }
                }

                if (lines[i].Contains("{}") && HaveTableInsertLine == true)
                {
                    lines[i] = lines[i].Replace("{}", "LuaHelperTableInsertValue");
                }
                //math functions fix
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

                if (lines[i].Contains(":magnitude2()") && lines[i].Contains("-"))
                {
                    string Checker = lines[i].Substring(lines[i].IndexOf(":magnitude2()") - 2);
                    if (Checker.Substring(0, 1) != "(" && Checker.Substring(1, 1) == ")")
                    {
                        //MessageBox.Show("OK");
                    }
                    else
                    {
                        string CheckerLine = lines[i].Substring(0, lines[i].IndexOf(":magnitude2()"));
                        int IndexOfMinuse = CheckerLine.LastIndexOf("-") - 1;
                        int IndexOfSpace = lines[i].Substring(0, IndexOfMinuse).LastIndexOf(" ") + 1;
                        lines[i] = lines[i].Insert(IndexOfSpace, "(").Replace(":magnitude2()", "):magnitude2()");
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

                //MoveVec GetOppositePosRot fix
                if (lines[i].Contains("MoveVec") && lines[i].Contains("GetOppositePosRot"))
                {
                    string LineWithGetOppositePosRot = lines[i];
                    LineWithGetOppositePosRot = lines[i].Substring(LineWithGetOppositePosRot.IndexOf("MoveVec(") + "MoveVec(".Length);
                    LineWithGetOppositePosRot = LineWithGetOppositePosRot.Substring(0, LineWithGetOppositePosRot.IndexOf(")") + 1);

                    //add dir and pos definition
                    lines[i - 1] = lines[i - 1] + Environment.NewLine + "LuaHelperOppositePos, LuaHelperOppositeDir = " + LineWithGetOppositePosRot;
                    //fix the definition
                    lines[i] = lines[i].Replace(LineWithGetOppositePosRot, "LuaHelperOppositePos, \"WALK\", LuaHelperOppositeDir");
                }

                //SetPos GetOppositePosRot fix
                if (lines[i].Contains("SetPos") && lines[i].Contains("GetOppositePosRot"))
                {
                    string LineWithGetOppositePosRot = lines[i];
                    LineWithGetOppositePosRot = lines[i].Substring(LineWithGetOppositePosRot.IndexOf("SetPos(") + "SetPos(".Length);
                    LineWithGetOppositePosRot = LineWithGetOppositePosRot.Substring(0, LineWithGetOppositePosRot.IndexOf(")") + 1);

                    //add dir and pos definition
                    lines[i - 1] = lines[i - 1] + Environment.NewLine + "LuaHelperOppositePos, LuaHelperOppositeDir = " + LineWithGetOppositePosRot;
                    //fix the definition
                    lines[i] = lines[i].Replace(LineWithGetOppositePosRot, "LuaHelperOppositePos");
                }

                //SetDir GetOppositePosRot fix
                if (lines[i].Contains("SetDir") && lines[i].Contains("GetOppositePosRot"))
                {
                    string LineWithGetOppositePosRot = lines[i];
                    LineWithGetOppositePosRot = lines[i].Substring(LineWithGetOppositePosRot.IndexOf("SetDir(") + "SetDir(".Length);
                    LineWithGetOppositePosRot = LineWithGetOppositePosRot.Substring(0, LineWithGetOppositePosRot.IndexOf(")") + 1);

                    //add dir and pos definition
                    lines[i - 1] = lines[i - 1] + Environment.NewLine + "LuaHelperOppositePos, LuaHelperOppositeDir = " + LineWithGetOppositePosRot;
                    //fix the definition
                    lines[i] = lines[i].Replace(LineWithGetOppositePosRot, "LuaHelperOppositeDir");
                }

                //MoveVec GetApproachPosDir fix
                if (lines[i].Contains("MoveVec") && lines[i].Contains("GetApproachPosDir"))
                {
                    string LineWithGetOppositePosRot = lines[i];
                    LineWithGetOppositePosRot = lines[i].Substring(LineWithGetOppositePosRot.IndexOf("MoveVec(") + "MoveVec(".Length);
                    LineWithGetOppositePosRot = LineWithGetOppositePosRot.Substring(0, LineWithGetOppositePosRot.IndexOf(")") + 1);

                    //add dir and pos definition
                    lines[i - 1] = lines[i - 1] + Environment.NewLine + "LuaHelperOppositePos, LuaHelperOppositeDir = " + LineWithGetOppositePosRot;
                    //fix the definition
                    lines[i] = lines[i].Replace(LineWithGetOppositePosRot, "LuaHelperOppositePos, \"WALK\", LuaHelperOppositeDir");
                }

                //SetPos GetApproachPosDir fix
                if (lines[i].Contains("SetPos") && lines[i].Contains("GetApproachPosDir"))
                {
                    string LineWithGetOppositePosRot = lines[i];
                    LineWithGetOppositePosRot = lines[i].Substring(LineWithGetOppositePosRot.IndexOf("SetPos(") + "SetPos(".Length);
                    LineWithGetOppositePosRot = LineWithGetOppositePosRot.Substring(0, LineWithGetOppositePosRot.IndexOf(")") + 1);

                    //add dir and pos definition
                    lines[i - 1] = lines[i - 1] + Environment.NewLine + "LuaHelperOppositePos, LuaHelperOppositeDir = " + LineWithGetOppositePosRot;
                    //fix the definition
                    lines[i] = lines[i].Replace(LineWithGetOppositePosRot, "LuaHelperOppositePos");
                }

                //SetDir GetApproachPosDir fix
                if (lines[i].Contains("SetDir") && lines[i].Contains("GetApproachPosDir"))
                {
                    string LineWithGetOppositePosRot = lines[i];
                    LineWithGetOppositePosRot = lines[i].Substring(LineWithGetOppositePosRot.IndexOf("SetDir(") + "SetDir(".Length);
                    LineWithGetOppositePosRot = LineWithGetOppositePosRot.Substring(0, LineWithGetOppositePosRot.IndexOf(")") + 1);

                    //add dir and pos definition
                    lines[i - 1] = lines[i - 1] + Environment.NewLine + "LuaHelperOppositePos, LuaHelperOppositeDir = " + LineWithGetOppositePosRot;
                    //fix the definition
                    lines[i] = lines[i].Replace(LineWithGetOppositePosRot, "LuaHelperOppositeDir");
                }

                //wrong list fix
                if (lines[i].Contains("}["))
                {
                    String LinesWithData = "";
                    for (int j = FirstListLine + 1; j < i; j++)
                    {
                        LinesWithData += lines[j].Replace(" ", "");
                        lines[j] = "--empty line";
                    }
                    lines[FirstListLine - 1] = lines[FirstListLine - 1] + Environment.NewLine + "LuaHelperListOfValues = {" + LinesWithData + "}";

                    lines[FirstListLine] = lines[FirstListLine].Remove(lines[FirstListLine].Length - 1) + "LuaHelperListOfValues" + lines[i].Replace(" ", "").Remove(0, 1);
                    lines[i] = "--empty line";

                }

                //warning on upvalues
                if (lines[i].Contains("UPVALUE"))
                {
                    lines[i - 1] = lines[i - 1] + Environment.NewLine + "--Warning, UPVALUES usually makes problems in game, you should change it to some of the values mentioned before this line!";
                }


                File.WriteAllLines(Name, lines);
            }
        }
    }
}
