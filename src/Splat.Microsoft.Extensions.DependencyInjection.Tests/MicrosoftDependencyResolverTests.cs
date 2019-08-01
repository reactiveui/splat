using System;
using System.Collections.Generic;
using System.Text;
using Splat.Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Splat.Tests.ServiceLocation
{
    /// <summary>
    /// Unit Tests for the Modern Dependency Resolver.
    /// </summary>
    public sealed class MicrosoftDependencyResolverTests : BaseDependencyResolverTests<MicrosoftDependencyResolver>
    {
        /// <inheritdoc />
        protected override MicrosoftDependencyResolver GetDependencyResolver() => new MicrosoftDependencyResolver();
    }
}
