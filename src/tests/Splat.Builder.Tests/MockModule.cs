// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Builder.Tests;

internal sealed class MockModule : IModule
{
    public void Configure(IMutableDependencyResolver resolver)
    {
        // This is a mock module for testing purposes.
        // It does not need to do anything specific.
        // In a real scenario, you would register services here.
    }
}
