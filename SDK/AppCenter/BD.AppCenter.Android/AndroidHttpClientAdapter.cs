﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using BD.AppCenter.Android.Http;

namespace BD.AppCenter
{
    internal class AndroidHttpClientAdapter : Java.Lang.Object, IHttpClient
    {
        private readonly IHttpNetworkAdapter _httpNetworkAdapter;

        public AndroidHttpClientAdapter(IHttpNetworkAdapter httpNetworkAdapter)
        {
            _httpNetworkAdapter = httpNetworkAdapter;
        }

        public IServiceCall CallAsync(string uri, string method, IDictionary<string, string> headers, IHttpClientCallTemplate callTemplate, IServiceCallback serviceCallback)
        {
            callTemplate?.OnBeforeCalling(new Java.Net.URL(uri), headers);
            var jsonContent = callTemplate?.BuildRequestBody();
            var cancellationTokenSource = new CancellationTokenSource();
            _httpNetworkAdapter.SendAsync(uri, method, headers, jsonContent, cancellationTokenSource.Token).ContinueWith(t =>
            {
                var innerException = t.Exception?.InnerException;
                if (innerException is HttpException)
                {
                    var response = (innerException as HttpException).HttpResponse;
                    serviceCallback.OnCallFailed(new Android.Http.HttpException(new Android.Http.HttpResponse(response.StatusCode, response.Content)));
                }
                else if (innerException != null)
                {
                    serviceCallback.OnCallFailed(new Java.Lang.Exception(innerException.Message));
                }
                else
                {
                    var response = t.Result;
                    serviceCallback.OnCallSucceeded(new Android.Http.HttpResponse(response.StatusCode, response.Content));
                }
            });
            return new ServiceCall(cancellationTokenSource);
        }

        public void Close()
        {
        }

        public void Reopen()
        {
        }
    }

    internal class ServiceCall : Java.Lang.Object, IServiceCall
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        internal ServiceCall(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
