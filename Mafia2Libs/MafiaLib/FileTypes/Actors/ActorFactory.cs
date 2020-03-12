using Gibbed.Illusion.FileFormats.Hashing;
using System;
namespace ResourceTypes.Actors
{
    public static class ActorFactory
    {
        public static IActorExtraDataInterface CreateExtraData(ActorTypes type)
        {
            switch(type)
            {
                case ActorTypes.C_TrafficCar:
                    return new ActorTrafficCar();
                case ActorTypes.C_TrafficHuman:
                    return new ActorTrafficHuman();
                case ActorTypes.C_TrafficTrain:
                    return new ActorTrafficTrain();
                case ActorTypes.C_Item:
                    return new ActorItem();
                case ActorTypes.C_Door:
                    return new ActorDoor();
                case ActorTypes.C_Sound:
                    return new ActorSoundEntity();
                case ActorTypes.StaticEntity:
                    return new ActorStaticEntity();
                case ActorTypes.FrameWrapper:
                    return new ActorFrameWrapper();
                case ActorTypes.C_ActorDetector:
                    return new ActorActorDetector();
                case ActorTypes.C_StaticParticle:
                    return new ActorStaticParticle();
                case ActorTypes.LightEntity:
                    return new ActorLight();
                case ActorTypes.C_Cutscene:
                    throw new NotImplementedException();
                case ActorTypes.C_ScriptEntity:
                    return new ActorScriptEntity();
                case ActorTypes.C_Pinup:
                    return new ActorPinup();
                default:
                    throw new NotImplementedException();
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
