using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.ReactiveUI;
using Avalonia.ReactiveUI.Splat;
using DryIoc;
using ReactiveUIDemo;
using Splat;
using Splat.DryIoc;

namespace ReactiveUI.Avalonia.DryIoc1.Tests
{
    public class AvaloniaUIThreadTestsDryIoc
    {
#if DRYIOC1
        [Fact]
        public void Test1()
        {
            DryIocDependencyResolver? container = default;
            DryIocDependencyResolver? resolver = default;
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUIWithDIContainer<DryIocDependencyResolver>(() => new(), con => container = con, res => resolver = res)
                .LogToTrace()
                .SetupWithoutStarting();
            Assert.IsType<AvaloniaScheduler>(RxApp.MainThreadScheduler);
            Assert.NotNull(container);
            Assert.NotNull(resolver);
            Assert.IsType<DryIocDependencyResolver>(Locator.Current);
        }
#endif
#if DRYIOC2
        [Fact]
        public void Test2()
        {
            Container? container = default;
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUIWithDryIoc(con => container = con)
                .LogToTrace()
                .SetupWithoutStarting();
            Assert.IsType<AvaloniaScheduler>(RxApp.MainThreadScheduler);
            Assert.NotNull(container);
            Assert.IsType<DryIocDependencyResolver>(Locator.Current);
        }
#endif
    }
}
