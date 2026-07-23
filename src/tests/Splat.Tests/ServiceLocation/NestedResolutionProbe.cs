// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.ServiceLocation;

/// <summary>Captures the result of a nested generic service lookup performed during its own construction.</summary>
/// <typeparam name="T">The nested service type to resolve.</typeparam>
/// <param name="resolver">The resolver used to perform the nested lookup.</param>
internal sealed class NestedResolutionProbe<T>(IDependencyResolver resolver)
{
    /// <summary>Gets the service resolved by a nested generic lookup performed during construction.</summary>
    internal T? Service { get; } = resolver.GetService<T>();
}
