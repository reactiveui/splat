// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;
using DryIoc;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Splat;
using Splat.Autofac;
using Splat.DryIoc;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Ninject;
using Splat.Prism;
using Splat.SimpleInjector;

namespace DispatchAdapters;

/// <summary>Reads the same dispatch queue through each supported container adapter.</summary>
internal static class DispatchContainers
{
    /// <summary>Checks an Autofac registration through the scope that owns the queue.</summary>
    internal static void UsingAutofac()
    {
        var builder = new ContainerBuilder();
        Queue<string> queue = new();
        _ = builder.RegisterInstance(queue);
        var resolver = builder.UseAutofacDependencyResolver();
        using var container = builder.Build();
        resolver.SetLifetimeScope(container);
        DispatchFrom(queue);
    }

    /// <summary>Resolves a queue that DryIoc owns.</summary>
    internal static void UsingDryIoc()
    {
        using var container = new Container();
        Queue<string> queue = new();
        container.RegisterInstance(queue);
        container.UseDryIocDependencyResolver();
        DispatchFrom(queue);
    }

    /// <summary>Registers through Splat before building a Microsoft service provider.</summary>
    internal static void UsingMicrosoftServices()
    {
        var services = new ServiceCollection();
        services.UseMicrosoftDependencyResolver();
        Queue<string> queue = new();
        AppLocator.CurrentMutable.RegisterConstant(queue);
        using var provider = services.BuildServiceProvider();
        provider.UseMicrosoftDependencyResolver();
        DispatchFrom(provider.GetRequiredService<Queue<string>>());
    }

    /// <summary>Uses the constant queue bound to a Ninject kernel.</summary>
    internal static void UsingNinject()
    {
        using var kernel = new StandardKernel();
        Queue<string> queue = new();
        _ = kernel.Bind<Queue<string>>().ToConstant(queue);
        kernel.UseNinjectDependencyResolver();
        DispatchFrom(queue);
    }

    /// <summary>Carries a registration from the initializer into Simple Injector.</summary>
    internal static void UsingSimpleInjector()
    {
        using var container = new SimpleInjector.Container();
        using var initializer = new SimpleInjectorInitializer();
        Queue<string> queue = new();
        initializer.RegisterConstant(queue);
        container.UseSimpleInjectorDependencyResolver(initializer);
        DispatchFrom(queue);
    }

    /// <summary>Uses a named Prism registration backed by Splat.</summary>
    /// <exception cref="InvalidOperationException">The named queue could not be resolved.</exception>
    internal static void UsingPrism()
    {
        const string Depot = "north";
        using var prism = new SplatContainerExtension();
        Queue<string> queue = new();
        _ = prism.RegisterInstance(typeof(Queue<string>), queue, Depot);
        if (prism.Resolve(typeof(Queue<string>), Depot) == queue && prism.Instance.GetService<Queue<string>>(Depot) == queue)
        {
            return;
        }

        throw new InvalidOperationException("Prism and Splat should resolve the same named queue.");
    }

    /// <summary>Dispatches a parcel through the active adapter and reads it from the original queue.</summary>
    /// <param name="queue">The queue installed in the container.</param>
    /// <exception cref="InvalidOperationException">The original queue did not receive the parcel.</exception>
    private static void DispatchFrom(Queue<string> queue)
    {
        const string Parcel = "Parcel for Ada";
        var active = AppLocator.Current.GetService<Queue<string>>() ?? throw new InvalidOperationException("No dispatch queue was registered.");
        active.Enqueue(Parcel);
        if (queue.TryDequeue(out var item) && item == Parcel)
        {
            return;
        }

        throw new InvalidOperationException("The parcel did not reach the registered queue.");
    }
}
