// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace Splat.Tests.Mocks;

/// <summary>
/// A dummy class used during Locator testing.
/// </summary>
[ExcludeFromCodeCoverage]
#pragma warning disable CA1515 // Consider making public types internal
public class DummyObjectClass3 : IDummyInterface;
#pragma warning restore CA1515 // Consider making public types internal
