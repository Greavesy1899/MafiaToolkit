using OodleSharp;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mafia2Tool.Utils
{
    public static class OodleDllResolver
    {
        private const string OodleDllName = "oo2core_8_win64.dll";

        private static string LibraryFullPath;
        private static bool IsInitialized;

        public static bool TryResolveFrom(string libraryLocation)
        {
            if (IsInitialized)
            {
                return true;
            }

            LibraryFullPath = Path.Combine(libraryLocation, OodleDllName);

            if (!File.Exists(LibraryFullPath))
            {
                return false;
            }

            NativeLibrary.SetDllImportResolver(typeof(Oodle).Assembly, OodleResolver);
            IsInitialized = true;

            return true;
        }

        private static IntPtr OodleResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            var libHandle = IntPtr.Zero;

            if (libraryName == OodleDllName)
            {
                NativeLibrary.TryLoad(LibraryFullPath, out libHandle);
            }
            
            return libHandle;
        }
    }
}
