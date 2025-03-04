// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#if WINDOWS
using System.Windows.Forms;
#endif

namespace Microsoft.AppCenter.Utils
{
    public class ApplicationLifecycleHelperDesktop : ApplicationLifecycleHelper
    {
#if WINDOWS
        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.
        private static WinEventDelegate _OnMinimizedDelegate = new WinEventDelegate(OnMinimized);
#endif
        private static void OnMinimized(IntPtr winEventHookHandle, uint eventType, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds)
        {
            // Filter out non-HWND
            if (objectId != 0 || childId != 0)
            {
                return;
            }

            if (WindowsHelper.IsAnyWindowNotMinimized())
            {
                InvokeResuming();
            }
            else
            {
                InvokeSuspended();
            }
        }

        static ApplicationLifecycleHelperDesktop()
        {
            // The change of the state of the flag in this place occurs at the start of the app
            // The `OnMinimized` method does not handle the first entry into the app,
            // so it must happen after initialization
            _suspended = false;

#if WINDOWS
            var platformHelper = IPlatformHelper.Instance;
            if (platformHelper != null && platformHelper.IsSupportedApplicationExit)
            {
                WindowsHelper.OnMinimized += _OnMinimizedDelegate;
                platformHelper.ApplicationExit += delegate { WindowsHelper.OnMinimized -= _OnMinimizedDelegate; };
            }
#endif
        }

        public ApplicationLifecycleHelperDesktop()
        {
            //if (WindowsHelper.IsRunningAsWpf)
            //{
            //    var eventInfo = WindowsHelper.WpfApplication.GetType().GetEvent("DispatcherUnhandledException");

            //    EventHandler<object> eventHandler = (sender, eventArgs) =>
            //    { 
            //        var exceptionProperty = eventArgs.GetType().GetProperty("Exception");
            //        var exception = (Exception)exceptionProperty.GetValue(eventArgs);
            //        InvokeUnhandledExceptionOccurred(sender, new UnhandledExceptionOccurredEventArgs(exception));
            //    };

            //    var runtimeDelegate = Delegate.CreateDelegate(eventInfo.EventHandlerType, eventHandler.Target, eventHandler.Method);
            //    eventInfo.AddEventHandler(WindowsHelper.WpfApplication, runtimeDelegate);
            //}
            //else 


            var platformHelper = IPlatformHelper.Instance;
            if (platformHelper != null && platformHelper.IsSupportedUnhandledException)
            {
                platformHelper.InvokeUnhandledExceptionOccurred = (sender, e) => InvokeUnhandledExceptionOccurred(sender!, new UnhandledExceptionOccurredEventArgs(e));
            }
            else
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    InvokeUnhandledExceptionOccurred(sender, new UnhandledExceptionOccurredEventArgs((Exception)eventArgs.ExceptionObject));
                };
            }
        }
    }
}
