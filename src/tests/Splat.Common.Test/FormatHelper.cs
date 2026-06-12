// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests.Logging;

/// <summary>Contains help with formatting.</summary>
public static class FormatHelper
{
    /// <summary>Gets the characters that make up the environment new line, as a fresh array on each access.</summary>
    public static char[] NewLine => Environment.NewLine.ToCharArray();

    /// <summary>Gets an exception for testing.</summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Deliberate for test")]
    public static Exception Exception => new();
}
