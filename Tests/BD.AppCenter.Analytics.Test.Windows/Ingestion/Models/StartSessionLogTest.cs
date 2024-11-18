// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using BD.AppCenter.Analytics.Ingestion.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BD.AppCenter.Test.Windows.Ingestion.Models
{
    using Device = BD.AppCenter.Ingestion.Models.Device;

    [TestClass]
    public class StartSessionLogTest
    {
        private readonly DateTime? Timestamp = null;

        /// <summary>
        /// Verify that instance is constructed properly.
        /// </summary>
        [TestMethod]
        public void TestInstanceConstruction()
        {
            var mockDevice = new Mock<Device>();

            StartSessionLog emptyLog = new StartSessionLog();
            StartSessionLog log = new StartSessionLog(mockDevice.Object, Timestamp);

            Assert.IsNotNull(emptyLog);
            Assert.IsNotNull(log);
        }
    }
}
