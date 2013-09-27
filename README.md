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

### Cross-platform Image Loading

```cs
//
// Load an Image
// This code even works in a Portable Library
//

var wc = new WebClient();
var imageBytes = await wc.DownloadDataTaskAsync("http://octodex.github.com/images/Professortocat_v2.png");
ProfileImage = await BitmapLoader.Current.Load(imageBytes, null /* Use original width */, null /* Use original height */);

```

Then later, in your View:

```
ImageView.Source = ViewModel.ProfileImage.ToNative();
```

### Using Cross-Platform Colors and Geometry

```cs
// This System.Drawing class works, even on WinRT or WP8 where it's not supposed to exist
// Also, this works in a Portable Library, in your ViewModel
ProfileBackgroundAccentColor = new Color(255, 255, 255, 255);
```

Later, in the view, we can use it:

```
ImageView.Background = ViewModel.ProfileBackgroundAccentColor.ToNativeBrush();
```

### Detecting whether you're in a unit test runner

```cs
// If true, we are running unit tests
ModeDetector.InUnitTestRunner();  

// If true, we are running inside Blend, so don't do anything
ModeDetector.InDesignMode();
```

### How do I install?

[Always Be NuGetting](https://nuget.org/packages/Splat/). Package contains binaries for:

* Xamarin.iOS
* Xamarin.Android
* Xamarin.Mac
* WPF (.NET 4.5)
* Windows Phone 8
* WinRT
