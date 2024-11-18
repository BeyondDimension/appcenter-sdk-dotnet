// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using BD.AppCenter.Utils;
using Xunit;

namespace BD.AppCenter.Test.WindowsDesktop.Utils
{
    public partial class DeviceInformationHelperTest
    {
        /// <summary>
        /// Verify sdk name in device information
        /// </summary>
        [Fact]
        public void VerifySdkName()
        {
            var device = Task.Run(() => new DeviceInformationHelper().GetDeviceInformationAsync()).Result;
            Assert.Equal("appcenter.winforms", device.SdkName);
        }
    }
}