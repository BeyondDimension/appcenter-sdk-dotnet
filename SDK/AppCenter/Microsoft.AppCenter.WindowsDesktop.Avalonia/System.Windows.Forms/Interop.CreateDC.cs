// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/winforms/blob/v6.0.1/src/System.Windows.Forms.Primitives/src/Interop/Gdi32/Interop.CreateDC.cs

#if !NETFRAMEWORK
#nullable enable
using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Gdi32
    {
        /// <remarks>
        ///  Use <see cref="DeleteDC(HDC)"/> when finished with the returned DC.
        ///  Calling with ("DISPLAY", null, null, IntPtr.Zero) will retrieve a DC for the entire desktop.
        /// </remarks>
        [DllImport(Libraries.Gdi32, SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern HDC CreateDC(string lpszDriver, string? lpszDeviceName, string? lpszOutput, IntPtr devMode);
    }
}
#nullable disable
#endif