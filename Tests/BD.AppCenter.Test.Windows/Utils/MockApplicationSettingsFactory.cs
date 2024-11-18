// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BD.AppCenter.Utils;
using Moq;

namespace BD.AppCenter.Test.Utils
{
    public class MockApplicationSettingsFactory : IApplicationSettingsFactory
    {
        private readonly Mock<IApplicationSettings> _applicationSettingsMock;

        public MockApplicationSettingsFactory(Mock<IApplicationSettings> applicationSettingsMock)
        {
            _applicationSettingsMock = applicationSettingsMock;
        }

        public IApplicationSettings CreateApplicationSettings()
        {
            return _applicationSettingsMock.Object;
        }
    }
}
