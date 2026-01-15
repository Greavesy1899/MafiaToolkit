using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Core.IO
{
    /// <summary>
    /// Factory registry for BIN file editors.
    /// This decouples Core.IO from Forms by allowing Forms to register editor factories
    /// that Core.IO can invoke without knowing about specific editor types.
    /// </summary>
    public static class BinEditorFactory
    {
        /// <summary>
        /// Delegate for creating an editor form for a given file.
        /// Returns true if the editor was created successfully.
        /// </summary>
        public delegate bool EditorCreator(FileInfo file);

        /// <summary>
        /// Delegate for checking if a file matches a specific type (e.g., by filename).
        /// </summary>
        public delegate bool FileMatcher(FileInfo file);

        /// <summary>
        /// Registered editor factories by magic number.
        /// </summary>
        private static readonly Dictionary<uint, EditorCreator> MagicEditors = new();

        /// <summary>
        /// Registered editor factories by file matcher (for files without magic numbers).
        /// </summary>
        private static readonly List<(FileMatcher Matcher, EditorCreator Creator)> MatcherEditors = new();

        /// <summary>
        /// Default fallback editor creator (e.g., for XML export).
        /// </summary>
        private static EditorCreator DefaultEditor = null;

        /// <summary>
        /// Register an editor factory for a specific magic number.
        /// </summary>
        /// <param name="magic">The 4-byte magic number at the start of the file.</param>
        /// <param name="creator">Factory delegate that creates the editor.</param>
        public static void RegisterMagicEditor(uint magic, EditorCreator creator)
        {
            MagicEditors[magic] = creator;
        }

        /// <summary>
        /// Register an editor factory using a custom file matcher.
        /// </summary>
        /// <param name="matcher">Delegate that checks if this factory handles the file.</param>
        /// <param name="creator">Factory delegate that creates the editor.</param>
        public static void RegisterMatcherEditor(FileMatcher matcher, EditorCreator creator)
        {
            MatcherEditors.Add((matcher, creator));
        }

        /// <summary>
        /// Set the default fallback editor for unrecognized files.
        /// </summary>
        public static void SetDefaultEditor(EditorCreator creator)
        {
            DefaultEditor = creator;
        }

        /// <summary>
        /// Try to create an editor for the given file.
        /// </summary>
        /// <param name="file">The file to open.</param>
        /// <returns>True if an editor was created, false otherwise.</returns>
        public static bool TryCreateEditor(FileInfo file)
        {
            // First, check magic-based editors
            uint? magic = ReadFileMagic(file);
            if (magic.HasValue && MagicEditors.TryGetValue(magic.Value, out var magicCreator))
            {
                return magicCreator(file);
            }

            // Then, check matcher-based editors
            foreach (var (matcher, creator) in MatcherEditors)
            {
                if (matcher(file))
                {
                    return creator(file);
                }
            }

            // Finally, use default editor if available
            if (DefaultEditor != null)
            {
                return DefaultEditor(file);
            }

            return false;
        }

        /// <summary>
        /// Check if an editor is registered for a given magic number.
        /// </summary>
        public static bool HasMagicEditor(uint magic)
        {
            return MagicEditors.ContainsKey(magic);
        }

        /// <summary>
        /// Read the first 4 bytes of a file as a magic number.
        /// </summary>
        private static uint? ReadFileMagic(FileInfo file)
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(file.FullName, FileMode.Open, FileAccess.Read)))
                {
                    if (reader.BaseStream.Length >= 4)
                    {
                        return reader.ReadUInt32();
                    }
                }
            }
            catch
            {
                // File couldn't be read
            }
            return null;
        }

        /// <summary>
        /// Clear all registered editors (useful for testing).
        /// </summary>
        public static void ClearAll()
        {
            MagicEditors.Clear();
            MatcherEditors.Clear();
            DefaultEditor = null;
        }
    }
}
