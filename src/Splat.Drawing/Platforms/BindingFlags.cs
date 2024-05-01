// Copyright (c) 2024 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splat
{
    internal enum BindingFlags
    {
        Public = 1,
        NonPublic = 1 << 1,
        Instance = 1 << 2,
        Static = 1 << 3,
        FlattenHierarchy = 1 << 4
    }
}
