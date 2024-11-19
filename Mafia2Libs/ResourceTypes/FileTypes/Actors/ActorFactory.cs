using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.IO;

namespace ResourceTypes.Actors
{
    public static class ActorFactory
    {
        public static IActorExtraDataInterface CreateExtraData(ActorTypes type)
        {
            switch(type)
            {
                case ActorTypes.Human:
                    return new ActorHuman();
                case ActorTypes.C_CrashObject:
                    return new ActorCrashObject();
                case ActorTypes.C_TrafficCar:
                    return new ActorTrafficCar();
                case ActorTypes.C_TrafficHuman:
                    return new ActorTrafficHuman();
                case ActorTypes.C_TrafficTrain:
                    return new ActorTrafficTrain();
                case ActorTypes.ActionPoint:
                    return new ActorActionPoint();
                case ActorTypes.ActionPointScript:
                    return new ActorActionPointScript();
                case ActorTypes.ActionPointSearch:
                    return new ActorActionPointSearch();
                case ActorTypes.C_Item:
                    return new ActorItem();
                case ActorTypes.C_Door:
                    return new ActorDoor();
                case ActorTypes.Tree:
                    return new ActorTree();
                case ActorTypes.Lift:
                    return new ActorLift();
                case ActorTypes.C_Sound:
                    return new ActorSoundEntity();
                case ActorTypes.SoundMixer:
                    return new ActorSoundMixer();
                case ActorTypes.StaticEntity:
                    return new ActorStaticEntity();
                case ActorTypes.Garage:
                    return new ActorGarage();
                case ActorTypes.FrameWrapper:
                    return new ActorFrameWrapper();
                case ActorTypes.C_ActorDetector:
                    return new ActorActorDetector();
                case ActorTypes.Blocker:
                    return new ActorBlocker();
                case ActorTypes.C_StaticWeapon:
                    return new ActorStaticWeapon();
                case ActorTypes.C_StaticParticle:
                    return new ActorStaticParticle();
                case ActorTypes.FireTarget:
                    return new ActorFireTarget();
                case ActorTypes.LightEntity:
                    return new ActorLight();
                case ActorTypes.C_Cutscene:
                    return new ActorCutscene();
                case ActorTypes.C_ScriptEntity:
                    return new ActorScriptEntity();
                case ActorTypes.C_Pinup:
                    return new ActorPinup();
                case ActorTypes.JukeBox:
                    return new ActorJukebox();
                case ActorTypes.PhysicsScene:
                    return new ActorPhysicsScene();
                case ActorTypes.Boat:
                    return new ActorBoat();
                case ActorTypes.Airplane:
                    return new ActorAircraft();
                case ActorTypes.Wardrobe:
                    return new ActorWardrobe();
                case ActorTypes.CleanEntity:
                    return new ActorCleanEntity();
                case ActorTypes.Radio:
                    return new ActorRadio();
                case ActorTypes.Telephone:
                    return new ActorTelephone();
                case ActorTypes.DangerZone:
                    return new ActorDamageZone();
                case ActorTypes.FramesController:
                    return new ActorFramesController();
                default:
                    Console.WriteLine("Cannot read type: " + type);
                    return null;
            }
        }

        public static IActorExtraDataInterface LoadEntityDataStorage(ActorEDSTypes type, MemoryStream stream, bool isBigEndian)
        {
            IActorExtraDataInterface NewExtraData = null;

            switch(type)
            {
                case ActorEDSTypes.C_Car:
                    NewExtraData = new ActorCar();
                    break;
                case ActorEDSTypes.C_ActionPointScript:
                    NewExtraData = new ActorActionPointScript();
                    break;
                case ActorEDSTypes.C_Train:
                    NewExtraData = new ActorTrain();
                    break;
                case ActorEDSTypes.Human:
                    NewExtraData = new ActorHuman();
                    break;
                case ActorEDSTypes.C_Player2:
                    NewExtraData = new ActorPlayer2();
                    break;
                default:
                    NewExtraData = null;
                    break;
            }

            NewExtraData.ReadFromFile(stream, isBigEndian);
            return NewExtraData;
        }

        public static IActorExtraDataInterface LoadExtraData(ActorTypes type, MemoryStream stream, bool isBigEndian)
        {
            IActorExtraDataInterface NewExtraData = CreateExtraData(type);
            if(NewExtraData == null)
            {
                Console.WriteLine("Cannot read type: " + type);
                return null;
            }

            NewExtraData.ReadFromFile(stream, isBigEndian);

            return NewExtraData;
        }

        public static ActorEntry CreateActorItem(ActorTypes type, string name)
        {
            ActorEntry item = new ActorEntry();
            item.ActorTypeID = (int)type;
            item.ActorTypeName = type.ToString();
            item.EntityName = name;
            item.EntityHash = FNV64.Hash(item.EntityName);
            return item;
        }
    }
}
