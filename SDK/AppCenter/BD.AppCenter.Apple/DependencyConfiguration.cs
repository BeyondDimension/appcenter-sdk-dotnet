// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BD.AppCenter.Apple.Bindings;

namespace BD.AppCenter
{
    static partial class DependencyConfiguration
    {
        private static IHttpNetworkAdapter _httpNetworkAdapter;
        private static IHttpNetworkAdapter PlatformHttpNetworkAdapter
        {
            get => _httpNetworkAdapter;
            set
            {
                MSACDependencyConfiguration.HttpClient = new AppleHttpClientAdapter(value);
                _httpNetworkAdapter = value;
            }
        }
    }
}
