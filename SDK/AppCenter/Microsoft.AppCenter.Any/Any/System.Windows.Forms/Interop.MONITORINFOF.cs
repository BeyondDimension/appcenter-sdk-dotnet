// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/winforms/blob/v6.0.1/src/System.Windows.Forms.Primitives/src/Interop/User32/Interop.MONITORINFOF.cs

#if !NETFRAMEWORK
using System;

internal static partial class Interop
{
    internal static partial class User32
    {
        [Flags]
        public enum MONITORINFOF : uint
        {
            PRIMARY = 0x00000001,
        }
    }
}
#endif