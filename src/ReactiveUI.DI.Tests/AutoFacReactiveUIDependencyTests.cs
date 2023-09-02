// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using Autofac;
using FluentAssertions;
using ReactiveUI.DI.Tests.Mocks;
using Splat;
using Splat.Autofac;

namespace ReactiveUI.DI.Tests;

/// <summary>
/// AutoFac ReactiveUI DependencyTests.
/// </summary>
public class AutoFacReactiveUIDependencyTests
{
    /// <summary>
    /// Should register ReactiveUI binding type converters.
    /// </summary>
    [Fact]
    public void AutofacDependencyResolverShouldRegisterReactiveUIBindingTypeConverters()
    {
        // Invoke RxApp which initializes the ReactiveUI platform.
        var builder = new ContainerBuilder();
        var locator = new AutofacDependencyResolver(builder);
        locator.InitializeReactiveUI();
        Locator.SetLocator(locator);
        var container = builder.Build();

        var converters = container.Resolve<IEnumerable<IBindingTypeConverter>>().ToList();

        converters.Should().NotBeNull();
        converters.Should().Contain(x => x.GetType() == typeof(StringConverter));
        converters.Should().Contain(x => x.GetType() == typeof(EqualityTypeConverter));
    }

    /// <summary>
    /// Should register ReactiveUI creates command bindings.
    /// </summary>
    [Fact]
    public void AutofacDependencyResolverShouldRegisterReactiveUICreatesCommandBinding()
    {
        // Invoke RxApp which initializes the ReactiveUI platform.
        var builder = new ContainerBuilder();
        var locator = new AutofacDependencyResolver(builder);
        locator.InitializeReactiveUI();
        Locator.SetLocator(locator);
        var container = builder.Build();

        var converters = container.Resolve<IEnumerable<ICreatesCommandBinding>>().ToList();

        converters.Should().NotBeNull();
        converters.Should().Contain(x => x.GetType() == typeof(CreatesCommandBindingViaEvent));
        converters.Should().Contain(x => x.GetType() == typeof(CreatesCommandBindingViaCommandParameter));
    }

    /// <summary>
    /// Automatics the fac when any test.
    /// </summary>
    [Fact]
    public void AutoFacWhenAnyTest()
    {
        var builder = new ContainerBuilder();

        var autofacResolver = builder.UseAutofacDependencyResolver();
        Locator.CurrentMutable.RegisterConstant(new ActivatingViewFetcher(), typeof(IActivationForViewFetcher));
        autofacResolver.InitializeSplat();
        autofacResolver.InitializeReactiveUI();
        _ = builder.Build();

        var vm = new ActivatingViewModel();
        var fixture = new ActivatingView { ViewModel = vm };

        Assert.Equal(0, vm.IsActiveCount);
        Assert.Equal(0, fixture.IsActiveCount);

        fixture.Loaded.OnNext(Unit.Default);
        Assert.Equal(1, vm.IsActiveCount);
        Assert.Equal(1, fixture.IsActiveCount);

        fixture.Unloaded.OnNext(Unit.Default);
        Assert.Equal(0, vm.IsActiveCount);
        Assert.Equal(0, fixture.IsActiveCount);
    }
}
