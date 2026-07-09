// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using Mafia2Tool;
using System;
using System.IO;

namespace Core.IO
{
    public class FileMaterialLibrary : FileBase
    {
        public FileMaterialLibrary(FileInfo info) : base(info)
        {
        }

        public override bool Open()
        {
            MaterialEditor editor = new MaterialEditor(file);
            return true;
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override string GetExtensionUpper()
        {
            return "MTL";
        }
    }
}
