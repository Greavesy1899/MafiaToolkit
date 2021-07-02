using System.IO;

namespace ResourceTypes.OC3.FaceFX
{
    public class FxAnimGroup : FxNamedObject
    {
        public FxAnim[] Animations { get; set; }

        public override void Deserialize(FxArchive Owner, BinaryReader Reader)
        {
            base.Deserialize(Owner, Reader);

            // Read animations
            uint NumAnims = Reader.ReadUInt32();
            Animations = new FxAnim[NumAnims];
            for (int x = 0; x < NumAnims; x++)
            {
                FxAnim AnimObject = new FxAnim();
                AnimObject.Deserialize(Owner, Reader);

                Animations[x] = AnimObject;
            }
        }

        public override void Serialize(FxArchive Owner, BinaryWriter Writer)
        {
            base.Serialize(Owner, Writer);

            // Write animations
            Writer.Write(Animations.Length);
            foreach (FxAnim Animation in Animations)
            {
                Animation.Serialize(Owner, Writer);
            }
        }

        public override void PopulateStringTable(FxArchive Owner)
        {
            base.PopulateStringTable(Owner);

            foreach(FxAnim Animation in Animations)
            {
                Animation.PopulateStringTable(Owner);
            }
        }
    }
}
