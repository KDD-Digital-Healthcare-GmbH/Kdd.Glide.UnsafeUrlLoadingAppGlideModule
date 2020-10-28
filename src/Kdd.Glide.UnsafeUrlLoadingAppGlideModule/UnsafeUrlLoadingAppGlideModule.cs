using Android.Content;
using Bumptech.Glide;
using Bumptech.Glide.Load.Model;
using Bumptech.Glide.Module;
using Java.IO;
using Javax.Net.Ssl;
using Kdd.Glide.UnsafeUrlLoadingAppGlideModule.Http;
using Square.OkHttp3;
using AndroidGlide = Bumptech.Glide.Glide;
using JavaClass = Java.Lang.Class;

namespace Kdd.Glide.UnsafeUrlLoadingAppGlideModule
{
    public class UnsafeUrlLoadingAppGlideModule : AppGlideModule
    {
        private const string SslProtocol = "SSL";

        public override void RegisterComponents(Context context, AndroidGlide glide, Registry registry)
        {
            var glideUrlClass = JavaClass.FromType(typeof(GlideUrl));
            var inputStreamType = JavaClass.FromType(typeof(InputStream));

            registry.Replace(glideUrlClass, inputStreamType, new OkHttpUrlLoaderFactory(CreateUnsafeOkHttpClient()));
        }

        // NOTE Translated from github java code:
        // https://github.com/futurestudio/android-tutorials-picasso/blob/master/PicassoTutorial/app/src/main/java/io/futurestud/tutorials/picasso/okhttp/UnsafeOkHttpClient.java
        private OkHttpClient CreateUnsafeOkHttpClient()
        {
            // Create a trust manager that does not validate certificate chains
            var trustAllCertificates = new IX509TrustManager[]
            {
                new UnsafeX509TrustManager()
            };

            // Install the all-trusting trust manager
            var sslContext = SSLContext.GetInstance(SslProtocol);
            sslContext.Init(null, trustAllCertificates, new Java.Security.SecureRandom());

            // Create an ssl socket factory with our all-trusting manager
            var sslSocketFactory = sslContext.SocketFactory;
            var builder = new OkHttpClient.Builder();
            builder.SslSocketFactory(sslSocketFactory, trustAllCertificates[0]);
            builder.HostnameVerifier((_, __) => true);

            var client = builder.Build();
            return client;
        }
    }
}