// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Management;
using System.Threading.Tasks;
using BD.AppCenter.Utils;
using Xunit;

namespace BD.AppCenter.Test.WindowsDesktop.Utils
{
    public partial class DeviceInformationHelperTest
    {
        /// <summary>
        /// Verify sdk name in device information.
        /// </summary>
        [Fact]
        public void VerifySdkName()
        {
            var device = Task.Run(() => new DeviceInformationHelper().GetDeviceInformationAsync()).Result;
            Assert.Equal("appcenter.wpf.netcore", device.SdkName);
        }

        /// <summary>
        /// Verify that sdk is not crashed during getting device info when management class is not available.
        /// </summary>
        [Fact]
        public void VerifyDeviceInfoWhenManagmentClassNotAvailable()
        {
            var deviceInformation = new DeviceInformationHelper();
            var factory = new MockManagmentClassFactory();
            deviceInformation.SetManagmentClassFactory(factory);
            var device = Task.Run(() => deviceInformation.GetDeviceInformationAsync()).Result;
        }
    }

    /// <summary>
    /// Fake class for ManagmentClassFactory.
    /// </summary>
    class MockManagmentClassFactory : IManagmentClassFactory
    {
        public ManagementClass GetComputerSystemClass()
        {
            throw new UnauthorizedAccessException();
        }

        public ManagementClass GetOperatingSystemClass()
        {
            throw new UnauthorizedAccessException();
        }
    }
}
