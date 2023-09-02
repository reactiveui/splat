using System.ComponentModel;
using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.Avalonia.Splat;
using ReactiveUIDemo;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace ReactiveUI.Avalonia.Microsoft.Tests;

public class AvaloniaUIThreadTestsMicrosoft
{
#if MICROSOFT1
    [Fact]
    public void Test1()
    {
        MicrosoftDependencyResolver? container = default;
        MicrosoftDependencyResolver? resolver = default;
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUIWithDIContainer<MicrosoftDependencyResolver>(() => new(), con => container = con, res => resolver = res)
            .LogToTrace()
            .SetupWithoutStarting();
        Assert.IsType<AvaloniaScheduler>(RxApp.MainThreadScheduler);
        Assert.NotNull(container);
        Assert.NotNull(resolver);
        Assert.IsType<MicrosoftDependencyResolver>(Locator.Current);
    }
#endif
////#if MICROSOFT2
////    [Fact]
////    public void Test2()
////    {
////        IServiceCollection? container = default;
////        IServiceProvider? resolver = default;
////        AppBuilder.Configure<App>()
////            .UsePlatformDetect()
////            .UseReactiveUIWithMicrosoftDependencyResolver(con => container = con, res => resolver = res)
////            .LogToTrace()
////            .SetupWithoutStarting();
////        Assert.IsType<AvaloniaScheduler>(RxApp.MainThreadScheduler);
////        Assert.NotNull(container);
////        Assert.NotNull(resolver);
////        Assert.IsType<MicrosoftDependencyResolver>(Locator.Current);
////    }
////#endif
}
