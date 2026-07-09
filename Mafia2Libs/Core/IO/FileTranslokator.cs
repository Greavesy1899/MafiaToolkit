// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using Mafia2Tool.Forms;
using System.IO;

namespace Core.IO
{
    class FileTranslokator : FileBase
    {
        public FileTranslokator(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "TRA";
        }

        public override bool Open()
        {
            TranslokatorEditor editor = new TranslokatorEditor(file);
            return true;
        }
    }
}
