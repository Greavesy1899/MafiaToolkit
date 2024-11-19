using System;
using Rendering.Graphics;

namespace Rendering.Core
{
    public class RenderableAdapter
    {
        private IRenderer RenderItem;
        private object ParentObject;

        public void InitAdaptor(IRenderer InRenderItem, object InTag)
        {
            ParentObject = InTag;
            RenderItem = InRenderItem;
        }

        public T GetRenderItem<T>() where T : IRenderer
        {
            return (T)Convert.ChangeType(RenderItem, typeof(T));
        }

        public IRenderer GetRenderItem()
        {
            return RenderItem;
        }
    }
}
