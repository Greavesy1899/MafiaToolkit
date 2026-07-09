// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering.Core
{
    public class SpatialCell_TranslokatorParams : SpatialCell_InitParams
    {
    }

    public class SpatialCell_Translokator : SpatialCell
    {
        public SpatialCell_Translokator(SpatialCell_InitParams InitParams) : base(InitParams)
        {
            if (InitParams is SpatialCell_TranslokatorParams)
            {
                // TODO:
            }
        }
    }
}
