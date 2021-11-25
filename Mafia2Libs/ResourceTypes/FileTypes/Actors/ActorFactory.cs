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
                case ActorTypes.C_Sound:
                    return new ActorSoundEntity();
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
                case ActorTypes.LightEntity:
                    return new ActorLight();
                case ActorTypes.C_Cutscene:
                    return new ActorCutscene();
                case ActorTypes.C_ScriptEntity:
                    return new ActorScriptEntity();
                case ActorTypes.C_Pinup:
                    return new ActorPinup();
                default:
                    throw new NotImplementedException();
            }
        }

        public static IActorExtraDataInterface LoadEntityDataStorage(ActorEDSTypes type, MemoryStream stream, bool isBigEndian)
        {
            switch(type)
            {
                case ActorEDSTypes.C_Car:
                    return new ActorCar(stream, isBigEndian);
                case ActorEDSTypes.C_ActionPointScript:
                    return new ActorActionPointScript(stream, isBigEndian);
                case ActorEDSTypes.C_Train:
                    throw new NotImplementedException();
                default:
                    return null;
            }
        }

        public static IActorExtraDataInterface LoadExtraData(ActorTypes type, MemoryStream stream, bool isBigEndian)
        {
            switch(type)
            {
                case ActorTypes.Human:
                    return new ActorHuman(stream, isBigEndian);
                case ActorTypes.C_CrashObject:
                    return new ActorCrashObject(stream, isBigEndian);
                case ActorTypes.C_TrafficCar:
                    return new ActorTrafficCar(stream, isBigEndian);
                case ActorTypes.C_TrafficHuman:
                    return new ActorTrafficHuman(stream, isBigEndian);
                case ActorTypes.C_TrafficTrain:
                    return new ActorTrafficTrain(stream, isBigEndian);
                case ActorTypes.ActionPoint:
                    return new ActorActionPoint(stream, isBigEndian);
                case ActorTypes.ActionPointScript:
                    return new ActorActionPointScript(stream, isBigEndian);
                case ActorTypes.ActionPointSearch:
                    return new ActorActionPointSearch(stream, isBigEndian);
                case ActorTypes.C_Item:
                    return new ActorItem(stream, isBigEndian);
                case ActorTypes.C_Door:
                    return new ActorDoor(stream, isBigEndian);
                case ActorTypes.Tree:
                    return new ActorTree(stream, isBigEndian);
                case ActorTypes.C_Sound:
                    return new ActorSoundEntity(stream, isBigEndian);
                case ActorTypes.Radio:
                    return new ActorRadio(stream, isBigEndian);
                case ActorTypes.StaticEntity:
                    return new ActorStaticEntity(stream, isBigEndian);
                case ActorTypes.Garage:
                    return new ActorGarage(stream, isBigEndian);
                case ActorTypes.FrameWrapper:
                    return new ActorFrameWrapper(stream, isBigEndian);
                case ActorTypes.C_ActorDetector:
                    return new ActorActorDetector(stream, isBigEndian);
                case ActorTypes.Blocker:
                    return new ActorBlocker(stream, isBigEndian);
                case ActorTypes.C_StaticWeapon:
                    return new ActorStaticWeapon(stream, isBigEndian);
                case ActorTypes.C_StaticParticle:
                    return new ActorStaticParticle(stream, isBigEndian);
                case ActorTypes.LightEntity:
                    return new ActorLight(stream, isBigEndian);
                case ActorTypes.C_Cutscene:
                    return new ActorCutscene(stream, isBigEndian);
                case ActorTypes.C_ScriptEntity:
                    return new ActorScriptEntity(stream, isBigEndian);
                case ActorTypes.C_Pinup:
                    return new ActorPinup(stream, isBigEndian);
                case ActorTypes.SpikeStrip:
                    return new ActorSpikeStrip(stream, isBigEndian);
                case ActorTypes.Wardrobe:
                    return new ActorWardrobe(stream, isBigEndian);
                case ActorTypes.CleanEntity:
                    return new ActorCleanEntity(stream, isBigEndian);
                default:
                    Console.WriteLine("Cannot read type: " + type);
                    return null;
            }
        }

        public static IActorExtraDataInterface CreateDuplicateExtraData(ActorTypes type, IActorExtraDataInterface extraData)
        {
            switch(type)
            {
                case ActorTypes.C_Sound:
                    return new ActorSoundEntity(extraData);
                default:
                    return null;
            }
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
