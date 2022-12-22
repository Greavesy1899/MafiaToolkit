using BitStreams;
using System.ComponentModel;
using Utils.Helpers.Reflection;

namespace ResourceTypes.Prefab.Vehicle
{
    [TypeConverter(typeof(ExpandableObjectConverter)), PropertyClassAllowReflection]
    public class S_AxleWheelInit
    {
        // TODO: Determine if NumAxle is always be Result * 2
        public S_InitAxle[] Axles { get; set; }

        public void Load(BitStream MemStream)
        {
            uint NumAxles = MemStream.ReadUInt32() * 2;
            Axles = new S_InitAxle[NumAxles];
            for (uint i = 0; i < NumAxles; i++)
            {
                S_InitAxle Axle = new S_InitAxle();
                Axle.Load(MemStream);

                Axles[i] = Axle;
            }
        }

        public void Save(BitStream MemStream)
        {
            // NB: We divide by two because we multiply by two in read function
            uint NumAxles = (uint)(Axles.Length / 2);
            MemStream.WriteUInt32(NumAxles);
            foreach(S_InitAxle Axle in Axles)
            {
                Axle.Save(MemStream);
            }
        }
    }
}
