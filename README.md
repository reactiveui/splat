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

## Table of Contents

- [What does it do?](#what-does-it-do)
- [How do I install?](#how-do-i-install)
- [Detecting whether you're in a unit test runner](#detecting-whether-youre-in-a-unit-test-runner)
- [Service Location](#service-location)
  - [The Default Dependency Resolver (v19+)](#the-default-dependency-resolver-v19)
  - [AppBuilder and IModule](#appbuilder-and-imodule-aot-friendly-configuration)
- [Logging](#logging)
- [Cross platform drawing](#cross-platform-drawing)
- [Cross-platform Image Loading](#cross-platform-image-loading)
- [Detecting if you're in design mode](#detecting-if-youre-in-design-mode)
- [Application Performance Monitoring](#application-performance-monitoring)
- [Dependency Resolver Performance Benchmarks](#dependency-resolver-performance-benchmarks)
- [Contribute](#contribute)

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

* **AppLocator.Current** The property to use to **retrieve** services. AppLocator.Current is a static variable that can be set on startup, to adapt Splat to other DI/IoC frameworks. We're currently working from v7 onward to make it easier to use your DI/IoC framework of choice. (see below)
* **AppLocator.CurrentMutable** The property to use to **register** services

To get a service:

```cs
// To get a single service registration
var toaster = AppLocator.Current.GetService<IToaster>();

// To get all service registrations
var allToasterImpls = AppLocator.Current.GetServices<IToaster>();
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

### The Default Dependency Resolver (v19+)

Starting with v19, Splat provides two high-performance resolver implementations optimized for AOT compilation: **GlobalGenericFirstDependencyResolver** and **InstanceGenericFirstDependencyResolver**. Both deliver significantly better performance than the legacy ModernDependencyResolver while supporting different isolation requirements.

#### Quick Start for New Users

The service locator is simple to use - register services at startup, then resolve them when needed:

```cs
// Register services at application startup
Locator.CurrentMutable.Register<IToaster>(() => new Toaster());
Locator.CurrentMutable.RegisterConstant<IConfiguration>(myConfig);
Locator.CurrentMutable.RegisterLazySingleton<ILogger>(() => new FileLogger());

// Resolve services anywhere in your application
var toaster = Locator.Current.GetService<IToaster>();
var config = Locator.Current.GetService<IConfiguration>();
```

**Key Concepts:**

- **Locator.CurrentMutable** - Use this to **register** services during initialization
- **Locator.Current** - Use this to **retrieve** services during runtime
- **Contracts** - Optional named registrations when you need multiple implementations: `Register<IToaster>(() => new FastToaster(), "Fast")`
- **Lazy Singletons** - Services created once on first access: `RegisterLazySingleton<T>`
- **Constants** - Pre-created singleton instances: `RegisterConstant<T>`

#### Choosing Between Global and Instance Resolvers

Splat provides two resolver implementations with identical APIs but different isolation characteristics:

| Feature | GlobalGenericFirstDependencyResolver | InstanceGenericFirstDependencyResolver |
|---------|--------------------------------------|----------------------------------------|
| **Container Storage** | Process-wide static containers | Per-resolver instance containers (ConditionalWeakTable) |
| **Isolation** | Shared across all resolver instances | Isolated per resolver instance |
| **Performance** | Fastest - direct static access | Very fast - one additional CWT lookup |
| **Memory** | Minimal - static fields only | Low - weak references, GC-friendly |
| **Use Case** | Single global service locator | Multiple independent resolvers, testing scenarios |
| **Thread Safety** | Lock-free reads, thread-safe writes | Lock-free reads, thread-safe writes |
| **AOT Compatible** | Yes | Yes |

**When to use GlobalGenericFirstDependencyResolver:**
- Single application-wide service locator (most common scenario)
- Maximum performance requirements
- Simple dependency injection needs
- Traditional singleton pattern usage

**When to use InstanceGenericFirstDependencyResolver:**
- Multiple independent DI containers in same process
- Unit testing with isolated container instances
- Plugin systems with separate service graphs
- Multi-tenant applications with isolated service scopes

**Example: Creating Instance Resolvers**

```cs
// Global resolver (default) - all instances share registrations
var resolver1 = new GlobalGenericFirstDependencyResolver();
var resolver2 = new GlobalGenericFirstDependencyResolver();
resolver1.Register<IService>(() => new ServiceA());
// resolver2 can also see ServiceA registration!

// Instance resolver - each has independent registrations
var resolver1 = new InstanceGenericFirstDependencyResolver();
var resolver2 = new InstanceGenericFirstDependencyResolver();
resolver1.Register<IService>(() => new ServiceA());
resolver2.Register<IService>(() => new ServiceB());
// Completely isolated - resolver1 only sees ServiceA, resolver2 only sees ServiceB
```

#### Container Architecture: How It Works

Both resolvers use a **generic-first, two-tier architecture** designed for maximum performance:

##### 1. Static Generic Containers (Fast Path)

When you call generic methods like `GetService<IToaster>()`, the resolver uses static generic containers:

```cs
// Each type T gets its own static container - no dictionary lookups needed!
internal static class Container<T>
{
    private static readonly Entry<Registration<T>> Entries = new();
    // ... resolution logic
}
```

##### New Container Performance

**Performance characteristics:**
- **O(1) constant-time** service resolution - no hash calculations, no dictionary lookups
- **Lock-free reads** using versioned snapshots with volatile memory semantics
- **O(1) registration** - snapshots are rebuilt lazily on first read, not on every registration
- **Zero boxing/unboxing** for value types
- **AOT-friendly** - no reflection required for generic registrations


##### 2. Type Registry (Compatibility Fallback)

When you call Type-based methods like `GetService(typeof(IToaster))`, the resolver uses a concurrent dictionary:

```cs
// Fallback for Type-based calls and interop with legacy code
internal static class ServiceTypeRegistry
{
    private static readonly ConcurrentDictionary<(Type, string?), Entry<Func<object?>>> Entries;
    // ... resolution logic with per-entry locking
}
```

**Why the fallback exists:**
- Compatibility with libraries that use Type-based APIs
- Interop with other DI containers (Autofac, Microsoft.Extensions.DependencyInjection, etc.)
- Support for dynamic scenarios where types aren't known at compile-time

##### 3. Contract Support (Named Services)

Contracts allow multiple registrations for the same type:

```cs
// Register different implementations with different contracts
Locator.CurrentMutable.Register<IToaster>(() => new FastToaster(), "Fast");
Locator.CurrentMutable.Register<IToaster>(() => new SlowToaster(), "Slow");

// Retrieve specific implementation
var fastToaster = Locator.Current.GetService<IToaster>("Fast");
```

Contracts use a separate `ContractContainer<T>` for each type, providing the same lock-free performance as the non-contract path.

#### Thread Safety and Concurrency

GenericFirstDependencyResolver is designed for high-concurrency scenarios:

**Lock-free reads:**
- All `GetService` calls use lock-free fast paths
- Snapshots are published with `Volatile.Write` for proper memory ordering
- Multiple threads can resolve services simultaneously with zero contention

**Fine-grained locking for writes:**
- Registrations use per-type or per-entry locks (not global locks)
- Lock contention only affects threads registering the *same* type simultaneously
- Most apps register at startup and read during runtime - optimal for this pattern

**Clear semantics:**
- `Clear()` operations are designed as "stop-the-world" teardown operations
- Threads with existing references may continue using old snapshots until next lookup
- This is standard behavior for DI containers during test cleanup or app shutdown

#### Performance: Generic vs Type-Based Methods

**Always prefer generic methods** when the type is known at compile-time:

```cs
// FAST: Generic method - no dictionary lookup, no boxing
var toaster = Locator.Current.GetService<IToaster>();

// SLOW: Type-based method - dictionary lookup + potential boxing
var toaster = (IToaster)Locator.Current.GetService(typeof(IToaster));
```

**Benchmark results** (500 registrations):
- Generic registration: ~2-3ms for 500 services (constant time)
- Type-based registration: Previously O(n²), now O(n) but still slower than generic
- Generic resolution: ~50-100ns per call (lock-free, no allocations)
- Type-based resolution: ~200-500ns per call (dictionary lookup)

#### Registration Patterns

**Transient (new instance every time):**
```cs
Locator.CurrentMutable.Register<IToaster>(() => new Toaster());
```

**Singleton (shared instance):**
```cs
// Created immediately
Locator.CurrentMutable.RegisterConstant<IToaster>(new Toaster());

// Created on first access (lazy)
Locator.CurrentMutable.RegisterLazySingleton<IToaster>(() => new Toaster());
```

**Multiple implementations:**
```cs
// Get all registered implementations
Locator.CurrentMutable.Register<IPlugin>(() => new PluginA());
Locator.CurrentMutable.Register<IPlugin>(() => new PluginB());

var allPlugins = Locator.Current.GetServices<IPlugin>(); // Returns both
```

**Constructor-based registration (requires parameterless constructor):**
```cs
// Registers a factory that calls new TImplementation()
Locator.CurrentMutable.Register<IToaster, Toaster>();
```

#### Important: Null Service Type Edge Case

Due to overload resolution, there's one edge case when registering with a null service type (rare in practice):

```cs
// AMBIGUOUS: Calls generic method Register<int> with contract: null
resolver.Register(() => 5, null);

// CORRECT: Use named parameters to call non-generic method with serviceType: null
resolver.Register(() => 5, serviceType: null);
```

In normal usage, this ambiguity doesn't occur:

```cs
// Unambiguous - calls generic method
resolver.Register<IToaster>(() => new Toaster());

// Unambiguous - calls Type-based method
resolver.Register(() => new Toaster(), typeof(IToaster));
```

#### Why GenericFirstDependencyResolver Replaced ModernDependencyResolver

**ModernDependencyResolver** (v1-v18) had several performance and correctness issues:

1. **O(n²) Registration Growth**
   - Every registration rebuilt the entire snapshot array
   - 500 registrations required 124,750 array copies
   - Benchmarks showed exponential time growth and wouldn't complete overnight

2. **Global Lock Contention**
   - Single `ReaderWriterLockSlim` for all operations
   - All readers blocked when any thread was registering
   - Poor scaling on multi-core systems

3. **Reflection-Heavy, Poor AOT Support**
   - Heavy use of `Type`-based dictionaries even for generic calls
   - Required runtime code generation for optimal performance
   - Not compatible with Native AOT compilation

4. **Memory Inefficiency**
   - Separate dictionary entries for every registration
   - High GC pressure from constant array rebuilds
   - No sharing of metadata between related generic types

**GenericFirstDependencyResolver** (v19+) fixes all these issues:

1. **O(1) Registration** - Lazy snapshot rebuilding eliminates O(n²) behavior
2. **Lock-Free Reads** - Zero contention for service resolution (the hot path)
3. **AOT-Compatible** - Static generic containers require no reflection
4. **Memory Efficient** - Versioned snapshots, minimal allocations
5. **Correct Concurrency** - Volatile semantics, proper memory ordering
6. **Backward Compatible** - Type-based APIs still work for legacy code

**Migration is seamless** - GenericFirstDependencyResolver implements the same `IMutableDependencyResolver` interface, so existing code works without changes. Just recompile against v19+ to get the performance benefits.

**When to use Type-based methods:**
- Interop with other containers (Autofac, Microsoft.Extensions.DependencyInjection)
- Dynamic plugin scenarios where types are loaded at runtime
- Legacy code that hasn't been updated to use generics

**When to use generic methods (preferred):**
- All new code
- Application startup registration
- Hot-path service resolution
- AOT/trimming scenarios

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

## AppBuilder and IModule (AOT-friendly configuration)

When targeting AOT or trimming scenarios, you can configure Splat (and consumers such as ReactiveUI) without reflection using the AppBuilder and IModule pattern.

- IModule defines a single Configure(IMutableDependencyResolver) method where you register services.
- AppBuilder gathers modules and custom registrations, then applies them to a chosen resolver when Build() is called.
- You can target the current AppLocator.CurrentMutable or an external container that implements IMutableDependencyResolver via UseCurrentSplatLocator().

### Define modules

```cs
using Splat;
using Splat.Builder;

// App services
public sealed class CoreModule : IModule
{
    public void Configure(IMutableDependencyResolver resolver)
    {
        // register your services here
        resolver.RegisterLazySingleton(() => new ApiClient(), typeof(IApiClient));
        resolver.Register(() => new MainViewModel(resolver.GetService<IApiClient>()!), typeof(IMainViewModel));
    }
}

// Logging (Serilog adapter) - requires Splat.Serilog package
using Splat.Serilog;
public sealed class SerilogModule : IModule
{
    public void Configure(IMutableDependencyResolver resolver)
    {
        // assumes Serilog.Log has been configured elsewhere
        resolver.UseSerilogFullLogger();
    }
}

// Cross-platform drawing (IBitmapLoader) - requires Splat.Drawing package
public sealed class DrawingModule : IModule
{
    public void Configure(IMutableDependencyResolver resolver)
    {
        resolver.RegisterPlatformBitmapLoader();
    }
}
```

Tip: You can also apply a module immediately to the current locator without a builder:

```cs
new CoreModule().Apply(); // calls module.Configure(Locator.CurrentMutable)
```

### Build your application registrations

```cs
using Splat;
using Splat.Builder;

// choose the resolver to configure (Locator.CurrentMutable by default)
new AppBuilder(Locator.CurrentMutable)
    .UsingModule(new CoreModule())
    .UsingModule(new SerilogModule())
    .UsingModule(new DrawingModule())
    // ad-hoc registrations can be added too
    .WithCustomRegistration(r => r.RegisterConstant(new AppConfig("Prod"), typeof(AppConfig)))
    .Build();
```

### Using an external container as the Splat resolver

If you adapt an external container to Splat first, UseCurrentSplatLocator() ensures later module registrations are applied to that container instance.

Microsoft.Extensions.DependencyInjection example:

```cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Splat;
using Splat.Builder;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.Logging;

var services = new ServiceCollection();
services.UseMicrosoftDependencyResolver(); // set AppLocator.Current to MS.DI resolver backed by IServiceCollection

// register framework logging provider that forwards to Splat
services.AddLogging(b => b.AddSplat());

// build container and rebind AppLocator.Current to the built provider
var provider = services.BuildServiceProvider();
provider.UseMicrosoftDependencyResolver();

new AppBuilder(AppLocator.CurrentMutable)
    .UseCurrentSplatLocator() // target whatever AppLocator.CurrentMutable points to (MS.DI in this case)
    .UsingModule(new CoreModule())
    .WithCustomRegistration(r =>
    {
        // forward Microsoft.Extensions.Logging to Splat
        var factory = provider.GetRequiredService<ILoggerFactory>();
        r.UseMicrosoftExtensionsLoggingWithWrappingFullLogger(factory);
    })
    .Build();
```

The same pattern works with other adapters (Autofac, DryIoc, SimpleInjector, Ninject) after setting AppLocator.Current to the adapter’s resolver.

### Extending AppBuilder

AppBuilder is extensible. Override WithCoreServices() to ensure common registrations are always applied before modules:

```cs
using Splat;
using Splat.Builder;

public sealed class MyAppBuilder : AppBuilder
{
    public MyAppBuilder(IMutableDependencyResolver resolver) : base(resolver) { }

    public override AppBuilder WithCoreServices()
    {
        // register core bits once for your app
        // e.g., default logger, scheduler, or platform services
        // resolver is obtained per registration in Build(),
        // so prefer idempotent registrations guarded by HasRegistration
        return this;
    }
}

// usage
new MyAppBuilder(Locator.CurrentMutable)
    .UsingModule(new CoreModule())
    .Build();
```

Notes:
- Build() is idempotent per process: subsequent calls no-op after the first successful build.
- Prefer RegisterLazySingleton for singletons and check HasRegistration when composing multiple modules.

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
the SplatColor class to define colors in your shared library
(since Cocoa doesn't include System.Drawing.Color).

```cs
// In a shared library
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

## Dependency Resolver Performance Benchmarks

Here is the comprehensive performance summary for the new `InstanceGenericFirstDependencyResolver` (referred to as the **New Resolver**) compared to the existing `ModernDependencyResolver` (referred to as the **Legacy Resolver**).

These statistics are based on the .NET 10.0 benchmark results provided.

The **New Resolver** is a massive upgrade over the legacy implementation. It is fully **AOT-compatible** and statistically superior in every critical metric:

* **3.5x Faster** in real-world usage (Mixed Read/Write).
* **3.6x Faster** for empty container checks (Startup/Misses).
* **18x Less Memory Allocated** during service registration.
* **2x Faster** at registering services.

It achieves this by replacing the old Dictionary-based lookup with a generic-first architecture that leverages static generic caching and `ConditionalWeakTable` for instance isolation.

### 1. Real-World Performance

*Simulates a realistic application lifecycle: registering services, resolving them, and checking for existence.*

| Workload | New Resolver | Legacy Resolver | Improvement |
| --- | --- | --- | --- |
| **Realistic Usage** | **431.1 μs** | 1,513.1 μs | **3.5x Faster** |

> **Why this matters:** This is the metric that users will actually "feel" in their applications. The overhead of using Splat for dependency resolution effectively disappears.

### 2. Retrieval (Read) Performance

*Resolving services (`GetService`) or checking for them (`HasRegistration`).*

| Operation | Scenario | New Resolver | Legacy Resolver | Improvement |
| --- | --- | --- | --- | --- |
| **Empty / Miss** | *Service not found* | **~8.6 ns** | ~31.4 ns | **3.6x Faster** |
| **Hit (Generic)** | *Service found* | **~54 ns** | ~71 ns | **1.3x Faster** |
| **Collection** | *GetServices* | **168 μs** | 581 μs | **3.4x Faster** |

> **Why this matters:** The "Empty/Miss" path is critical for app startup and unit testing where containers are often empty or checking for optional services. The optimizations we applied reduced this cost from ~7000ns to just ~9ns.

### 3. Mutation (Write) Performance

*Registering new services into the container.*

| Operation | New Resolver | Legacy Resolver | Improvement |
| --- | --- | --- | --- |
| **Register (Generic)** | **73 μs** | 152 μs | **2.1x Faster** |
| **Register (Constant)** | **92 μs** | 173 μs | **1.9x Faster** |
| **Register (Contract)** | **107 μs** | 163 μs | **1.5x Faster** |

> **Why this matters:** Faster registration means faster application startup times, especially for apps with hundreds of services.

### 4. Memory Efficiency (Allocations)

*How much garbage (memory) is created during operations.*

| Operation | New Resolver | Legacy Resolver | Improvement |
| --- | --- | --- | --- |
| **Register (Constant)** | **66 KB** | 1,212 KB | **18x Less** |
| **Register (Generic)** | **59 KB** | 1,177 KB | **20x Less** |
| **Mixed Workload** | **832 KB** | 1,856 KB | **2.2x Less** |

> **Why this matters:** The Legacy resolver allocated over **1 MB** just to register 300 simple services due to boxing and closure overhead. The New Resolver slashes this to just **60 KB**, significantly reducing GC pressure and pauses in mobile and desktop apps.

### 5. Architectural Comparison

| Feature | New Resolver (`InstanceGenericFirst`) | Legacy Resolver (`Modern`) |
| --- | --- | --- |
| **Storage** | Generic Static Cache (`ContainerCache<T>`) | `Dictionary<Type, List<Func>>` |
| **Isolation** | `ConditionalWeakTable` (Per-Instance) | Dictionary Instance |
| **Registration** | `readonly record struct` (Zero Alloc) | `Func<object>` Delegate (High Alloc) |
| **AOT Support** | **Native** (No reflection on hot paths) | **Poor** (Requires reflection/boxing) |
| **Concurrency** | Lock-Free Reads (Volatile Snapshots) | Lock-Free Reads (Volatile Snapshots) |

### Conclusion for Consumers

By switching to the new resolver, consumers get a **free performance boost** and **AOT compatibility** without changing their code. The memory savings alone make this a mandatory upgrade for memory-constrained environments like mobile (MAUI/Xamarin) and WebAssembly (Blazor).

## Contribute

Splat is developed under an OSI-approved open source license, making it freely usable and distributable, even for commercial use. We ❤ the people who are involved in this project, and we’d love to have you on board, especially if you are just getting started or have never contributed to open-source before.

So here's to you, lovely person who wants to join us — this is how you can support us:

* [Responding to questions on StackOverflow](https://stackoverflow.com/questions/tagged/splat)
* [Passing on knowledge and teaching the next generation of developers](http://ericsink.com/entries/dont_use_rxui.html)
* Submitting documentation updates where you see fit or lacking.
* Making contributions to the code base.
