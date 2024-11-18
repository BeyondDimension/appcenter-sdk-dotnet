// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BD.AppCenter.Analytics.Channel;
using BD.AppCenter.Channel;
using BD.AppCenter.Utils;
using Moq;

namespace BD.AppCenter.Analytics.Test.Windows
{
    public class SessionTrackerFactory : ISessionTrackerFactory
    {
        public Mock<ISessionTracker> ReturningSessionTrackerMock = new Mock<ISessionTracker>();

        public ISessionTracker CreateSessionTracker(IChannelGroup channelGroup, IChannel channel, IApplicationSettings applicationSettings)
        {
            return ReturningSessionTrackerMock.Object;
        }
    }
}
