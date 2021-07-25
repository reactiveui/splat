using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Splat.Tests.ServiceLocation
{
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
            var foo = 5;
            resolver.Register(() => foo, null!);

            var bar = 4;
            var contract = "foo";
            resolver.Register(() => bar, null!, contract);

            Assert.True(resolver.HasRegistration(null));
            var value = resolver.GetService(null!);
            Assert.Equal(foo, value);

            Assert.True(resolver.HasRegistration(null, contract));
            value = resolver.GetService(null!, contract);
            Assert.Equal(bar, value);

            var values = resolver.GetServices(null);
            Assert.Equal(1, values.Count());

            resolver.UnregisterCurrent(null);
            values = resolver.GetServices(null);
            Assert.Equal(0, values.Count());
        }

        /// <inheritdoc />
        protected override MicrosoftDependencyResolver GetDependencyResolver() => new();
    }
}
