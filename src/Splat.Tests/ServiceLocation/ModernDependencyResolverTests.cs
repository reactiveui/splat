using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Splat.Tests.ServiceLocation
{
    /// <summary>
    /// Unit Tests for the Modern Dependency Resolver.
    /// </summary>
    public sealed class ModernDependencyResolverTests
    {
        /// <summary>
        /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
        /// </summary>
        [Fact]
        public void UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
            var resolver = new ModernDependencyResolver();
            var type = typeof(ILogManager);
            resolver.Register(() => new DefaultLogManager(), type);
            resolver.UnregisterCurrent(type);
            resolver.UnregisterCurrent(type);
        }

        /// <summary>
        /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
        /// </summary>
        [Fact]
        public void UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
            var resolver = new ModernDependencyResolver();
            var type = typeof(ILogManager);
            resolver.Register(() => new DefaultLogManager(), type);
            resolver.UnregisterAll(type);
            resolver.UnregisterCurrent(type);
        }
    }
}
