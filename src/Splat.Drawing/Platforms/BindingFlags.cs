// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat;

/// <summary>
/// Specifies flags that control binding and the way in which members are searched for by reflection.
/// </summary>
/// <remarks>Use the BindingFlags enumeration to specify which types of members to include when using reflection
/// methods to search for fields, properties, methods, or events. Multiple flags can be combined using a bitwise OR
/// operation to refine search behavior. This enumeration is typically used with methods such as Type.GetMethod,
/// Type.GetField, and similar reflection APIs.</remarks>
internal enum BindingFlags
{
    Public = 1,
    NonPublic = 1 << 1,
    Instance = 1 << 2,
    Static = 1 << 3,
    FlattenHierarchy = 1 << 4
}
