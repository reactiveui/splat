// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;
using Xunit;

namespace Splat.Tests.Aot;

/// <summary>
/// Comprehensive tests to verify AOT and trimming compatibility of the core Splat library.
/// These tests ensure that the library works correctly when used with Native AOT
/// and when assemblies are trimmed by the linker.
/// </summary>
public class CoreAotCompatibilityTests
{
    /// <summary>
    /// Interface for testing dependency injection.
    /// </summary>
    private interface ITestInterface
    {
        /// <summary>
        /// Gets a test value.
        /// </summary>
        /// <returns>A test string.</returns>
        string GetValue();
    }

    /// <summary>
    /// Test that basic service registration and resolution works in AOT scenarios.
    /// </summary>
    [Fact]
    public void BasicServiceRegistration_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act - Register services using factory methods (AOT-safe)
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        resolver.Register<IEnableLogger>(() => new TestService());

        // Assert - Verify services can be resolved
        var logger = resolver.GetService<ILogger>();
        var logManager = resolver.GetService<ILogManager>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        Assert.NotNull(logger);
        Assert.NotNull(logManager);
        Assert.NotNull(enableLogger);
        Assert.IsType<DebugLogger>(logger);
        Assert.IsType<DefaultLogManager>(logManager);
        Assert.IsType<TestService>(enableLogger);
    }

    /// <summary>
    /// Test that all registration mixins work with AOT.
    /// </summary>
    [Fact]
    public void RegistrationMixins_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act - Test all registration methods
        resolver.Register<ITestInterface>(() => new TestImplementation());
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterLazySingleton<ILogManager>(() => new DefaultLogManager(resolver));

        // Test fluent registration
        resolver.RegisterAnd<IEnableLogger>(() => new TestService())
                .RegisterConstantAnd<ITestInterface>(new TestImplementation())
                .RegisterLazySingletonAnd<ILogger>(() => new ConsoleLogger());

        // Assert - All services should resolve
        var testInterface = resolver.GetService<ITestInterface>();
        var logger = resolver.GetService<ILogger>();
        var logManager = resolver.GetService<ILogManager>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        Assert.NotNull(testInterface);
        Assert.NotNull(logger);
        Assert.NotNull(logManager);
        Assert.NotNull(enableLogger);
    }

    /// <summary>
    /// Test that service callbacks work with AOT.
    /// </summary>
    [Fact]
    public void ServiceRegistrationCallbacks_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        var callbackTriggered = false;

        // Act
        using var subscription = resolver.ServiceRegistrationCallback(
            typeof(ILogger),
            null,
            _ => callbackTriggered = true);

        resolver.RegisterConstant<ILogger>(new DebugLogger());

        // Assert
        Assert.True(callbackTriggered);
    }

    /// <summary>
    /// Test that logging functionality works with AOT.
    /// </summary>
    [Fact]
    public void Logging_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var locatorScope = resolver.WithResolver();

        // Act
        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();

        // Assert
        Assert.NotNull(logger);

        // Test logging methods don't throw in AOT
        logger.Debug("Test debug message");
        logger.Info("Test info message");
        logger.Warn("Test warning message");
        logger.Error("Test error message");
        logger.Fatal("Test fatal message");
    }

    /// <summary>
    /// Test that mode detection works with AOT.
    /// </summary>
    [Fact]
    public void ModeDetection_WorksWithAot()
    {
        // Arrange
        var modeDetector = new DefaultModeDetector();

        // Act & Assert - Should not throw in AOT
        var inUnitTest = modeDetector.InUnitTestRunner();
        Assert.NotNull(inUnitTest);
        Assert.True(inUnitTest.Value); // We're running in a unit test
    }

    /// <summary>
    /// Test that lazy singleton registration works with AOT.
    /// </summary>
    [Fact]
    public void LazySingleton_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        var creationCount = 0;

        // Act
        resolver.RegisterLazySingleton<IEnableLogger>(() =>
        {
            creationCount++;
            return new TestService();
        });

        // Assert
        var service1 = resolver.GetService<IEnableLogger>();
        var service2 = resolver.GetService<IEnableLogger>();

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.Same(service1, service2); // Should be the same instance
        Assert.Equal(1, creationCount); // Should only be created once
    }

    /// <summary>
    /// Test that unregistering services works with AOT.
    /// </summary>
    [Fact]
    public void ServiceUnregistration_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        // Act
        var logger1 = resolver.GetService<ILogger>();
        Assert.NotNull(logger1);

        resolver.UnregisterCurrent<ILogger>();
        var logger2 = resolver.GetService<ILogger>();

        // Assert
        Assert.Null(logger2);
    }

    /// <summary>
    /// Test that target framework extensions work with AOT.
    /// </summary>
    [Fact]
    public void TargetFrameworkExtensions_WorksWithAot()
    {
        // Arrange
        var assembly = typeof(CoreAotCompatibilityTests).Assembly;

        // Act - This uses reflection but should be AOT-safe with proper attributes
        var targetFramework = assembly.GetTargetFrameworkName();

        // Assert - Should return a valid framework name or null
        if (targetFramework != null)
        {
            Assert.Contains("net", targetFramework, StringComparison.InvariantCulture);
        }
    }

    /// <summary>
    /// Test that multiple service registration works with AOT.
    /// </summary>
    [Fact]
    public void MultipleServiceRegistration_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act
        resolver.Register<ITestInterface>(() => new TestImplementation());
        resolver.Register<ITestInterface>(() => new AlternateTestImplementation());

        // Assert
        var services = resolver.GetServices<ITestInterface>().ToList();
        Assert.Equal(2, services.Count);
        Assert.Contains(services, s => s is TestImplementation);
        Assert.Contains(services, s => s is AlternateTestImplementation);
    }

    /// <summary>
    /// Test that locator static methods work with AOT.
    /// </summary>
    [Fact]
    public void LocatorStatic_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        // Act
        using var locatorScope = resolver.WithResolver();
        var logger = Locator.Current.GetService<ILogger>();

        // Assert
        Assert.NotNull(logger);
        Assert.IsType<DebugLogger>(logger);
    }

    /// <summary>
    /// Test that generic type registration with constraints works with AOT.
    /// This tests DynamicallyAccessedMembers annotations on generic type parameters.
    /// </summary>
    [Fact]
    public void GenericTypeRegistrationWithConstraints_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act - Register types with new() constraint (tests DynamicallyAccessedMembers.PublicConstructors)
        resolver.Register<ITestInterface, TestImplementation>();
        resolver.Register<IEnableLogger, TestService>();

        // Assert
        var testInterface = resolver.GetService<ITestInterface>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        Assert.NotNull(testInterface);
        Assert.NotNull(enableLogger);
        Assert.IsType<TestImplementation>(testInterface);
        Assert.IsType<TestService>(enableLogger);
    }

    /// <summary>
    /// Test that service registrations with contracts work with AOT.
    /// </summary>
    [Fact]
    public void ContractBasedRegistration_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        const string contract1 = "Contract1";
        const string contract2 = "Contract2";

        // Act
        resolver.RegisterConstant<ITestInterface>(new TestImplementation(), contract1);
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), contract2);
        resolver.RegisterConstant<ITestInterface>(new TestImplementation()); // Default

        // Assert
        var service1 = resolver.GetService<ITestInterface>(contract1);
        var service2 = resolver.GetService<ITestInterface>(contract2);
        var defaultService = resolver.GetService<ITestInterface>();

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.NotNull(defaultService);
        Assert.IsType<TestImplementation>(service1);
        Assert.IsType<AlternateTestImplementation>(service2);
        Assert.IsType<TestImplementation>(defaultService);
    }

    /// <summary>
    /// Test that type-based service queries work with AOT.
    /// </summary>
    [Fact]
    public void TypeBasedServiceQueries_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act
#pragma warning disable CA2263 // Prefer generic overload when type is known
        resolver.RegisterConstant(new DebugLogger(), typeof(ILogger));
        resolver.RegisterConstant(new DefaultLogManager(resolver), typeof(ILogManager));
#pragma warning restore CA2263 // Prefer generic overload when type is known

        // Assert - Test non-generic methods
        var logger = resolver.GetService(typeof(ILogger));
        var logManager = resolver.GetService(typeof(ILogManager));
        var services = resolver.GetServices(typeof(ILogger)).ToList();

        Assert.NotNull(logger);
        Assert.NotNull(logManager);
        Assert.Single(services);
        Assert.IsType<DebugLogger>(logger);
        Assert.IsType<DefaultLogManager>(logManager);
    }

    /// <summary>
    /// Test that HasRegistration queries work with AOT.
    /// </summary>
    [Fact]
    public void HasRegistrationQueries_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act & Assert - Test before registration
        Assert.False(resolver.HasRegistration(typeof(ITestInterface)));
        Assert.False(resolver.HasRegistration(typeof(ITestInterface), "contract"));

        // Register and test again
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), "contract");

        Assert.True(resolver.HasRegistration(typeof(ITestInterface)));
        Assert.True(resolver.HasRegistration(typeof(ITestInterface), "contract"));
    }

    /// <summary>
    /// Test that comprehensive logging functionality works with AOT.
    /// This tests generic logging methods with type parameters.
    /// </summary>
    [Fact]
    public void ComprehensiveLogging_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var locatorScope = resolver.WithResolver();
        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();
        Assert.NotNull(logger);

        // Act & Assert - Test generic logging methods that use type parameters
        logger.Debug<string>("Debug with generic type");
        logger.Info<int>("Info with generic type");
        logger.Warn<CoreAotCompatibilityTests>("Warn with generic type");
        logger.Error<ITestInterface>("Error with generic type");
        logger.Fatal<TestImplementation>("Fatal with generic type");

        // Test logging with format providers
        logger.Debug(CultureInfo.InvariantCulture, "Debug with culture: {0}", "test");
        logger.Info(CultureInfo.InvariantCulture, "Info with culture: {0}", "test");
        logger.Warn(CultureInfo.InvariantCulture, "Warn with culture: {0}", "test");
        logger.Error(CultureInfo.InvariantCulture, "Error with culture: {0}", "test");
        logger.Fatal(CultureInfo.InvariantCulture, "Fatal with culture: {0}", "test");

        // Test exception logging
        var testException = new InvalidOperationException("Test exception");
        logger.Debug(testException, "Debug with exception");
        logger.Info(testException, "Info with exception");
        logger.Warn(testException, "Warn with exception");
        logger.Error(testException, "Error with exception");
        logger.Fatal(testException, "Fatal with exception");
    }

    /// <summary>
    /// Test that allocation-free logging methods work with AOT.
    /// This ensures that high-performance logging scenarios are AOT-compatible.
    /// </summary>
    [Fact]
    public void AllocationFreeLogging_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var locatorScope = resolver.WithResolver();
        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();
        Assert.NotNull(logger);

        // Act & Assert - Test allocation-free logging methods
        logger.Debug("Simple message {0}", "arg1");
        logger.Debug("Two args: {0} {1}", "arg1", "arg2");
        logger.Debug("Three args: {0} {1} {2}", "arg1", "arg2", "arg3");

        logger.Info("Simple message {0}", "arg1");
        logger.Info("Two args: {0} {1}", "arg1", "arg2");
        logger.Info("Three args: {0} {1} {2}", "arg1", "arg2", "arg3");

        logger.Warn("Simple message {0}", "arg1");
        logger.Warn("Two args: {0} {1}", "arg1", "arg2");
        logger.Warn("Three args: {0} {1} {2}", "arg1", "arg2", "arg3");

        logger.Error("Simple message {0}", "arg1");
        logger.Error("Two args: {0} {1}", "arg1", "arg2");
        logger.Error("Three args: {0} {1} {2}", "arg1", "arg2", "arg3");

        // Test higher-arity logging methods
        logger.Debug("Four args: {0} {1} {2} {3}", "arg1", "arg2", "arg3", "arg4");
        logger.Debug("Five args: {0} {1} {2} {3} {4}", "arg1", "arg2", "arg3", "arg4", "arg5");
    }

    /// <summary>
    /// Test that static logging host works with AOT.
    /// This tests the static logger functionality that doesn't rely on instance types.
    /// </summary>
    [Fact]
    public void StaticLoggingHost_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var locatorScope = resolver.WithResolver();

        // Act & Assert - Test static logging host (should not throw)
        LogHost.Default.Debug("Static debug message");
        LogHost.Default.Info("Static info message");
        LogHost.Default.Warn("Static warn message");
        LogHost.Default.Error("Static error message");
        LogHost.Default.Fatal("Static fatal message");

        // Test static logging with parameters
        LogHost.Default.Debug("Static debug with param: {0}", "test");
    }

    /// <summary>
    /// Test that WrappingFullLogger works correctly with AOT.
    /// This tests the logger that wraps ILogger to IFullLogger.
    /// </summary>
    [Fact]
    public void WrappingFullLogger_WorksWithAot()
    {
        // Arrange
        var baseLogger = new DebugLogger();
        var wrappingLogger = new WrappingFullLogger(baseLogger);

        // Act & Assert - Test all logging levels
        wrappingLogger.Debug("Debug message");
        wrappingLogger.Info("Info message");
        wrappingLogger.Warn("Warn message");
        wrappingLogger.Error("Error message");
        wrappingLogger.Fatal("Fatal message");

        // Test with generic types
        wrappingLogger.Debug<string>("Generic debug");
        wrappingLogger.Info<int>("Generic info");
        wrappingLogger.Warn<CoreAotCompatibilityTests>("Generic warn");

        // Test with exceptions
        var exception = new InvalidOperationException("Test exception");
        wrappingLogger.Debug(exception, "Debug with exception");
        wrappingLogger.Info(exception, "Info with exception");
        wrappingLogger.Error(exception, "Error with exception");

        // Test formatted logging
        wrappingLogger.Debug("Formatted message: {0}", "test");
        wrappingLogger.Info("Multiple params: {0} {1}", "test1", "test2");
    }

    /// <summary>
    /// Test that platform mode detector works with AOT including design mode detection.
    /// This tests platform-specific mode detection functionality that uses reflection.
    /// </summary>
    [Fact]
    public void PlatformModeDetector_WorksWithAot()
    {
        // Arrange & Act - Test different mode detectors
        var defaultDetector = new DefaultModeDetector();
        var platformDetector = new DefaultPlatformModeDetector();

        // Assert - Should not throw in AOT
        var inUnitTest1 = defaultDetector.InUnitTestRunner();
#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
        var inDesignMode = platformDetector.InDesignMode();
#pragma warning restore IL3050 // Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code

        Assert.NotNull(inUnitTest1);
        Assert.True(inUnitTest1.Value);

        // Test design mode detection (should not throw even though it uses reflection)
        Assert.NotNull(inDesignMode);
        Assert.False(inDesignMode.Value); // We're not in design mode during tests
    }

    /// <summary>
    /// Test that service registration callbacks with different parameters work with AOT.
    /// </summary>
    [Fact]
    public void ServiceRegistrationCallbacksWithContracts_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        var defaultCallbackTriggered = false;
        var contractCallbackTriggered = false;
        const string contract = "TestContract";

        // Act
        using var defaultSubscription = resolver.ServiceRegistrationCallback(
            typeof(ITestInterface),
            null,
            _ => defaultCallbackTriggered = true);

        using var contractSubscription = resolver.ServiceRegistrationCallback(
            typeof(ITestInterface),
            contract,
            _ => contractCallbackTriggered = true);

        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), contract);

        // Assert
        Assert.True(defaultCallbackTriggered);
        Assert.True(contractCallbackTriggered);
    }

    /// <summary>
    /// Test that service unregistration scenarios work with AOT.
    /// </summary>
    [Fact]
    public void ComprehensiveServiceUnregistration_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        const string contract = "TestContract";

        // Register multiple services
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), contract);
        resolver.Register<ITestInterface>(() => new TestImplementation(), "factory");

        // Verify initial registrations
        Assert.True(resolver.HasRegistration(typeof(ITestInterface)));
        Assert.True(resolver.HasRegistration(typeof(ITestInterface), contract));
        Assert.True(resolver.HasRegistration(typeof(ITestInterface), "factory"));

        // Act & Assert - Test UnregisterCurrent
        resolver.UnregisterCurrent<ITestInterface>();
        Assert.False(resolver.HasRegistration(typeof(ITestInterface)));
        Assert.True(resolver.HasRegistration(typeof(ITestInterface), contract)); // Should still exist

        // Test UnregisterCurrent with contract
        resolver.UnregisterCurrent<ITestInterface>(contract);
        Assert.False(resolver.HasRegistration(typeof(ITestInterface), contract));
        Assert.True(resolver.HasRegistration(typeof(ITestInterface), "factory")); // Should still exist

        // Test UnregisterAll
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.Register<ITestInterface>(() => new AlternateTestImplementation());
        Assert.True(resolver.HasRegistration(typeof(ITestInterface)));

        resolver.UnregisterAll<ITestInterface>();
        Assert.False(resolver.HasRegistration(typeof(ITestInterface)));
        Assert.True(resolver.HasRegistration(typeof(ITestInterface), "factory")); // Different contract should still exist
    }

    /// <summary>
    /// Test that fluent registration APIs work with AOT.
    /// This tests method chaining and ensures all fluent methods are AOT-compatible.
    /// </summary>
    [Fact]
    public void FluentRegistrationApis_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act - Test fluent registration chains
        resolver.RegisterAnd<ITestInterface>(() => new TestImplementation())
                .RegisterConstantAnd<ILogger>(new DebugLogger())
                .RegisterLazySingletonAnd<ILogManager>(() => new DefaultLogManager(resolver))
                .Register<IEnableLogger>(() => new TestService());

        // Assert
        Assert.True(resolver.HasRegistration(typeof(ITestInterface)));
        Assert.True(resolver.HasRegistration(typeof(ILogger)));
        Assert.True(resolver.HasRegistration(typeof(ILogManager)));
        Assert.True(resolver.HasRegistration(typeof(IEnableLogger)));

        var testInterface = resolver.GetService<ITestInterface>();
        var logger = resolver.GetService<ILogger>();
        var logManager = resolver.GetService<ILogManager>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        Assert.NotNull(testInterface);
        Assert.NotNull(logger);
        Assert.NotNull(logManager);
        Assert.NotNull(enableLogger);
    }

    /// <summary>
    /// Test that resolver scoping and isolation work with AOT.
    /// </summary>
    [Fact]
    public void ResolverScoping_WorksWithAot()
    {
        // Arrange
        using var originalResolver = new ModernDependencyResolver();
        using var scopedResolver = new ModernDependencyResolver();

        originalResolver.RegisterConstant<ILogger>(new DebugLogger());
        scopedResolver.RegisterConstant<ILogger>(new ConsoleLogger());

        // Act & Assert - Test resolver scoping
        using (var scope = scopedResolver.WithResolver())
        {
            var logger = Locator.Current.GetService<ILogger>();
            Assert.NotNull(logger);
            Assert.IsType<ConsoleLogger>(logger);
        }

        // Test that resolver reverts correctly (this may be DebugLogger from the default resolver or null)
        var revertedLogger = Locator.Current.GetService<ILogger>();

        // Don't assert specific type as it depends on the global state, just ensure it's different
        Assert.True(revertedLogger == null || revertedLogger is not ConsoleLogger);
    }

    /// <summary>
    /// Test that callback suppression works with AOT.
    /// </summary>
    [Fact]
    public void CallbackSuppression_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        var resolverCallbackTriggered = false;
        var serviceCallbackTriggered = false;

        // Register a callback for resolver changes (this should be suppressed)
        using var resolverSubscription = Locator.RegisterResolverCallbackChanged(() => resolverCallbackTriggered = true);

        // Clear the flag after registration since the callback is called immediately upon registration
        resolverCallbackTriggered = false;

        // Register a callback for service registration (this should NOT be suppressed)
        using var serviceSubscription = resolver.ServiceRegistrationCallback(
            typeof(ILogger),
            null,
            _ => serviceCallbackTriggered = true);

        // Act - Test with callback suppression (should only suppress resolver change callbacks)
        using (var scope = resolver.WithResolver(suppressResolverCallback: true))
        {
            // This should not trigger resolver change callbacks due to suppression
            // but service registration callbacks within the resolver should still work
            resolver.RegisterConstant<ILogger>(new DebugLogger());
        }

        // Assert - Service registration callback should be triggered, resolver callback should not
        Assert.True(serviceCallbackTriggered, "Service registration callbacks should not be affected by resolver callback suppression");
        Assert.False(resolverCallbackTriggered, "Resolver change callbacks should be suppressed");

        // Reset for next test
        resolverCallbackTriggered = false;
        serviceCallbackTriggered = false;

        // Test without suppression - both callbacks should trigger
        using (var scope = resolver.WithResolver(suppressResolverCallback: false))
        {
            resolver.RegisterConstant<ILogger>(new ConsoleLogger());
        }

        // Both should be triggered now since resolver callback suppression is disabled
        Assert.True(serviceCallbackTriggered, "Service registration callbacks should always work");
        Assert.True(resolverCallbackTriggered, "Resolver change callbacks should work when not suppressed");
    }

    /// <summary>
    /// Test that cross-casting and type conversions work with AOT.
    /// </summary>
    [Fact]
    public void TypeConversions_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act - Register services that implement multiple interfaces
        var service = new TestServiceWithMultipleInterfaces();
        resolver.RegisterConstant<ITestInterface>(service);
        resolver.RegisterConstant<IEnableLogger>(service);

        // Assert
        var testInterface = resolver.GetService<ITestInterface>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        Assert.NotNull(testInterface);
        Assert.NotNull(enableLogger);
        Assert.Same(service, testInterface);
        Assert.Same(service, enableLogger);
    }

    /// <summary>
    /// Test that complex nested generic scenarios work with AOT.
    /// </summary>
    [Fact]
    public void NestedGenericScenarios_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act - Test nested generic service registration
        resolver.Register<IEnumerable<ITestInterface>>(() => new List<ITestInterface>
        {
            new TestImplementation(),
            new AlternateTestImplementation()
        });

        resolver.Register<Func<ITestInterface>>(() => () => new TestImplementation());
        resolver.Register<Lazy<ILogger>>(() => new Lazy<ILogger>(() => new DebugLogger()));

        // Assert
        var enumerable = resolver.GetService<IEnumerable<ITestInterface>>();
        var factory = resolver.GetService<Func<ITestInterface>>();
        var lazy = resolver.GetService<Lazy<ILogger>>();

        Assert.NotNull(enumerable);
        Assert.NotNull(factory);
        Assert.NotNull(lazy);

        Assert.Equal(2, enumerable.Count());
        Assert.NotNull(factory());
        Assert.NotNull(lazy.Value);
        Assert.IsType<DebugLogger>(lazy.Value);
    }

    /// <summary>
    /// Test that concurrent access scenarios work with AOT.
    /// This tests thread safety in AOT compilation scenarios.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task ConcurrentAccess_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterLazySingleton<ILogger>(() => new DebugLogger());

        var exceptions = new List<Exception>();
        const int taskCount = 10;
        var tasks = new Task[taskCount];

        // Act - Test concurrent access
        for (var i = 0; i < taskCount; i++)
        {
            tasks[i] = Task.Run(() =>
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    for (var j = 0; j < 100; j++)
                    {
                        var logger = resolver.GetService<ILogger>();
                        Assert.NotNull(logger);

                        var services = resolver.GetServices<ILogger>();
                        Assert.NotNull(services);

                        var hasRegistration = resolver.HasRegistration(typeof(ILogger));
                        Assert.True(hasRegistration);
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
#pragma warning restore CA1031 // Do not catch general exception types
            });
        }

        await Task.WhenAll(tasks);

        // Assert - No exceptions should occur
        Assert.Empty(exceptions);
    }

    /// <summary>
    /// Test that null service type handling works with AOT.
    /// This tests edge cases with null type parameters.
    /// </summary>
    [Fact]
    public void NullServiceTypeHandling_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act & Assert - Test null service type handling
        var nullService = resolver.GetService(null);
        var nullServices = resolver.GetServices(null);

        // These should handle null gracefully
        Assert.Null(nullService);
        Assert.NotNull(nullServices);

        // Test HasRegistration with null
        var hasNullRegistration = resolver.HasRegistration(null);
        Assert.False(hasNullRegistration);
    }

    /// <summary>
    /// Test that dependency resolver extension methods work with AOT.
    /// This tests various extension methods that use generic type parameters.
    /// </summary>
    [Fact]
    public void DependencyResolverExtensions_WorksWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();

        // Act & Assert - Test extension methods with generic parameters
        Assert.False(resolver.HasRegistration(typeof(ITestInterface)));

        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        Assert.True(resolver.HasRegistration(typeof(ITestInterface)));

        var service = resolver.GetService<ITestInterface>();
        Assert.NotNull(service);
        Assert.IsType<TestImplementation>(service);

        var services = resolver.GetServices<ITestInterface>();
        Assert.NotNull(services);
        Assert.Single(services);
    }

    /// <summary>
    /// Test that dependency injection with parameterized constructors works with AOT.
    /// This tests that AOT trimming doesn't remove necessary constructor metadata.
    /// </summary>
    [Fact]
    public void ParameterizedConstructors_WorkWithAot()
    {
        // Arrange
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        // Act - Register service with parameterized constructor
        resolver.Register<ITestInterface>(() => new TestImplementationWithDependency(resolver.GetService<ILogger>()!));

        // Assert
        var service = resolver.GetService<ITestInterface>();
        Assert.NotNull(service);
        Assert.IsType<TestImplementationWithDependency>(service);

        var typedService = (TestImplementationWithDependency)service;
        Assert.NotNull(typedService.Logger);
        Assert.IsType<DebugLogger>(typedService.Logger);
    }

    /// <summary>
    /// Test that service disposal works correctly with AOT.
    /// This tests that the ModernDependencyResolver properly disposes registered services.
    /// </summary>
    [Fact]
    public void ServiceDisposal_WorksWithAot()
    {
        // Arrange
        var disposableService = new DisposableTestService();
        var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ITestInterface>(disposableService);

        // Act
        resolver.Dispose();

        // Assert - Service should have been disposed
        Assert.True(disposableService.IsDisposed);
    }

    /// <summary>
    /// Test implementation for dependency injection testing.
    /// </summary>
    private sealed class TestImplementation : ITestInterface
    {
        /// <inheritdoc/>
        public string GetValue() => "Test Implementation";
    }

    /// <summary>
    /// Alternate test implementation for multiple service testing.
    /// </summary>
    private sealed class AlternateTestImplementation : ITestInterface
    {
        /// <inheritdoc/>
        public string GetValue() => "Alternate Test Implementation";
    }

    /// <summary>
    /// Test implementation with dependency injection.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="TestImplementationWithDependency"/> class.
    /// </remarks>
    /// <param name="logger">The logger dependency.</param>
    private sealed class TestImplementationWithDependency(ILogger logger) : ITestInterface
    {
        /// <summary>
        /// Gets the injected logger.
        /// </summary>
        public ILogger Logger { get; } = logger;

        /// <inheritdoc/>
        public string GetValue() => "Test Implementation With Dependency";
    }

    /// <summary>
    /// Test service that implements IDisposable.
    /// </summary>
    private sealed class DisposableTestService : ITestInterface, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether this instance has been disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public string GetValue() => "Disposable Test Service";

        /// <inheritdoc/>
        public void Dispose() => IsDisposed = true;
    }

    /// <summary>
    /// Simple test service that implements IEnableLogger for testing purposes.
    /// </summary>
    private sealed class TestService : IEnableLogger
    {
        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        public IFullLogger? Logger { get; set; }
    }

    /// <summary>
    /// Test service that implements multiple interfaces for cross-casting tests.
    /// </summary>
    private sealed class TestServiceWithMultipleInterfaces : ITestInterface, IEnableLogger
    {
        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        public IFullLogger? Logger { get; set; }

        /// <inheritdoc/>
        public string GetValue() => "Multi-Interface Service";
    }
}
