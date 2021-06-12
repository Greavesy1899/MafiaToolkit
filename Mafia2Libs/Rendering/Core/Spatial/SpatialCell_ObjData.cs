using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rendering.Graphics;
using ResourceTypes.Navigation;
using Utils.StringHelpers;

namespace Rendering.Core
{
    public class SpatialCell_ObjDataParams : SpatialCell_InitParams
    {
        public KynogonRuntimeMesh.Cell CellInfo { get; set; }
    }

    public class SpatialCell_ObjData : SpatialCell
    {
        private KynogonRuntimeMesh.Cell OurCellInfo = null;

        public RenderNavCell NavCell { get; set; }

        public SpatialCell_ObjData(SpatialCell_InitParams InitParams) : base(InitParams)
        {
            if(InitParams is SpatialCell_ObjDataParams)
            {
                OurCellInfo = (InitParams as SpatialCell_ObjDataParams).CellInfo;
            }
        }

        public override void PreInitialise()
        {
            base.PreInitialise();

            NavCell = new RenderNavCell(OwnGraphicsClass);
            NavCell.Init(OurCellInfo);
            assets.Add(StringHelpers.GetNewRefID(), NavCell);
        }
    }
}
