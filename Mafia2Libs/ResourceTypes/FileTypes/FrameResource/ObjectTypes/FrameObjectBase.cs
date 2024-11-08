using Rendering.Core;
using Rendering.Graphics;
using ResourceTypes.FrameNameTable;
using ResourceTypes.Misc;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using Utils.Extensions;
using Utils.Logging;
using Utils.Types;
using Utils.VorticeUtils;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectBase : FrameEntry
    {
        protected HashName name;
        protected int secondaryFlags;
        protected short unk3;
        protected ParentInfo parentIndex1;
        protected ParentInfo parentIndex2;
        protected short unk6;
        protected bool isOnTable;
        protected NameTableFlags nameTableFlags;

        protected Matrix4x4 worldTransform = Matrix4x4.Identity;
        protected Matrix4x4 localTransform = Matrix4x4.Identity;

        FrameObjectBase parent;
        FrameObjectBase root;
        List<FrameObjectBase> children = new List<FrameObjectBase>();

        public RenderableAdapter RenderAdapter { get; set; }

        public FrameObjectBase Parent {
            get { return parent; }
            set { parent = value; }
        }
        public FrameObjectBase Root {
            get { return root; }
            set { root = value; }
        }
        public List<FrameObjectBase> Children {
            get { return children; }
            set { children = value; }
        }

        public HashName Name {
            get { return name; }
            set { name = value; }
        }
        public int SecondaryFlags {
            get { return secondaryFlags; }
            set { secondaryFlags = value; }
        }
        //[Browsable(false)]
        public Matrix4x4 LocalTransform {
            get { return localTransform; }
            set { localTransform = value; SetWorldTransform(); }
        }
        public Matrix4x4 WorldTransform {
            get { SetWorldTransform(); return worldTransform; }
            set { worldTransform = value; }
        }
        [Browsable(true)]
        public short Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }
        public ParentInfo ParentIndex1 {
            get { return parentIndex1; }
            set { parentIndex1 = value; }
        }
        public ParentInfo ParentIndex2 {
            get { return parentIndex2; }
            set { parentIndex2 = value; }
        }
        [Browsable(true)]
        public short Unk6 {
            get { return unk6; }
            set { unk6 = value; }
        }
        [Description("Only use this if the object is going to be saved in the FrameNameTable"), Category("FrameNameTable Data"), Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public NameTableFlags FrameNameTableFlags {
            get { return nameTableFlags; }
            set { nameTableFlags = value; }
        }
        [Description("If this is true, it will be added onto the FrameNameTable and the flags will be saved"), Category("FrameNameTable Data")]
        public bool IsOnFrameTable {
            get { return isOnTable; }
            set { isOnTable = value; }
        }
        [Category("Object Type"), ReadOnly(true)]
        public string Type {
            get { return GetType().ToString(); }
        }
        [ReadOnly(true)]
        public int Index { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public FrameProps.FrameInfo OurFrameProps { get; set; }

        public FrameObjectBase(FrameResource OwningResource) : base(OwningResource)
        {
            //do example name.
            name = new HashName("NewObject");
            secondaryFlags = 1;
            localTransform = Matrix4x4.Identity;
            worldTransform = Matrix4x4.Identity;
            unk3 = -1;
            parentIndex1 = new ParentInfo(-1);
            parentIndex2 = new ParentInfo(-1);
            unk6 = -1;
        }

        public FrameObjectBase(FrameObjectBase other) : base(other)
        {
            name = new HashName(other.name.String);
            secondaryFlags = other.secondaryFlags;
            localTransform = other.localTransform;
            worldTransform = other.worldTransform;
            unk3 = other.unk3;
            parentIndex1 = new ParentInfo(other.parentIndex1);
            parentIndex2 = new ParentInfo(other.parentIndex2);
            unk6 = -1;
            isOnTable = other.isOnTable;
            nameTableFlags = other.nameTableFlags;
        }

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            name = new HashName(stream, isBigEndian);
            secondaryFlags = stream.ReadInt32(isBigEndian);
            localTransform = MatrixUtils.ReadFromFile(stream, isBigEndian);
            unk3 = stream.ReadInt16(isBigEndian);
            parentIndex1 = new ParentInfo(stream.ReadInt32(isBigEndian));
            parentIndex2 = new ParentInfo(stream.ReadInt32(isBigEndian));
            unk6 = stream.ReadInt16(isBigEndian);
        }

        public virtual void WriteToFile(BinaryWriter writer)
        {
            SanitizeOnSave();

            name.WriteToFile(writer);
            writer.Write(secondaryFlags);
            MatrixUtils.WriteToFile(localTransform, writer);
            writer.Write(unk3);
            writer.Write(parentIndex1.Index);
            writer.Write(parentIndex2.Index);
            writer.Write(unk6);
        }

        protected virtual void SanitizeOnSave() { }

        public void SetWorldTransform()
        {
            //The world transform is calculated and then decomposed because some reason,
            //the renderer does not update on the first startup of the editor.
            Vector3 position, scale, newPos;
            Quaternion rotation, newRot;
            Matrix4x4 parentTransform = Matrix4x4.Identity;
            Matrix4x4.Decompose(localTransform, out scale, out rotation, out position);
            worldTransform = Matrix4x4.Identity;

            if (parent != null)
            {
                parentTransform = parent.worldTransform;
            }
            else if (root != null)
            {
                parentTransform = root.worldTransform;
            }

            if(parent != null || root != null)
            {
                Vector3 parentPosition = Vector3.Zero;
                Vector3 parentScale = Vector3.One;
                Quaternion parentRotation = Quaternion.Identity;
                Matrix4x4.Decompose(parentTransform, out parentScale, out parentRotation, out parentPosition);

                newRot = parentRotation * rotation;
                newPos = Vector3Utils.TransformCoordinate(position, parentTransform);
            }
            else
            {
                newRot = rotation;
                newPos = position;
            }

            worldTransform = MatrixUtils.SetMatrix(newRot, scale, newPos);
            ToolkitAssert.Ensure(!worldTransform.IsNaN(), string.Format("Frame: {0} caused NaN()!", name.ToString()));
            foreach (var child in children)
            {
                child.SetWorldTransform();
            }
        }

        public void SetParent(ParentInfo.ParentType ParentType, FrameEntry NewParent)
        {
            // Fix any parent-children relationships.
            if (Parent != null)
            {
                Parent.children.Remove(this);
                Parent = null;
            }

            // This is if the user wants to change parent
            if (NewParent != null)
            {
                int index = (NewParent is FrameHeaderScene) ? OwningResource.FrameScenes.IndexOfValue(NewParent.RefID) : OwningResource.GetIndexOfObject(NewParent.RefID);
                FrameObjectBase ParentObject = (NewParent as FrameObjectBase);

                // Fix any parent relationships only if ParentObj is not null.
                if (ParentObject != null)
                {
                    ParentObject.children.Add(this);
                    Parent = ParentObject;
                }

                // Update Parent
                InternalSetParent(ParentType, NewParent, index);
            }
            else // this is if the user wants to remove the parent relationship, therefore -1 = root.
            {
                RemoveParent(ParentType);
            }
        }

        private void InternalSetParent(ParentInfo.ParentType ParentType, FrameEntry NewParent, int ParentIndex)
        {
            // Get type of FrameEntryRefType we want to replace/add
            FrameEntryRefTypes ParentRef = (ParentType == ParentInfo.ParentType.ParentIndex1) ?
                FrameEntryRefTypes.Parent1 : FrameEntryRefTypes.Parent2;

            ReplaceRef(ParentRef, NewParent.RefID);

            int index = (NewParent is FrameHeaderScene) ? OwningResource.FrameScenes.IndexOfValue(NewParent.RefID) : OwningResource.GetIndexOfObject(NewParent.RefID);

            // Update ParentInfo
            if (ParentType == ParentInfo.ParentType.ParentIndex1)
            {
                ParentIndex1.SetParent(NewParent, ParentIndex);
                ReplaceRef(FrameEntryRefTypes.Parent1, NewParent.RefID);
            }
            else
            {
                ParentIndex2.SetParent(NewParent, ParentIndex);
                ReplaceRef(FrameEntryRefTypes.Parent2, NewParent.RefID);
            }
        }

        private void RemoveParent(ParentInfo.ParentType ParentType)
        {
            // Get type of FrameEntryRefType we want to remove
            FrameEntryRefTypes ParentRef = (ParentType == ParentInfo.ParentType.ParentIndex1) ? 
                FrameEntryRefTypes.Parent1 : FrameEntryRefTypes.Parent2;

            // Remove the reference
            SubRef(ParentRef);
            
            // Remove the parent from the desired ParentIndex
            if (ParentType == ParentInfo.ParentType.ParentIndex1)
            {
                ParentIndex1.RemoveParent();
            }
            else
            {
                ParentIndex2.RemoveParent();
            }

        }

        public IRenderer GetRenderItem()
        {
            if (RenderAdapter != null)
            {
                return RenderAdapter.GetRenderItem();
            }

            return null;
        }

        public virtual void ConstructRenderable(Dictionary<int, IRenderer> assets)
        {
            // Empty, FrameObjectBase doesn't have anything to create.
            // TODO: Consider making empty adapter?5
        }
        
        public bool IsFrameOwnChildren(int newParentRefID)
        {
            if (RefID==newParentRefID)
            {
                return true;
            }
            
            foreach (var child in children)
            {
                if (child.IsFrameOwnChildren(newParentRefID))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasMeshObject()
        {
            foreach (var child in Children)
            {
                if (child is FrameObjectSingleMesh || child.HasMeshObject())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
