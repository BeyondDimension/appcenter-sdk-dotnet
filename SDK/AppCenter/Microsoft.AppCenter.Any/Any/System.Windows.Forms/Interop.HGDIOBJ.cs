﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/winforms/blob/v6.0.1/src/System.Windows.Forms.Primitives/src/Interop/Gdi32/Interop.HGDIOBJ.cs

#if !NETFRAMEWORK
#nullable enable
using System;

internal static partial class Interop
{
    internal static partial class Gdi32
    {
        public struct HGDIOBJ
        {
            public IntPtr Handle { get; }

            public HGDIOBJ(IntPtr handle) => Handle = handle;

            public bool IsNull => Handle == IntPtr.Zero;

            public static explicit operator IntPtr(HGDIOBJ hgdiobj) => hgdiobj.Handle;
            public static explicit operator HGDIOBJ(IntPtr hgdiobj) => new HGDIOBJ(hgdiobj);

            public static bool operator ==(HGDIOBJ value1, HGDIOBJ value2) => value1.Handle == value2.Handle;
            public static bool operator !=(HGDIOBJ value1, HGDIOBJ value2) => value1.Handle != value2.Handle;
            public override bool Equals(object? obj) => obj is HGDIOBJ hgdiobj && hgdiobj.Handle == Handle;
            public override int GetHashCode() => Handle.GetHashCode();

            //public OBJ ObjectType => GetObjectType(this);
        }
    }
}
#nullable disable
#endif