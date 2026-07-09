// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using System;

namespace ResourceTypes.Collisions.PhysX
{
    public class PhysXException : Exception
    {
        public PhysXException(string message) : base(message)
        {
        }
    }
}
