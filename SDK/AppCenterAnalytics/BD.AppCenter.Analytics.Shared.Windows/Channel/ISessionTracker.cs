// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BD.AppCenter.Analytics.Channel
{
    public interface ISessionTracker
    {
        void Resume();
        void Pause();
        void Stop();
        void EnableManualSessionTracker();
        void StartSession();
    }
}
