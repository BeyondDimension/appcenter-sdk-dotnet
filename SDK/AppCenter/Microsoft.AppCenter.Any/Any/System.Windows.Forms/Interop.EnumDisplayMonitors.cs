﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/winforms/blob/v6.0.1/src/System.Windows.Forms.Primitives/src/Interop/User32/Interop.EnumDisplayMonitors.cs

#if !NETFRAMEWORK
using System;
using System.Runtime.InteropServices;

internal static partial class Interop
{
    internal static partial class User32
    {
        [DllImport(Libraries.User32, ExactSpelling = true)]
        public unsafe static extern BOOL EnumDisplayMonitors(IntPtr hdc, RECT* rcClip, MONITORENUMPROC lpfnEnum, IntPtr dwData);
    }
}
#endif