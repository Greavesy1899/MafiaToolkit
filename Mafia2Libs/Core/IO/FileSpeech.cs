// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using Mafia2Tool;
using System.IO;

namespace Core.IO
{
    class FileSpeech : FileBase
    {
        public FileSpeech(FileInfo info) : base(info)
        {
        }

        public override string GetExtensionUpper()
        {
            return "SPE";
        }

        public override bool Open()
        {
            SpeechEditor editor = new SpeechEditor(file);
            return true;
        }
    }
}
