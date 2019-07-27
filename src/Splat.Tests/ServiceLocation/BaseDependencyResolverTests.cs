using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Splat.Tests.ServiceLocation
{
    /// <summary>
    /// Common tests for Dependency Resolver interaction with Splat.
    /// </summary>
    /// <typeparam name="T">The dependency resolver to test.</typeparam>
    public abstract class BaseDependencyResolverTests<T>
        where T : IDependencyResolver
    {
        /// <summary>
        /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
        /// </summary>
        [Fact]
        public void UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
            var resolver = GetDependencyResolver();
            var type = typeof(ILogManager);
            resolver.Register(() => new DefaultLogManager(), type);
            resolver.Register(() => new DefaultLogManager(), type, "named");
            resolver.UnregisterCurrent(type);
            resolver.UnregisterCurrent(type);
        }

        /// <summary>
        /// Test to ensure UnregisterCurrent removes last entry.
        /// </summary>
        [Fact]
        public void UnregisterCurrent_Remove_Last()
        {
            var resolver = GetDependencyResolver();
            var type = typeof(ILogManager);
            resolver.Register(() => new DefaultLogManager(), type);
            resolver.Register(() => new FuncLogManager(_ => new WrappingFullLogger(new DebugLogger())), type);
            resolver.Register(() => new DefaultLogManager(), type, "named");

            var service = resolver.GetService(type);
            Assert.IsType<FuncLogManager>(service);

            resolver.UnregisterCurrent(type);

            service = resolver.GetService(type);
            Assert.IsType<DefaultLogManager>(service);
        }

        /// <summary>
        /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
        /// </summary>
        [Fact]
        public void UnregisterCurrentByName_Doesnt_Throw_When_List_Empty()
        {
            var resolver = GetDependencyResolver();
            var type = typeof(ILogManager);
            var contract = "named";
            resolver.Register(() => new DefaultLogManager(), type);
            resolver.Register(() => new DefaultLogManager(), type, contract);
            resolver.UnregisterCurrent(type, contract);
            resolver.UnregisterCurrent(type, contract);
        }

        /// <summary>
        /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
        /// </summary>
        [Fact]
        public void UnregisterAll_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
            var resolver = GetDependencyResolver();
            var type = typeof(ILogManager);
            resolver.Register(() => new DefaultLogManager(), type);
            resolver.Register(() => new DefaultLogManager(), type, "named");
            resolver.UnregisterAll(type);
            resolver.UnregisterCurrent(type);
        }

        /// <summary>
        /// Test to ensure Unregister doesn't cause an IndexOutOfRangeException.
        /// </summary>
        [Fact]
        public void UnregisterAllByContract_UnregisterCurrent_Doesnt_Throw_When_List_Empty()
        {
            var resolver = GetDependencyResolver();
            var type = typeof(ILogManager);
            var contract = "named";
            resolver.Register(() => new DefaultLogManager(), type);
            resolver.Register(() => new DefaultLogManager(), type, contract);
            resolver.UnregisterAll(type, contract);
            resolver.UnregisterCurrent(type, contract);
        }

        /// <summary>
        /// Gets an instance of a dependency resolver to test.
        /// </summary>
        /// <returns>Dependency Resolver.</returns>
        protected abstract T GetDependencyResolver();
    }
}
