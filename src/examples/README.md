# Dispatch desk

The dispatch desk gives each parcel a fresh packing label and shares one delivery queue. Its evening desk copies the registrations, overrides the named depot and then removes that override. A log sink records the shipment and its severity.

| Source | What to look for |
| --- | --- |
| [DispatchModule.cs](DispatchDesk/DispatchModule.cs) | Register a factory for fresh labels and a lazy singleton for the queue. |
| [Program.cs](DispatchDesk/Program.cs) | Apply a module when the app builds, watch a registration, resolve services, copy a resolver and write through `DefaultLogManager`. |
| [Container adapters](DispatchAdapters/DispatchContainers.cs) | Register one queue in Autofac, DryIoc, Microsoft DI, Ninject or Simple Injector. Resolve a named queue from Prism and Splat. |
| [Dispatch logs](DispatchAdapters/DispatchLogs.cs) | Follow a warning from Microsoft logging through Splat to Serilog. Inspect the same warning in NLog, log4net and Exceptionless. |
| [Parcel labels](DispatchAdapters/ParcelLabel.cs) | Split the label rectangle, cache the barcode center, draw with a Splat color, save a PNG and decode a smaller image using the Skia loader. |
| [Packing telemetry](DispatchTelemetry/PackingTelemetry.cs) | Record a view and link a print operation to its packing session using Application Insights v2 or v3. |
| [Printer errors](DispatchTelemetry/RaygunPacking.cs) | Report a printer exception from a Raygun child session. |

Run from `src`.

```shell
dotnet run --project examples/DispatchDesk -f net11.0
dotnet run --project examples/DispatchAdapters -f net11.0
```

Use `-f net10.0` for the .NET 10 build.

The telemetry examples accept the application's existing client. Run any of the following from `src` to exercise them with local test clients.

```shell
dotnet test --project tests/Splat.ApplicationInsights.Tests/Splat.ApplicationInsights.Tests.csproj
dotnet test --project tests/Splat.ApplicationInsightsV3.Tests/Splat.ApplicationInsightsV3.Tests.csproj
dotnet test --project tests/Splat.Raygun.Tests/Splat.Raygun.Tests.csproj
```
