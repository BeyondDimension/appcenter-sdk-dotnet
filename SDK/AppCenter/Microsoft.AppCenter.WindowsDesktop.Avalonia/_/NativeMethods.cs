// https://github.com/dotnet/winforms/blob/v5.0.4/src/System.Windows.Forms.Primitives/src/System/Windows/Forms/Internals/NativeMethods.cs
#if !NETFRAMEWORK
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    internal static class NativeMethods
    {
        public static HandleRef NullHandleRef = new(null, IntPtr.Zero);
    }
}
#endif