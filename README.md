# Unsafe Url Loading AppGlideModule - module for loading images with untrusted (self-signed) certificates

This package gives the ability to load images from sources with untrusted (self-signed) certificates

Instruction:
1. Install 2 nuget packages on your Xamarin.Android project:
    - https://www.nuget.org/packages/Kdd.Glide.AppModuleInjector/
    - https://www.nuget.org/packages/Kdd.Glide.UnsafeUrlLoadingAppGlideModule/

2. Inject instance of `UnsafeUrlLoadingAppGlideModule` during application startup:
    `GlideAppModuleInjector.Inject(new UnsafeUrlLoadingAppGlideModule());`
  
3. Enjoy using it!
