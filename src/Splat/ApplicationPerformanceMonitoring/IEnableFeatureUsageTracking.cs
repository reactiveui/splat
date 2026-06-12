// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>Defines a contract for enabling feature usage tracking within an implementing class.</summary>
/// <remarks>Implement this interface to indicate that a component supports tracking or reporting of feature
/// usage. The presence of this interface may be used by frameworks or tools to discover and interact with feature usage
/// tracking capabilities.</remarks>
[ComVisible(false)]
[SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Marker interface used to discover feature-usage-tracking-enabled types and expose the FeatureUsageTracking() mixin.")]
public interface IEnableFeatureUsageTracking;
