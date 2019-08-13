using System;
using System.Collections.Generic;
using System.Text;
using Splat.Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Splat.Tests.ServiceLocation
{
    /// <summary>
    /// Unit Tests for the Modern Dependency Resolver.
    /// The tests from here should really be brought down to the base test and be tested in all DIs.
    /// </summary>
    public sealed class MicrosoftDependencyResolverTests : BaseDependencyResolverTests<MicrosoftDependencyResolver>
    {
        /// <summary>
        /// Test to ensure container allows registration with null service type.
        /// </summary>
        [Fact]
        public void Can_Register_And_Resolve_Null_Types()
        {
            var resolver = GetDependencyResolver();
            var foo = 5;
            resolver.Register(() => foo, null);

            var value = resolver.GetService(null);
            Assert.Equal(foo, value);

            var bar = 4;
            var contract = "foo";
            resolver.Register(() => bar, null, contract);

            value = resolver.GetService(null, contract);
            Assert.Equal(bar, value);
        }

        /// <summary>
        /// Ensures <see cref="IReadonlyDependencyResolver.GetServices(Type, string)"/> never returns null.
        /// </summary>
        [Fact]
        public void GetServices_ShouldNeverReturnNull()
        {
            var resolver = GetDependencyResolver();

            Assert.NotNull(resolver.GetServices<string>());
            Assert.NotNull(resolver.GetServices<string>("Landscape"));
        }

        /// <inheritdoc />
        protected override MicrosoftDependencyResolver GetDependencyResolver() => new MicrosoftDependencyResolver();
    }
}
