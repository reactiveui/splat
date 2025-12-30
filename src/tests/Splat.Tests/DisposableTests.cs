// Copyright (c) 2025 ReactiveUI. All rights reserved.
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
    /// Test CompositeDisposable throws for null array.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [Test]
    public async Task CompositeDisposable_NullArray_ThrowsArgumentNullException() =>

        // Act & Assert
        await Assert.That(() => _ = new CompositeDisposable((IDisposable[])null!)).Throws<ArgumentNullException>();

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
}
