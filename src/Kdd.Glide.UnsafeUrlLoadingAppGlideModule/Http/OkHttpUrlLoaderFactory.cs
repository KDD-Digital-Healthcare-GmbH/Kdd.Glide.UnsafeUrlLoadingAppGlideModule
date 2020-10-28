using Bumptech.Glide.Load.Model;
using Square.OkHttp3;
using JavaObject = Java.Lang.Object;

namespace Kdd.Glide.UnsafeUrlLoadingAppGlideModule.Http
{
    // NOTE Translated from github java code:
    // https://github.com/bumptech/glide/blob/master/integration/okhttp/src/main/java/com/bumptech/glide/integration/okhttp/OkHttpUrlLoader.java
    public class OkHttpUrlLoaderFactory : JavaObject, IModelLoaderFactory
    {
        private static readonly object _lockObject = new object();
        private static volatile ICallFactory _internalClient;

        private readonly ICallFactory _client;

        public OkHttpUrlLoaderFactory(ICallFactory client)
        {
            _client = client;
        }

        public OkHttpUrlLoaderFactory() : this(GetInternalClient())
        {
        }

        private static ICallFactory GetInternalClient()
        {
            if (_internalClient is null)
            {
                lock (_lockObject)
                {
                    _internalClient ??= new OkHttpClient();
                }
            }

            return _internalClient;
        }

        public IModelLoader Build(MultiModelLoaderFactory p0)
        {
            return new OkHttpUrlLoader(_client);
        }

        public void Teardown()
        {
        }
    }
}