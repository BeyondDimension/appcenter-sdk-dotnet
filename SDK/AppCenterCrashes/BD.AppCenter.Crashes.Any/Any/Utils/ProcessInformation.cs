// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BD.AppCenter.Crashes.Utils;

public sealed class ProcessInformation : IProcessInformation
{
    static readonly Lazy<(int pid, DateTime startTime, string name)?> _ProcessInfo = new(() =>
    {
        try
        {
            var proc = Process.GetCurrentProcess();
            return (proc.Id, proc.StartTime, proc.ProcessName);
        }
        catch (Exception e)
        {
            AppCenterLog.Warn(Crashes.LogTag, "Unable to get process info.", e);
            return null;
        }
    });

    public DateTime? ProcessStartTime
    {
        get
        {
            var procInfo = _ProcessInfo.Value;
            if (procInfo.HasValue)
            {
                return procInfo.Value.startTime;
            }
            return null;
        }
    }

    public int? ProcessId
    {
        get
        {
            try
            {
#if NET5_0_OR_GREATER
                return Environment.ProcessId;
#else
                var procInfo = _ProcessInfo.Value;
                if (procInfo.HasValue)
                {
                    return procInfo.Value.pid;
                }
                return null;
#endif
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
#if NET6_0_OR_GREATER
                var procPath = Environment.ProcessPath;
                if (!string.IsNullOrWhiteSpace(procPath))
                {
                    var procName = Path.GetFileNameWithoutExtension(procPath);
                    if (!string.IsNullOrWhiteSpace(procName))
                    {
                        return procName;
                    }
                }
#endif

                var procInfo = _ProcessInfo.Value;
                if (procInfo.HasValue)
                {
                    return procInfo.Value.name;
                }
                return null;
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
                    _ => arch.ToString().ToUpperInvariant(),
                };
            }
            return null;
        }
    }
}