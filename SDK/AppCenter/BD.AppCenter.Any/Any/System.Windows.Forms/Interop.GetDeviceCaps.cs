// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/winforms/blob/v6.0.1/src/System.Windows.Forms.Primitives/src/Interop/Gdi32/Interop.GetDeviceCaps.cs

#if !NETFRAMEWORK
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class Gdi32
    {
        public enum DeviceCapability : int
        {
            BITSPIXEL = 12,
            PLANES = 14,
            LOGPIXELSX = 88,
            LOGPIXELSY = 90
        }

#if !NETCOREAPP3_0 // https://github.com/dotnet/winforms/blob/v3.1.20/src/Common/src/Interop/Gdi32/Interop.GetDeviceCaps.cs
        [SuppressGCTransition]
#endif
        [DllImport(Libraries.Gdi32, SetLastError = true, ExactSpelling = true)]
        public static extern int GetDeviceCaps(HDC hDC, DeviceCapability nIndex);
    }
}
#endif