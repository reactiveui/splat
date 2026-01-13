// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.ApplicationPerformanceMonitoring;

/// <summary>
/// Defines a contract for a feature usage tracking session that exposes unique references for the current session and
/// its parent session.
/// </summary>
/// <typeparam name="TReferenceType">The type of the unique reference used to identify the feature usage tracking session and its parent. This type must
/// be reference type compatible.</typeparam>
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
