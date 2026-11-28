// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

/// <summary>Checks the dispatch examples against the library and its adapters.</summary>
public sealed class DispatchExampleTests
{
    /// <summary>Checks service lifetimes, the named depot and the submitted log event.</summary>
    /// <returns>Completion after the desk has dispatched its parcel.</returns>
    [Test]
    public async Task DeskDispatchesParcel() => await Assert.That(DispatchDesk.Program.Main).ThrowsNothing();

    /// <summary>Checks native container resolution, log routing and the decoded preview pixel.</summary>
    /// <returns>Completion after the adapters have processed the parcel.</returns>
    [Test]
    public async Task AdaptersPreserveDispatchData() => await Assert.That(DispatchAdapters.Program.Main).ThrowsNothing();
}
