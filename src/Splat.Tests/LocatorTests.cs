// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shouldly;
using Splat.Tests.Mocks;
using Xunit;

namespace Splat.Tests
{
    /// <summary>
    /// Tests to confirm that the locator is working.
    /// </summary>
    public class LocatorTests
    {
        /// <summary>
        /// Tests to make sure that the unregister all functions correctly.
        /// This is a test when there are values registered.
        /// </summary>
        [Fact]
        public void ModernDependencyResolver_UnregisterAll_WithValuesWorks()
        {
            var currentMutable = new ModernDependencyResolver();

            var dummy1 = new DummyObjectClass1();
            var dummy2 = new DummyObjectClass2();
            var dummy3 = new DummyObjectClass3();

            var testContracts = new[] { string.Empty, "test" };

            foreach (var testContract in testContracts)
            {
                currentMutable.RegisterConstant<IDummyInterface>(dummy1, testContract);
                currentMutable.RegisterConstant<IDummyInterface>(dummy2, testContract);
                currentMutable.RegisterConstant<IDummyInterface>(dummy3, testContract);
            }

            foreach (var testContract in testContracts)
            {
                var items = currentMutable.GetServices<IDummyInterface>(testContract);

                items.ShouldBe(new IDummyInterface[] { dummy1, dummy2, dummy3 });

                currentMutable.UnregisterAll<IDummyInterface>(testContract);

                items = currentMutable.GetServices<IDummyInterface>(testContract);

                items.ShouldBeEmpty();
            }
        }

        /// <summary>
        /// Tests to make sure that the unregister all functions correctly.
        /// This is a test when there are values not registered.
        /// </summary>
        [Fact]
        public void ModernDependencyResolver_UnregisterAll_NoValuesWorks()
        {
            var currentMutable = new ModernDependencyResolver();

            var items = currentMutable.GetServices<IDummyInterface>();

            items.ShouldBeEmpty();

            currentMutable.UnregisterAll<IDummyInterface>();

            items = currentMutable.GetServices<IDummyInterface>();

            items.ShouldBeEmpty();
        }

        /// <summary>
        /// Tests tomake sure that the unregister current functions correctly.
        /// This is a test when there are values registered.
        /// </summary>
        [Fact]
        public void ModernDependencyResolver_UnregisterCurrent_WithValuesWorks()
        {
            var dummy1 = new DummyObjectClass1();
            var dummy2 = new DummyObjectClass2();
            var dummy3 = new DummyObjectClass3();

            var currentMutable = new ModernDependencyResolver();

            var testContracts = new[] { string.Empty, "test" };

            foreach (var testContract in testContracts)
            {
                currentMutable.RegisterConstant<IDummyInterface>(dummy1, testContract);
                currentMutable.RegisterConstant<IDummyInterface>(dummy2, testContract);
                currentMutable.RegisterConstant<IDummyInterface>(dummy3, testContract);
            }

            foreach (var testContract in testContracts)
            {
                var items = currentMutable.GetServices<IDummyInterface>(testContract);

                items.ShouldBe(new IDummyInterface[] { dummy1, dummy2, dummy3 });

                currentMutable.UnregisterCurrent<IDummyInterface>(testContract);

                items = currentMutable.GetServices<IDummyInterface>(testContract);

                items.ShouldBe(new IDummyInterface[] { dummy1, dummy2 });
            }
        }

        /// <summary>
        /// Tests to make sure that the unregister all functions correctly.
        /// This is a test when there are values not registered.
        /// </summary>
        [Fact]
        public void ModernDependencyResolver_UnregisterCurrent_NoValuesWorks()
        {
            var currentMutable = new ModernDependencyResolver();
            var items = currentMutable.GetServices<IDummyInterface>();

            items.ShouldBeEmpty();

            currentMutable.UnregisterCurrent<IDummyInterface>();

            items = currentMutable.GetServices<IDummyInterface>();

            items.ShouldBeEmpty();
        }

        /// <summary>
        /// Tests to make sure that the unregister all functions correctly.
        /// This is a test when there are values not registered.
        /// </summary>
        [Fact]
        public void FuncDependencyResolver_UnregisterAll()
        {
            bool unregisterAllCalled = false;
            Type type = null;
            string contract = null;

            var currentMutable = new FuncDependencyResolver(
                (funcType, funcContract) => Array.Empty<object>(),
                unregisterAll: (passedType, passedContract) =>
                {
                    unregisterAllCalled = true;
                    contract = passedContract;
                    type = passedType;
                });

            currentMutable.UnregisterAll<IDummyInterface>();
            type.ShouldBe(typeof(IDummyInterface));
            contract.ShouldBeNull();
            unregisterAllCalled.ShouldBeTrue();

            unregisterAllCalled = false;
            currentMutable.UnregisterAll<IEnableLogger>("test");
            type.ShouldBe(typeof(IEnableLogger));
            contract.ShouldBe("test");
            unregisterAllCalled.ShouldBeTrue();
        }

        /// <summary>
        /// Tests tomake sure that the unregister current functions correctly.
        /// This is a test when there are values registered.
        /// </summary>
        [Fact]
        public void FuncDependencyResolver_UnregisterCurrent()
        {
            bool unregisterAllCalled = false;
            Type type = null;
            string contract = null;

            var currentMutable = new FuncDependencyResolver(
                (funcType, funcContract) => Array.Empty<object>(),
                unregisterCurrent: (passedType, passedContract) =>
                {
                    unregisterAllCalled = true;
                    contract = passedContract;
                    type = passedType;
                });

            currentMutable.UnregisterCurrent<IDummyInterface>();
            type.ShouldBe(typeof(IDummyInterface));
            contract.ShouldBeNull();
            unregisterAllCalled.ShouldBeTrue();

            unregisterAllCalled = false;
            currentMutable.UnregisterCurrent<IEnableLogger>("test");
            type.ShouldBe(typeof(IEnableLogger));
            contract.ShouldBe("test");
            unregisterAllCalled.ShouldBeTrue();
        }
    }
}
