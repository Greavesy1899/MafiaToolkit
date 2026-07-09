// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using Toolkit.Forms;
using System.IO;

namespace Core.IO
{
    public class FileFxActor : FileBase
    {
        public FileFxActor(FileInfo info) : base(info) { }

        public override string GetExtensionUpper()
        {
            return "FXA";
        }

        public override bool Open()
        {
            FxActorEditor ActorEditor = new FxActorEditor(GetUnderlyingFileInfo());
            return true;
        }
    }
}
