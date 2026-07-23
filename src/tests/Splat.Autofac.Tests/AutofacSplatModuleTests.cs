// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Autofac;

using Splat.Builder;
using Splat.Common.Test;

namespace Splat.Autofac.Tests;

/// <summary>Tests for <see cref="AutofacSplatModule"/>.</summary>
[NotInParallel]
public class AutofacSplatModuleTests
{
    /// <summary>Configure should register the Autofac resolver instance so it can be resolved from the built container.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Configure_RegistersResolverForRetrievalFromBuiltContainer()
    {
        using var scope = new AppLocatorScope();
        var builder = new ContainerBuilder();
        var module = new AutofacSplatModule(builder);

        module.Configure(AppLocator.CurrentMutable);
        var configuredResolver = AppLocator.Current;

        await using var container = builder.Build();
        var resolvedResolver = container.Resolve<AutofacDependencyResolver>();

        await Assert.That(resolvedResolver).IsNotNull();
        await Assert.That(resolvedResolver).IsSameReferenceAs(configuredResolver);
    }
}
