// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

namespace ResourceTypes.FrameResource
{
    public class FrameObjectPoint : FrameObjectJoint
    {
        public FrameObjectPoint(FrameResource OwningResource) : base(OwningResource) { }

        public FrameObjectPoint(FrameObjectPoint other) : base(other) { }
    }
}
