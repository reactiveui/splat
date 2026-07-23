// Copyright (c) 2019-2026 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Common.Test;

/// <summary>Represents a view bound to a view model.</summary>
/// <seealso cref="IViewFor" />
public interface IViewFor
{
    /// <summary>Gets or sets the view model.</summary>
    object? ViewModel { get; set; }
}
