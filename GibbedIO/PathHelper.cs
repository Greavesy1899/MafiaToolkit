/* Copyright (c) 2017 Rick (rick 'at' gibbed 'dot' us)
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would
 *    be appreciated but is not required.
 * 
 * 2. Altered source versions must be plainly marked as such, and must not
 *    be misrepresented as being the original software.
 * 
 * 3. This notice may not be removed or altered from any source
 *    distribution.
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace Gibbed.IO
{
    public static class PathHelper
    {
        public static string GetRelativePath(string fromPath, string toPath)
        {
            if (fromPath == null)
            {
                throw new ArgumentNullException("fromPath");
            }

            if (toPath == null)
            {
                throw new ArgumentNullException("toPath");
            }

            if (Path.IsPathRooted(fromPath) == true && Path.IsPathRooted(toPath) == true)
            {
                if (string.Compare(Path.GetPathRoot(fromPath),
                                   Path.GetPathRoot(toPath),
                                   StringComparison.OrdinalIgnoreCase) != 0)
                {
                    return null;
                }
            }

            var relativePath = new List<string>();
            var fromDirectories = fromPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var toDirectories = toPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            int length = Math.Min(fromDirectories.Length, toDirectories.Length);
            int lastCommonRoot = -1;

            // find common root
            for (int x = 0; x < length; x++)
            {
                if (string.Compare(fromDirectories[x], toDirectories[x], StringComparison.OrdinalIgnoreCase) != 0)
                {
                    break;
                }

                lastCommonRoot = x;
            }

            if (lastCommonRoot == -1)
            {
                return toPath;
            }

            // add relative folders in from path
            for (int x = lastCommonRoot + 1; x < fromDirectories.Length; x++)
            {
                if (fromDirectories[x].Length > 0)
                {
                    relativePath.Add("..");
                }
            }

            // add to folders to path
            for (int x = lastCommonRoot + 1; x < toDirectories.Length; x++)
            {
                relativePath.Add(toDirectories[x]);
            }

            // create relative path
            return Path.Combine(relativePath.ToArray());
        }
    }
}
