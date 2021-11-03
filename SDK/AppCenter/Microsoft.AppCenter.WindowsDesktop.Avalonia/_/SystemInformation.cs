// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/winforms/blob/v6.0.0-rtm.21523.1/src/System.Windows.Forms/src/System/Windows/Forms/SystemInformation.cs
#if !NETFRAMEWORK
using System.Drawing;
using static Interop;
using static Interop.User32;
using SizeInt32 = System.Drawing.Size;

namespace System.Windows.Forms
{
    /// <summary>
    ///  Provides information about the operating system.
    /// </summary>
    internal static class SystemInformation
    {
        private static bool s_checkMultiMonitorSupport;
        private static bool s_multiMonitorSupport;

        private static bool MultiMonitorSupport
        {
            get
            {
                if (!s_checkMultiMonitorSupport)
                {
                    s_multiMonitorSupport = (GetSystemMetrics(SystemMetric.SM_CMONITORS) != 0);
                    s_checkMultiMonitorSupport = true;
                }

                return s_multiMonitorSupport;
            }
        }

        /// <summary>
        ///  Gets the bounds of the virtual screen.
        /// </summary>
        public static Rectangle VirtualScreen
        {
            get
            {
                if (MultiMonitorSupport)
                {
                    return new Rectangle(GetSystemMetrics(SystemMetric.SM_XVIRTUALSCREEN),
                                         GetSystemMetrics(SystemMetric.SM_YVIRTUALSCREEN),
                                         GetSystemMetrics(SystemMetric.SM_CXVIRTUALSCREEN),
                                         GetSystemMetrics(SystemMetric.SM_CYVIRTUALSCREEN));
                }

                var size = PrimaryMonitorSize;
                return new Rectangle(0, 0, size.Width, size.Height);
            }
        }

        /// <summary>
        ///  Gets the dimensions of the primary display monitor in pixels.
        /// </summary>
        public static SizeInt32 PrimaryMonitorSize
            => new SizeInt32(GetSystemMetrics(SystemMetric.SM_CXSCREEN),
                        GetSystemMetrics(SystemMetric.SM_CYSCREEN));

        /// <summary>
        ///  Gets the size of the working area in pixels.
        /// </summary>
        public static Rectangle WorkingArea
        {
            get
            {
                var rect = new RECT();
                User32.SystemParametersInfoW(User32.SPI.GETWORKAREA, ref rect);
                return rect;
            }
        }
    }
}
#endif