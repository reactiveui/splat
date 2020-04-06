# Splat.Microsoft.Extensions.DependencyInjection

## Using Microsoft.Extensions.DependencyInjection

`Splat.Microsoft.Extensions.DependencyInjection` is an adapter for `IMutableDependencyResolver`.
It allows you to register your application dependencies in a MS DI `Container`.  You can then use the container as Splat's internal dependency resolver.
You can also choose to have the container controlled externally, for example using a [Generic Host](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-2.2#configureappconfiguration).

### Register the Container

```cs
// call this method from your apps constructor or early initialization code

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.Logging;
using System;
using System.Linq;
// using Windows.UI.Xaml

sealed partial class App // : Application
{   
  public App()
  {
    Init();
    /* Some other initialization stuff */
  }                     

  public IServiceProvider Container { get; private set; }

  void Init()
  {
    var host = Host
      .CreateDefaultBuilder()
      .ConfigureServices(services =>
      {
        services.UseMicrosoftDependencyResolver();
        var resolver = Locator.CurrentMutable;
        resolver.InitializeSplat();
        resolver.InitializeReactiveUI();

        // Configure our local services and access the host configuration
        ConfigureServices(services);
      })
      .ConfigureLogging(loggingBuilder =>
      {
        /*
        //remove loggers incompatible with UWP
        {
          var eventLoggers = loggingBuilder.Services
            .Where(l => l.ImplementationType == typeof(EventLogLoggerProvider))
            .ToList();

          foreach (var el in eventLoggers)
            loggingBuilder.Services.Remove(el);
        }
        */

        loggingBuilder.AddSplat();
      })
      .UseEnvironment(Environments.Development)
      .Build();

    // Since MS DI container is a different type,
    // we need to re-register the built container with Splat again
    Container = host.Services;
    Container.UseMicrosoftDependencyResolver();
  }

  void ConfigureServices(IServiceCollection services)
  {
    // register your personal services here, for example
    services.AddSingleton<MainViewModel>(); //Implements IScreen
	
    // this passes IScreen resolution through to the previous viewmodel registration.
	// this is to prevent multiple instances by mistake.
    services.AddSingleton<IScreen, MainViewModel>(x => x.GetRequiredService<MainViewModel>());
	
    services.AddSingleton<IViewFor<MainViewModel>, MainPage>();
    
    //alternatively search assembly for `IRoutedViewFor` implementations
    //see https://reactiveui.net/docs/handbook/routing to learn more about routing in RxUI
    services.AddTransient<IViewFor<SecondaryViewModel>, SecondaryPage>();    
    services.AddTransient<SecondaryViewModel>();
  }
}  
```

Note: the code below uses the [`Microsoft.Extensions.Hosting`](https://www.nuget.org/packages/Microsoft.Extensions.Hosting) and [`Microsoft.Extensions.Logging`](https://www.nuget.org/packages/Microsoft.Extensions.Logging) packages.

### Register the Adapter to Splat

First, call:

```cs
IServiceCollection services = ...
services.UseMicrosoftDependencyResolver();
```

then, if you wish to have the MS DI container controlled by an external service other than Splat, re-register it as above (using Generic Host), or as follows:

```cs
IServiceProvider container = services.BuildServiceProvider();
container.UseMicrosoftDependencyResolver();
```

### Use the Locator

Now, when registering or resolving services using Locator.Current, or via ReactiveUI, they will be directed to the Microsoft DI container.
