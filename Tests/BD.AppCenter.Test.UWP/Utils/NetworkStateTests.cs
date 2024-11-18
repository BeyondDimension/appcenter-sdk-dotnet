// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using BD.AppCenter.Ingestion.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BD.AppCenter.Test.UWP.Utils
{
    [TestClass]
    public class NetworkStateTests
    {
        private NetworkStateAdapter _networkState;

        [TestInitialize]
        public void InitializeNetworkStateTest()
        {
            _networkState = new NetworkStateAdapter();
        }

        [TestMethod]
        public void NetWorkInterfaceThrowsExceptionCanBeHandled()
        {
            _networkState.IsNetworkAvailable = () => throw new Exception();
            Assert.IsFalse(_networkState.IsConnected);
        }
    }
}
