// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace BD.AppCenter.Utils
{
    /// <summary>
    /// Various constants used by the SDK.
    /// </summary>
    public static partial class Constants
    {
        // Prefix for App Center application settings
        public const string KeyPrefix = "AppCenter";

        // Channel constants
        public const int DefaultTriggerCount = 50;
        public static readonly TimeSpan DefaultTriggerInterval = TimeSpan.FromSeconds(6);
        public const int DefaultTriggerMaxParallelRequests = 3;
    }
}
