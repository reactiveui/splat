// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.InteropServices;

namespace Splat;

/// <summary>
/// "Implement" this interface in your class to get access to the Log()
/// Mixin, which will give you a Logger that includes the class name in the
/// log.
/// </summary>
[ComVisible(false)]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1040:Avoid empty interfaces", Justification = "By Design")]
public interface IEnableLogger;
