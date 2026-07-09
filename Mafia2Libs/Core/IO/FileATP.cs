// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using Toolkit.Forms;
using System.IO;

namespace Core.IO
{
    class FileATP : FileBase
    {
        public FileATP(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "ATP";
        }

        public override bool Open()
        {
            ATPEditor editor = new ATPEditor(file);
            return true;
        }
    }
}
