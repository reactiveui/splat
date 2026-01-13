// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat;

/// <summary>
/// Represents errors that occur during bitmap loading operations.
/// </summary>
/// <remarks>This exception is thrown when a bitmap cannot be loaded due to invalid data, unsupported formats, or
/// other failures encountered during the loading process. Catch this exception to handle bitmap loading errors
/// specifically, rather than general exceptions.</remarks>
[Serializable]
public class BitmapLoaderException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapLoaderException"/> class.
    /// </summary>
    public BitmapLoaderException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapLoaderException"/> class.
    /// </summary>
    /// <param name="message">The message about the exception.</param>
    public BitmapLoaderException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BitmapLoaderException"/> class.
    /// </summary>
    /// <param name="message">The message about the exception.</param>
    /// <param name="innerException">Any other internal exceptions we are mapping.</param>
    public BitmapLoaderException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
