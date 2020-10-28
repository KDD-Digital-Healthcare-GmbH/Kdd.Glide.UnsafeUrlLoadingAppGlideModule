using Android.Runtime;
using Bumptech.Glide.Load;
using Bumptech.Glide.Load.Model;
using Square.OkHttp3;
using JavaObject = Java.Lang.Object;

namespace Kdd.Glide.UnsafeUrlLoadingAppGlideModule.Http
{
    // NOTE Translated from github java code:
    // https://github.com/bumptech/glide/blob/master/integration/okhttp/src/main/java/com/bumptech/glide/integration/okhttp/OkHttpUrlLoader.java
    public class OkHttpUrlLoader : JavaObject, IModelLoader
    {
        private readonly ICallFactory _client;

        public OkHttpUrlLoader(ICallFactory client)
        {
            _client = client;
        }

        public bool Handles(JavaObject p0)
        {
            return true;
        }

        public ModelLoaderLoadData BuildLoadData(JavaObject p0, int p1, int p2, Options p3)
        {
            var glideUrl = p0.JavaCast<GlideUrl>();
            return new ModelLoaderLoadData(glideUrl, new OkHttpStreamFetcher(_client, glideUrl));
        }
    }
}