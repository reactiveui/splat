using Splat.Microsoft.Extensions.DependencyInjection;

namespace Splat.Tests.ServiceLocation;

/// <summary>
/// Unit Tests for the Modern Dependency Resolver.
/// </summary>
public sealed class MicrosoftDependencyResolverTests : BaseDependencyResolverTests<MicrosoftDependencyResolver>
{
    /// <summary>
    /// Test to ensure container allows registration with null service type.
    /// Should really be brought down to the <see cref="BaseDependencyResolverTests{T}"/>,
    /// it fails for some of the DIs.
    /// </summary>
    [Fact]
    public void Can_Register_And_Resolve_Null_Types()
    {
        var resolver = GetDependencyResolver();
        const int foo = 5;
        resolver.Register(() => foo, null);

        const int bar = 4;
        const string contract = "foo";
        resolver.Register(() => bar, null, contract);

        Assert.True(resolver.HasRegistration(null));
        var value = resolver.GetService(null);
        Assert.Equal(foo, value);

        Assert.True(resolver.HasRegistration(null, contract));
        value = resolver.GetService(null, contract);
        Assert.Equal(bar, value);

        var values = resolver.GetServices(null);
        Assert.Equal(1, values.Count());

        resolver.UnregisterCurrent(null);
        var valuesNC = resolver.GetServices(null);
        Assert.Equal(0, valuesNC.Count());
        var valuesC = resolver.GetServices(null, contract);
        Assert.Equal(1, valuesC.Count());

        resolver.UnregisterAll(null);
        valuesNC = resolver.GetServices(null);
        Assert.Equal(0, valuesNC.Count());

        resolver.UnregisterAll(null, contract);
        valuesC = resolver.GetServices(null, contract);
        Assert.Equal(0, valuesC.Count());
    }

    /// <inheritdoc />
    protected override MicrosoftDependencyResolver GetDependencyResolver() => new();
}
