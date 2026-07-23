// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

using Splat.Common.Test;

namespace Splat.Microsoft.Extensions.DependencyInjection.Tests;

/// <summary>
/// Tests for the container lifecycle of <see cref="MicrosoftDependencyResolver"/>: construction from a provider,
/// re-hosting via <c>UpdateContainer</c>, the immutable-after-build guards, the null-provider and non-keyed-provider
/// resolution paths, and the finalizer disposal path.
/// </summary>
[NotInParallel]
public class MicrosoftDependencyResolverLifecycleTests
{
    /// <summary>A contract name reused across the keyed-resolution tests.</summary>
    private const string Contract = "contract";

    /// <summary>Verifies that a resolver constructed from an <see cref="IServiceProvider"/> resolves from that provider and is immutable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Constructor_WithServiceProvider_ResolvesFromProviderAndIsImmutable()
    {
        var services = new ServiceCollection();
        _ = services.AddTransient<ViewModelOne>();
        var provider = services.BuildServiceProvider();

        await using var resolver = new MicrosoftDependencyResolver(provider);

        await Assert.That(resolver.GetService(typeof(ViewModelOne))).IsNotNull();
        await Assert.That(() => resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne)))
            .Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that <c>UpdateContainer</c> with a new collection replaces the registrations before the provider is built.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UpdateContainer_WithCollection_BeforeBuild_ReplacesRegistrations()
    {
        await using var resolver = new MicrosoftDependencyResolver();

        var replacement = new ServiceCollection();
        _ = replacement.AddTransient<ViewModelOne>();
        resolver.UpdateContainer(replacement);

        await Assert.That(resolver.GetService(typeof(ViewModelOne))).IsNotNull();
    }

    /// <summary>Verifies that <c>UpdateContainer</c> with a new collection disposes an already-built provider and rebuilds from the new collection.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UpdateContainer_WithCollection_AfterBuild_DisposesProviderAndRebuilds()
    {
        await using var resolver = new MicrosoftDependencyResolver();
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));

        // Force the provider to build so the next UpdateContainer must dispose it.
        await Assert.That(resolver.GetService(typeof(ViewModelOne))).IsNotNull();

        var replacement = new ServiceCollection();
        _ = replacement.AddTransient<ViewModelTwo>();
        resolver.UpdateContainer(replacement);

        using (Assert.Multiple())
        {
            await Assert.That(resolver.GetService(typeof(ViewModelTwo))).IsNotNull();
            await Assert.That(resolver.GetService(typeof(ViewModelOne))).IsNull();
        }
    }

    /// <summary>Verifies that <c>UpdateContainer</c> with a collection throws once the container has been built from a provider.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UpdateContainer_WithCollection_AfterImmutable_Throws()
    {
        await using var resolver = CreateImmutableResolver();

        await Assert.That(() => resolver.UpdateContainer(new ServiceCollection()))
            .Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that <c>UpdateContainer</c> with a new provider disposes the previously built provider and resolves from the new one.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task UpdateContainer_WithProvider_AfterBuild_DisposesPreviousProvider()
    {
        await using var resolver = new MicrosoftDependencyResolver();
        resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne));

        // Build the first provider so UpdateContainer(IServiceProvider) has one to dispose.
        await Assert.That(resolver.GetService(typeof(ViewModelOne))).IsNotNull();

        var replacement = new ServiceCollection();
        _ = replacement.AddTransient<ViewModelTwo>();
        resolver.UpdateContainer(replacement.BuildServiceProvider());

        await Assert.That(resolver.GetService(typeof(ViewModelTwo))).IsNotNull();
    }

    /// <summary>Verifies that the non-generic <c>GetService</c> with a null contract delegates to the non-contract resolution path.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_NonGeneric_WithNullContract_DelegatesToNonContractPath()
    {
        await using var resolver = new MicrosoftDependencyResolver();
        var instance = new ViewModelOne();
        resolver.RegisterConstant(instance);

        var result = resolver.GetService(typeof(ViewModelOne), null);

        await Assert.That(result).IsSameReferenceAs(instance);
    }

    /// <summary>Verifies that every resolution entry point throws when the service provider is unexpectedly null.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Resolution_WithNullProvider_Throws()
    {
        await using var resolver = new NullProviderResolver();

        using (Assert.Multiple())
        {
            await Assert.That(() => resolver.GetService(typeof(ViewModelOne))).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.GetService(typeof(ViewModelOne), Contract)).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.GetService<ViewModelOne>()).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.GetService<ViewModelOne>(Contract)).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.GetServices(typeof(ViewModelOne)).ToList()).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.GetServices(typeof(ViewModelOne), Contract).ToList()).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.GetServices<ViewModelOne>().ToList()).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.GetServices<ViewModelOne>(Contract).ToList()).Throws<InvalidOperationException>();
        }
    }

    /// <summary>Verifies that a keyed lookup against a provider that is not keyed returns the default value.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetService_Generic_WithContract_NonKeyedProvider_ReturnsDefault()
    {
        await using var resolver = new MicrosoftDependencyResolver(new NonKeyedServiceProvider());

        var result = resolver.GetService<ViewModelOne>(Contract);

        await Assert.That(result).IsNull();
    }

    /// <summary>Verifies that a keyed enumeration against a provider that is not keyed returns an empty sequence.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetServices_Generic_WithContract_NonKeyedProvider_ReturnsEmpty()
    {
        await using var resolver = new MicrosoftDependencyResolver(new NonKeyedServiceProvider());

        var results = resolver.GetServices<ViewModelOne>(Contract).ToList();

        await Assert.That(results).IsEmpty();
    }

    /// <summary>Verifies that every mutating method throws once the container has been built and is immutable.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MutatingMethods_OnImmutableResolver_Throw()
    {
        await using var resolver = CreateImmutableResolver();

        using (Assert.Multiple())
        {
            await Assert.That(() => resolver.Register(static () => new ViewModelOne(), typeof(ViewModelOne), Contract)).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.Register<IViewModelOne, ViewModelOne>()).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.Register<IViewModelOne, ViewModelOne>(Contract)).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.UnregisterCurrent(typeof(ViewModelOne))).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.UnregisterCurrent(typeof(ViewModelOne), Contract)).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.UnregisterAll(typeof(ViewModelOne))).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.UnregisterAll(typeof(ViewModelOne), Contract)).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.RegisterConstant(new ViewModelOne())).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.RegisterConstant(new ViewModelOne(), Contract)).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.RegisterLazySingleton(static () => new ViewModelOne())).Throws<InvalidOperationException>();
            await Assert.That(() => resolver.RegisterLazySingleton(static () => new ViewModelOne(), Contract)).Throws<InvalidOperationException>();
        }
    }

    /// <summary>Verifies that <c>HasRegistration</c> on an immutable resolver reflects what the built provider can resolve.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task HasRegistration_OnImmutableResolver_ReflectsProvider()
    {
        var services = new ServiceCollection();
        _ = services.AddTransient<ViewModelOne>();
        await using var resolver = new MicrosoftDependencyResolver(services.BuildServiceProvider());

        using (Assert.Multiple())
        {
            await Assert.That(resolver.HasRegistration(typeof(ViewModelOne))).IsTrue();
            await Assert.That(resolver.HasRegistration(typeof(ViewModelTwo))).IsFalse();
        }
    }

    /// <summary>Verifies that the finalizer disposal path (<c>Dispose(false)</c>) leaves managed resources intact, while <c>Dispose(true)</c> releases them.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_FinalizerPath_DoesNotReleaseProvider()
    {
        await using var resolver = new DisposeSpyResolver();
        var probe = new DisposableProbe();
        resolver.RegisterLazySingleton(() => probe);

        _ = resolver.GetService<DisposableProbe>();
        await Assert.That(probe.IsDisposed).IsFalse();

        // Finalizer path: managed cleanup is skipped, so the provider-owned singleton survives.
        resolver.InvokeDispose(false);
        await Assert.That(probe.IsDisposed).IsFalse();

        // Deterministic dispose path: the provider and its owned singleton are released.
        resolver.InvokeDispose(true);
        await Assert.That(probe.IsDisposed).IsTrue();
    }

    /// <summary>Creates a resolver that has already been built from a provider and is therefore immutable.</summary>
    /// <returns>An immutable <see cref="MicrosoftDependencyResolver"/>.</returns>
    private static MicrosoftDependencyResolver CreateImmutableResolver() =>
        new(new ServiceCollection().BuildServiceProvider());

    /// <summary>A resolver whose service provider is always null, used to exercise the null-provider guards.</summary>
    private sealed class NullProviderResolver : MicrosoftDependencyResolver
    {
        /// <inheritdoc/>
        protected override IServiceProvider? ServiceProvider => null;
    }

    /// <summary>A minimal service provider that does not implement <see cref="IKeyedServiceProvider"/>.</summary>
    private sealed class NonKeyedServiceProvider : IServiceProvider
    {
        /// <inheritdoc/>
        public object? GetService(Type serviceType) => null;
    }

    /// <summary>A resolver that exposes the protected <c>Dispose(bool)</c> method so both disposal paths can be exercised.</summary>
    private sealed class DisposeSpyResolver : MicrosoftDependencyResolver
    {
        /// <summary>Invokes the protected disposal method.</summary>
        /// <param name="disposing">Whether the deterministic (<see langword="true"/>) or finalizer (<see langword="false"/>) path is taken.</param>
        public void InvokeDispose(bool disposing) => Dispose(disposing);
    }

    /// <summary>A disposable probe used to observe whether the provider disposed the services it owns.</summary>
    private sealed class DisposableProbe : IDisposable
    {
        /// <summary>Gets a value indicating whether this instance has been disposed.</summary>
        public bool IsDisposed { get; private set; }

        /// <inheritdoc/>
        public void Dispose() => IsDisposed = true;
    }
}
