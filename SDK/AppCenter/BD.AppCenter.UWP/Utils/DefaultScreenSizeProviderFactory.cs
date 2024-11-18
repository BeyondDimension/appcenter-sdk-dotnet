// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BD.AppCenter.Utils
{
    public class DefaultScreenSizeProviderFactory : IScreenSizeProviderFactory
    {
        public IScreenSizeProvider CreateScreenSizeProvider()
        {
            return new DefaultScreenSizeProvider();
        }
    }
}
