// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

namespace Splat.Tests.Aot;

/// <summary>
/// Comprehensive tests to verify AOT and trimming compatibility of the core Splat library.
/// These tests ensure that the library works correctly when used with Native AOT
/// and when assemblies are trimmed by the linker.
/// </summary>
[TestFixture]
[NonParallelizable] // Uses Locator/AppLocator and resolver scopes; keep serialized to avoid cross-fixture interference.
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
    [Test]
    public void BasicServiceRegistration_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        // Register services using factory methods (AOT-safe)
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        resolver.Register<IEnableLogger>(() => new TestService());

        // Verify services can be resolved
        var logger = resolver.GetService<ILogger>();
        var logManager = resolver.GetService<ILogManager>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logger, Is.Not.Null);
            Assert.That(logManager, Is.Not.Null);
            Assert.That(enableLogger, Is.Not.Null);
            Assert.That(logger, Is.TypeOf<DebugLogger>());
            Assert.That(logManager, Is.TypeOf<DefaultLogManager>());
            Assert.That(enableLogger, Is.TypeOf<TestService>());
        }
    }

    /// <summary>
    /// Test that all registration mixins work with AOT.
    /// </summary>
    [Test]
    public void RegistrationMixins_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        // Test all registration methods
        resolver.Register<ITestInterface>(() => new TestImplementation());
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterLazySingleton<ILogManager>(() => new DefaultLogManager(resolver));

        // Fluent registration
        resolver.RegisterAnd<IEnableLogger>(() => new TestService())
                .RegisterConstantAnd<ITestInterface>(new TestImplementation())
                .RegisterLazySingletonAnd<ILogger>(() => new ConsoleLogger());

        // All services should resolve
        var testInterface = resolver.GetService<ITestInterface>();
        var logger = resolver.GetService<ILogger>();
        var logManager = resolver.GetService<ILogManager>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(testInterface, Is.Not.Null);
            Assert.That(logger, Is.Not.Null);
            Assert.That(logManager, Is.Not.Null);
            Assert.That(enableLogger, Is.Not.Null);
        }
    }

    /// <summary>
    /// Test that service callbacks work with AOT.
    /// </summary>
    [Test]
    public void ServiceRegistrationCallbacks_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        var callbackTriggered = false;

        using var subscription = resolver.ServiceRegistrationCallback(
            typeof(ILogger),
            null,
            _ => callbackTriggered = true);

        resolver.RegisterConstant<ILogger>(new DebugLogger());

        Assert.That(callbackTriggered, Is.True);
    }

    /// <summary>
    /// Test that logging functionality works with AOT.
    /// </summary>
    [Test]
    public void Logging_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var resolveInner = resolver.WithResolver();

        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();
        Assert.That(logger, Is.Not.Null);

        // Logging methods shouldn't throw in AOT
        Assert.DoesNotThrow(() =>
        {
            logger!.Debug("Test debug message");
            logger.Info("Test info message");
            logger.Warn("Test warning message");
            logger.Error("Test error message");
            logger.Fatal("Test fatal message");
        });
    }

    /// <summary>
    /// Test that mode detection works with AOT.
    /// </summary>
    [Test]
    public void ModeDetection_WorksWithAot()
    {
        var modeDetector = new DefaultModeDetector();

        var inUnitTest = modeDetector.InUnitTestRunner();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(inUnitTest, Is.Not.Null);
            Assert.That(inUnitTest!.Value, Is.True); // We're running in a unit test
        }
    }

    /// <summary>
    /// Test that lazy singleton registration works with AOT.
    /// </summary>
    [Test]
    public void LazySingleton_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        var creationCount = 0;

        resolver.RegisterLazySingleton<IEnableLogger>(() =>
        {
            creationCount++;
            return new TestService();
        });

        var service1 = resolver.GetService<IEnableLogger>();
        var service2 = resolver.GetService<IEnableLogger>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(service1, Is.Not.Null);
            Assert.That(service2, Is.Not.Null);
            Assert.That(service1, Is.SameAs(service2)); // Should be the same instance
            Assert.That(creationCount, Is.EqualTo(1));  // Created once
        }
    }

    /// <summary>
    /// Test that unregistering services works with AOT.
    /// </summary>
    [Test]
    public void ServiceUnregistration_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        var logger1 = resolver.GetService<ILogger>();
        Assert.That(logger1, Is.Not.Null);

        resolver.UnregisterCurrent<ILogger>();
        var logger2 = resolver.GetService<ILogger>();

        Assert.That(logger2, Is.Null);
    }

    /// <summary>
    /// Test that target framework extensions work with AOT.
    /// </summary>
    [Test]
    public void TargetFrameworkExtensions_WorksWithAot()
    {
        var assembly = typeof(CoreAotCompatibilityTests).Assembly;

        // This uses reflection but should be AOT-safe with proper attributes
        var targetFramework = assembly.GetTargetFrameworkName();

        if (targetFramework is not null)
        {
            Assert.That(targetFramework, Does.Contain("net"));
        }
    }

    /// <summary>
    /// Test that multiple service registration works with AOT.
    /// </summary>
    [Test]
    public void MultipleServiceRegistration_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        resolver.Register<ITestInterface>(() => new TestImplementation());
        resolver.Register<ITestInterface>(() => new AlternateTestImplementation());

        var services = resolver.GetServices<ITestInterface>().ToList();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(services, Has.Count.EqualTo(2));
            Assert.That(services, Has.Some.InstanceOf<TestImplementation>());
            Assert.That(services, Has.Some.InstanceOf<AlternateTestImplementation>());
        }
    }

    /// <summary>
    /// Test that locator static methods work with AOT.
    /// </summary>
    [Test]
    public void LocatorStatic_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        using var resolveInner = resolver.WithResolver();
        var logger = AppLocator.Current.GetService<ILogger>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logger, Is.Not.Null);
            Assert.That(logger, Is.TypeOf<DebugLogger>());
        }
    }

    /// <summary>
    /// Test that generic type registration with constraints works with AOT.
    /// This tests DynamicallyAccessedMembers annotations on generic type parameters.
    /// </summary>
    [Test]
    public void GenericTypeRegistrationWithConstraints_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        // Register types with new() constraint
        resolver.Register<ITestInterface, TestImplementation>();
        resolver.Register<IEnableLogger, TestService>();

        var testInterface = resolver.GetService<ITestInterface>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(testInterface, Is.Not.Null.And.TypeOf<TestImplementation>());
            Assert.That(enableLogger, Is.Not.Null.And.TypeOf<TestService>());
        }
    }

    /// <summary>
    /// Test that service registrations with contracts work with AOT.
    /// </summary>
    [Test]
    public void ContractBasedRegistration_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        const string contract1 = "Contract1";
        const string contract2 = "Contract2";

        resolver.RegisterConstant<ITestInterface>(new TestImplementation(), contract1);
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), contract2);
        resolver.RegisterConstant<ITestInterface>(new TestImplementation()); // Default

        var service1 = resolver.GetService<ITestInterface>(contract1);
        var service2 = resolver.GetService<ITestInterface>(contract2);
        var defaultService = resolver.GetService<ITestInterface>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(service1, Is.Not.Null.And.TypeOf<TestImplementation>());
            Assert.That(service2, Is.Not.Null.And.TypeOf<AlternateTestImplementation>());
            Assert.That(defaultService, Is.Not.Null.And.TypeOf<TestImplementation>());
        }
    }

    /// <summary>
    /// Test that type-based service queries work with AOT.
    /// </summary>
    [Test]
    public void TypeBasedServiceQueries_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

#pragma warning disable CA2263 // Prefer generic overload when type is known
        resolver.RegisterConstant(new DebugLogger(), typeof(ILogger));
        resolver.RegisterConstant(new DefaultLogManager(resolver), typeof(ILogManager));
#pragma warning restore CA2263 // Prefer generic overload when type is known

        var logger = resolver.GetService(typeof(ILogger));
        var logManager = resolver.GetService(typeof(ILogManager));
        var services = resolver.GetServices(typeof(ILogger)).ToList();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(logger, Is.Not.Null.And.TypeOf<DebugLogger>());
            Assert.That(logManager, Is.Not.Null.And.TypeOf<DefaultLogManager>());
            Assert.That(services, Has.Count.EqualTo(1));
        }
    }

    /// <summary>
    /// Test that HasRegistration queries work with AOT.
    /// </summary>
    [Test]
    public void HasRegistrationQueries_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        // Before registration
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.False);
            Assert.That(resolver.HasRegistration(typeof(ITestInterface), "contract"), Is.False);
        }

        // After registration
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), "contract");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.True);
            Assert.That(resolver.HasRegistration(typeof(ITestInterface), "contract"), Is.True);
        }
    }

    /// <summary>
    /// Test that comprehensive logging functionality works with AOT.
    /// This tests generic logging methods with type parameters.
    /// </summary>
    [Test]
    public void ComprehensiveLogging_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var resolverInner = resolver.WithResolver();
        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();
        Assert.That(logger, Is.Not.Null);

        // Generic logging methods + culture + exceptions shouldn't throw
        Assert.DoesNotThrow(() =>
        {
            logger!.Debug<string>("Debug with generic type");
            logger.Info<int>("Info with generic type");
            logger.Warn<CoreAotCompatibilityTests>("Warn with generic type");
            logger.Error<ITestInterface>("Error with generic type");
            logger.Fatal<TestImplementation>("Fatal with generic type");

            logger.Debug(CultureInfo.InvariantCulture, "Debug with culture: {0}", "test");
            logger.Info(CultureInfo.InvariantCulture, "Info with culture: {0}", "test");
            logger.Warn(CultureInfo.InvariantCulture, "Warn with culture: {0}", "test");
            logger.Error(CultureInfo.InvariantCulture, "Error with culture: {0}", "test");
            logger.Fatal(CultureInfo.InvariantCulture, "Fatal with culture: {0}", "test");

            var testException = new InvalidOperationException("Test exception");
            logger.Debug(testException, "Debug with exception");
            logger.Info(testException, "Info with exception");
            logger.Warn(testException, "Warn with exception");
            logger.Error(testException, "Error with exception");
            logger.Fatal(testException, "Fatal with exception");
        });
    }

    /// <summary>
    /// Test that allocation-free logging methods work with AOT.
    /// This ensures that high-performance logging scenarios are AOT-compatible.
    /// </summary>
    [Test]
    public void AllocationFreeLogging_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var resolverInner = resolver.WithResolver();
        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();
        Assert.That(logger, Is.Not.Null);

        Assert.DoesNotThrow(() =>
        {
            logger!.Debug("Simple message {0}", "arg1");
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

            // Higher-arity
            logger.Debug("Four args: {0} {1} {2} {3}", "arg1", "arg2", "arg3", "arg4");
            logger.Debug("Five args: {0} {1} {2} {3} {4}", "arg1", "arg2", "arg3", "arg4", "arg5");
        });
    }

    /// <summary>
    /// Test that static logging host works with AOT.
    /// </summary>
    [Test]
    public void StaticLoggingHost_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var resolvedInner = resolver.WithResolver();

        Assert.DoesNotThrow(() =>
        {
            LogHost.Default.Debug("Static debug message");
            LogHost.Default.Info("Static info message");
            LogHost.Default.Warn("Static warn message");
            LogHost.Default.Error("Static error message");
            LogHost.Default.Fatal("Static fatal message");
            LogHost.Default.Debug("Static debug with param: {0}", "test");
        });
    }

    /// <summary>
    /// Test that WrappingFullLogger works correctly with AOT.
    /// </summary>
    [Test]
    public void WrappingFullLogger_WorksWithAot()
    {
        var baseLogger = new DebugLogger();
        var wrappingLogger = new WrappingFullLogger(baseLogger);

        Assert.DoesNotThrow(() =>
        {
            wrappingLogger.Debug("Debug message");
            wrappingLogger.Info("Info message");
            wrappingLogger.Warn("Warn message");
            wrappingLogger.Error("Error message");
            wrappingLogger.Fatal("Fatal message");

            // Generic types
            wrappingLogger.Debug<string>("Generic debug");
            wrappingLogger.Info<int>("Generic info");
            wrappingLogger.Warn<CoreAotCompatibilityTests>("Generic warn");

            // Exceptions
            var exception = new InvalidOperationException("Test exception");
            wrappingLogger.Debug(exception, "Debug with exception");
            wrappingLogger.Info(exception, "Info with exception");
            wrappingLogger.Error(exception, "Error with exception");

            // Formatting
            wrappingLogger.Debug("Formatted message: {0}", "test");
            wrappingLogger.Info("Multiple params: {0} {1}", "test1", "test2");
        });
    }

    /// <summary>
    /// Test that platform mode detector works with AOT including design mode detection.
    /// </summary>
    [Test]
    public void PlatformModeDetector_WorksWithAot()
    {
        var defaultDetector = new DefaultModeDetector();
        var platformDetector = new DefaultPlatformModeDetector();

        var inUnitTest1 = defaultDetector.InUnitTestRunner();
#pragma warning disable IL2026 // RequiresUnreferencedCode
#pragma warning disable IL3050 // RequiresDynamicCode
        var inDesignMode = platformDetector.InDesignMode();
#pragma warning restore IL3050
#pragma warning restore IL2026

        using (Assert.EnterMultipleScope())
        {
            Assert.That(inUnitTest1, Is.Not.Null);
            Assert.That(inUnitTest1!.Value, Is.True);

            // We're not in design mode during tests
            Assert.That(inDesignMode, Is.Not.Null);
            Assert.That(inDesignMode!.Value, Is.False);
        }
    }

    /// <summary>
    /// Test that service registration callbacks with different parameters work with AOT.
    /// </summary>
    [Test]
    public void ServiceRegistrationCallbacksWithContracts_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        var defaultCallbackTriggered = false;
        var contractCallbackTriggered = false;
        const string contract = "TestContract";

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

        using (Assert.EnterMultipleScope())
        {
            Assert.That(defaultCallbackTriggered, Is.True);
            Assert.That(contractCallbackTriggered, Is.True);
        }
    }

    /// <summary>
    /// Test that service unregistration scenarios work with AOT.
    /// </summary>
    [Test]
    public void ComprehensiveServiceUnregistration_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        const string contract = "TestContract";

        // Register multiple services
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), contract);
        resolver.Register<ITestInterface>(() => new TestImplementation(), "factory");

        // Verify initial registrations
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.True);
            Assert.That(resolver.HasRegistration(typeof(ITestInterface), contract), Is.True);
            Assert.That(resolver.HasRegistration(typeof(ITestInterface), "factory"), Is.True);
        }

        // Unregister current (default contract)
        resolver.UnregisterCurrent<ITestInterface>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.False);
            Assert.That(resolver.HasRegistration(typeof(ITestInterface), contract), Is.True);
        }

        // Unregister current with contract
        resolver.UnregisterCurrent<ITestInterface>(contract);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(typeof(ITestInterface), contract), Is.False);
            Assert.That(resolver.HasRegistration(typeof(ITestInterface), "factory"), Is.True);
        }

        // Unregister all (default contract only)
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.Register<ITestInterface>(() => new AlternateTestImplementation());
        Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.True);

        resolver.UnregisterAll<ITestInterface>();
        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.False);
            Assert.That(resolver.HasRegistration(typeof(ITestInterface), "factory"), Is.True);
        }
    }

    /// <summary>
    /// Test that fluent registration APIs work with AOT.
    /// </summary>
    [Test]
    public void FluentRegistrationApis_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        resolver.RegisterAnd<ITestInterface>(() => new TestImplementation())
                .RegisterConstantAnd<ILogger>(new DebugLogger())
                .RegisterLazySingletonAnd<ILogManager>(() => new DefaultLogManager(resolver))
                .Register<IEnableLogger>(() => new TestService());

        using (Assert.EnterMultipleScope())
        {
            Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.True);
            Assert.That(resolver.HasRegistration(typeof(ILogger)), Is.True);
            Assert.That(resolver.HasRegistration(typeof(ILogManager)), Is.True);
            Assert.That(resolver.HasRegistration(typeof(IEnableLogger)), Is.True);
        }

        var testInterface = resolver.GetService<ITestInterface>();
        var logger = resolver.GetService<ILogger>();
        var logManager = resolver.GetService<ILogManager>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(testInterface, Is.Not.Null);
            Assert.That(logger, Is.Not.Null);
            Assert.That(logManager, Is.Not.Null);
            Assert.That(enableLogger, Is.Not.Null);
        }
    }

    /// <summary>
    /// Test that resolver scoping and isolation work with AOT.
    /// </summary>
    [Test]
    public void ResolverScoping_WorksWithAot()
    {
        using var originalResolver = new ModernDependencyResolver();
        using var scopedResolver = new ModernDependencyResolver();

        originalResolver.RegisterConstant<ILogger>(new DebugLogger());
        scopedResolver.RegisterConstant<ILogger>(new ConsoleLogger());

        using (scopedResolver.WithResolver())
        {
            var logger = Locator.Current.GetService<ILogger>();
            using (Assert.EnterMultipleScope())
            {
                Assert.That(logger, Is.Not.Null);
                Assert.That(logger, Is.TypeOf<ConsoleLogger>());
            }
        }

        // After reverting scope, ensure it's not the scoped ConsoleLogger
        var revertedLogger = Locator.Current.GetService<ILogger>();
        Assert.That(revertedLogger, Is.Null.Or.Not.InstanceOf<ConsoleLogger>());
    }

    /// <summary>
    /// Test that callback suppression works with AOT.
    /// </summary>
    [Test]
    public void CallbackSuppression_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        var resolverCallbackTriggered = false;
        var serviceCallbackTriggered = false;

        // Register resolver change callback (called immediately once)
        using var resolverSubscription = Locator.RegisterResolverCallbackChanged(() => resolverCallbackTriggered = true);
        resolverCallbackTriggered = false; // clear immediate-call effect

        // Register service registration callback
        using var serviceSubscription = resolver.ServiceRegistrationCallback(
            typeof(ILogger),
            null,
            _ => serviceCallbackTriggered = true);

        // Suppressed resolver callback
        using (resolver.WithResolver(suppressResolverCallback: true))
        {
            resolver.RegisterConstant<ILogger>(new DebugLogger());
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(serviceCallbackTriggered, Is.True, "Service registration callback should not be suppressed.");
            Assert.That(resolverCallbackTriggered, Is.False, "Resolver change callbacks should be suppressed.");
        }

        // Reset
        resolverCallbackTriggered = false;
        serviceCallbackTriggered = false;

        // Without suppression
        using (resolver.WithResolver(suppressResolverCallback: false))
        {
            resolver.RegisterConstant<ILogger>(new ConsoleLogger());
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(serviceCallbackTriggered, Is.True);
            Assert.That(resolverCallbackTriggered, Is.True);
        }
    }

    /// <summary>
    /// Test that cross-casting and type conversions work with AOT.
    /// </summary>
    [Test]
    public void TypeConversions_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        var service = new TestServiceWithMultipleInterfaces();
        resolver.RegisterConstant<ITestInterface>(service);
        resolver.RegisterConstant<IEnableLogger>(service);

        var testInterface = resolver.GetService<ITestInterface>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(testInterface, Is.Not.Null);
            Assert.That(enableLogger, Is.Not.Null);
            Assert.That(testInterface, Is.SameAs(service));
            Assert.That(enableLogger, Is.SameAs(service));
        }
    }

    /// <summary>
    /// Test that complex nested generic scenarios work with AOT.
    /// </summary>
    [Test]
    public void NestedGenericScenarios_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        resolver.Register<IEnumerable<ITestInterface>>(() => new List<ITestInterface>
        {
            new TestImplementation(),
            new AlternateTestImplementation()
        });

        resolver.Register<Func<ITestInterface>>(() => () => new TestImplementation());
        resolver.Register(() => new Lazy<ILogger>(() => new DebugLogger()));

        var enumerable = resolver.GetService<IEnumerable<ITestInterface>>();
        var factory = resolver.GetService<Func<ITestInterface>>();
        var lazy = resolver.GetService<Lazy<ILogger>>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(enumerable, Is.Not.Null);
            Assert.That(factory, Is.Not.Null);
            Assert.That(lazy, Is.Not.Null);
            Assert.That(enumerable!.Count(), Is.EqualTo(2));
            Assert.That(factory!(), Is.Not.Null);
            Assert.That(lazy!.Value, Is.Not.Null.And.TypeOf<DebugLogger>());
        }
    }

    /// <summary>
    /// Tests concurrent access scenarios with AOT compatibility.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Test]
    public async Task ConcurrentAccess_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterLazySingleton<ILogger>(() => new DebugLogger());

        var exceptions = new List<Exception>();
        const int taskCount = 10;
        var tasks = new Task[taskCount];

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
                        Assert.That(logger, Is.Not.Null);

                        var services = resolver.GetServices<ILogger>();
                        Assert.That(services, Is.Not.Null);

                        var hasRegistration = resolver.HasRegistration(typeof(ILogger));
                        Assert.That(hasRegistration, Is.True);
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
#pragma warning restore CA1031
            });
        }

        await Task.WhenAll(tasks);

        Assert.That(exceptions, Is.Empty);
    }

    /// <summary>
    /// Test that null service type handling works with AOT.
    /// </summary>
    [Test]
    public void NullServiceTypeHandling_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        var nullService = resolver.GetService(null);
        var nullServices = resolver.GetServices(null);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(nullService, Is.Null);
            Assert.That(nullServices, Is.Not.Null);
            Assert.That(resolver.HasRegistration(null), Is.False);
        }
    }

    /// <summary>
    /// Test that dependency resolver extension methods work with AOT.
    /// </summary>
    [Test]
    public void DependencyResolverExtensions_WorksWithAot()
    {
        using var resolver = new ModernDependencyResolver();

        Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.False);

        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        Assert.That(resolver.HasRegistration(typeof(ITestInterface)), Is.True);

        var service = resolver.GetService<ITestInterface>();
        var services = resolver.GetServices<ITestInterface>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(service, Is.Not.Null.And.TypeOf<TestImplementation>());
            Assert.That(services, Is.Not.Null);
            Assert.That(services!.Count(), Is.EqualTo(1));
        }
    }

    /// <summary>
    /// Test that dependency injection with parameterized constructors works with AOT.
    /// </summary>
    [Test]
    public void ParameterizedConstructors_WorkWithAot()
    {
        using var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        resolver.Register<ITestInterface>(() => new TestImplementationWithDependency(resolver.GetService<ILogger>()!));

        var service = resolver.GetService<ITestInterface>();

        using (Assert.EnterMultipleScope())
        {
            Assert.That(service, Is.Not.Null.And.TypeOf<TestImplementationWithDependency>());
            var typed = (TestImplementationWithDependency)service!;
            Assert.That(typed.Logger, Is.Not.Null.And.TypeOf<DebugLogger>());
        }
    }

    /// <summary>
    /// Test that service disposal works correctly with AOT.
    /// </summary>
    [Test]
    public void ServiceDisposal_WorksWithAot()
    {
        var disposableService = new DisposableTestService();
        var resolver = new ModernDependencyResolver();
        resolver.RegisterConstant<ITestInterface>(disposableService);

        resolver.Dispose();

        Assert.That(disposableService.IsDisposed, Is.True);
    }

    private sealed class TestImplementation : ITestInterface
    {
        public string GetValue() => "Test Implementation";
    }

    private sealed class AlternateTestImplementation : ITestInterface
    {
        public string GetValue() => "Alternate Test Implementation";
    }

    private sealed class TestImplementationWithDependency(ILogger logger) : ITestInterface
    {
        /// <summary>
        /// Gets the injected logger.
        /// </summary>
        public ILogger Logger { get; } = logger;

        public string GetValue() => "Test Implementation With Dependency";
    }

    private sealed class DisposableTestService : ITestInterface, IDisposable
    {
        public bool IsDisposed { get; private set; }

        public string GetValue() => "Disposable Test Service";

        public void Dispose() => IsDisposed = true;
    }

    /// <summary>
    /// Simple test service that implements IEnableLogger for testing purposes.
    /// </summary>
    private sealed class TestService : IEnableLogger
    {
        public IFullLogger? Logger { get; set; }
    }

    /// <summary>
    /// Test service that implements multiple interfaces for cross-casting tests.
    /// </summary>
    private sealed class TestServiceWithMultipleInterfaces : ITestInterface, IEnableLogger
    {
        public IFullLogger? Logger { get; set; }

        public string GetValue() => "Multi-Interface Service";
    }
}
