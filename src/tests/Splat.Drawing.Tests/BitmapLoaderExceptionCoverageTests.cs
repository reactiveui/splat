// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Drawing.Tests;

/// <summary>Unit tests covering the constructors of <see cref="BitmapLoaderException"/>.</summary>
public sealed class BitmapLoaderExceptionCoverageTests
{
    /// <summary>Verifies the parameterless constructor produces a usable exception.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task DefaultConstructor_CreatesInstance()
    {
        var exception = new BitmapLoaderException();

        await Assert.That(exception).IsNotNull();
    }

    /// <summary>Verifies the message constructor stores the supplied message.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MessageConstructor_StoresMessage()
    {
        const string message = "boom";
        var exception = new BitmapLoaderException(message);

        await Assert.That(exception.Message).IsEqualTo(message);
    }

    /// <summary>Verifies the message and inner-exception constructor stores both values.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task MessageAndInnerConstructor_StoresBoth()
    {
        const string message = "boom";
        var inner = new InvalidOperationException("inner");
        var exception = new BitmapLoaderException(message, inner);

        using (Assert.Multiple())
        {
            await Assert.That(exception.Message).IsEqualTo(message);
            await Assert.That(exception.InnerException).IsSameReferenceAs(inner);
        }
    }
}
