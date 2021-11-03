// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// https://github.com/dotnet/winforms/blob/v6.0.1/src/System.Windows.Forms/src/System/Windows/Forms/Application.cs

#if !NETFRAMEWORK
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;

// ReSharper disable once CheckNamespace
namespace System.Windows.Forms
{
    static class Application
    {
        private static string s_executablePath;
        private static Type s_mainType;
        private static object s_appFileVersion;
        private static string s_productVersion;
        private static readonly object s_internalSyncObject = new();

        /// <summary>
        ///  Gets the product version associated with this application.
        /// </summary>
        public static string ProductVersion
        {
            get
            {
                lock (s_internalSyncObject)
                {
                    if (s_productVersion is null)
                    {
                        // Custom attribute
                        Assembly entryAssembly = Assembly.GetEntryAssembly();
                        if (entryAssembly is not null)
                        {
                            object[] attrs = entryAssembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
                            if (attrs is not null && attrs.Length > 0)
                            {
                                s_productVersion = ((AssemblyInformationalVersionAttribute)attrs[0]).InformationalVersion;
                            }
                        }

                        // Win32 version info
                        if (s_productVersion is null || s_productVersion.Length == 0)
                        {
                            s_productVersion = GetAppFileVersionInfo().ProductVersion;
                            if (s_productVersion is not null)
                            {
                                s_productVersion = s_productVersion.Trim();
                            }
                        }

                        // fake it
                        if (s_productVersion is null || s_productVersion.Length == 0)
                        {
                            s_productVersion = "1.0.0.0";
                        }
                    }
                }

                return s_productVersion;
            }
        }

        /// <summary>
        ///  Retrieves the Type that contains the "Main" method.
        /// </summary>
        private static Type GetAppMainType()
        {
            lock (s_internalSyncObject)
            {
                if (s_mainType is null)
                {
                    Assembly exe = Assembly.GetEntryAssembly();

                    // Get Main type...This doesn't work in MC++ because Main is a global function and not
                    // a class static method (it doesn't belong to a Type).
                    if (exe is not null)
                    {
                        s_mainType = exe.EntryPoint.ReflectedType;
                    }
                }
            }

            return s_mainType;
        }

        /// <summary>
        ///  Retrieves the FileVersionInfo associated with the main module for
        ///  the application.
        /// </summary>
#if NET5_0_OR_GREATER
        [UnconditionalSuppressMessage("SingleFile", "IL3002", Justification = "Single-file case is handled")]
#endif
        private static FileVersionInfo GetAppFileVersionInfo()
        {
            lock (s_internalSyncObject)
            {
                if (s_appFileVersion is null)
                {
                    Type t = GetAppMainType();
                    if (t is not null && t.Assembly.Location.Length > 0)
                    {
                        s_appFileVersion = FileVersionInfo.GetVersionInfo(t.Module.FullyQualifiedName);
                    }
                    else
                    {
                        s_appFileVersion = FileVersionInfo.GetVersionInfo(ExecutablePath);
                    }
                }
            }

            return (FileVersionInfo)s_appFileVersion;
        }

        /// <summary>
        ///  Gets the path for the executable file that started the application.
        /// </summary>
        public static string ExecutablePath
        {
            get
            {
                if (s_executablePath is null)
                {
                    StringBuilder sb = UnsafeNativeMethods.GetModuleFileNameLongPath(NativeMethods.NullHandleRef);
                    s_executablePath = Path.GetFullPath(sb.ToString());
                }

                return s_executablePath;
            }
        }
    }
}
#endif