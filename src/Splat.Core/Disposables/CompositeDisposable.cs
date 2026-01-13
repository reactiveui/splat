// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET8_0_OR_GREATER
using System.Runtime.InteropServices;
#endif

namespace Splat;

/// <summary>
/// Represents a group of <see cref="IDisposable"/> objects that are disposed together when the <see cref="Dispose"/>
/// method is called.
/// </summary>
/// <remarks>Use <see cref="CompositeDisposable"/> to manage multiple disposable resources as a single unit. When
/// disposed, all contained disposables are disposed in the order they were added. This class is thread-safe for
/// disposal, but adding or removing disposables after disposal is not supported. Once disposed, further calls to <see
/// cref="Dispose"/> have no effect.</remarks>
internal sealed class CompositeDisposable : IDisposable
{
    // Default initial capacity of the _disposables list in case
    // The number of items is not known upfront
    private const int DefaultCapacity = 16;

    private readonly object _lock = new();
    private List<IDisposable>? _disposables;
    private volatile bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeDisposable"/> class with no contained disposables.
    /// </summary>
    /// <remarks>Use this constructor to create an empty CompositeDisposable, to which disposable resources
    /// can be added and managed collectively. Disposing the CompositeDisposable will dispose all contained
    /// resources.</remarks>
    public CompositeDisposable() => _disposables = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeDisposable"/> class with the specified initial capacity for contained.
    /// disposables.
    /// </summary>
    /// <remarks>Specifying a nonzero capacity can improve performance when the expected number of disposables
    /// is known in advance, as it reduces the need for internal resizing.</remarks>
    /// <param name="capacity">The initial number of elements that the internal collection can contain. Must be zero or greater.</param>
    public CompositeDisposable(int capacity)
    {
        ArgumentExceptionHelper.ThrowIfNegative(capacity);

        _disposables = new(capacity);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeDisposable"/> class that contains the specified disposables.
    /// </summary>
    /// <remarks>Each IDisposable provided will be disposed when the CompositeDisposable is disposed. The
    /// disposables are managed as a group, allowing for collective resource cleanup.</remarks>
    /// <param name="disposables">An array of IDisposable objects to be managed and disposed together. Cannot be null.</param>
    public CompositeDisposable(params IDisposable[] disposables)
    {
        ArgumentExceptionHelper.ThrowIfNull(disposables);

        _disposables = new(disposables.Length);
        InitFromArray(disposables);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CompositeDisposable"/> class that contains the specified collection of.
    /// IDisposable objects.
    /// </summary>
    /// <remarks>The disposables provided are added to the CompositeDisposable and will be disposed when the
    /// CompositeDisposable itself is disposed. The capacity of the internal storage is optimized based on the type and
    /// size of the input collection for performance.</remarks>
    /// <param name="disposables">An enumerable collection of IDisposable objects to be contained within the CompositeDisposable. Cannot be null.</param>
    public CompositeDisposable(IEnumerable<IDisposable> disposables)
    {
        ArgumentExceptionHelper.ThrowIfNull(disposables);

        switch (disposables)
        {
            // Fast path for arrays - use direct indexing
            case IDisposable[] array:
                _disposables = new(array.Length);
                InitFromArray(array);
                break;

            // If the disposables is a collection, get its size
            // and use it as a capacity hint for the copy.
            case ICollection<IDisposable> c:
                _disposables = new(c.Count);
                Init(disposables);
                break;
            default:
                _disposables = new(DefaultCapacity);

                // Unknown sized disposables, use the default capacity hint
                Init(disposables);
                break;
        }
    }

    /// <summary>
    /// Disposes all disposables in the group and removes them from the group.
    /// </summary>
    public void Dispose()
    {
        List<IDisposable>? disposables;

        lock (_lock)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            disposables = _disposables;
            _disposables = null;
        }

        if (disposables is null)
        {
            return;
        }

#if NET8_0_OR_GREATER
        // Use Span-based iteration for zero-allocation enumeration
        var span = CollectionsMarshal.AsSpan(disposables);
        for (var i = 0; i < span.Length; i++)
        {
            span[i].Dispose();
        }
#else
        // Fall back to foreach for older frameworks
        foreach (var disposable in disposables)
        {
            disposable.Dispose();
        }
#endif
    }

    /// <summary>
    /// Initialize the inner disposable list from an array.
    /// Optimized path that avoids enumerator allocation.
    /// </summary>
    /// <param name="disposables">The array of disposables.</param>
    private void InitFromArray(IDisposable[] disposables)
    {
        if (_disposables is null)
        {
            return;
        }

#if NET8_0_OR_GREATER
        // Ensure capacity is set for the list
        CollectionsMarshal.SetCount(_disposables, disposables.Length);
        var span = CollectionsMarshal.AsSpan(_disposables);

        for (var i = 0; i < disposables.Length; i++)
        {
            var d = disposables[i];
            ArgumentExceptionHelper.ThrowIfNullWithMessage(d, "The disposables collection contains null items", nameof(disposables));
            span[i] = d;
        }
#else
        // Fallback for older frameworks - use direct indexing on array
        for (var i = 0; i < disposables.Length; i++)
        {
            var d = disposables[i];
            ArgumentExceptionHelper.ThrowIfNullWithMessage(d, "The disposables collection contains null items", nameof(disposables));
            _disposables.Add(d);
        }
#endif
    }

    /// <summary>
    /// Adds the specified collection of disposable objects to the internal disposables list for later management.
    /// </summary>
    /// <remarks>This method ensures that all items in the <paramref name="disposables"/> collection are
    /// non-null before adding them. If the internal disposables list is not initialized, the method performs no
    /// action.</remarks>
    /// <param name="disposables">An enumerable collection of objects that implement <see cref="IDisposable"/> to be tracked. Each item in the
    /// collection must not be <see langword="null"/>.</param>
    private void Init(IEnumerable<IDisposable> disposables)
    {
        if (_disposables is null)
        {
            return;
        }

        // do the copy and null-check in one step to avoid a
        // second loop for just checking for null items
        foreach (var d in disposables)
        {
            ArgumentExceptionHelper.ThrowIfNullWithMessage(d, "The disposables collection contains null items", nameof(disposables));

            _disposables.Add(d);
        }
    }
}
