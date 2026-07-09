// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

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
