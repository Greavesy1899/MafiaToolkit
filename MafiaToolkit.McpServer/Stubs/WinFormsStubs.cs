// Stub implementations for System.Windows.Forms types used by Gibbed.Mafia2.FileFormats
// These stubs allow the MCP server to compile without WinForms dependencies

namespace System.Windows.Forms
{
    /// <summary>
    /// Stub for MessageBox - logs to stderr instead of showing dialogs
    /// </summary>
    public static class MessageBox
    {
        public static DialogResult Show(string text)
        {
            Console.Error.WriteLine($"[MessageBox] {text}");
            return DialogResult.OK;
        }

        public static DialogResult Show(string text, string caption)
        {
            Console.Error.WriteLine($"[MessageBox:{caption}] {text}");
            return DialogResult.OK;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            Console.Error.WriteLine($"[MessageBox:{caption}] {text}");
            return DialogResult.OK;
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Console.Error.WriteLine($"[MessageBox:{caption}] {text}");
            return DialogResult.OK;
        }
    }

    public enum DialogResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7
    }

    public enum MessageBoxButtons
    {
        OK = 0,
        OKCancel = 1,
        AbortRetryIgnore = 2,
        YesNoCancel = 3,
        YesNo = 4,
        RetryCancel = 5
    }

    public enum MessageBoxIcon
    {
        None = 0,
        Error = 16,
        Hand = 16,
        Stop = 16,
        Question = 32,
        Exclamation = 48,
        Warning = 48,
        Asterisk = 64,
        Information = 64
    }

    /// <summary>
    /// Stub for Application - provides minimal implementation
    /// </summary>
    public static class Application
    {
        public static string StartupPath => AppContext.BaseDirectory;
        public static string ExecutablePath => Environment.ProcessPath ?? string.Empty;
    }
}

namespace System.Windows
{
    /// <summary>
    /// Stub for WPF MessageBox (used by some Utils code)
    /// </summary>
    public static class MessageBox
    {
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            Console.Error.WriteLine($"[MessageBox:{caption}] {messageBoxText}");
            return MessageBoxResult.OK;
        }

        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            Console.Error.WriteLine($"[MessageBox:{caption}] {messageBoxText}");
            return MessageBoxResult.OK;
        }
    }

    public enum MessageBoxButton
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 3,
        YesNo = 4
    }

    public enum MessageBoxResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Yes = 6,
        No = 7
    }

    public enum MessageBoxImage
    {
        None = 0,
        Error = 16,
        Hand = 16,
        Stop = 16,
        Question = 32,
        Exclamation = 48,
        Warning = 48,
        Asterisk = 64,
        Information = 64
    }
}
