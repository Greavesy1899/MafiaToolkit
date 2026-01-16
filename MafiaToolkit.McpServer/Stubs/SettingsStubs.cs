// Stub implementations for Utils.Settings types used by Gibbed.Mafia2.FileFormats
// These stubs provide minimal implementations for the MCP server

namespace Utils.Settings
{
    /// <summary>
    /// Game type enumeration
    /// </summary>
    public enum GamesEnumerator
    {
        None = 0,
        MafiaII = 1,
        MafiaII_DE = 2,
        MafiaIII = 3,
        MafiaI_DE = 4
    }

    /// <summary>
    /// Represents a game configuration
    /// </summary>
    public class Game
    {
        public string Name { get; }
        public string Description { get; }
        public string Directory { get; set; } = string.Empty;
        public string Materials { get; set; } = string.Empty;
        public GamesEnumerator GameType { get; }

        public Game(string name, string description, GamesEnumerator gameType)
        {
            Name = name;
            Description = description;
            GameType = gameType;
        }
    }

    /// <summary>
    /// Stub for ToolkitSettings - provides sensible defaults
    /// </summary>
    public static class ToolkitSettings
    {
        public static float CompressionRatio { get; set; } = 0.9f;
        public static bool bUseOodleCompression { get; set; } = true;
        public static int SerializeSDSOption { get; set; } = 0;
        public static bool DecompileLUA { get; set; } = false;
        public static bool EnableLuaHelper { get; set; } = false;
        public static bool LoggingEnabled { get; set; } = false;
        public static int Language { get; set; } = 0;
        public static string TexturePath { get; set; } = string.Empty;
        public static string ExportPath { get; set; } = string.Empty;
        public static int IndexMemorySizePerBuffer { get; set; } = 945000;
        public static int VertexMemorySizePerBuffer { get; set; } = 6000000;
        public static bool CookCollisions { get; set; } = true;
        public static bool UseSDSToolFormat { get; set; } = false;
        public static bool bBackupEnabled { get; set; } = false;
        public static bool AddTimeDataBackup { get; set; } = false;

        public static void ReadINI() { }

        public static string ReadKey(string key, string section, string defaultValue = "")
        {
            return defaultValue;
        }

        public static void WriteKey(string key, string section, string defaultValue) { }

        public static string ReadDirectoryKey(string key)
        {
            return string.Empty;
        }

        public static void WriteDirectoryKey(string key, string value) { }

        public static void UpdateRichPresence(string? details = null) { }
    }

    /// <summary>
    /// Stub for GameStorage - provides minimal implementation
    /// </summary>
    public sealed class GameStorage
    {
        private static readonly Lazy<GameStorage> _instance = new(() => new GameStorage());
        private Game? _selectedGame;

        public static GameStorage Instance => _instance.Value;

        public List<Game> Games { get; } = new();

        public void InitStorage()
        {
            Games.Clear();
            // Add default games for reference
            Games.Add(new Game("Mafia II", "Mafia II Classic", GamesEnumerator.MafiaII));
            Games.Add(new Game("Mafia II: Definitive Edition", "Mafia II DE", GamesEnumerator.MafiaII_DE));
            Games.Add(new Game("Mafia III", "Mafia III / Definitive Edition", GamesEnumerator.MafiaIII));
            Games.Add(new Game("Mafia I: Definitive Edition", "Mafia I DE", GamesEnumerator.MafiaI_DE));
        }

        public void SetSelectedGame(Game game)
        {
            _selectedGame = game;
        }

        public void SetSelectedGameByIndex(int index)
        {
            if (index >= 0 && index < Games.Count)
            {
                _selectedGame = Games[index];
            }
        }

        public void SetSelectedGameByType(GamesEnumerator gameType)
        {
            _selectedGame = Games.FirstOrDefault(g => g.GameType == gameType)
                ?? new Game("Unknown", "Unknown Game", gameType);
        }

        public Game? GetSelectedGame()
        {
            return _selectedGame;
        }

        public static bool IsGameType(GamesEnumerator gameType)
        {
            return Instance._selectedGame?.GameType == gameType;
        }

        public static string GetExecutableName(GamesEnumerator type)
        {
            return type switch
            {
                GamesEnumerator.MafiaII => "mafia2.exe",
                GamesEnumerator.MafiaII_DE => "mafia ii definitive edition.exe",
                GamesEnumerator.MafiaIII => "mafia3definitiveedition.exe",
                GamesEnumerator.MafiaI_DE => "mafiadefinitiveedition.exe",
                _ => string.Empty
            };
        }
    }
}

namespace Utils.Logging
{
    /// <summary>
    /// Stub for Log - outputs to stderr
    /// </summary>
    public static class Log
    {
        public static string LogPath { get; set; } = "log.txt";
        public static bool LoggingEnabled { get; set; } = false;
        public static bool ExtensiveLogging { get; set; } = false;

        public static void DeleteOldLog() { }
        public static void CreateFile(bool append = false) { }

        public static void WriteLine(string text, LoggingTypes logType = LoggingTypes.MESSAGE, LogCategoryTypes catType = LogCategoryTypes.FUNCTION, bool append = true)
        {
            // Only output warnings/errors in the MCP server
            if (logType == LoggingTypes.WARNING || logType == LoggingTypes.ERROR || logType == LoggingTypes.FATAL)
            {
                Console.Error.WriteLine($"[{logType}] {text}");
            }
        }
    }

    public enum LoggingTypes
    {
        WARNING,
        ERROR,
        MESSAGE,
        FATAL
    }

    public enum LogCategoryTypes
    {
        APPLICATION,
        FUNCTION,
        IO
    }

    /// <summary>
    /// Stub for ToolkitAssert - throws exceptions instead of showing message boxes
    /// </summary>
    public static class ToolkitAssert
    {
        public static void Ensure(bool condition, string messageFormat, params object[] messageArgs)
        {
            if (!condition)
            {
                var message = string.Format(messageFormat, messageArgs);
                Console.Error.WriteLine($"[Assert] {message}");
                // In the MCP server, we don't want to throw exceptions that halt operation
                // Just log the issue
            }
        }
    }
}

namespace Utils.Language
{
    /// <summary>
    /// Stub for Language - returns the key if no localization found
    /// </summary>
    public static class Language
    {
        private static readonly Dictionary<string, string> _localisation = new();

        public static void ReadLanguageXML() { }

        public static string GetString(string key)
        {
            return _localisation.TryGetValue(key, out var text) ? text : key;
        }
    }
}

namespace Mafia2Tool.Utils
{
    using OodleSharp;
    using System.Reflection;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Stub for OodleDllResolver - allows setting a custom Oodle DLL path
    /// </summary>
    public static class OodleDllResolver
    {
        private const string OodleDllName = "oo2core_8_win64.dll";
        private static string? _libraryFullPath;
        private static bool _isInitialized;

        public static bool TryResolveFrom(string libraryLocation)
        {
            if (_isInitialized)
            {
                return true;
            }

            _libraryFullPath = Path.Combine(libraryLocation, OodleDllName);

            if (!File.Exists(_libraryFullPath))
            {
                // For MCP server, we just return false - Oodle decompression won't work for M1:DE files
                Console.Error.WriteLine($"[OodleDllResolver] Oodle DLL not found at: {_libraryFullPath}");
                return false;
            }

            try
            {
                NativeLibrary.SetDllImportResolver(typeof(Oodle).Assembly, OodleResolver);
                _isInitialized = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[OodleDllResolver] Failed to set DLL resolver: {ex.Message}");
                return false;
            }
        }

        private static IntPtr OodleResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            var libHandle = IntPtr.Zero;

            if (libraryName == OodleDllName && _libraryFullPath != null)
            {
                NativeLibrary.TryLoad(_libraryFullPath, out libHandle);
            }

            return libHandle;
        }
    }
}

namespace Utils.Types
{
    /// <summary>
    /// Stub for any utility types
    /// </summary>
    public static class TypeExtensions
    {
        // Add any needed extension methods here
    }
}

namespace Gibbed.Mafia2.ResourceFormats
{
    // Stub namespace - ResourceEntry references this but doesn't use any types
}

namespace Gibbed.Mafia2.FileFormats
{
    /// <summary>
    /// Stub for ArchiveFile - provides the Signature constant used by ArchiveEncryption
    /// </summary>
    public partial class ArchiveFile
    {
        public const uint Signature = 0x53445300; // 'SDS\0'
    }
}
