using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ModelViewer.Graphics
{
    public class TextureArrayClass : ICollection<TextureClass>
    {
        private Device _Device;
        public List<TextureClass> TextureList { get; private set; }

        public bool Init(Device device, string[] fileNames)
        {
            try
            {
                TextureList = new List<TextureClass>();
                _Device = device;
                foreach (var fileName in fileNames)
                {
                    if (!AddFromFile(fileName))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public void Shutdown()
        {
            Clear();
        }
        public bool AddFromFile(string FileName)
        {
            TextureClass texture = new TextureClass();
            if (!texture.Init(_Device, FileName))
            {
                return false;
            }
            Add(texture);
            return true;
        }
        public void Add(TextureClass item)
        {
            if (TextureList != null)
            {
                TextureList.Add(item);
            }
        }
        public void Clear()
        {
            foreach (var texture in TextureList)
            {
                texture.Shutdown();
            }
            TextureList.Clear();
        }
        public bool Contains(TextureClass item)
        {
            throw new NotImplementedException();
        }
        public void CopyTo(TextureClass[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public int Count {
            get { return TextureList.Count; }
        }
        public bool IsReadOnly {
            get { return true; }
        }
        public bool Remove(TextureClass item)
        {
            throw new NotImplementedException();
        }
        public IEnumerator<TextureClass> GetEnumerator()
        {
            return TextureList.GetEnumerator();
        }
        global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}