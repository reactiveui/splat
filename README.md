## Splat

![](http://f.cl.ly/items/1307401C3x2g3F2p2Z36/Logo.png)

Certain types of things are basically impossible to do in cross-platform
mobile code today, yet there's no reason why. Writing a ViewModel that handles
loading a gallery of pictures from disk will be completely riddled with
`#ifdefs` and basically unreadable.

Splat aims to fix that, by providing a usable leaky abstraction above platform
code. It is leaky, because it always provides an extension method `ToNative()`
and `FromNative()`, which converts the abstraction to the platform-specific
version. Load the image in the cross-platform code, then call `ToNative()` in
your view to actually display it.

### What does it do?

Splat currently supports:

* Cross-platform image loading/saving
* A port of System.Drawing.Color for portable libraries
* Cross-platform geometry primitives (PointF, SizeF, RectangleF), as well as a bunch of
  additional extension methods to make using them easier.
* A way to detect whether you're in a Unit Test runner / Design Mode
* A cross-platform logging framework
* Simple yet flexible Service Location

### How do I install?

[Always Be NuGetting](https://nuget.org/packages/Splat/). Package contains binaries for:

* Xamarin.iOS
* Xamarin.iOS 64-bit
* Xamarin.Android
* Xamarin.Mac
* Xamarin.Mac 64-bit
* WPF (.NET 4.5)
* Windows Forms
* Windows Phone 8
* Windows Phone 8.1 Universal
* WinRT

## Cross-platform Image Loading

```cs
//
// Load an Image
// This code even works in a Portable Library
//

var wc = new WebClient();
byte[] imageBytes = await wc.DownloadDataTaskAsync("http://octodex.github.com/images/Professortocat_v2.png");

// IBitmap is a type that provides basic image information such as dimensions
IBitmap profileImage = await BitmapLoader.Current.Load(imageBytes, null /* Use original width */, null /* Use original height */);
```

Then later, in your View:

```cs
// ToNative always converts an IBitmap into the type that the platform
// uses, such as UIBitmap on iOS or BitmapSource in WPF
ImageView.Source = ViewModel.ProfileImage.ToNative();
```

Images can also be loaded from a Resource. On Android, this can either be a
Resource ID casted to a string, or the name of the resource *as* as string
(optionally including the extension).

```cs
var profileImage = await BitmapLoader.Current.LoadFromResource("DefaultAvatar.png", null, null);
```

Bitmaps can also be created and saved - actually *drawing* on the image is
beyond the scope of this library, you should do this in your view-specific
code.

```cs
var blankImage = BitmapLoader.Current.Create(512.0f, 512.0f);
await blankImage.Save(CompressedBitmapFormat.Png, 0.0, File.Open("ItsBlank.png"));
```

## Service Location

Splat provides a simple service location implementation that is optimized for
Desktop and Mobile applications, while still remaining reasonably flexible. To
get a type:

```cs
var toaster = Locator.Current.GetService<IToaster>();
var allToasterImpls = Locator.Current.GetServices<IToaster>();
```

Locator.Current is a static variable that can be set on startup, to adapt Splat
to other DI/IoC frameworks. This is usually a bad idea.

The default implementation of Service Location also allows new types to be
registered at runtime.

```cs
// Create a new Toaster any time someone asks
Locator.CurrentMutable.Register(() => new Toaster(), typeof(IToaster));

// Register a singleton instance
Locator.CurrentMutable.RegisterConstant(new ExtraGoodToaster(), typeof(IToaster));

// Register a singleton which won't get created until the first user accesses it
Locator.CurrentMutable.RegisterLazySingleton(() => new LazyToaster(), typeof(IToaster));
```

## Logging

Splat provides a simple logging proxy for libraries and applications to set up.
By default, this logging isn't configured (i.e. it logs to the Null Logger). To
set up logging:

1. Register an implementation of `ILogger` using Service Location.
1. In the class in which you want to log stuff, "implement" the `IEnableLogger`
   interface (this is a tag interface, no implementation actually needed).
1. Call the `Log` method to write log entries:

```cs
this.Log().Warn("Something bad happened: {0}", errorMessage);
this.Log().ErrorException("Tried to do a thing and failed", exception);
```

For static methods, `LogHost.Default` can be used as the object to write a log
entry for. 

## Using Cross-Platform Colors and Geometry

```cs
// This System.Drawing class works, even on WinRT or WP8 where it's not supposed to exist
// Also, this works in a Portable Library, in your ViewModel
ProfileBackgroundAccentColor = new Color(255, 255, 255, 255);
```

Later, in the view, we can use it:

```
ImageView.Background = ViewModel.ProfileBackgroundAccentColor.ToNativeBrush();
```

## Detecting whether you're in a unit test runner

```cs
// If true, we are running unit tests
ModeDetector.InUnitTestRunner();  

// If true, we are running inside Blend, so don't do anything
ModeDetector.InDesignMode();
```
