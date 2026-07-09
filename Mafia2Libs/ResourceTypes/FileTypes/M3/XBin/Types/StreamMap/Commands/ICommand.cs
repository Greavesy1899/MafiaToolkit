// SPDX-License-Identifier: LicenseRef-PolyForm-Strict-1.0.0
// Copyright (c) 2018-2026 Greavesy

using ResourceTypes.M3.XBin;
using System.IO;

namespace FileTypes.XBin.StreamMap.Commands
{
    public interface ICommand
    {
        void ReadFromFile(BinaryReader reader);

        void WriteToFile(XBinWriter writer);
        int GetSize();
        uint GetMagic();
    }
}
