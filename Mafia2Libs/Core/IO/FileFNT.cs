// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using Mafia2Tool;
using System.IO;

namespace Core.IO
{
    public class FileFNT : FileBase
    {
        public FileFNT(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "FNT";
        }

        public override bool Open()
        {
            FrameNameTableEditor editor = new FrameNameTableEditor(file);
            return true;
        }
    }
}
