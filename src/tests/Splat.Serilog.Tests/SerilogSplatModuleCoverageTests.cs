// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Serilog;

using Splat.Builder;
using Splat.Serilog;

namespace Splat.Tests.Logging;

/// <summary>Tests that verify the <see cref="SerilogSplatModule"/> and Serilog registration extensions.</summary>
public class SerilogSplatModuleCoverageTests
{
    /// <summary>Tests that the default constructor configures an <see cref="ILogManager"/> using the default Serilog logger.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_With_Default_Constructor_Registers_LogManager()
    {
        var resolver = new ModernDependencyResolver();
        var module = new SerilogSplatModule();

        module.Configure(resolver);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ILogManager))).IsTrue();
            await Assert.That(resolver.GetService<ILogManager>()).IsNotNull();
        }
    }

    /// <summary>Tests that the parameterless-equivalent null logger constructor still registers an <see cref="ILogManager"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_With_Null_Logger_Registers_LogManager()
    {
        var resolver = new ModernDependencyResolver();
        var module = new SerilogSplatModule(null);

        module.Configure(resolver);

        await Assert.That(resolver.GetService<ILogManager>()).IsNotNull();
    }

    /// <summary>Tests that supplying an explicit Serilog logger configures an <see cref="ILogManager"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_With_Explicit_Logger_Registers_LogManager()
    {
        var resolver = new ModernDependencyResolver();
        global::Serilog.ILogger logger = new LoggerConfiguration().CreateLogger();
        var module = new SerilogSplatModule(logger);

        module.Configure(resolver);

        await Assert.That(resolver.GetService<ILogManager>()).IsNotNull();
    }

    /// <summary>Tests that calling <see cref="IModule.Configure(IMutableDependencyResolver)"/> twice remains functional.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_Is_Idempotent()
    {
        var resolver = new ModernDependencyResolver();
        var module = new SerilogSplatModule();

        module.Configure(resolver);
        module.Configure(resolver);

        await Assert.That(resolver.GetService<ILogManager>()).IsNotNull();
    }

    /// <summary>Tests that the parameterless extension method registers an <see cref="ILogManager"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseSerilogFullLogger_Default_Registers_LogManager()
    {
        using var resolver = new ModernDependencyResolver();

        resolver.UseSerilogFullLogger();

        await Assert.That(resolver.GetService<ILogManager>()).IsNotNull();
    }

    /// <summary>Tests that the explicit-logger extension method registers an <see cref="ILogManager"/>.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseSerilogFullLogger_With_Logger_Registers_LogManager()
    {
        using var resolver = new ModernDependencyResolver();
        global::Serilog.ILogger logger = new LoggerConfiguration().CreateLogger();

        resolver.UseSerilogFullLogger(logger);

        await Assert.That(resolver.GetService<ILogManager>()).IsNotNull();
    }

    /// <summary>Tests that the parameterless extension method throws when the resolver is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseSerilogFullLogger_Default_Throws_For_Null_Resolver()
    {
        const IMutableDependencyResolver resolver = null!;

        await Assert.That(static () => resolver.UseSerilogFullLogger()).Throws<ArgumentNullException>();
    }

    /// <summary>Tests that the explicit-logger extension method throws when the resolver is null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UseSerilogFullLogger_With_Logger_Throws_For_Null_Resolver()
    {
        const IMutableDependencyResolver resolver = null!;
        global::Serilog.ILogger logger = new LoggerConfiguration().CreateLogger();

        await Assert.That(() => resolver.UseSerilogFullLogger(logger)).Throws<ArgumentNullException>();
    }
}
