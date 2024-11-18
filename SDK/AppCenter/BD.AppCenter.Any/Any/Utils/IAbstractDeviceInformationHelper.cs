using System.Drawing;
using BD.AppCenter.Utils;

namespace BD.AppCenter.Utils
{
    public interface IAbstractDeviceInformationHelper
    {
        static class SdkNames
        {
            /// <summary>
            /// .NET Framework + Windows Presentation Foundation (WPF)
            /// </summary>
            public const string WPF = "appcenter.wpf";

            /// <summary>
            /// .NET Framework + Windows Forms
            /// </summary>
            public const string WinForms = "appcenter.winforms";

            /// <summary>
            /// .NET Core 3.1 + Windows Presentation Foundation (WPF)
            /// </summary>
            public const string WPF_NETCore = "appcenter.wpf.netcore";

            /// <summary>
            /// .NET Core 3.1 + Windows Forms
            /// </summary>
            public const string WinForms_NETCore = "appcenter.winforms.netcore";

            /// <summary>
            /// .NET 5+ WindowsDesktop + Windows Presentation Foundation (WPF)
            /// </summary>
            public const string WPF_NET5_0_WINDOWS_OR_GREATER = "appcenter.wpf.net";

            /// <summary>
            /// .NET 5+ WindowsDesktop + Windows Forms
            /// </summary>
            public const string WinForms_NET5_0_WINDOWS_OR_GREATER = "appcenter.winforms.net";

            /// <summary>
            /// IsRunningAsUwp + unreferenced Windows Forms
            /// </summary>
            public const string SdkName_WinUI = "appcenter.winui";
        }

        static IAbstractDeviceInformationHelper? Instance { get; internal set; }

        string? GetSdkName() => null;

        string? GetDeviceModel() => null;

        string? GetAppNamespace() => null;

        string? GetDeviceOemName() => null;

        string? GetOsName() => null;

        string? GetOsBuild() => null;

        string? GetOsVersion() => null;

        string? GetAppVersion() => null;

        string? GetAppBuild() => null;

        Size? GetScreenSize() => null;
    }
}

namespace BD.AppCenter
{
    partial class AppCenter
    {
        public static void SetDeviceInformationHelper(IAbstractDeviceInformationHelper value)
        {
            IAbstractDeviceInformationHelper.Instance = value;
        }
    }
}