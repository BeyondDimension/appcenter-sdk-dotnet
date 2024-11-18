// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BD.AppCenter.Channel;
using BD.AppCenter.Utils;

namespace BD.AppCenter.Analytics.Channel
{
    public interface ISessionTrackerFactory
    {
        ISessionTracker CreateSessionTracker(IChannelGroup channelGroup, IChannel channel, IApplicationSettings applicationSettings);
    }
}
