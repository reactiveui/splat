// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Defines a contract for enabling feature usage tracking within an implementing class.
/// </summary>
/// <remarks>Implement this interface to indicate that a component supports tracking or reporting of feature
/// usage. The presence of this interface may be used by frameworks or tools to discover and interact with feature usage
/// tracking capabilities.</remarks>
[ComVisible(false)]
public interface IEnableFeatureUsageTracking;
