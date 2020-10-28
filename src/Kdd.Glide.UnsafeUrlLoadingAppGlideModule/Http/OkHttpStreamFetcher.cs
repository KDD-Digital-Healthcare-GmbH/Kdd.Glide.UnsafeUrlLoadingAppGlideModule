using Android.Runtime;
using Android.Util;
using Bumptech.Glide;
using Bumptech.Glide.Load;
using Bumptech.Glide.Load.Data;
using Bumptech.Glide.Load.Model;
using Bumptech.Glide.Util;
using Java.IO;
using Square.OkHttp3;
using System;
using IOException = System.IO.IOException;
using JavaClass = Java.Lang.Class;
using JavaIOException = Java.IO.IOException;

namespace Kdd.Glide.UnsafeUrlLoadingAppGlideModule.Http
{
    // NOTE Translated from github java code:
    // https://github.com/bumptech/glide/blob/master/integration/okhttp/src/main/java/com/bumptech/glide/integration/okhttp/OkHttpStreamFetcher.java
    public class OkHttpStreamFetcher : Java.Lang.Object, IDataFetcher
    {
        private readonly ICallFactory _client;
        private readonly GlideUrl _url;

        private InputStream _stream;
        private ResponseBody _responseBody;

        public OkHttpStreamFetcher(ICallFactory client, GlideUrl url)
        {
            _client = client;
            _url = url;
        }

        public JavaClass DataClass => JavaClass.FromType(typeof(InputStream));

        public DataSource DataSource => DataSource.Remote;

        public void LoadData(Priority priority, IDataFetcherDataCallback callback)
        {
            var requestBuilder = new Request.Builder().Url(_url.ToStringUrl());
            foreach (var header in _url.Headers)
            {
                requestBuilder.AddHeader(header.Key, header.Value);
            }

            var request = requestBuilder.Build();
            var responseCallback = new ResponseCallback(SetStream, SetResponseBody, callback);

            _client.NewCall(request).Enqueue(responseCallback);

            void SetStream(InputStream stream)
            {
                _stream = stream;
            }

            void SetResponseBody(ResponseBody responseBody)
            {
                _responseBody = responseBody;
            }
        }

        public void Cancel()
        {
            // NOTE Ignored
        }

        public void Cleanup()
        {
            try
            {
                _stream?.Close();
                _responseBody?.Close();
            }
            catch (IOException)
            {
                // NOTE Ignored
            }
            catch (JavaIOException)
            {
                // NOTE Ignored
            }
        }

        private class ResponseCallback : Java.Lang.Object, ICallback
        {
            private readonly Action<InputStream> _setStreamAction;
            private readonly Action<ResponseBody> _setResponseBodyAction;
            private readonly IDataFetcherDataCallback _callback;

            public ResponseCallback(Action<InputStream> setStreamAction,
                                    Action<ResponseBody> setResponseBodyAction,
                                    IDataFetcherDataCallback callback)
            {
                _setStreamAction = setStreamAction;
                _setResponseBodyAction = setResponseBodyAction;
                _callback = callback;
            }

            public void OnFailure(ICall call, JavaIOException exception)
            {
                Log.Warn(nameof(UnsafeUrlLoadingAppGlideModule), $"{nameof(OkHttpStreamFetcher)} failed to obtain result");
                _callback.OnLoadFailed(exception);
            }

            public void OnResponse(ICall call, Response response)
            {
                var responseBody = response.Body();
                _setResponseBodyAction.Invoke(responseBody);
                
                if (!response.IsSuccessful)
                {
                    var exception = new HttpException(response.ToString(), response.Code());
                    Log.Warn(nameof(UnsafeUrlLoadingAppGlideModule), $"{nameof(OkHttpStreamFetcher)} failed to receive image");
                    _callback.OnLoadFailed(exception);
                    return;
                }

                var contentLength = responseBody.ContentLength();
                var stream = ContentLengthInputStream.Obtain(responseBody.ByteStream(), contentLength);
                var inputStream = ((InputStreamInvoker)stream).BaseInputStream;
                _setStreamAction.Invoke(inputStream);
                _callback.OnDataReady(inputStream);
            }
        }
    }
}