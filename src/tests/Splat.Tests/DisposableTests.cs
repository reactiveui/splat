// Copyright (c) 2026 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

public class DisposableTests
{
    /// <summary>
    /// Test BooleanDisposable initial state.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task BooleanDisposable_InitialState_IsNotDisposed()
    {
        // Arrange & Act
        var disposable = new BooleanDisposable();

        // Assert
        await Assert.That(disposable.IsDisposed).IsFalse();
    }

    /// <summary>
    /// Test BooleanDisposable dispose sets IsDisposed to true.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task BooleanDisposable_Dispose_SetsIsDisposedToTrue()
    {
        // Arrange
        var disposable = new BooleanDisposable();

        // Act
        disposable.Dispose();

        // Assert
        await Assert.That(disposable.IsDisposed).IsTrue();
    }

    /// <summary>
    /// Test BooleanDisposable multiple dispose calls.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task BooleanDisposable_MultipleDisposeCallsAreSafe()
    {
        // Arrange
        var disposable = new BooleanDisposable();

        // Act
        disposable.Dispose();
        disposable.Dispose();
        disposable.Dispose();

        // Assert
        await Assert.That(disposable.IsDisposed).IsTrue();
    }

    /// <summary>
    /// Test ActionDisposable executes action on dispose.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ActionDisposable_Dispose_ExecutesAction()
    {
        // Arrange
        var actionExecuted = false;
        var disposable = new ActionDisposable(() => actionExecuted = true);

        // Act
        disposable.Dispose();

        // Assert
        await Assert.That(actionExecuted).IsTrue();
    }

    /// <summary>
    /// Test ActionDisposable executes action only once.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task ActionDisposable_MultipleDispose_ExecutesActionOnlyOnce()
    {
        // Arrange
        var executionCount = 0;
        var disposable = new ActionDisposable(() => executionCount++);

        // Act
        disposable.Dispose();
        disposable.Dispose();
        disposable.Dispose();

        // Assert
        await Assert.That(executionCount).IsEqualTo(1);
    }

    /// <summary>
    /// Test ActionDisposable.Empty does nothing.
    /// </summary>
    [Test]
    public void ActionDisposable_Empty_DoesNothing()
    {
        // Arrange & Act
        var disposable = ActionDisposable.Empty;

        // Assert - should not throw
        disposable.Dispose();
        disposable.Dispose();
    }

    /// <summary>
    /// Test CompositeDisposable default constructor.
    /// </summary>
    [Test]
    public void CompositeDisposable_DefaultConstructor_Works()
    {
        // Arrange & Act
        var disposable = new CompositeDisposable();

        // Assert - should not throw
        disposable.Dispose();
    }

    /// <summary>
    /// Test CompositeDisposable with capacity constructor.
    /// </summary>
    [Test]
    public void CompositeDisposable_CapacityConstructor_Works()
    {
        // Arrange & Act
        var disposable = new CompositeDisposable(10);

        // Assert - should not throw
        disposable.Dispose();
    }

    /// <summary>
    /// Test CompositeDisposable with zero capacity constructor.
    /// </summary>
    [Test]
    public void CompositeDisposable_ZeroCapacityConstructor_Works()
    {
        // Arrange & Act
        var disposable = new CompositeDisposable(0);

        // Assert - should not throw
        disposable.Dispose();
    }

    /// <summary>
    /// Test CompositeDisposable with negative capacity throws ArgumentOutOfRangeException.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NegativeCapacity_ThrowsArgumentOutOfRangeException() =>

        // Act & Assert
        await Assert.That(() => _ = new CompositeDisposable(-1)).Throws<ArgumentOutOfRangeException>();

    /// <summary>
    /// Test CompositeDisposable with array constructor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_ArrayConstructor_DisposesAllItems()
    {
        // Arrange
        var disposed1 = false;
        var disposed2 = false;
        var action1 = new ActionDisposable(() => disposed1 = true);
        var action2 = new ActionDisposable(() => disposed2 = true);

        var disposable = new CompositeDisposable(action1, action2);

        // Act
        disposable.Dispose();

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(disposed1).IsTrue();
            await Assert.That(disposed2).IsTrue();
        }
    }

    /// <summary>
    /// Test CompositeDisposable with empty array constructor.
    /// </summary>
    [Test]
    public void CompositeDisposable_EmptyArrayConstructor_Works()
    {
        // Arrange & Act
        var disposable = new CompositeDisposable(Array.Empty<IDisposable>());

        // Assert - should not throw
        disposable.Dispose();
    }

    /// <summary>
    /// Test CompositeDisposable with single item array constructor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_SingleItemArrayConstructor_DisposesItem()
    {
        // Arrange
        var disposed = false;
        var action = new ActionDisposable(() => disposed = true);

        var disposable = new CompositeDisposable(action);

        // Act
        disposable.Dispose();

        // Assert
        await Assert.That(disposed).IsTrue();
    }

    /// <summary>
    /// Test CompositeDisposable with enumerable constructor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_EnumerableConstructor_DisposesAllItems()
    {
        // Arrange
        var disposed1 = false;
        var disposed2 = false;
        var disposed3 = false;
        var disposables = new List<IDisposable>
        {
            new ActionDisposable(() => disposed1 = true),
            new ActionDisposable(() => disposed2 = true),
            new ActionDisposable(() => disposed3 = true)
        };

        var compositeDisposable = new CompositeDisposable(disposables);

        // Act
        compositeDisposable.Dispose();

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(disposed1).IsTrue();
            await Assert.That(disposed2).IsTrue();
            await Assert.That(disposed3).IsTrue();
        }
    }

    /// <summary>
    /// Test CompositeDisposable with enumerable constructor passing array as IEnumerable.
    /// This tests the array fast path in the IEnumerable constructor.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_EnumerableConstructorWithArray_DisposesAllItems()
    {
        // Arrange
        var disposed1 = false;
        var disposed2 = false;
        IEnumerable<IDisposable> disposables = new IDisposable[]
        {
            new ActionDisposable(() => disposed1 = true),
            new ActionDisposable(() => disposed2 = true)
        };

        var compositeDisposable = new CompositeDisposable(disposables);

        // Act
        compositeDisposable.Dispose();

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(disposed1).IsTrue();
            await Assert.That(disposed2).IsTrue();
        }
    }

    /// <summary>
    /// Test CompositeDisposable with non-collection enumerable (yield return).
    /// This tests the default case branch that uses DefaultCapacity.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NonCollectionEnumerable_DisposesAllItems()
    {
        // Arrange
        var disposed1 = false;
        var disposed2 = false;
        var disposed3 = false;

        static IEnumerable<IDisposable> CreateDisposables(
            Action setDisposed1,
            Action setDisposed2,
            Action setDisposed3)
        {
            yield return new ActionDisposable(setDisposed1);
            yield return new ActionDisposable(setDisposed2);
            yield return new ActionDisposable(setDisposed3);
        }

        var compositeDisposable = new CompositeDisposable(
            CreateDisposables(
                () => disposed1 = true,
                () => disposed2 = true,
                () => disposed3 = true));

        // Act
        compositeDisposable.Dispose();

        using (Assert.Multiple())
        {
            // Assert
            await Assert.That(disposed1).IsTrue();
            await Assert.That(disposed2).IsTrue();
            await Assert.That(disposed3).IsTrue();
        }
    }

    /// <summary>
    /// Test CompositeDisposable with empty non-collection enumerable.
    /// </summary>
    [Test]
    public void CompositeDisposable_EmptyNonCollectionEnumerable_Works()
    {
        // Arrange
        static IEnumerable<IDisposable> CreateEmptyDisposables()
        {
            yield break;
        }

        // Act
        var disposable = new CompositeDisposable(CreateEmptyDisposables());

        // Assert - should not throw
        disposable.Dispose();
    }

    /// <summary>
    /// Test CompositeDisposable throws for null array.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NullArray_ThrowsArgumentNullException() =>

        // Act & Assert
        await Assert.That(() => _ = new CompositeDisposable(null!)).Throws<ArgumentNullException>();

    /// <summary>
    /// Test CompositeDisposable throws for null enumerable.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NullEnumerable_ThrowsArgumentNullException() =>

        // Act & Assert
        await Assert.That(() => _ = new CompositeDisposable((IEnumerable<IDisposable>)null!)).Throws<ArgumentNullException>();

    /// <summary>
    /// Test CompositeDisposable throws for null item in array.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NullItemInArray_ThrowsArgumentException() =>

        // Act & Assert
        await Assert.That(() => _ = new CompositeDisposable(new ActionDisposable(() => { }), null!)).Throws<ArgumentNullException>();

    /// <summary>
    /// Test CompositeDisposable throws for null item in enumerable.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NullItemInEnumerable_ThrowsArgumentException()
    {
        // Arrange
        var disposables = new List<IDisposable> { new ActionDisposable(() => { }), null! };

        // Act & Assert
        await Assert.That(() => _ = new CompositeDisposable(disposables)).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Test CompositeDisposable throws for null item in non-collection enumerable.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NullItemInNonCollectionEnumerable_ThrowsArgumentException()
    {
        // Arrange
        static IEnumerable<IDisposable> CreateDisposablesWithNull()
        {
            yield return new ActionDisposable(() => { });
            yield return null!;
        }

        // Act & Assert
        await Assert.That(() => _ = new CompositeDisposable(CreateDisposablesWithNull())).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Test CompositeDisposable throws for null as first item in array.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NullAsFirstItemInArray_ThrowsArgumentException() =>

        // Act & Assert
        await Assert.That(() => _ = new CompositeDisposable(null!, new ActionDisposable(() => { }))).Throws<ArgumentNullException>();

    /// <summary>
    /// Test CompositeDisposable multiple dispose calls are safe.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_MultipleDisposeCallsAreSafe()
    {
        // Arrange
        var executionCount = 0;
        var action = new ActionDisposable(() => executionCount++);
        var disposable = new CompositeDisposable(action);

        // Act
        disposable.Dispose();
        disposable.Dispose();
        disposable.Dispose();

        // Assert
        await Assert.That(executionCount).IsEqualTo(1);
    }

    /// <summary>
    /// Test CompositeDisposable with empty enumerable.
    /// </summary>
    [Test]
    public void CompositeDisposable_EmptyEnumerable_Works()
    {
        // Arrange & Act
        var disposable = new CompositeDisposable(new List<IDisposable>());

        // Assert - should not throw
        disposable.Dispose();
    }

    /// <summary>
    /// Test CompositeDisposable concurrent dispose calls from multiple threads.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_ConcurrentDispose_DisposesOnlyOnce()
    {
        // Arrange
        var executionCount = 0;
        var action = new ActionDisposable(() => Interlocked.Increment(ref executionCount));
        var disposable = new CompositeDisposable(action);

        // Act - dispose from multiple threads concurrently
        var tasks = new Task[10];
        for (var i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() => disposable.Dispose());
        }

        await Task.WhenAll(tasks);

        // Assert - should only be disposed once despite concurrent calls
        await Assert.That(executionCount).IsEqualTo(1);
    }

    /// <summary>
    /// Test CompositeDisposable with many items disposes all.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_ManyItems_DisposesAll()
    {
        // Arrange
        const int itemCount = 100;
        var disposedCount = 0;
        var disposables = new IDisposable[itemCount];

        for (var i = 0; i < itemCount; i++)
        {
            disposables[i] = new ActionDisposable(() => Interlocked.Increment(ref disposedCount));
        }

        var compositeDisposable = new CompositeDisposable(disposables);

        // Act
        compositeDisposable.Dispose();

        // Assert
        await Assert.That(disposedCount).IsEqualTo(itemCount);
    }

    /// <summary>
    /// Test CompositeDisposable with many items via enumerable disposes all.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_ManyItemsViaEnumerable_DisposesAll()
    {
        // Arrange
        const int itemCount = 100;
        var disposedCount = 0;
        var disposables = new List<IDisposable>();

        for (var i = 0; i < itemCount; i++)
        {
            disposables.Add(new ActionDisposable(() => Interlocked.Increment(ref disposedCount)));
        }

        var compositeDisposable = new CompositeDisposable(disposables);

        // Act
        compositeDisposable.Dispose();

        // Assert
        await Assert.That(disposedCount).IsEqualTo(itemCount);
    }

    /// <summary>
    /// Test CompositeDisposable disposes items in order.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_DisposesItemsInOrder()
    {
        // Arrange
        var disposeOrder = new List<int>();
        var disposable1 = new ActionDisposable(() => disposeOrder.Add(1));
        var disposable2 = new ActionDisposable(() => disposeOrder.Add(2));
        var disposable3 = new ActionDisposable(() => disposeOrder.Add(3));

        var compositeDisposable = new CompositeDisposable(disposable1, disposable2, disposable3);

        // Act
        compositeDisposable.Dispose();

        // Assert
        await Assert.That(disposeOrder).IsEquivalentTo(new[] { 1, 2, 3 });
    }

    /// <summary>
    /// Test CompositeDisposable with large capacity constructor.
    /// </summary>
    [Test]
    public void CompositeDisposable_LargeCapacityConstructor_Works()
    {
        // Arrange & Act
        var disposable = new CompositeDisposable(1000);

        // Assert - should not throw
        disposable.Dispose();
    }
}
