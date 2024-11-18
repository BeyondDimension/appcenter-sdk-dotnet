// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/winforms/blob/v6.0.1/src/System.Windows.Forms.Primitives/src/Interop/Kernel32/Interop.MAX_UNICODESTRING_LEN.cs

#if !NETFRAMEWORK
internal partial class Interop
{
    internal partial class Kernel32
    {
        public const int MAX_UNICODESTRING_LEN = short.MaxValue;
    }
}
#endif