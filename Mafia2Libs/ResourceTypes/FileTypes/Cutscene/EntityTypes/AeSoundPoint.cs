namespace ResourceTypes.Cutscene.AnimEntities
{
    public class AeSoundPointWrapper : AnimEntityWrapper
    {
        public AeSoundPointWrapper() : base()
        {
            AnimEntityData = new AeUnk7Data();
        }

        public override AnimEntityTypes GetEntityType()
        {
            return AnimEntityTypes.AeSoundPoint;
        }
    }
}
