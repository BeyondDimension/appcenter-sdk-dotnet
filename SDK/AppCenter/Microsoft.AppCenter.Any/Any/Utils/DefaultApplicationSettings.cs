// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.AppCenter.Utils
{
    sealed class DefaultApplicationSettings : IApplicationSettings
    {
        public DefaultApplicationSettings()
        {
            throw new NotImplementedException("You must call AppCenter.SetApplicationSettingsFactory use custom implementation.");
        }

        bool IApplicationSettings.ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        T IApplicationSettings.GetValue<T>(string key, T defaultValue)
        {
            throw new NotImplementedException();
        }

        void IApplicationSettings.Remove(string key)
        {
            throw new NotImplementedException();
        }

        void IApplicationSettings.SetValue(string key, object value)
        {
            throw new NotImplementedException();
        }
    }
}
