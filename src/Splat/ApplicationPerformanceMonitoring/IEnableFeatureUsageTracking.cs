// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Interface for exposing Feature Usage Tracking as an extension to a class.
/// </summary>
[ComVisible(false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Intended through design")]
public interface IEnableFeatureUsageTracking;
