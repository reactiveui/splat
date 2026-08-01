// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Splat;

/// <summary>Marks a class as eligible for logger injection by supporting frameworks or libraries.</summary>
/// <remarks><para>
/// Implement this interface in your class to get access to the Log() Mixin,
/// which will give you a Logger that includes the class name in the log,
/// indicating that a type can participate in logging infrastructure that
/// relies on marker interfaces.
/// </para>
/// <para>
/// This interface does not define any members and serves only as a marker for logger
/// enablement.
/// </para></remarks>
[ComVisible(false)]
[SuppressMessage(
    "Design",
    "SST1437:Avoid empty interfaces",
    Justification = "Marker interface for logger enablement; the Log() mixin keys off it.")]
public interface IEnableLogger;
