### 2.0.0

**Features**

 - .NET Standard 1.0 support has been added to replace `Profile259` Portable Class Library
 - .NET Standard 1.4 support has been added to emulate Xamarin* and Mono* TFMs

**Breaking Changes**

 - Windows Phone 8 support has been deprecated, as MSBuild tooling doesn't support 64-bit compilation
 - specific builds for Xamarin and Mono tooling have been removed due to existing limitations of .NET command line tooling

