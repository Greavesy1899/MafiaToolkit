// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

namespace Toolkit.Core
{
    public static class RefManager
    {
        //set to 10 because the first 10 are placeholders for render assets.
        private static int currentRefID = 10;

        public static int GetNewRefID()
        {
            currentRefID++;
            return currentRefID;
        }
    }
}
