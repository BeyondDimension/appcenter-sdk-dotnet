// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Drawing;
#if WINDOWS
using System.Management;
#endif
using System.Reflection;
using System.Runtime.InteropServices;
using static Microsoft.AppCenter.Utils.IAbstractDeviceInformationHelper;

namespace Microsoft.AppCenter.Utils;

/// <summary>
/// Implements the abstract device information helper class
/// </summary>
public class DeviceInformationHelper : AbstractDeviceInformationHelper
{
    private const string _defaultVersion = "Unknown";

    protected override string GetSdkName()
    {
        var sdkName = Instance?.GetSdkName();
        if (sdkName != null) return sdkName;
#if WINDOWS
        if (WindowsHelper.IsRunningAsWinUI)
        {
            return SdkNames.SdkName_WinUI;
        }
        return SdkNames.WPF_NET5_0_WINDOWS_OR_GREATER;
#else
        return SdkNames.WinForms_NET5_0_WINDOWS_OR_GREATER;
#endif
    }

    protected override string GetDeviceModel()
    {
        var deviceModel = Instance?.GetDeviceModel();
        if (deviceModel != null) return deviceModel;
#if WINDOWS
        try
        {
            var managementClass = new ManagementClass("Win32_ComputerSystem");
            foreach (var managementObject in managementClass.GetInstances())
            {
                var model = (string)managementObject["Model"];
                return string.IsNullOrEmpty(model) || DefaultSystemProductName == model ? null : model;
            }
        }
        catch (UnauthorizedAccessException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device model with error: ", exception);
            return string.Empty;
        }
        catch (COMException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device model. Make sure that WMI service is enabled.", exception);
            return string.Empty;
        }
        catch (ManagementException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device model. Make sure that WMI service is enabled.", exception);
            return string.Empty;
        }
        catch (PlatformNotSupportedException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device model. Make sure that .NET Framework is up to date.", exception);
            return string.Empty;
        }
#endif
        return string.Empty;
    }

    protected override string GetAppNamespace()
    {
        var appNamespace = Instance?.GetAppNamespace();
        if (appNamespace != null) return appNamespace;
        return Assembly.GetEntryAssembly()?.EntryPoint.DeclaringType?.Namespace;
    }

    protected override string GetDeviceOemName()
    {
        var deviceOemName = Instance?.GetDeviceOemName();
        if (deviceOemName != null) return deviceOemName;
#if WINDOWS
        try
        {
            var managementClass = new ManagementClass("Win32_ComputerSystem");
            foreach (var managementObject in managementClass.GetInstances())
            {
                var manufacturer = (string)managementObject["Manufacturer"];
                return string.IsNullOrEmpty(manufacturer) || DefaultSystemManufacturer == manufacturer ? null : manufacturer;
            }
        }
        catch (UnauthorizedAccessException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device OEM name with error: ", exception);
            return string.Empty;
        }
        catch (COMException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device OEM name. Make sure that WMI service is enabled.", exception);
            return string.Empty;
        }
        catch (ManagementException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device OEM name. Make sure that WMI service is enabled.", exception);
            return string.Empty;
        }
        catch (PlatformNotSupportedException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device OEM name. Make sure that .NET Framework is up to date.", exception);
            return string.Empty;
        }
#endif
        return string.Empty;
    }

    protected override string GetOsName()
    {
        var osName = Instance?.GetOsName();
        if (osName != null) return osName;
#if WINDOWS
        return "WINDOWS";
#else
        return "WINDOWS";
#endif
    }

    protected override string GetOsBuild()
    {
        var osBuild = Instance?.GetOsBuild();
        if (osBuild != null) return osBuild;
#if WINDOWS
        using (var hklmKey = Win32.Registry.LocalMachine)
        using (var subKey = hklmKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
        {
            // CurrentMajorVersionNumber present in registry starting with Windows 10
            var majorVersion = subKey.GetValue("CurrentMajorVersionNumber");
            if (majorVersion != null)
            {
                var minorVersion = subKey.GetValue("CurrentMinorVersionNumber", "0");
                var buildNumber = subKey.GetValue("CurrentBuildNumber", "0");
                var revisionNumber = subKey.GetValue("UBR", "0");
                return $"{majorVersion}.{minorVersion}.{buildNumber}.{revisionNumber}";
            }
            else
            {
                // If CurrentMajorVersionNumber not present in registry then use CurrentVersion
                var version = subKey.GetValue("CurrentVersion", "0.0");
                var buildNumber = subKey.GetValue("CurrentBuild", "0");
                var buildLabEx = subKey.GetValue("BuildLabEx")?.ToString().Split('.');
                var revisionNumber = buildLabEx?.Length >= 2 ? buildLabEx[1] : "0";
                return $"{version}.{buildNumber}.{revisionNumber}";
            }
        }
#else
        return Environment.OSVersion.Version.ToString();
#endif
    }

    protected override string GetOsVersion()
    {
        var osVersion = Instance?.GetOsVersion();
        if (osVersion != null) return osVersion;
#if WINDOWS
        try
        {
            var managementClass = new ManagementClass("Win32_OperatingSystem");
            foreach (var managementObject in managementClass.GetInstances())
            {
                return (string)managementObject["Version"];
            }
        }
        catch (UnauthorizedAccessException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device OS version with error: ", exception);
            return string.Empty;
        }
        catch (COMException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device OS version. Make sure that WMI service is enabled.", exception);
            return string.Empty;
        }
        catch (ManagementException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device OS version. Make sure that WMI service is enabled.", exception);
            return string.Empty;
        }
        catch (PlatformNotSupportedException exception)
        {
            AppCenterLog.Warn(AppCenterLog.LogTag, "Failed to get device OS version. Make sure that .NET Framework is up to date.", exception);
            return string.Empty;
        }
#endif
        return string.Empty;
    }

    protected override string GetAppVersion()
    {
        var appVersion = Instance?.GetAppVersion();
        if (appVersion != null) return appVersion;
        return DeploymentVersion ?? ProductVersion ?? _defaultVersion;
    }

    protected override string GetAppBuild()
    {
        var appBuild = Instance?.GetAppBuild();
        if (appBuild != null) return appBuild;
        return DeploymentVersion ?? AssemblyVersion?.FileVersion ?? _defaultVersion;
    }

    protected override string GetScreenSize()
    {
        var screenSize = Instance?.GetScreenSize();
        if (screenSize.HasValue) return $"{screenSize.Value.Width}x{screenSize.Value.Height}";
#if WINDOWS
        WindowsHelper.GetScreenSize(out int width, out int height);
        return $"{width}x{height}";
#else
        return "1920x1080";
#endif
    }

    private static string? ProductVersion
    {
        get
        {
            try
            {
                return WindowsHelper.GetWinFormsProductVersion();
            }
            catch
            {
                var assemblyVersion = AssemblyVersion;
                return assemblyVersion?.ProductVersion ?? assemblyVersion?.FileVersion;
            }
        }
    }

    private static string DeploymentVersion
    {
        get
        {
#if NETFRAMEWORK
            // Get ClickOnce version.
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }

#elif WINDOWS10_0_17763_0_OR_GREATER
            if (WindowsHelper.IsRunningAsUwp)
            {
                try
                {
                    var packageVersion = global::Windows.ApplicationModel.Package.Current.Id.Version;
                    return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}";
                }
                catch (InvalidOperationException exception)
                {
                    AppCenterLog.Warn(AppCenterLog.LogTag, "Package version is available only in MSIX-packaged applications. See link https://docs.microsoft.com/en-us/windows/apps/desktop/modernize/desktop-to-uwp-supported-api.", exception);
                }
            }
#endif
            return null;
        }
    }

    private static FileVersionInfo? AssemblyVersion
    {
        get
        {
            // The AssemblyFileVersion uniquely identifies a build.
            var entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
            {
                var assemblyLocation = entryAssembly.Location;
                if (string.IsNullOrWhiteSpace(assemblyLocation))
                {
                    // This is a fix for single file (self-contained publish) api incompatibility in runtime.
                    // Read at https://docs.microsoft.com/en-us/dotnet/core/deploying/single-file#api-incompatibility
                    assemblyLocation = Environment.GetCommandLineArgs()[0];
                }
                return FileVersionInfo.GetVersionInfo(assemblyLocation);
            }
            return null;
        }
    }
}
