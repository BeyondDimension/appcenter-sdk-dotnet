﻿using System.Drawing;
using Microsoft.AppCenter.Utils;

namespace Microsoft.AppCenter.Utils
{
    public interface IPlatformHelper
    {
        static IPlatformHelper? Instance { get; internal set; }

        //bool IsRunningAsWpf => false;

        bool IsRunningAsUwp => false;

        bool IsRunningAsWinUI => false;

        bool? IsAnyWindowNotMinimized() => null;

#if WINDOWS
        IEnumerable<Rectangle?>? GetNotMinimizedWindowRestoreBounds() => null;
#endif

        string? ProductVersion => null;

        bool IsSupportedApplicationExit => false;

        event EventHandler ApplicationExit;

        bool IsSupportedUnhandledException => false;

        Action<object?, Exception> InvokeUnhandledExceptionOccurred { set; }
    }
}

namespace Microsoft.AppCenter
{
    partial class AppCenter
    {
        public static void SetPlatformHelper(IPlatformHelper value)
        {
            IPlatformHelper.Instance = value;
        }
    }
}