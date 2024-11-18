// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BD.AppCenter.Crashes.Utils;

public sealed class ProcessInformation : IProcessInformation
{
    public DateTime? ProcessStartTime
    {
        get
        {
            try
            {
                return Process.GetCurrentProcess().StartTime;
            }
            catch (Exception e)
            {
                AppCenterLog.Warn(Crashes.LogTag, "Unable to get process start time.", e);
                return null;
            }
        }
    }

    public int? ProcessId
    {
        get
        {
            try
            {
                return Environment.ProcessId;
            }
            catch (Exception e)
            {
                AppCenterLog.Warn(Crashes.LogTag, "Unable to get process ID.", e);
                return null;
            }
        }
    }

    public string? ProcessName
    {
        get
        {
            try
            {
                return Process.GetCurrentProcess().ProcessName;
            }
            catch (Exception e)
            {
                AppCenterLog.Warn(Crashes.LogTag, "Unable to get process name.", e);
                return null;
            }
        }
    }

    // Parent process information is not (easily) available, and not necessary.
    public int? ParentProcessId => null;

    // Parent process information is not (easily) available, and not necessary.
    public string? ParentProcessName => null;

    public string? ProcessArchitecture
    {
        get
        {
            if (OperatingSystem.IsWindows())
            {
                try
                {
                    return Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
                }
                catch (Exception e)
                {
                    AppCenterLog.Warn(Crashes.LogTag, "Unable to get process architecture.", e);
                }
            }
            else
            {
                var arch = RuntimeInformation.ProcessArchitecture;
                return arch switch
                {
                    Architecture.X86 => "x86",
                    Architecture.X64 => "AMD64",
                    Architecture.Arm => "ARM",
                    Architecture.Arm64 => "ARM64",
                    _ => null,
                };
            }
            return null;
        }
    }
}