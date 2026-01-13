// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Splat;

/// <summary>
/// Marks a class as eligible for logger injection by supporting frameworks or libraries.
/// </summary>
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
public interface IEnableLogger;
