using System.IO;
using System.ComponentModel;
using ResourceTypes.FrameNameTable;
using Utils.Extensions;
using Utils.Types;
using SharpDX;
using Utils.SharpDXExtensions;
using System.Collections.Generic;
using System.Diagnostics;
using Mafia2Tool;
using Rendering.Core;
using Rendering.Graphics;

namespace ResourceTypes.FrameResource
{
    public class FrameObjectBase : FrameEntry
    {
        protected HashName name;
        protected int secondaryFlags;
        protected short unk3;
        protected ParentStruct parentIndex1;
        protected ParentStruct parentIndex2;
        protected short unk6;
        protected bool isOnTable;
        protected NameTableFlags nameTableFlags;
        protected string[] extraData;

        protected Matrix worldTransform = Matrix.Identity;
        protected Matrix localTransform = Matrix.Identity;

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
        public Matrix LocalTransform {
            get { return localTransform; }
            set { localTransform = value; SetWorldTransform(); }
        }
        public Matrix WorldTransform {
            get { SetWorldTransform(); return worldTransform; }
            set { worldTransform = value; }
        }
        [Browsable(true)]
        public short Unk3 {
            get { return unk3; }
            set { unk3 = value; }
        }
        public ParentStruct ParentIndex1 {
            get { return parentIndex1; }
            set { parentIndex1 = value; }
        }
        public ParentStruct ParentIndex2 {
            get { return parentIndex2; }
            set { parentIndex2 = value; }
        }
        [Browsable(true)]
        public short Unk6 {
            get { return unk6; }
            set { unk6 = value; }
        }
        [Category("Frame Properties"), Description("Data stored in FrameProps.bin"), Browsable(false)]
        public string[] ExtraData {
            get { return extraData; }
            set { extraData = value; }
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

        public FrameObjectBase() : base()
        {
            //do example name.
            name = new HashName("NewObject");
            secondaryFlags = 1;
            localTransform = Matrix.Identity;
            worldTransform = Matrix.Identity;
            unk3 = -1;
            parentIndex1 = new ParentStruct(-1);
            parentIndex2 = new ParentStruct(-1);
            unk6 = -1;
        }

        public FrameObjectBase(FrameObjectBase other) : base(other)
        {
            name = new HashName(other.name.String);
            secondaryFlags = other.secondaryFlags;
            localTransform = other.localTransform;
            worldTransform = other.worldTransform;
            unk3 = other.unk3;
            parentIndex1 = new ParentStruct(other.parentIndex1);
            parentIndex2 = new ParentStruct(other.parentIndex2);
            unk6 = -1;
            isOnTable = other.isOnTable;
            nameTableFlags = other.nameTableFlags;
        }

        public virtual void ReadFromFile(MemoryStream stream, bool isBigEndian)
        {
            name = new HashName(stream, isBigEndian);
            secondaryFlags = stream.ReadInt32(isBigEndian);
            localTransform = MatrixExtensions.ReadFromFile(stream, isBigEndian);
            unk3 = stream.ReadInt16(isBigEndian);
            parentIndex1 = new ParentStruct(stream.ReadInt32(isBigEndian));
            parentIndex2 = new ParentStruct(stream.ReadInt32(isBigEndian));
            unk6 = stream.ReadInt16(isBigEndian);

            if (SceneData.FrameProperties != null)
            {
                if (SceneData.FrameProperties.FrameExtraData.ContainsKey(name.Hash))
                {
                    extraData = SceneData.FrameProperties.FrameExtraData[name.Hash].Data;
                }
            }
        }

        public virtual void WriteToFile(BinaryWriter writer)
        {
            SanitizeOnSave();

            name.WriteToFile(writer);
            writer.Write(secondaryFlags);
            MatrixExtensions.WriteToFile(localTransform, writer);
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
            Matrix parentTransform = Matrix.Identity;
            localTransform.Decompose(out scale, out rotation, out position);
            worldTransform = Matrix.Identity;

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
                parentTransform.Decompose(out parentScale, out parentRotation, out parentPosition);

                newRot = parentRotation * rotation;
                newPos = Vector3.TransformCoordinate(position, parentTransform);
            }
            else
            {
                newRot = rotation;
                newPos = position;
            }

            worldTransform = MatrixExtensions.SetMatrix(newRot, scale, newPos);
            Debug.Assert(!worldTransform.IsNaN(), string.Format("Frame: {0} caused NaN()!", name.ToString()));
            foreach (var child in children)
            {
                child.SetWorldTransform();
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

        public virtual void ConstructRenderable()
        {
            // Empty, FrameObjectBase doesn't have anything to create.
            // TODO: Consider making empty adapter?5
        }
    }
}
