using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Splat.Tests.ServiceLocation
{
    /// <summary>
    /// Unit Tests for the Modern Dependency Resolver.
    /// </summary>
    public sealed class ModernDependencyResolverTests : BaseDependencyResolverTests<ModernDependencyResolver>
    {
        /// <inheritdoc />
        protected override ModernDependencyResolver GetDependencyResolver() => new ModernDependencyResolver();
    }
}
