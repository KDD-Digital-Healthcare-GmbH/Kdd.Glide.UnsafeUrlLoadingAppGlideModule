using Java.Security.Cert;
using Javax.Net.Ssl;
using System;
using JavaObject = Java.Lang.Object;

namespace Kdd.Glide.UnsafeUrlLoadingAppGlideModule.Http
{
    // NOTE Translated from github java code:
    // https://github.com/futurestudio/android-tutorials-picasso/blob/master/PicassoTutorial/app/src/main/java/io/futurestud/tutorials/picasso/okhttp/UnsafeOkHttpClient.java
    public class UnsafeX509TrustManager : JavaObject, IX509TrustManager
    {
        public void CheckClientTrusted(X509Certificate[] chain, string authType)
        {
        }

        public void CheckServerTrusted(X509Certificate[] chain, string authType)
        {
        }

        public X509Certificate[] GetAcceptedIssuers()
        {
            return Array.Empty<X509Certificate>();
        }
    }
}