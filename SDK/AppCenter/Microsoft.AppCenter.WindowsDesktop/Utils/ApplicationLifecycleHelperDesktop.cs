// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
#if AVALONIA
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using AvaloniaApplication = Avalonia.Application;
#endif

namespace Microsoft.AppCenter.Utils
{
    public class ApplicationLifecycleHelperDesktop : ApplicationLifecycleHelper
    {

        #region WinEventHook

        private delegate void WinEventDelegate(IntPtr winEventHookHandle, uint eventType, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr eventHookAssemblyHandle, WinEventDelegate eventHookHandle, uint processId, uint threadId, uint dwFlags);
        [DllImport("user32.dll")]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        private const uint EVENT_SYSTEM_MINIMIZESTART = 0x0016;
        private const uint EVENT_SYSTEM_MINIMIZEEND = 0x0017;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.
        private static WinEventDelegate hookDelegate = new WinEventDelegate(WinEventHook);
        private static bool suspended = false;
        private static bool started = false;
        private static Action Minimize;
        private static Action Restore;
        private static Action Start;
#if AVALONIA
        private static readonly IClassicDesktopStyleApplicationLifetime ApplicationLifetime;
#else
        private static readonly dynamic WpfApplication;
        private static readonly int WpfMinimizedState;
#endif
        private static void WinEventHook(IntPtr winEventHookHandle, uint eventType, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds)
        {
            // Filter out non-HWND
            if (objectId != 0 || childId != 0)
            {
                return;
            }

            var anyNotMinimized = IsAnyWindowNotMinimized();

            if (!started && anyNotMinimized)
            {
                started = true;
                Start?.Invoke();
            }
            if (suspended && anyNotMinimized)
            {
                suspended = false;
                Restore?.Invoke();
            }
            else if (!suspended && !anyNotMinimized)
            {
                suspended = true;
                Minimize?.Invoke();
            }
        }

        static ApplicationLifecycleHelperDesktop()
        {
#if AVALONIA
            if (AvaloniaApplication.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime applicationLifetime)
            {
                ApplicationLifetime = applicationLifetime;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
#else
            // Retrieve the WPF APIs through reflection, if they are available
            if (WpfHelper.IsRunningOnWpf)
            {
                // Store the WPF Application singleton
                // This is equivalent to `WpfApplication = System.Windows.Application.Current;`
                var appType = WpfHelper.PresentationFramework.GetType("System.Windows.Application");
                WpfApplication = appType.GetRuntimeProperty("Current")?.GetValue(appType);

                // Store the int corresponding to the "Minimized" state for WPF Windows
                // This is equivalent to `WpfMinimizedState = (int)System.Windows.WindowState.Minimized;`
                WpfMinimizedState = (int)WpfHelper.PresentationFramework.GetType("System.Windows.WindowState")
                    .GetField("Minimized")
                    .GetRawConstantValue();
            }
#endif

            var hook = SetWinEventHook(EVENT_SYSTEM_MINIMIZESTART, EVENT_SYSTEM_MINIMIZEEND, IntPtr.Zero, hookDelegate, (uint)Process.GetCurrentProcess().Id, 0, WINEVENT_OUTOFCONTEXT);
#if AVALONIA
            applicationLifetime.Exit += delegate { UnhookWinEvent(hook); };
#else
            Application.ApplicationExit += delegate { UnhookWinEvent(hook); };
#endif
        }

        private static bool IsAnyWindowNotMinimized()
        {
#if !AVALONIA
            // If not in WPF, query the available forms
            if (WpfApplication == null)
            {
                return Application.OpenForms.Cast<Form>().Any(form => form.WindowState != FormWindowState.Minimized);
            }
#endif

            // If in WPF, query the available windows
            foreach (var window in
#if AVALONIA
                ApplicationLifetime.Windows
#else
                WpfApplication.Windows
#endif
                )
            {
                // Not minimized is true if WindowState is not "Minimized" and the window is on screen
#if AVALONIA
                if (window.WindowState != Avalonia.Controls.WindowState.Minimized && WindowIntersectsWithAnyScreen(window))
#else
                if ((int)window.WindowState != WpfMinimizedState && WindowIntersectsWithAnyScreen(window))
#endif
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        public bool HasShownWindow => started;

        public ApplicationLifecycleHelperDesktop()
        {
            Enabled = true;
            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
               base.InvokeUnhandledExceptionOccurred(sender, new UnhandledExceptionOccurredEventArgs((Exception)eventArgs.ExceptionObject));
            };
        }

        private void InvokeResuming()
        {
            base.InvokeResuming(null, EventArgs.Empty);
        }

        private void InvokeStarted()
        {
            base.InvokeStarted(null, EventArgs.Empty);
        }

        private void InvokeSuspended()
        {
            base.InvokeSuspended(null, EventArgs.Empty);
        }

        private bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (value == enabled)
                {
                    return;
                }
                if (value)
                {
                    Start = InvokeStarted;
                    Restore = InvokeResuming;
                    Minimize = InvokeSuspended;
                }
                else
                {
                    Start = null;
                    Restore = null;
                    Minimize = null;
                }
                enabled = value;
            }
        }

        private static Rectangle WindowsRectToRectangle(
#if AVALONIA
            Avalonia.Rect
#else
            dynamic
#endif
            windowsRect)
        {
            return new Rectangle
            {
                X = (int)windowsRect.X,
                Y = (int)windowsRect.Y,
                Width = (int)windowsRect.Width,
                Height = (int)windowsRect.Height
            };
        }

        private static bool WindowIntersectsWithAnyScreen(
#if AVALONIA
            Avalonia.Controls.Window
#else
            dynamic
#endif
            window)
        {
            var windowsRect =
#if AVALONIA
                window.Bounds;
#else
                window.RestoreBounds;
#endif
            var windowBounds = WindowsRectToRectangle(windowsRect);
            return Screen.AllScreens.Any(screen => screen.Bounds.IntersectsWith(windowBounds));
        }
    }
}
