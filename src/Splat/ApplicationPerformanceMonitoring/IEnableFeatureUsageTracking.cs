// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Interface for exposing Feature Usage Tracking as an extension to a class.
/// </summary>
[ComVisible(false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "Intended through design")]
public interface IEnableFeatureUsageTracking;
