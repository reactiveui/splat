// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

namespace Splat.Drawing.Tests;

/// <summary>Unit tests covering the static <see cref="BitmapLoader"/> accessor and its state.</summary>
[NotInParallel] // Mutates the global BitmapLoader.Current static state.
public sealed class BitmapLoaderCoverageTests
{
    /// <summary>Verifies that setting the current loader and reading it back returns the same instance.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Current_SetThenGet_ReturnsSameInstance()
    {
        var saved = BitmapLoader.GetState();
        try
        {
            var stub = new StubBitmapLoader();
            BitmapLoader.Current = stub;

            await Assert.That(BitmapLoader.Current).IsSameReferenceAs(stub);
        }
        finally
        {
            BitmapLoader.RestoreState(saved);
        }
    }

    /// <summary>Verifies that getting the current loader while the state is null throws.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task Current_WhenNull_Throws()
    {
        var saved = BitmapLoader.GetState();
        try
        {
            BitmapLoader.RestoreState(null);

            await Assert.That(static () => BitmapLoader.Current).Throws<BitmapLoaderException>();
        }
        finally
        {
            BitmapLoader.RestoreState(saved);
        }
    }

    /// <summary>Verifies that <see cref="BitmapLoader.GetState"/> reflects the value previously set.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task GetState_ReflectsCurrentValue()
    {
        var saved = BitmapLoader.GetState();
        try
        {
            var stub = new StubBitmapLoader();
            BitmapLoader.Current = stub;

            await Assert.That(BitmapLoader.GetState()).IsSameReferenceAs(stub);
        }
        finally
        {
            BitmapLoader.RestoreState(saved);
        }
    }

    /// <summary>Verifies that <see cref="BitmapLoader.RestoreState"/> restores the supplied loader state.</summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task RestoreState_RestoresProvidedValue()
    {
        var saved = BitmapLoader.GetState();
        try
        {
            var stub = new StubBitmapLoader();
            BitmapLoader.RestoreState(stub);

            await Assert.That(BitmapLoader.GetState()).IsSameReferenceAs(stub);
        }
        finally
        {
            BitmapLoader.RestoreState(saved);
        }
    }

    /// <summary>A throwaway <see cref="IBitmapLoader"/> implementation used to verify the setter round-trip.</summary>
    private sealed class StubBitmapLoader : IBitmapLoader
    {
        /// <inheritdoc />
        public IBitmap? Create(float width, float height) => null;

        /// <inheritdoc />
        public Task<IBitmap?> Load(Stream sourceStream, float? desiredWidth, float? desiredHeight) => Task.FromResult<IBitmap?>(null);

        /// <inheritdoc />
        public Task<IBitmap?> LoadFromResource(string source, float? desiredWidth, float? desiredHeight) => Task.FromResult<IBitmap?>(null);
    }
}
