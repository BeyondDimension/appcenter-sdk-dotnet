// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BD.AppCenter.Ingestion.Http;

namespace BD.AppCenter.Channel
{
    public interface IChannelGroupFactory
    {
        IChannelGroup CreateChannelGroup(string appSecret, INetworkStateAdapter networkState);
    }
}
