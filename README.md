[![NuGet Stats](https://img.shields.io/nuget/v/splat.svg)](https://www.nuget.org/packages/splat) ![Build](https://github.com/reactiveui/splat/workflows/Build/badge.svg) [![Code Coverage](https://codecov.io/gh/reactiveui/splat/branch/main/graph/badge.svg)](https://codecov.io/gh/reactiveui/splat)
<br>
<a href="https://www.nuget.org/packages/splat">
        <img src="https://img.shields.io/nuget/dt/splat.svg">
</a>
<a href="https://reactiveui.net/slack">
        <img src="https://img.shields.io/badge/chat-slack-blue.svg">
</a>

<img src="https://github.com/reactiveui/styleguide/blob/master/logo_splat/logo.png?raw=true" width="200" />

# Splat

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

### Core Team

<table>
  <tbody>
    <tr>
      <td align="center" valign="top">
        <img width="100" height="100" src="https://github.com/glennawatson.png?s=150">
        <br>
        <a href="https://github.com/glennawatson">Glenn Watson</a>
        <p>Melbourne, Australia</p>
      </td>
      <td align="center" valign="top">
        <img width="100" height="100" src="https://github.com/rlittlesii.png?s=150">
        <br>
        <a href="https://github.com/rlittlesii">Rodney Littles II</a>
        <p>Texas, USA</p>
      </td>
    </tr>
    <tr>
      <td align="center" valign="top">
        <img width="100" height="100" src="https://github.com/dpvreony.png?s=150">
        <br>
        <a href="https://github.com/dpvreony">David Vreony</a>
        <p>UK</p>
      </td>
      <td align="center" valign="top">
        <img width="100" height="100" src="https://github.com/chrispulman.png?s=150">
        <br>
        <a href="https://github.com/chrispulman">Chris Pulman</a>
        <p>UK</p>
      </td>
    </tr>
  </tbody>
</table>

### How do I install?

[Always Be NuGetting](https://nuget.org/packages/Splat/). Package contains binaries for:

* .NET Framework 4.6.2, .NET Framework 4.7.2, .NET Standard 2.0, .NET 6.0, and .NET 8.0
- Works with:
  * WPF
  * Windows Forms
  * WinUI 3
  * Maui (WinUI, Android, iOS and Mac)
  * Avalonia

## Detecting whether you're in a unit test runner

```cs
// If true, we are running unit tests
ModeDetector.InUnitTestRunner();  
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

### Dependency Injection Source Generator
There is a source generator that will inject constructor and properties. See [here](https://github.com/reactivemarbles/Splat.DI.SourceGenerator) for instructions.

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
| [Splat.SimpleInjector][SplatSimpleInjectorNuGet] | [![SplatSimpleInjectorBadge]][SplatSimpleInjectorNuGet] | [Setup Simple Injector][SplatSimpleInjectorReadme] |

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
[SplatSimpleInjectorReadme]: ./src/Splat.SimpleInjector/README.md

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
entry for. The Static logger uses a different interface from the main logger to allow capture of additional
caller context as it doesn't have the details of the class instance etc. when compared to the normal logger.
To get the benefit of these you don't need to do much as they are optional parameters at the end of the methods
that are utilised by the compiler\framework. Currently we only capture [CallerMemberName](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute).

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

## Cross platform drawing

| Target | Package | NuGet |
|---------|-------|------|
| Splat.Drawing | [Splat.Drawing][SplatDrawingNuGet] | [![SplatDrawingBadge]][SplatDrawingNuGet] |

[SplatDrawingNuGet]: https://www.nuget.org/packages/Splat.Drawing/
[SplatDrawingBadge]: https://img.shields.io/nuget/v/Splat.Drawing.svg

### Using Cross-Platform Colors and Geometry


```cs
// This System.Drawing class works, even on WinRT where it's not supposed to exist
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
### Cross-platform Image Loading

You can register with the Splat locators.

```cs
Locator.CurrentMutable.RegisterPlatformBitmapLoader();
```

You can then load your images in a cross platform way:

```cs
//
// Load an Image
// This code even works in a Portable Library
//

var wc = new WebClient();
Stream imageStream = wc.OpenRead("http://octodex.github.com/images/Professortocat_v2.png");

// IBitmap is a type that provides basic image information such as dimensions
IBitmap profileImage = await BitmapLoader.Current.Load(imageStream, null /* Use original width */, null /* Use original height */);
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
### Detecting if you're in design mode

```cs
// If true, we are running inside Blend, so don't do anything
PlatformModeDetector.InDesignMode();
```

### Application Performance Monitoring

Application Performance Monitoring is split into the follow sections

* Error Reporting
* Feature Usage Tracking
* View Tracking

The table below shows the support across various APM packages

| Product | Package | NuGet | Maturity Level | Error Reporting | Feature Usage Tracking | View Tracking |
|----|----|----|----|----|----|----|
| Appcenter | [Splat.AppCenter][SplatAppcenterNuGet] | [![SplatAppcenterBadge]][SplatAppcenterNuGet] | Alpha | TODO | Native | Native |
| Application Insights | [Splat.ApplicationInsights][SplatApplicationInsightsNuGet] | [![SplatApplicationInsightsBadge]][SplatApplicationInsightsNuGet] | Alpha | TODO | Native | Native |
| Exceptionless | [Splat.Exceptionless][SplatExceptionlessNuGet] | [![SplatExceptionlessBadge]][SplatExceptionlessNuGet] | Alpha | TODO | Native | By Convention |
| New Relic | N\A | N\A | Not Started | TODO | TODO | TODO
| OpenTrace | N\A | N\A | Not Started |TODO | TODO | TODO
| Raygun | [Splat.Raygun][SplatRaygunNuGet] | [![SplatRaygunBadge]][SplatRaygunNuGet] | Prototype | TODO | By Convention | By Convention |

[SplatAppcenterNuGet]: https://www.nuget.org/packages/Splat.Appcenter/
[SplatAppcenterBadge]: https://img.shields.io/nuget/v/Splat.Appcenter.svg
[SplatApplicationInsightsNuGet]: https://www.nuget.org/packages/Splat.ApplicationInsights/
[SplatApplicationInsightsBadge]: https://img.shields.io/nuget/v/Splat.ApplicationInsights.svg
[SplatExceptionlessNuGet]: https://www.nuget.org/packages/Splat.Exceptionless/
[SplatExceptionlessBadge]: https://img.shields.io/nuget/v/Splat.Exceptionless.svg
[SplatRaygunNuGet]: https://www.nuget.org/packages/Splat.Raygun/
[SplatRaygunBadge]: https://img.shields.io/nuget/v/Splat.Raygun.svg

#### Goals of the Splat APM feature

* To sit on top of existing APM libaries using native features where possible, or by using a common convention that gives parity in behaviour.
** Where there is a convention behaviour it will be detailed under the relevant frameworks documentation.
* To define basic behaviours that are dropped into consuming libraries, for example with ReactiveUI
** Commands
** ViewModels
** Views

#### Getting started with APM with Splat

Splat comes with a default implementation that pushes events into your active Splat logging framework. This allows for design and testing prior to hooking up a full APM offering.

#### Error Reporting

TODO

#### Feature Usage Tracking

The most basic ability for feature usage tracking is to implement the Splat.ApplicationPerformanceMonitoring.IEnableFeatureUsageTracking interface. This has the same behaviour as the logging interface and allows Splat to inject whichever
APM platform is registered with the ServiceLocator at initialization.

```cs
        /// <summary>
        /// Dummy object for testing IEnableFeatureUsageTracking.
        /// </summary>
        public sealed class TestObjectThatSupportsFeatureUsageTracking : IEnableFeatureUsageTracking
        {
			public async Task SomeFeatureIWantToTrack()
			{
                using (var trackingSession = this.FeatureUsageTrackingSession("featureName"))
                {
					try
					{
						// do some work here.
					}
					catch (Exception exception)
					{
						trackingSession.OnException(exception);
					}
                }
			}
        }
```

Splat also has the notion of subfeatures, some APM platforms support this natively, others have been done by convention, which will be explained in the relevant library.
Splat itself does not dictate when these should be used. It's up to you. You may have a primary feature (such as a search view) and then track buttons, etc. on that view
as subfeatures.

```cs
        /// <summary>
        /// Dummy object for testing IEnableFeatureUsageTracking.
        /// </summary>
        public sealed class TestObjectThatSupportsFeatureUsageTracking : IEnableFeatureUsageTracking
        {
			public async Task SomeFeatureIWantToTrack()
			{
                using (var mainFeature = this.FeatureUsageTrackingSession("featureName"))
                {
					try
					{
						await DoSubFeature(mainFeature).ConfigureAwait(false);
					}
					catch (Exception exception)
					{
						mainFeature.OnException(exception);
					}
                }
			}

			public async Task SomeFeatureIWantToTrack(IFeatureUsageTrackingSession parentFeature)
			{
                using (var subFeature = parentFeature.SubFeature("subFeatureName"))
                {
					try
					{
						// do some work here.
					}
					catch (Exception exception)
					{
						subFeature.OnException(exception);
					}
                }
			}
        }
```

#### View Tracking

TODO

#### Configuring Appcenter

First configure Appcenter. For guidance see https://docs.microsoft.com/en-us/appcenter/diagnostics/enabling-diagnostics

```cs
using Splat.AppCenter;

// then in your service locator initialisation
Locator.CurrentMutable.UseAppcenterApm();
```

#### Configuring Application Insights

First configure Application Insights. For guidance see https://docs.microsoft.com/en-us/azure/azure-monitor/app/worker-service

```cs
using Splat.ApplicationInsights;

// then in your service locator initialisation
Locator.CurrentMutable.UseApplicationInsightsApm();
```

#### Configuring Exceptionless

First configure Exceptionless. For guidance see https://github.com/exceptionless/Exceptionless/wiki/Getting-Started

```cs
using Splat.Exceptionless;

// then in your service locator initialisation
Locator.CurrentMutable.UseExceptionlessApm();
```

#### Configuring New Relic

New Relic support isn't currently available.

#### Configuring OpenTrace

OpenTrace support isn't currently available.

#### Configuring Raygun

First configure Raygun. For guidance see TODO

```cs
using Splat.Raygun;

// then in your service locator initialisation
Locator.CurrentMutable.UseRaygunApm();
```

#### Testing and developing the APM functionality

The unit tests for this functionality do not generate activity to the relevant platform.
The integration tests DO SEND TEST DATA to the relevant platforms, so they need to have
the user-secrets configured. There is a script in the \scripts\inttestusersecrets.cmd
that shows how to set the relevant secrets up.

## Contribute

Splat is developed under an OSI-approved open source license, making it freely usable and distributable, even for commercial use. We ❤ the people who are involved in this project, and we’d love to have you on board, especially if you are just getting started or have never contributed to open-source before.

So here's to you, lovely person who wants to join us — this is how you can support us:

* [Responding to questions on StackOverflow](https://stackoverflow.com/questions/tagged/splat)
* [Passing on knowledge and teaching the next generation of developers](http://ericsink.com/entries/dont_use_rxui.html)
* Submitting documentation updates where you see fit or lacking.
* Making contributions to the code base.
