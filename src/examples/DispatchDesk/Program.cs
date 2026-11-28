// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Text;
using Splat;
using Splat.Builder;

namespace DispatchDesk;

/// <summary>Resolves dispatch services, overrides a depot and logs a shipment.</summary>
internal static class Program
{
    /// <summary>The named registration for the dispatch location.</summary>
    private const string Depot = "depot";

    /// <summary>The location shared by the daytime desks.</summary>
    private const string DayDepot = "Richmond";

    /// <summary>Runs a dispatch session with its own service registrations.</summary>
    /// <exception cref="InvalidOperationException">A registration has the wrong lifetime or resolves the wrong service.</exception>
    internal static void Main()
    {
        using var resolver = new ModernDependencyResolver();
        var builder = new AppBuilder(resolver, resolver);
        var labelsRegistered = false;
        using var watch = resolver.ServiceRegistrationCallback<StringBuilder>(_ => labelsRegistered = true);
        IAppInstance app = builder.UsingModule(new DispatchModule())
            .WithCustomRegistration(static services => services.RegisterConstant(DayDepot, Depot))
            .Build();
        var services = app.Current ?? throw new InvalidOperationException("The dispatch desk requires a read resolver.");
        var queue = services.GetService<Queue<string>>() ?? throw new InvalidOperationException("Missing delivery queue.");
        queue.Enqueue("Parcel for Ada");
        var firstLabel = services.GetService<StringBuilder>();
        var secondLabel = services.GetService<StringBuilder>();
        if (!labelsRegistered || firstLabel is null || secondLabel is null || ReferenceEquals(firstLabel, secondLabel)
            || !ReferenceEquals(queue, services.GetService<Queue<string>>()))
        {
            throw new InvalidOperationException("Packing labels should be fresh and the delivery queue should be shared.");
        }

        CheckEveningDepot(resolver);
        LogShipment(resolver, queue.Dequeue());
    }

    /// <summary>Overrides the depot at one desk and restores the original registration.</summary>
    /// <param name="resolver">The daytime resolver to copy.</param>
    /// <exception cref="InvalidOperationException">The two desks share a changed registration.</exception>
    private static void CheckEveningDepot(ModernDependencyResolver resolver)
    {
        using var eveningDesk = resolver.Duplicate();
        eveningDesk.RegisterConstant("Carlton", Depot);
        if (eveningDesk.GetService<string>(Depot) != "Carlton" || resolver.GetService<string>(Depot) != DayDepot)
        {
            throw new InvalidOperationException("The evening desk should have an independent depot override.");
        }

        eveningDesk.UnregisterCurrent<string>(Depot);
        if (eveningDesk.GetService<string>(Depot) == DayDepot)
        {
            return;
        }

        throw new InvalidOperationException("Removing the override should reveal the original depot.");
    }

    /// <summary>Captures the severity and text that the dispatch logger sends to its sink.</summary>
    /// <param name="resolver">The resolver used to create the log manager.</param>
    /// <param name="shipment">The parcel being dispatched.</param>
    /// <exception cref="InvalidOperationException">The log sink did not receive the shipment.</exception>
    private static void LogShipment(ModernDependencyResolver resolver, string shipment)
    {
        List<(LogLevel Level, string Message)> entries = [];
        ActionLogger sink = new(
            (message, level) => entries.Add((level, message)),
            (message, _, level) => entries.Add((level, message)),
            (_, message, level) => entries.Add((level, message)),
            (_, message, _, level) => entries.Add((level, message)));
        resolver.RegisterConstant<ILogger>(sink);
        var manager = new DefaultLogManager(resolver);
        var log = manager.GetLogger(typeof(DispatchModule));
        log.Info(shipment);
        if (entries is not [(LogLevel.Info, var message)] || !message.Contains(shipment, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Expected one information message for the dispatched parcel.");
        }
    }
}
