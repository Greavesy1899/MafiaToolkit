using System;
using System.IO;
using Gibbed.Illusion.FileFormats.Hashing;
using Utils.Logging;

namespace ResourceTypes.Cutscene.CurveParams
{
    public static class CutsceneCurveParamFactory
    {
        public static ICurveParam ReadFromFile(BinaryReader br, string CutsceneName, int index)
        {
            ICurveParam param = null;

            int Type = br.ReadInt32();

            //DumpParamData(br, Type, CutsceneName, index);

            switch (Type)
            {
                case 0:
                    param = new FloatLinear(br);
                    break;

                case 1:
                    param = new FloatBezier(br);
                    break;

                case 2:
                    param = new FloatTCB(br);
                    break;

                case 3:
                    param = new Vector2Linear(br);
                    break;

                case 4:
                    param = new Vector2Bezier(br);
                    break;

                case 5:
                    param = new Vector2TCB(br);
                    break;

                case 6:
                    param = new Vector3Linear(br);
                    break;

                case 7:
                    param = new Vector3Bezier(br);
                    break;

                case 8:
                    param = new Vector3TCB(br);
                    break;

                case 9:
                    param = new QuaternionLinear(br);
                    break;

                case 10:
                    param = new QuaternionBezier(br);
                    break;

                case 11:
                    param = new QuaternionTCB(br);
                    break;

                case 12:
                    param = new SubtitleParam(br);
                    break;

                case 13:
                    param = new AnimBlockParam(br);
                    break;

                case 15:
                    param = new ParticleObject(br);
                    break;

                case 16:
                    param = new CameraCut(br);
                    break;

                case 17:
                    param = new CutChange(br);
                    break;

                case 18:
                    param = new SoundObjectAmbient(br);
                    break;

                case 19:
                    param = new SoundObjectPoint(br);
                    break;

                case 21:
                    param = new Script(br);
                    break;

                case 22:
                    param = new ModelAction(br);
                    break;

                case 26:
                    param = new DOF(br);
                    break;

                case 27:
                    param = new PositionXYZ(br);
                    break;

                case 28:
                    param = new EulerXYZ(br);
                    break;

                case 30:
                    param = new Unk30Action(br);
                    break;

                case 36:
                    param = new Unk36Action(br);
                    break;

                case 39:
                    param = new CameraAction(br);
                    break;

                case 40:
                    param = new CameraTargetAction(br);
                    break;

                case 41:
                    param = new EntityAction(br);
                    break;

                case 42:
                    param = new EffectAction(br);
                    break;

                case 43:
                    param = new SoundObjectSphereAmbient(br);
                    break;

                case 45:
                    param = new SoundListener(br);
                    break;

                case 46:
                    param = new SoundEntityAction(br);
                    break;

                case 47:
                    param = new WeatherPreset(br);
                    break;

                case 48:
                    param = new RadioAction(br);
                    break;

                case 31:
                case 32:
                case 33:
                case 34:
                    param = new TempParam(br, Type); //CurveListParam
                    break;
                case 14:
                    param = new TempParam(br, Type); //VideoObject //MultimediaParam //Couldn't find any
                    break;
                case 44:
                    param = new TempParam(br, Type); //ParticleSettings //ParticleParam //Couldn't find any
                    break;
                case 20:
                    param = new TempParam(br, Type); //SoundObjectCone //SoundParam //Couldn't find any
                    break;
                case 24:
                    param = new TempParam(br, Type); //HumanAction //ActionParam //Couldn't find any
                    break;
                case 23:
                    param = new TempParam(br, Type); //Couldn't find any
                    break;
                case 25:
                    param = new TempParam(br, Type); //HumanParam //Couldn't find any
                    break;
                case 35:
                    param = new TempParam(br, Type); //HumanMRParam //Couldn't find any
                    break;
                case 37:
                    param = new TempParam(br, Type); //ContestParam //Couldn't find any
                    break;
                case 38:
                    param = new TempParam(br, Type); //GameEventParam //Couldn't find any
                    break;
                default:
                    param = new TempParam(br, Type);
                    break;
            }

            ToolkitAssert.Ensure(br.BaseStream.Position == br.BaseStream.Length, $"Failed to read param type: {Type}");

            return param;
        }

        private static void DumpParamData(BinaryReader br, int Type, string CutsceneName, int index)
        {
            string folderPath = "%userprofile%\\Desktop\\KeyParams";
            string path = Environment.ExpandEnvironmentVariables(folderPath);
            path = Path.Combine(path, CutsceneName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
            File.WriteAllBytes(Path.Combine(path, $"KeyParam_Type_{Type}_{index.ToString("0000")}_{FNV32.Hash(data, 0, data.Length)}.bin"), data);
            br.BaseStream.Position = 4;
        }
    }
}
