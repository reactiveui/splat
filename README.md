## Splat

![](http://f.cl.ly/items/1307401C3x2g3F2p2Z36/Logo.png)

[![NuGet Stats](https://img.shields.io/nuget/v/splat.svg)](https://www.nuget.org/packages/splat) [![Build Status](https://dev.azure.com/dotnet/ReactiveUI/_apis/build/status/Splat-CI)](https://dev.azure.com/dotnet/ReactiveUI/_build/latest?definitionId=48) [![Code Coverage](https://codecov.io/gh/reactiveui/splat/branch/master/graph/badge.svg)](https://codecov.io/gh/reactiveui/splat)
<br>
<a href="https://www.nuget.org/packages/splat">
        <img src="https://img.shields.io/nuget/dt/splat.svg">
</a>
<a href="#backers">
        <img src="https://opencollective.com/reactiveui/backers/badge.svg">
</a>
<a href="#sponsors">
        <img src="https://opencollective.com/reactiveui/sponsors/badge.svg">
</a>
<a href="https://reactiveui.net/slack">
        <img src="https://img.shields.io/badge/chat-slack-blue.svg">
</a>

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

* WPF (.NET 4.5)
* Windows Forms
* UWP
* Xamarin (Android, iOS and Mac)
* .NET Standard 1.0 and 2.0

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
Desktop and Mobile applications, while still remaining reasonably flexible.

There are 2 parts to the locator design:

* **Locator.Current** The property to use to **retrieve** services. Locator.Current is a static variable that can be set on startup, to adapt Splat to other DI/IoC frameworks. We're currently working from v7 onward to make it easier to use your DI/IoC framework of choice. (see below)
* **Locator.CurrentMutable** The property to use to **register** services

To get a service:

```cs
// To get a single service registration
var toaster = Locator.Current.GetService<IToaster>();

// To get all service registrations
var allToasterImpls = Locator.Current.GetServices<IToaster>();
```

Locator.Current is a static variable that can be set on startup, to adapt Splat
to other DI/IoC frameworks. We're currently working from v7 onward to make it easier to use your DI/IoC framework of choice.

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

### Dependency Resolver Packages
For each of the provided dependency resolver adapters, there is a specific package that allows the service locator to be implemented by another ioc container.

**Note:** When using ReactiveUI and overriding Splat's default behavior, you have to be sure to [initialize ReactiveUI](https://reactiveui.net/docs/handbook/dependency-inversion/custom-dependency-inversion#set-the-locator.current-to-your-implementation) before your container finalizes.

Please note: If you are adjusting behaviours of Splat by working with your custom container directly. Please read the relevant projects documentation on
REPLACING the registration. If the container supports appending\ multiple registrations you may get undesired behaviours, such as the wrong logger factory
being used.

| Container | NuGet | Read Me
|---------|-------|-------|
| [Splat.Autofac][SplatAutofacNuGet] | [![SplatAutofacBadge]][SplatAutofacNuGet] | [Setup Autofac][SplatAutofacReadme]
| [Splat.DryIoc][SplatDryIocNuGet] | [![SplatDryIocBadge]][SplatDryIocNuGet] | [Setup DryIoc][SplatDryIocReadme]
| [Splat.Microsoft.Extensions.DependencyInjection][SplatMicrosoftNuGet] | [![SplatMicrosoftBadge]][SplatMicrosoftNuGet] | [Setup Microsoft DI][SplatMicrosoftReadme]
| [Splat.Ninject][SplatNinjectNuGet] | [![SplatNinjectBadge]][SplatNinjectNuGet] | [Setup Ninject][SplatNinjectReadme]
| [Splat.SimpleInjector][SplatSimpleInjectorNuGet] | [![SplatSimpleInjectorBadge]][SplatSimpleInjectorNuGet] | |

[SplatAutofacNuGet]: https://www.nuget.org/packages/Splat.Autofac/
[SplatAutofacBadge]: https://img.shields.io/nuget/v/Splat.Autofac.svg
[SplatAutofacReadme]: ./src/Splat.Autofac/README.md
[SplatDryIocNuGet]: https://www.nuget.org/packages/Splat.DryIoc/
[SplatDryIocBadge]: https://img.shields.io/nuget/v/Splat.DryIoc.svg
[SplatDryIocReadme]: ./src/Splat.DryIoc/README.md
[SplatMicrosoftNuGet]: https://www.nuget.org/packages/Splat.Microsoft.Extensions.DependencyInjection/
[SplatMicrosoftBadge]: https://img.shields.io/nuget/v/Splat.Microsoft.Extensions.DependencyInjection.svg
[SplatMicrosoftReadme]: ./src/Splat.Microsoft.Extensions.DependencyInjection/README.md
[SplatNinjectNuGet]: https://www.nuget.org/packages/Splat.Ninject/
[SplatNinjectBadge]: https://img.shields.io/nuget/v/Splat.Ninject.svg
[SplatNinjectReadme]: ./src/Splat.Ninject/README.md
[SplatSimpleInjectorNuGet]: https://www.nuget.org/packages/Splat.SimpleInjector/
[SplatSimpleInjectorBadge]: https://img.shields.io/nuget/v/Splat.SimpleInjector.svg

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

### Available logging adapters

Splat has support for the following logging frameworks

| Target | Package | NuGet |
|---------|-------|------|
| Console | [Splat][SplatNuGet] | [![SplatBadge]][SplatNuGet] |
| Debug | [Splat][SplatNuGet] | [![SplatBadge]][SplatNuGet] |
| Log4Net | [Splat.Log4Net][SplatLog4NetNuGet] | [![SplatLog4NetBadge]][SplatLog4NetNuGet]  |
| Microsoft Extensions Logging | [Splat.Microsoft.Extensions.Logging][SplatMicrosoftExtensionsLoggingNuGet] | [![SplatMicrosoftExtensionsLoggingBadge]][SplatMicrosoftExtensionsLoggingNuGet] |
| NLog | [Splat.NLog][SplatNLogNuGet] | [![SplatNLogBadge]][SplatNLogNuGet] |
| Serilog | [Splat.Serilog][SplatSerilogNuGet] | [![SplatSerilogBadge]][SplatSerilogNuGet] |

[SplatNuGet]: https://www.nuget.org/packages/Splat/
[SplatBadge]: https://img.shields.io/nuget/v/Splat.svg
[SplatLog4NetNuGet]: https://www.nuget.org/packages/Splat.Log4Net/
[SplatLog4NetBadge]: https://img.shields.io/nuget/v/Splat.Log4Net.svg
[SplatMicrosoftExtensionsLoggingNuGet]: https://www.nuget.org/packages/Splat.Microsoft.Extensions.Logging/
[SplatMicrosoftExtensionsLoggingBadge]: https://img.shields.io/nuget/v/Splat.Microsoft.Extensions.Logging.svg
[SplatNLogNuGet]: https://www.nuget.org/packages/Splat.NLog/
[SplatNLogBadge]: https://img.shields.io/nuget/v/Splat.NLog.svg
[SplatSerilogNuGet]: https://www.nuget.org/packages/Splat.Serilog/
[SplatSerilogBadge]: https://img.shields.io/nuget/v/Splat.Serilog.svg

### Log4Net

First configure Log4Net. For guidance see https://logging.apache.org/log4net/release/manual/configuration.html

```cs
using Splat.Log4Net;

// then in your service locator initialisation
Locator.CurrentMutable.UseLog4NetWithWrappingFullLogger();
```

Thanks to @dpvreony for first creating this logger.

### Microsoft.Extensions.Logging

First configure Microsoft.Extensions.Logging. For guidance see https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/

```cs
using Splat.Microsoft.Extensions.Logging;

// note: this is different from the other adapter extension methods
//       as it needs knowledge of the logger factory
//       also the "container" is how you configured the Microsoft.Logging.Extensions
var loggerFactory = container.Resolve<ILoggerFactory>();
// in theory it could also be
// var loggerFactory = new LoggerFactory();


/// then in your service locator initialisation
Locator.CurrentMutable.UseMicrosoftExtensionsLoggingWithWrappingFullLogger(loggerFactory);
```

Thanks to @dpvreony for first creating this logger.

### NLog

First configure NLog. For guidance see https://github.com/nlog/nlog/wiki/Tutorial and https://github.com/nlog/nlog/wiki/Configuration-file

```cs
using Splat.NLog;

//  then in your service locator initialisation
Locator.CurrentMutable.UseNLogWithWrappingFullLogger();
```

Thanks to @dpvreony for first creating this logger.

### Serilog

First configure Serilog. For guidance see https://github.com/serilog/serilog/wiki/Configuration-Basics

```cs
using Splat.Serilog;

// Then in your service locator initialisation
Locator.CurrentMutable.UseSerilogFullLogger()
```

Thanks to @joelweiss for first creating this logger.

## Using Cross-Platform Colors and Geometry

```cs
// This System.Drawing class works, even on WinRT or WP8 where it's not supposed to exist
// Also, this works in a Portable Library, in your ViewModel
ProfileBackgroundAccentColor = Color.FromArgb(255, 255, 255, 255);
```

Later, in the view, we can use it:

```
ImageView.Background = ViewModel.ProfileBackgroundAccentColor.ToNativeBrush();
```

If targeting iOS or Mac in a cross-platform solution (e.g. iOS & Android), use
the SplatColor class to define colors in your netstandard library
(since Cocoa doesn't include System.Drawing.Color).

```cs
// In a netstandard library
SplatColor BackgroundColor = SplatColor.Red;
```

```
// From an iOS project
UIColor bgColor = ViewModel.BackgroundColor.ToNative();
```

```
// From an Android project
Android.Graphics.Color bgColor = ViewModel.BackgroundColor.ToNative();
```

## Detecting whether you're in a unit test runner

```cs
// If true, we are running unit tests
ModeDetector.InUnitTestRunner();  

// If true, we are running inside Blend, so don't do anything
ModeDetector.InDesignMode();
```

## Contribute

Splat is developed under an OSI-approved open source license, making it freely usable and distributable, even for commercial use. Because of our Open Collective model for funding and transparency, we are able to funnel support and funds through to our contributors and community. We ❤ the people who are involved in this project, and we’d love to have you on board, especially if you are just getting started or have never contributed to open-source before.

So here's to you, lovely person who wants to join us — this is how you can support us:

* [Responding to questions on StackOverflow](https://stackoverflow.com/questions/tagged/splat)
* [Passing on knowledge and teaching the next generation of developers](http://ericsink.com/entries/dont_use_rxui.html)
* [Donations](https://reactiveui.net/donate) and [Corporate Sponsorships](https://reactiveui.net/sponsorship)
* [Asking your employer to reciprocate and contribute to open-source](https://github.com/github/balanced-employee-ip-agreement)
* Submitting documentation updates where you see fit or lacking.
* Making contributions to the code base.
