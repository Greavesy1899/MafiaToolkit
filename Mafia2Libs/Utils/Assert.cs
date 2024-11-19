using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Utils.Logging
{
    /**
     * A wrapper for asserts. They don't fire on release builds, so we need a solution to
     * still throw errors rather than silently failing and not alerting the user.
     */
    public static class ToolkitAssert
    {
        public static void Ensure(bool bCondition, string MessageFormat, params object[] MessageArgs)
        {
            // Trigger assert if debug build
            if(Debugger.IsAttached)
            {
                string FinalMessage = string.Format(MessageFormat, MessageArgs);
                Debug.Assert(bCondition, FinalMessage);
                return;
            }

            // Trigger message box if release build
            if(!bCondition)
            {
                string FinalMessage = string.Format(MessageFormat, MessageArgs);
                MessageBox.Show(FinalMessage, "Toolkit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
