// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Globalization;

using Splat.Common.Test;

namespace Splat.Tests.Aot;

/// <summary>
/// AOT compatibility tests for core Splat functionality.
/// </summary>
[NotInParallel] // Uses Locator/AppLocator and resolver scopes; keep serialized to avoid cross-fixture interference.
[System.Diagnostics.CodeAnalysis.SuppressMessage("Trimming", "IL2026:Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code", Justification = "Testing Purposes")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2263:Prefer generic overload when type is known", Justification = "Testing purposes")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("AOT", "IL3050:Calling members annotated with 'RequiresDynamicCodeAttribute' may break functionality when AOT compiling.", Justification = "Testing Purposes")]
public class CoreAotCompatibilityTests
{
    private AppLocatorScope? _appLocatorScope;

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
    /// Setup method to initialize AppLocatorScope before each test.
    /// </summary>
    [Before(HookType.Test)]
    public void SetUpAppLocatorScope() => _appLocatorScope = new();

    /// <summary>
    /// Teardown method to dispose AppLocatorScope after each test.
    /// </summary>
    [After(HookType.Test)]
    public void TearDownAppLocatorScope()
    {
        _appLocatorScope?.Dispose();
        _appLocatorScope = null;
    }

    /// <summary>
    /// Test that basic service registration and resolution works in AOT scenarios.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task BasicServiceRegistration_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        // Register services using factory methods (AOT-safe)
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        resolver.Register<IEnableLogger>(() => new TestService());

        // Verify services can be resolved
        var logger = resolver.GetService<ILogger>();
        var logManager = resolver.GetService<ILogManager>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.Multiple())
        {
            await Assert.That(logger).IsNotNull();
            await Assert.That(logManager).IsNotNull();
            await Assert.That(enableLogger).IsNotNull();
            await Assert.That(logger).IsTypeOf<DebugLogger>();
            await Assert.That(logManager).IsTypeOf<DefaultLogManager>();
            await Assert.That(enableLogger).IsTypeOf<TestService>();
        }
    }

    /// <summary>
    /// Test that all registration mixins work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegistrationMixins_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

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

        using (Assert.Multiple())
        {
            await Assert.That(testInterface).IsNotNull();
            await Assert.That(logger).IsNotNull();
            await Assert.That(logManager).IsNotNull();
            await Assert.That(enableLogger).IsNotNull();
        }
    }

    /// <summary>
    /// Test that service callbacks work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallbacks_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        var callbackTriggered = false;

        using var subscription = resolver.ServiceRegistrationCallback(
            typeof(ILogger),
            null,
            _ => callbackTriggered = true);

        resolver.RegisterConstant<ILogger>(new DebugLogger());

        await Assert.That(callbackTriggered).IsTrue();
    }

    /// <summary>
    /// Test that logging functionality works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Logging_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var resolveInner = resolver.WithResolver();

        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();
        await Assert.That(logger).IsNotNull();

        // Logging methods shouldn't throw in AOT
        await Assert.That(() =>
        {
            logger!.Debug("Test debug message");
            logger.Info("Test info message");
            logger.Warn("Test warning message");
            logger.Error("Test error message");
            logger.Fatal("Test fatal message");
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test that mode detection works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ModeDetection_WorksWithAot()
    {
        var modeDetector = new DefaultModeDetector();

        var inUnitTest = modeDetector.InUnitTestRunner();
        using (Assert.Multiple())
        {
            await Assert.That(inUnitTest).IsNotNull();
            await Assert.That(inUnitTest!.Value).IsTrue(); // We're running in a unit test
        }
    }

    /// <summary>
    /// Test that lazy singleton registration works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task LazySingleton_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        var creationCount = 0;

        resolver.RegisterLazySingleton<IEnableLogger>(() =>
        {
            creationCount++;
            return new TestService();
        });

        var service1 = resolver.GetService<IEnableLogger>();
        var service2 = resolver.GetService<IEnableLogger>();

        using (Assert.Multiple())
        {
            await Assert.That(service1).IsNotNull();
            await Assert.That(service2).IsNotNull();
            await Assert.That(service1).IsSameReferenceAs(service2); // Should be the same instance
            await Assert.That(creationCount).IsEqualTo(1);  // Created once
        }
    }

    /// <summary>
    /// Test that unregistering services works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceUnregistration_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        var logger1 = resolver.GetService<ILogger>();
        await Assert.That(logger1).IsNotNull();

        resolver.UnregisterCurrent<ILogger>();
        var logger2 = resolver.GetService<ILogger>();

        await Assert.That(logger2).IsNull();
    }

    /// <summary>
    /// Test that target framework extensions work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TargetFrameworkExtensions_WorksWithAot()
    {
        var assembly = typeof(CoreAotCompatibilityTests).Assembly;

        // This uses reflection but should be AOT-safe with proper attributes
        var targetFramework = assembly.GetTargetFrameworkName();

        if (targetFramework is not null)
        {
            await Assert.That(targetFramework).Contains("net");
        }
    }

    /// <summary>
    /// Test that multiple service registration works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MultipleServiceRegistration_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        resolver.Register<ITestInterface>(() => new TestImplementation());
        resolver.Register<ITestInterface>(() => new AlternateTestImplementation());

        var services = resolver.GetServices<ITestInterface>().ToList();

        using (Assert.Multiple())
        {
            await Assert.That(services).Count().IsEqualTo(2);
            await Assert.That(services).Any(s => s is TestImplementation);
            await Assert.That(services).Any(s => s is AlternateTestImplementation);
        }
    }

    /// <summary>
    /// Test that locator static methods work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task LocatorStatic_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        using var resolveInner = resolver.WithResolver();
        var logger = AppLocator.Current.GetService<ILogger>();

        using (Assert.Multiple())
        {
            await Assert.That(logger).IsNotNull();
            await Assert.That(logger).IsTypeOf<DebugLogger>();
        }
    }

    /// <summary>
    /// Test that generic type registration with constraints works with AOT.
    /// This tests DynamicallyAccessedMembers annotations on generic type parameters.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GenericTypeRegistrationWithConstraints_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        // Register types with new() constraint
        resolver.Register<ITestInterface, TestImplementation>();
        resolver.Register<IEnableLogger, TestService>();

        var testInterface = resolver.GetService<ITestInterface>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.Multiple())
        {
            await Assert.That(testInterface).IsTypeOf<TestImplementation>();
            await Assert.That(enableLogger).IsTypeOf<TestService>();
        }
    }

    /// <summary>
    /// Test that service registrations with contracts work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ContractBasedRegistration_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        const string contract1 = "Contract1";
        const string contract2 = "Contract2";

        resolver.RegisterConstant<ITestInterface>(new TestImplementation(), contract1);
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), contract2);
        resolver.RegisterConstant<ITestInterface>(new TestImplementation()); // Default

        var service1 = resolver.GetService<ITestInterface>(contract1);
        var service2 = resolver.GetService<ITestInterface>(contract2);
        var defaultService = resolver.GetService<ITestInterface>();

        using (Assert.Multiple())
        {
            await Assert.That(service1).IsTypeOf<TestImplementation>();
            await Assert.That(service2).IsTypeOf<AlternateTestImplementation>();
            await Assert.That(defaultService).IsTypeOf<TestImplementation>();
        }
    }

    /// <summary>
    /// Test that type-based service queries work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TypeBasedServiceQueries_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant(new DebugLogger(), typeof(ILogger));
        resolver.RegisterConstant(new DefaultLogManager(resolver), typeof(ILogManager));
        var logger = resolver.GetService(typeof(ILogger));
        var logManager = resolver.GetService(typeof(ILogManager));
        var services = resolver.GetServices(typeof(ILogger)).ToList();

        using (Assert.Multiple())
        {
            await Assert.That(logger).IsTypeOf<DebugLogger>();
            await Assert.That(logManager).IsTypeOf<DefaultLogManager>();
            await Assert.That(services).Count().IsEqualTo(1);
        }
    }

    /// <summary>
    /// Test that HasRegistration queries work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistrationQueries_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        // Before registration
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsFalse();
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface), "contract")).IsFalse();
        }

        // After registration
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), "contract");

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsTrue();
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface), "contract")).IsTrue();
        }
    }

    /// <summary>
    /// Test that comprehensive logging functionality works with AOT.
    /// This tests generic logging methods with type parameters.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ComprehensiveLogging_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var resolverInner = resolver.WithResolver();
        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();
        await Assert.That(logger).IsNotNull();

        // Generic logging methods + culture + exceptions shouldn't throw
        await Assert.That(() =>
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
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test that allocation-free logging methods work with AOT.
    /// This ensures that high-performance logging scenarios are AOT-compatible.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task AllocationFreeLogging_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var resolverInner = resolver.WithResolver();
        var logger = resolver.GetService<ILogManager>()?.GetLogger<CoreAotCompatibilityTests>();
        await Assert.That(logger).IsNotNull();

        await Assert.That(() =>
        {
            logger!.Debug(CultureInfo.InvariantCulture, "Simple message {0}", "arg1");
            logger.Debug(CultureInfo.InvariantCulture, "Two args: {0} {1}", "arg1", "arg2");
            logger.Debug(CultureInfo.InvariantCulture, "Three args: {0} {1} {2}", "arg1", "arg2", "arg3");

            logger.Info(CultureInfo.InvariantCulture, "Simple message {0}", "arg1");
            logger.Info(CultureInfo.InvariantCulture, "Two args: {0} {1}", "arg1", "arg2");
            logger.Info(CultureInfo.InvariantCulture, "Three args: {0} {1} {2}", "arg1", "arg2", "arg3");

            logger.Warn(CultureInfo.InvariantCulture, "Simple message {0}", "arg1");
            logger.Warn(CultureInfo.InvariantCulture, "Two args: {0} {1}", "arg1", "arg2");
            logger.Warn(CultureInfo.InvariantCulture, "Three args: {0} {1} {2}", "arg1", "arg2", "arg3");

            logger.Error(CultureInfo.InvariantCulture, "Simple message {0}", "arg1");
            logger.Error(CultureInfo.InvariantCulture, "Two args: {0} {1}", "arg1", "arg2");
            logger.Error(CultureInfo.InvariantCulture, "Three args: {0} {1} {2}", "arg1", "arg2", "arg3");

            // Higher-arity
            logger.Debug(CultureInfo.InvariantCulture, "Four args: {0} {1} {2} {3}", "arg1", "arg2", "arg3", "arg4");
            logger.Debug(CultureInfo.InvariantCulture, "Five args: {0} {1} {2} {3} {4}", "arg1", "arg2", "arg3", "arg4", "arg5");
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test that static logging host works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task StaticLoggingHost_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());
        resolver.RegisterConstant<ILogManager>(new DefaultLogManager(resolver));
        using var resolvedInner = resolver.WithResolver();

        await Assert.That(() =>
        {
            LogHost.Default.Debug("Static debug message");
            LogHost.Default.Info("Static info message");
            LogHost.Default.Warn("Static warn message");
            LogHost.Default.Error("Static error message");
            LogHost.Default.Fatal("Static fatal message");
            LogHost.Default.Debug("Static debug with param: {0}", "test");
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test that WrappingFullLogger works correctly with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task WrappingFullLogger_WorksWithAot()
    {
        var baseLogger = new DebugLogger();
        var wrappingLogger = new WrappingFullLogger(baseLogger);

        await Assert.That(() =>
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
            wrappingLogger.Debug(CultureInfo.InvariantCulture, "Formatted message: {0}", "test");
            wrappingLogger.Info(CultureInfo.InvariantCulture, "Multiple params: {0} {1}", "test1", "test2");
        }).ThrowsNothing();
    }

    /// <summary>
    /// Test that platform mode detector works with AOT including design mode detection.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task PlatformModeDetector_WorksWithAot()
    {
        var defaultDetector = new DefaultModeDetector();
        var platformDetector = new DefaultPlatformModeDetector();

        var inUnitTest1 = defaultDetector.InUnitTestRunner();
        var inDesignMode = platformDetector.InDesignMode();
        using (Assert.Multiple())
        {
            await Assert.That(inUnitTest1).IsNotNull();
            await Assert.That(inUnitTest1!.Value).IsTrue();

            // We're not in design mode during tests
            await Assert.That(inDesignMode).IsNotNull();
            await Assert.That(inDesignMode!.Value).IsFalse();
        }
    }

    /// <summary>
    /// Test that service registration callbacks with different parameters work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceRegistrationCallbacksWithContracts_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
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

        using (Assert.Multiple())
        {
            await Assert.That(defaultCallbackTriggered).IsTrue();
            await Assert.That(contractCallbackTriggered).IsTrue();
        }
    }

    /// <summary>
    /// Test that service unregistration scenarios work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ComprehensiveServiceUnregistration_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        const string contract = "TestContract";

        // Register multiple services
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.RegisterConstant<ITestInterface>(new AlternateTestImplementation(), contract);
        resolver.Register<ITestInterface>(() => new TestImplementation(), "factory");

        // Verify initial registrations
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsTrue();
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface), contract)).IsTrue();
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface), "factory")).IsTrue();
        }

        // Unregister current (default contract)
        resolver.UnregisterCurrent<ITestInterface>();
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsFalse();
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface), contract)).IsTrue();
        }

        // Unregister current with contract
        resolver.UnregisterCurrent<ITestInterface>(contract);
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface), contract)).IsFalse();
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface), "factory")).IsTrue();
        }

        // Unregister all (default contract only)
        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        resolver.Register<ITestInterface>(() => new AlternateTestImplementation());
        await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsTrue();

        resolver.UnregisterAll<ITestInterface>();
        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsFalse();
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface), "factory")).IsTrue();
        }
    }

    /// <summary>
    /// Test that fluent registration APIs work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task FluentRegistrationApis_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        resolver.RegisterAnd<ITestInterface>(() => new TestImplementation())
                .RegisterConstantAnd<ILogger>(new DebugLogger())
                .RegisterLazySingletonAnd<ILogManager>(() => new DefaultLogManager(resolver))
                .Register<IEnableLogger>(() => new TestService());

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsTrue();
            await Assert.That(resolver.HasRegistration(typeof(ILogger))).IsTrue();
            await Assert.That(resolver.HasRegistration(typeof(ILogManager))).IsTrue();
            await Assert.That(resolver.HasRegistration(typeof(IEnableLogger))).IsTrue();
        }

        var testInterface = resolver.GetService<ITestInterface>();
        var logger = resolver.GetService<ILogger>();
        var logManager = resolver.GetService<ILogManager>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.Multiple())
        {
            await Assert.That(testInterface).IsNotNull();
            await Assert.That(logger).IsNotNull();
            await Assert.That(logManager).IsNotNull();
            await Assert.That(enableLogger).IsNotNull();
        }
    }

    /// <summary>
    /// Test that resolver scoping and isolation work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ResolverScoping_WorksWithAot()
    {
        using var originalResolver = new InstanceGenericFirstDependencyResolver();
        originalResolver.RegisterConstant<ILogger>(new DebugLogger());

        // Set as current resolver
        using var originalScope = originalResolver.WithResolver();

        // Create a scoped resolver with a different logger
        using var scopedResolver = new InstanceGenericFirstDependencyResolver();
        scopedResolver.RegisterConstant<ILogger>(new ConsoleLogger());

        using (scopedResolver.WithResolver())
        {
            var logger = Locator.Current.GetService<ILogger>();
            using (Assert.Multiple())
            {
                await Assert.That(logger).IsNotNull();
                await Assert.That(logger).IsTypeOf<ConsoleLogger>();
            }
        }

        // After exiting the scoped resolver, we should be back to the original resolver
        var revertedLogger = Locator.Current.GetService<ILogger>();
        if (revertedLogger is not null)
        {
            await Assert.That(revertedLogger).IsNotTypeOf<ConsoleLogger>();
        }
    }

    /// <summary>
    /// Test that callback suppression works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CallbackSuppression_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
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

        using (Assert.Multiple())
        {
            await Assert.That(serviceCallbackTriggered).IsTrue();
            await Assert.That(resolverCallbackTriggered).IsFalse();
        }

        // Reset
        resolverCallbackTriggered = false;
        serviceCallbackTriggered = false;

        // Without suppression
        using (resolver.WithResolver(suppressResolverCallback: false))
        {
            resolver.RegisterConstant<ILogger>(new ConsoleLogger());
        }

        using (Assert.Multiple())
        {
            await Assert.That(serviceCallbackTriggered).IsTrue();
            await Assert.That(resolverCallbackTriggered).IsTrue();
        }
    }

    /// <summary>
    /// Test that cross-casting and type conversions work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task TypeConversions_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        var service = new TestServiceWithMultipleInterfaces();
        resolver.RegisterConstant<ITestInterface>(service);
        resolver.RegisterConstant<IEnableLogger>(service);

        var testInterface = resolver.GetService<ITestInterface>();
        var enableLogger = resolver.GetService<IEnableLogger>();

        using (Assert.Multiple())
        {
            await Assert.That(testInterface).IsNotNull();
            await Assert.That(enableLogger).IsNotNull();
            await Assert.That(testInterface).IsSameReferenceAs(service);
            await Assert.That(enableLogger).IsSameReferenceAs(service);
        }
    }

    /// <summary>
    /// Test that complex nested generic scenarios work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NestedGenericScenarios_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

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

        using (Assert.Multiple())
        {
            await Assert.That(enumerable).IsNotNull();
            await Assert.That(factory).IsNotNull();
            await Assert.That(lazy).IsNotNull();
            await Assert.That(enumerable!.Count()).IsEqualTo(2);
            await Assert.That(factory!()).IsNotNull();
            await Assert.That(lazy!.Value).IsTypeOf<DebugLogger>();
        }
    }

    /// <summary>
    /// Tests concurrent access scenarios with AOT compatibility.
    /// </summary>
    /// <returns>A task that represents the asynchronous test operation.</returns>
    [Test]
    public async Task ConcurrentAccess_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterLazySingleton<ILogger>(() => new DebugLogger());

        var exceptions = new List<Exception>();
        const int taskCount = 10;
        var tasks = new Task[taskCount];

        for (var i = 0; i < taskCount; i++)
        {
            tasks[i] = Task.Run(async () =>
            {
                try
                {
                    for (var j = 0; j < 100; j++)
                    {
                        var logger = resolver.GetService<ILogger>();
                        await Assert.That(logger).IsNotNull();

                        var services = resolver.GetServices<ILogger>();
                        await Assert.That(services).IsNotNull();

                        var hasRegistration = resolver.HasRegistration(typeof(ILogger));
                        await Assert.That(hasRegistration).IsTrue();
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            });
        }

        await Task.WhenAll(tasks);

        await Assert.That(exceptions).IsEmpty();
    }

    /// <summary>
    /// Test that null service type handling works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task NullServiceTypeHandling_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        var nullService = resolver.GetService(null);
        var nullServices = resolver.GetServices(null);

        using (Assert.Multiple())
        {
            await Assert.That(nullService).IsNull();
            await Assert.That(nullServices).IsNotNull();
            await Assert.That(resolver.HasRegistration(null)).IsFalse();
        }
    }

    /// <summary>
    /// Test that dependency resolver extension methods work with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DependencyResolverExtensions_WorksWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();

        await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsFalse();

        resolver.RegisterConstant<ITestInterface>(new TestImplementation());
        await Assert.That(resolver.HasRegistration(typeof(ITestInterface))).IsTrue();

        var service = resolver.GetService<ITestInterface>();
        var services = resolver.GetServices<ITestInterface>();

        using (Assert.Multiple())
        {
            await Assert.That(service).IsTypeOf<TestImplementation>();
            await Assert.That(services).IsNotNull();
            await Assert.That(services!.Count()).IsEqualTo(1);
        }
    }

    /// <summary>
    /// Test that dependency injection with parameterized constructors works with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ParameterizedConstructors_WorkWithAot()
    {
        using var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant<ILogger>(new DebugLogger());

        resolver.Register<ITestInterface>(() => new TestImplementationWithDependency(resolver.GetService<ILogger>()!));

        var service = resolver.GetService<ITestInterface>();

        using (Assert.Multiple())
        {
            await Assert.That(service).IsTypeOf<TestImplementationWithDependency>();
            var typed = (TestImplementationWithDependency)service!;
            await Assert.That(typed.Logger).IsTypeOf<DebugLogger>();
        }
    }

    /// <summary>
    /// Test that service disposal works correctly with AOT.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ServiceDisposal_WorksWithAot()
    {
        var disposableService = new DisposableTestService();
        var resolver = new InstanceGenericFirstDependencyResolver();
        resolver.RegisterConstant<ITestInterface>(disposableService);

        resolver.Dispose();

        await Assert.That(disposableService.IsDisposed).IsTrue();
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
