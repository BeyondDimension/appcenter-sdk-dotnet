// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETFRAMEWORK
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using static Interop;

internal partial class Interop
{
    internal partial class Kernel32
    {
        public const int MAX_PATH = 260;
        public const int MAX_UNICODESTRING_LEN = short.MaxValue;
    }

    public static class Libraries
    {
        public const string Kernel32 = "kernel32.dll";
        public const string User32 = "user32.dll";
        public const string Gdi32 = "gdi32.dll";
    }

    // https://docs.microsoft.com/windows/win32/debug/system-error-codes--0-499-
    internal static class ERROR
    {
        public const int INSUFFICIENT_BUFFER = 0x007A;
    }

    internal static partial class User32
    {
        public enum SystemMetric
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
            SM_CXVSCROLL = 2,
            SM_CYHSCROLL = 3,
            SM_CYCAPTION = 4,
            SM_CXBORDER = 5,
            SM_CYBORDER = 6,
            SM_CXFIXEDFRAME = 7,
            SM_CYFIXEDFRAME = 8,
            SM_CYVTHUMB = 9,
            SM_CXHTHUMB = 10,
            SM_CXICON = 11,
            SM_CYICON = 12,
            SM_CXCURSOR = 13,
            SM_CYCURSOR = 14,
            SM_CYMENU = 15,
            SM_CYKANJIWINDOW = 18,
            SM_MOUSEPRESENT = 19,
            SM_CYVSCROLL = 20,
            SM_CXHSCROLL = 21,
            SM_DEBUG = 22,
            SM_SWAPBUTTON = 23,
            SM_CXMIN = 28,
            SM_CYMIN = 29,
            SM_CXSIZE = 30,
            SM_CYSIZE = 31,
            SM_CXFRAME = 32,
            SM_CYFRAME = 33,
            SM_CXMINTRACK = 34,
            SM_CYMINTRACK = 35,
            SM_CXDOUBLECLK = 36,
            SM_CYDOUBLECLK = 37,
            SM_CXICONSPACING = 38,
            SM_CYICONSPACING = 39,
            SM_MENUDROPALIGNMENT = 40,
            SM_PENWINDOWS = 41,
            SM_DBCSENABLED = 42,
            SM_CMOUSEBUTTONS = 43,
            SM_SECURE = 44,
            SM_CXEDGE = 45,
            SM_CYEDGE = 46,
            SM_CXMINSPACING = 47,
            SM_CYMINSPACING = 48,
            SM_CXSMICON = 49,
            SM_CYSMICON = 50,
            SM_CYSMCAPTION = 51,
            SM_CXSMSIZE = 52,
            SM_CYSMSIZE = 53,
            SM_CXMENUSIZE = 54,
            SM_CYMENUSIZE = 55,
            SM_ARRANGE = 56,
            SM_CXMINIMIZED = 57,
            SM_CYMINIMIZED = 58,
            SM_CXMAXTRACK = 59,
            SM_CYMAXTRACK = 60,
            SM_CXMAXIMIZED = 61,
            SM_CYMAXIMIZED = 62,
            SM_NETWORK = 63,
            SM_CLEANBOOT = 67,
            SM_CXDRAG = 68,
            SM_CYDRAG = 69,
            SM_SHOWSOUNDS = 70,
            SM_CXMENUCHECK = 71,
            SM_CYMENUCHECK = 72,
            SM_MIDEASTENABLED = 74,
            SM_MOUSEWHEELPRESENT = 75,
            SM_XVIRTUALSCREEN = 76,
            SM_YVIRTUALSCREEN = 77,
            SM_CXVIRTUALSCREEN = 78,
            SM_CYVIRTUALSCREEN = 79,
            SM_CMONITORS = 80,
            SM_SAMEDISPLAYFORMAT = 81,
            SM_CYFOCUSBORDER = 84,
            SM_CXFOCUSBORDER = 83,
            SM_REMOTESESSION = 0x1000,
            SM_CXSIZEFRAME = SM_CXFRAME,
            SM_CYSIZEFRAME = SM_CYFRAME
        }

        [DllImport(Libraries.User32, ExactSpelling = true)]
        public static extern int GetSystemMetrics(SystemMetric nIndex);

        [DllImport(Libraries.User32, ExactSpelling = true)]
        private static extern int GetSystemMetricsForDpi(SystemMetric nIndex, uint dpi);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public unsafe struct MONITORINFOEXW
        {
            public uint cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public MONITORINFOF dwFlags;
            public fixed char szDevice[32];
        }

        [Flags]
        public enum MONITORINFOF : uint
        {
            PRIMARY = 0x00000001,
        }

        [DllImport(Libraries.User32, ExactSpelling = true)]
        public static extern BOOL GetMonitorInfoW(IntPtr hMonitor, ref MONITORINFOEXW lpmi);

        public delegate BOOL MONITORENUMPROC(IntPtr monitor, Gdi32.HDC hdc, IntPtr lprcMonitor, IntPtr lParam);

        [DllImport(Libraries.User32, ExactSpelling = true)]
        public unsafe static extern BOOL EnumDisplayMonitors(IntPtr hdc, RECT* rcClip, MONITORENUMPROC lpfnEnum, IntPtr dwData);

        [DllImport(Libraries.User32, CharSet = CharSet.Unicode, ExactSpelling = true)]
        private unsafe static extern bool SystemParametersInfoW(SPI uiAction, uint uiParam, void* pvParam, uint fWinIni);

        public unsafe static bool SystemParametersInfoW<T>(SPI uiAction, ref T value) where T : unmanaged
        {
            fixed (void* p = &value)
            {
                return SystemParametersInfoW(uiAction, 0, p, 0);
            }
        }

        public enum SPI : uint
        {
            GETBORDER = 0x0005,
            GETKEYBOARDSPEED = 0x000A,
            ICONHORIZONTALSPACING = 0x000D,
            SETSCREENSAVEACTIVE = 0x0011,
            SETDESKWALLPAPER = 0x0014,
            GETKEYBOARDDELAY = 0x0016,
            SETKEYBOARDDELAY = 0x0017,
            ICONVERTICALSPACING = 0x0018,
            GETICONTITLEWRAP = 0x0019,
            GETMENUDROPALIGNMENT = 0x001B,
            SETMENUDROPALIGNMENT = 0x001C,
            SETDOUBLECLICKTIME = 0x0020,
            GETDRAGFULLWINDOWS = 0x0026,
            GETNONCLIENTMETRICS = 0x0029,
            GETICONMETRICS = 0x002D,
            GETWORKAREA = 0x0030,
            GETHIGHCONTRAST = 0x0042,
            SETHIGHCONTRAST = 0x0043,
            GETKEYBOARDPREF = 0x0044,
            GETANIMATION = 0x0048,
            GETFONTSMOOTHING = 0x004A,
            SETLOWPOWERACTIVE = 0x0055,
            GETDEFAULTINPUTLANG = 0x0059,
            GETSNAPTODEFBUTTON = 0x005F,
            GETMOUSEHOVERWIDTH = 0x0062,
            GETMOUSEHOVERHEIGHT = 0x0064,
            GETMOUSEHOVERTIME = 0x0066,
            GETWHEELSCROLLLINES = 0x0068,
            GETMENUSHOWDELAY = 0x006A,
            GETMOUSESPEED = 0x0070,
            GETACTIVEWINDOWTRACKING = 0x1000,
            GETMENUANIMATION = 0x1002,
            GETCOMBOBOXANIMATION = 0x1004,
            GETLISTBOXSMOOTHSCROLLING = 0x1006,
            GETGRADIENTCAPTIONS = 0x1008,
            GETKEYBOARDCUES = 0x100A,
            SETKEYBOARDCUES = 0x100B,
            GETHOTTRACKING = 0x100E,
            GETMENUFADE = 0x1012,
            GETSELECTIONFADE = 0x1014,
            GETTOOLTIPANIMATION = 0x1016,
            GETFLATMENU = 0x1022,
            GETDROPSHADOW = 0x1024,
            GETUIEFFECTS = 0x103E,
            GETACTIVEWNDTRKTIMEOUT = 0x2002,
            GETCARETWIDTH = 0x2006,
            GETFONTSMOOTHINGTYPE = 0x200A,
            GETFONTSMOOTHINGCONTRAST = 0x200C,
        }

        [DllImport(Libraries.User32, ExactSpelling = true)]
        public static extern IntPtr MonitorFromPoint(Point pt, MONITOR dwFlags);

        public enum MONITOR : uint
        {
            DEFAULTTONULL = 0x00000000,
            DEFAULTTOPRIMARY = 0x00000001,
            DEFAULTTONEAREST = 0x00000002
        }

        [DllImport(Libraries.User32, ExactSpelling = true)]
        public static extern IntPtr MonitorFromRect(ref RECT lprc, MONITOR dwFlags);

        [DllImport(Libraries.User32, ExactSpelling = true)]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, MONITOR dwFlags);
    }

    internal static partial class Gdi32
    {
        public readonly struct HDC
        {
            public IntPtr Handle { get; }

            public HDC(IntPtr handle) => Handle = handle;

            public bool IsNull => Handle == IntPtr.Zero;

            public static explicit operator IntPtr(HDC hdc) => hdc.Handle;
            public static explicit operator HDC(IntPtr hdc) => new(hdc);
            public static implicit operator HGDIOBJ(HDC hdc) => new(hdc.Handle);

            public static bool operator ==(HDC value1, HDC value2) => value1.Handle == value2.Handle;
            public static bool operator !=(HDC value1, HDC value2) => value1.Handle != value2.Handle;
            public override bool Equals(object? obj) => obj is HDC hdc && hdc.Handle == Handle;
            public override int GetHashCode() => Handle.GetHashCode();
        }

        public struct HGDIOBJ
        {
            public IntPtr Handle { get; }

            public HGDIOBJ(IntPtr handle) => Handle = handle;

            public bool IsNull => Handle == IntPtr.Zero;

            public static explicit operator IntPtr(HGDIOBJ hgdiobj) => hgdiobj.Handle;
            public static explicit operator HGDIOBJ(IntPtr hgdiobj) => new(hgdiobj);

            public static bool operator ==(HGDIOBJ value1, HGDIOBJ value2) => value1.Handle == value2.Handle;
            public static bool operator !=(HGDIOBJ value1, HGDIOBJ value2) => value1.Handle != value2.Handle;
            public override bool Equals(object? obj) => obj is HGDIOBJ hgdiobj && hgdiobj.Handle == Handle;
            public override int GetHashCode() => Handle.GetHashCode();
        }

        /// <remarks>
        ///  Use <see cref="DeleteDC(HDC)"/> when finished with the returned DC.
        ///  Calling with ("DISPLAY", null, null, IntPtr.Zero) will retrieve a DC for the entire desktop.
        /// </remarks>
        [DllImport(Libraries.Gdi32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HDC CreateDC(string lpszDriver, string? lpszDeviceName, string? lpszOutput, IntPtr devMode);

        public enum DeviceCapability : int
        {
            BITSPIXEL = 12,
            PLANES = 14,
            LOGPIXELSX = 88,
            LOGPIXELSY = 90
        }

        // https://github.com/dotnet/winforms/blob/v3.1.20/src/Common/src/Interop/Gdi32/Interop.GetDeviceCaps.cs
        // https://github.com/dotnet/winforms/blob/v6.0.0-rtm.21523.1/src/System.Windows.Forms.Primitives/src/Interop/Gdi32/Interop.GetDeviceCaps.cs
#if !NETCOREAPP3_0
        [SuppressGCTransition]
#endif
        [DllImport(Libraries.Gdi32, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(HDC hDC, DeviceCapability nIndex);

        [DllImport(Libraries.Gdi32, SetLastError = true, ExactSpelling = true)]
        public static extern BOOL DeleteDC(HDC hDC);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public RECT(Rectangle r)
        {
            left = r.Left;
            top = r.Top;
            right = r.Right;
            bottom = r.Bottom;
        }

        public static implicit operator Rectangle(RECT r)
            => Rectangle.FromLTRB(r.left, r.top, r.right, r.bottom);

        public static implicit operator RECT(Rectangle r)
            => new(r);

        public int X => left;

        public int Y => top;

        public int Width
            => right - left;

        public int Height
            => bottom - top;

        public Size Size
            => new(Width, Height);

        public override string ToString()
            => $"{{{left}, {top}, {right}, {bottom} (LTRB)}}";
    }

    /// <summary>
    ///  Blittable version of Windows BOOL type. It is convenient in situations where
    ///  manual marshalling is required, or to avoid overhead of regular bool marshalling.
    /// </summary>
    /// <remarks>
    ///  Some Windows APIs return arbitrary integer values although the return type is defined
    ///  as BOOL. It is best to never compare BOOL to TRUE. Always use bResult != BOOL.FALSE
    ///  or bResult == BOOL.FALSE .
    /// </remarks>
    internal enum BOOL : int
    {
        FALSE = 0,
        TRUE = 1,
    }
}

internal static class BoolExtensions
{
    public static bool IsTrue(this BOOL b) => b != BOOL.FALSE;
    public static bool IsFalse(this BOOL b) => b == BOOL.FALSE;
    public static BOOL ToBOOL(this bool b) => b ? BOOL.TRUE : BOOL.FALSE;
}
#endif