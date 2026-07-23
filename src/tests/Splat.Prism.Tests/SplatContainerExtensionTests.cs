// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat.Common.Test;

namespace Splat.Prism.Tests;

/// <summary>
/// Tests for the registration and resolution surface of <see cref="SplatContainerExtension"/>: the creation-func
/// and factory overloads, the unsupported-operation guards, the parameterized-resolution paths, and disposal.
/// </summary>
[NotInParallel]
public class SplatContainerExtensionTests
{
    /// <summary>The contract name reused across the named-registration tests.</summary>
    private const string Name = "name";

    /// <summary>Verifies that reading the unsupported <c>CurrentScope</c> property throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CurrentScope_Throws_NotSupportedException()
    {
        using var container = new SplatContainerExtension();

        await Assert.That(() => container.CurrentScope).Throws<NotSupportedException>();
    }

    /// <summary>Verifies that the type-to-type scoped registration overload throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterScoped_TypeToType_Throws_NotSupportedException()
    {
        using var container = new SplatContainerExtension();

        await Assert.That(() => container.RegisterScoped(typeof(IViewFor<ViewModelOne>), typeof(ViewOne)))
            .Throws<NotSupportedException>();
    }

    /// <summary>Verifies that the provider-factory scoped registration overload throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterScoped_ProviderFactory_Throws_NotSupportedException()
    {
        using var container = new SplatContainerExtension();

        await Assert.That(() => container.RegisterScoped(typeof(IViewFor<ViewModelOne>), static _ => new ViewOne()))
            .Throws<NotSupportedException>();
    }

    /// <summary>Verifies that registering with an explicit default creation func resolves via that func.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WithDefaultCreationFunc_Resolves()
    {
        using var container = new SplatContainerExtension();
        _ = container.Register(typeof(IScreen), typeof(MockScreen), static () => new MockScreen());

        await Assert.That(container.Resolve(typeof(IScreen))).IsTypeOf<MockScreen>();
    }

    /// <summary>Verifies that registering a type with a plain factory resolves via that factory.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_TypeWithFactory_Resolves()
    {
        using var container = new SplatContainerExtension();
        _ = container.Register(typeof(IScreen), static () => new MockScreen());

        await Assert.That(container.Resolve(typeof(IScreen))).IsTypeOf<MockScreen>();
    }

    /// <summary>Verifies that registering a type with a provider-aware factory passes the container and resolves.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_TypeWithProviderFactory_PassesContainerAndResolves()
    {
        using var container = new SplatContainerExtension();
        IContainerProvider? capturedProvider = null;
        _ = container.Register(typeof(IScreen), provider =>
        {
            capturedProvider = provider;
            return new MockScreen();
        });

        var resolved = container.Resolve(typeof(IScreen));

        using (Assert.Multiple())
        {
            await Assert.That(resolved).IsTypeOf<MockScreen>();
            await Assert.That(capturedProvider).IsSameReferenceAs(container);
        }
    }

    /// <summary>Verifies that registering with a name and default creation func resolves the named registration.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Register_WithNameAndDefaultCreationFunc_Resolves()
    {
        using var container = new SplatContainerExtension();
        _ = container.Register(typeof(IScreen), typeof(MockScreen), Name, static () => new MockScreen());

        await Assert.That(container.Resolve(typeof(IScreen), Name)).IsTypeOf<MockScreen>();
    }

    /// <summary>Verifies that <c>RegisterMany</c> with a null service-type array returns the registry without registering.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterMany_WithNullServiceTypes_ReturnsRegistry()
    {
        using var container = new SplatContainerExtension();

        var result = container.RegisterMany(typeof(ViewOne), (Type[])null!);

        using (Assert.Multiple())
        {
            await Assert.That(result).IsSameReferenceAs(container);
            await Assert.That(container.IsRegistered(typeof(ViewOne))).IsFalse();
        }
    }

    /// <summary>Verifies that registering an instance with a name resolves the named singleton.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterInstance_WithName_Resolves()
    {
        using var container = new SplatContainerExtension();
        var screen = new MockScreen();
        _ = container.RegisterInstance(typeof(IScreen), screen, Name);

        await Assert.That(container.Resolve(typeof(IScreen), Name)).IsSameReferenceAs(screen);
    }

    /// <summary>Verifies that registering a singleton with a default creation func resolves the same instance each time.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterSingleton_WithDefaultCreationFunc_ResolvesSameInstance()
    {
        using var container = new SplatContainerExtension();
        _ = container.RegisterSingleton(typeof(IScreen), typeof(MockScreen), static () => new MockScreen());

        var first = container.Resolve(typeof(IScreen));
        var second = container.Resolve(typeof(IScreen));

        await Assert.That(first).IsSameReferenceAs(second);
    }

    /// <summary>Verifies that registering a named singleton with a default creation func resolves the same named instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterSingleton_WithNameAndDefaultCreationFunc_ResolvesSameInstance()
    {
        using var container = new SplatContainerExtension();
        _ = container.RegisterSingleton(typeof(IScreen), typeof(MockScreen), Name, static () => new MockScreen());

        var first = container.Resolve(typeof(IScreen), Name);
        var second = container.Resolve(typeof(IScreen), Name);

        await Assert.That(first).IsSameReferenceAs(second);
    }

    /// <summary>Verifies that the plain-factory singleton registration overload throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterSingleton_TypeWithFactory_Throws_NotSupportedException()
    {
        using var container = new SplatContainerExtension();

        await Assert.That(() => container.RegisterSingleton(typeof(IScreen), static () => new MockScreen()))
            .Throws<NotSupportedException>();
    }

    /// <summary>Verifies that the provider-factory singleton registration overload throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RegisterSingleton_TypeWithProviderFactory_Throws_NotSupportedException()
    {
        using var container = new SplatContainerExtension();

        await Assert.That(() => container.RegisterSingleton(typeof(IScreen), static _ => new MockScreen()))
            .Throws<NotSupportedException>();
    }

    /// <summary>Verifies that named parameterized resolution of an unregistered type throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Resolve_WithNameAndParameters_WhenNotRegistered_Throws()
    {
        using var container = new SplatContainerExtension();

        await Assert.That(() => container.Resolve(typeof(ViewModelOne), Name, (typeof(string), (object)Name)))
            .Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies the current behavior of named parameterized resolution when the type is registered: it throws.
    /// This pins the pre-existing inverted-condition behavior of the overload so any future fix is a conscious change.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Resolve_WithNameAndParameters_WhenRegistered_Throws()
    {
        using var container = new SplatContainerExtension();
        _ = container.Register(typeof(ViewModelOne), typeof(ViewModelOne), Name);

        await Assert.That(() => container.Resolve(typeof(ViewModelOne), Name, (typeof(string), (object)Name)))
            .Throws<InvalidOperationException>();
    }

    /// <summary>Verifies that the finalizer disposal path leaves the global locator untouched.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Dispose_FinalizerPath_DoesNotResetLocator()
    {
        using var container = new DisposeSpyContainer();
        _ = container.Register(typeof(IScreen), typeof(MockScreen));

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNotNull();

        // Finalizer path: managed cleanup is skipped, so the locator the constructor installed remains in place.
        container.InvokeDispose(false);

        await Assert.That(AppLocator.Current.GetService<IScreen>()).IsNotNull();
    }

    /// <summary>A container that exposes the protected <c>Dispose(bool)</c> method so the finalizer path can be exercised.</summary>
    private sealed class DisposeSpyContainer : SplatContainerExtension
    {
        /// <summary>Invokes the protected disposal method.</summary>
        /// <param name="isDisposing">Whether the deterministic (<see langword="true"/>) or finalizer (<see langword="false"/>) path is taken.</param>
        public void InvokeDispose(bool isDisposing) => Dispose(isDisposing);
    }
}
