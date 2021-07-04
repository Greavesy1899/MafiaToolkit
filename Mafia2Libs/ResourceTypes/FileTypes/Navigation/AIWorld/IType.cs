using Rendering.Core;
using SharpDX;
using System.IO;
using System.Windows.Forms;
using Utils.StringHelpers;

namespace ResourceTypes.Navigation
{
    public class IType
    {
        protected int RefID;
        protected AIWorld OwnWorld;
        
        public bool bIsVisible { get; protected set; }

        public IType(AIWorld InWorld) 
        { 
            RefID = StringHelpers.GetNewRefID(); 
            OwnWorld = InWorld;
            bIsVisible = true;
        }

        //~ Interface
        public virtual void Read(BinaryReader Reader) { }
        public virtual void Write(BinaryWriter Writer) { }
        public virtual void DebugWrite(StreamWriter Writer) { }
        public virtual void ConstructRenderable(PrimitiveBatch BBoxBatcher) { }
        public virtual TreeNode PopulateTreeNode() { return null; }
        public virtual Vector3 GetPosition() { return Vector3.Zero; }
        //~ Interface

        public void NotifyUpdate() 
        { 
            OwnWorld.RequestPrimitiveBatchUpdate(); 
        }

        public void SetVisiblity(bool bInVisiblity)
        {
            bIsVisible = bInVisiblity;
            NotifyUpdate();
        }
    }
}
