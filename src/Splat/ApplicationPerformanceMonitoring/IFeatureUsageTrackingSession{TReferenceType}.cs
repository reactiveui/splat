// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Generic interface for a Feature Usage Tracking Session.
/// </summary>
/// <typeparam name="TReferenceType">The Type for the Unique Tracking References.</typeparam>
public interface IFeatureUsageTrackingSession<out TReferenceType> : IFeatureUsageTrackingSession
{
    /// <summary>
    /// Gets the current Feature Usage Unique Reference.
    /// </summary>
    TReferenceType FeatureReference { get; }

    /// <summary>
    /// Gets the unique reference for the Parent Tracking Session, if any.
    /// </summary>
    TReferenceType ParentReference { get; }
}
