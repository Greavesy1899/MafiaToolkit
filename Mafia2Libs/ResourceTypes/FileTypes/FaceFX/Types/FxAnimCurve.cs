using System.IO;

namespace ResourceTypes.OC3.FaceFX
{

    /* A structure to allow manipulation of FxFaceGroupNode throughout 
    * an animation. Uses AnimKeys to pinpoint where in the animation 
    * the manipulation should take place and how strong it should be.
    */
    public class FxAnimCurve : FxNamedObject
    {
        public uint Unk0 { get; set; }
        public FxAnimKey[] AnimKeys { get; set; }

        public FxAnimCurve() : base()
        {
            AnimKeys = new FxAnimKey[0];
        }

        public override void Deserialize(FxArchive Owner, BinaryReader Reader)
        {
            base.Deserialize(Owner, Reader);

            Unk0 = Reader.ReadUInt32();
        }

        public override void Serialize(FxArchive Owner, BinaryWriter Writer)
        {
            base.Serialize(Owner, Writer);

            Writer.Write(Unk0);
        }
    }

}
