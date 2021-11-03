// https://github.com/dotnet/winforms/blob/v5.0.4/src/System.Windows.Forms.Primitives/src/System/Windows/Forms/Internals/UnsafeNativeMethods.cs
#if !NETFRAMEWORK
using System.Runtime.InteropServices;
using System.Text;
using static Interop;

namespace System.Windows.Forms
{
    internal static class UnsafeNativeMethods
    {
        [DllImport(Libraries.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
#pragma warning disable CA1838 // Avoid 'StringBuilder' parameters for P/Invokes
        public static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);
#pragma warning restore CA1838 // Avoid 'StringBuilder' parameters for P/Invokes

        public static StringBuilder GetModuleFileNameLongPath(HandleRef hModule)
        {
            StringBuilder buffer = new StringBuilder(Kernel32.MAX_PATH);
            int noOfTimes = 1;
            int length;
            // Iterating by allocating chunk of memory each time we find the length is not sufficient.
            // Performance should not be an issue for current MAX_PATH length due to this change.
            while (((length = GetModuleFileName(hModule, buffer, buffer.Capacity)) == buffer.Capacity)
                && Marshal.GetLastWin32Error() == ERROR.INSUFFICIENT_BUFFER
                && buffer.Capacity < Kernel32.MAX_UNICODESTRING_LEN)
            {
                noOfTimes += 2; // Increasing buffer size by 520 in each iteration.
                int capacity = noOfTimes * Kernel32.MAX_PATH < Kernel32.MAX_UNICODESTRING_LEN ? noOfTimes * Kernel32.MAX_PATH : Kernel32.MAX_UNICODESTRING_LEN;
                buffer.EnsureCapacity(capacity);
            }
            buffer.Length = length;
            return buffer;
        }
    }
}
#endif