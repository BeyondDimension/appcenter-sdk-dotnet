// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

global using WindowsHelper = BD.AppCenter.Utils.PlatformHelper;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
#if WINDOWS
using System.Windows.Forms;
#endif

namespace BD.AppCenter.Utils
{
#if WINDOWS
    public delegate void WinEventDelegate(IntPtr winEventHookHandle, uint eventType, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds);
#endif

    public static class PlatformHelper
    {
        //public static bool IsRunningAsWpf => IPlatformHelper.Instance?.IsRunningAsWpf ?? false;

        public static bool IsRunningAsUwp => IPlatformHelper.Instance?.IsRunningAsUwp ?? false;

        public static bool IsRunningAsWinUI => IPlatformHelper.Instance?.IsRunningAsWinUI ?? false;

        //public static dynamic WpfApplication { get; }

        #region IsRunningAsUwp

        //const long APPMODEL_ERROR_NO_PACKAGE = 15700L;

        //[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        //static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);

        //private static bool _IsRunningAsUwp()
        //{
        //    if (Environment.OSVersion.Version < new Version(6, 2))
        //    {
        //        return false;
        //    }

        //    int length = 0;
        //    var sb = new StringBuilder(0);
        //    GetCurrentPackageFullName(ref length, sb);

        //    sb = new StringBuilder(length);
        //    int result = GetCurrentPackageFullName(ref length, sb);

        //    return result != APPMODEL_ERROR_NO_PACKAGE;
        //}

        #endregion

        #region WinEventHook

#if WINDOWS
        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr eventHookAssemblyHandle, WinEventDelegate eventHookHandle, uint processId, uint threadId, uint dwFlags);
        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        private const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
        private const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        private static readonly IDictionary<WinEventDelegate, IntPtr> _hooks = new Dictionary<WinEventDelegate, IntPtr>();
        public static event WinEventDelegate OnMinimized
        {
            add
            {
                uint processId = (uint)Environment.ProcessId;
                var hook = SetWinEventHook(EVENT_SYSTEM_MINIMIZESTART, EVENT_SYSTEM_MINIMIZEEND, IntPtr.Zero, value, processId, 0, WINEVENT_OUTOFCONTEXT);
                _hooks.Add(value, hook);
            }
            remove
            {
                IntPtr hook;
                if (_hooks.TryGetValue(value, out hook))
                {
                    UnhookWinEvent(hook);
                }
            }
        }
#endif

        #endregion

        #region ScreenSize

#if WINDOWS
        /// <summary>
        /// Import GetDeviceCaps function to retreive scale-independent screen size.
        /// </summary>
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private const int DESKTOPVERTRES = 117;
        private const int DESKTOPHORZRES = 118;

        public static void GetScreenSize(out int width, out int height)
        {
            using var graphics = Graphics.FromHwnd(IntPtr.Zero);
            var desktop = graphics.GetHdc();
            width = GetDeviceCaps(desktop, DESKTOPHORZRES);
            height = GetDeviceCaps(desktop, DESKTOPVERTRES);
        }
#endif

        #endregion

        //static PlatformHelper()
        //{
        //    try
        //    {
        //        var presentationFramework = GetAssembly("PresentationFramework");
        //        IsRunningAsWpf = presentationFramework != null;
        //        if (IsRunningAsWpf)
        //        {
        //            var appType = presentationFramework.GetType("System.Windows.Application");
        //            WpfApplication = appType.GetRuntimeProperty("Current")?.GetValue(appType);

        //            var windowStateType = presentationFramework.GetType("System.Windows.WindowState");
        //            Minimized = (int)windowStateType.GetField("Minimized").GetRawConstantValue();
        //        }
        //    }
        //    catch (AppDomainUnloadedException)
        //    {
        //        AppCenterLog.Warn(AppCenterLog.LogTag, "Unabled to determine whether this application is WPF or Windows Forms; proceeding as though it is Windows Forms.");
        //    }
        //    IsRunningAsUwp = _IsRunningAsUwp();
        //    IsRunningAsWinUI = IsRunningAsUwp || GetAssembly("System.Windows.Forms") == null;
        //}

        //// Store the int corresponding to the "Minimized" state for WPF Windows
        //// This is equivalent to `System.Windows.WindowState.Minimized`
        //private static readonly int Minimized;

        //private static Assembly GetAssembly(string name)
        //{
        //    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        //    return assemblies.FirstOrDefault(assembly => assembly.GetName().Name == name);
        //}

        //private static Rectangle WindowsRectToRectangle(dynamic windowsRect)
        //{
        //    return new Rectangle
        //    {
        //        X = (int)windowsRect.X,
        //        Y = (int)windowsRect.Y,
        //        Width = (int)windowsRect.Width,
        //        Height = (int)windowsRect.Height
        //    };
        //}

        //private static bool WindowIntersectsWithAnyScreen(dynamic window)
        //{
        //    var windowBounds = WindowsRectToRectangle(window.RestoreBounds);
        //    return Screen.AllScreens.Any(screen => screen.Bounds.IntersectsWith(windowBounds));
        //}

        public static bool IsAnyWindowNotMinimized()
        {
            var isAnyWindowNotMinimized = IPlatformHelper.Instance?.IsAnyWindowNotMinimized();
            if (isAnyWindowNotMinimized.HasValue) return isAnyWindowNotMinimized.Value;

#if WINDOWS
            var notMinimizedWindowRestoreBounds = IPlatformHelper.Instance?.GetNotMinimizedWindowRestoreBounds();
            if (notMinimizedWindowRestoreBounds != null)
            {
                foreach (var windowBounds in notMinimizedWindowRestoreBounds)
                {
                    if (windowBounds.HasValue)
                    {
                        if (Screen.AllScreens.Any(screen => screen.Bounds.IntersectsWith(windowBounds.Value)))
                        {
                            return true;
                        }
                    }
                }
            }
#endif

            //// If not in WPF, query the available forms
            //if (WpfApplication == null)
            //{
            //    return Application.OpenForms.Cast<Form>().Any(form => form.WindowState != FormWindowState.Minimized);
            //}

            //// If in WPF, query the available windows
            //foreach (var window in WpfApplication.Windows)
            //{
            //    // Not minimized is true if WindowState is not "Minimized" and the window is on screen
            //    if ((int)window.WindowState != Minimized && WindowIntersectsWithAnyScreen(window))
            //    {
            //        return true;
            //    }
            //}
            return false;
        }

        public static string GetWinFormsProductVersion()
        {
            var productVersion = IPlatformHelper.Instance?.ProductVersion;
            if (productVersion != null) return productVersion;
#if WINDOWS
            /*
             * Application.ProductVersion returns the value from AssemblyInformationalVersion.
             * If the AssemblyInformationalVersion is not applied to an assembly,
             * the version number specified by the AssemblyFileVersion attribute is used instead.
             */
            return Application.ProductVersion;
#else
            return "1.0.0.0";
#endif
        }
    }
}
