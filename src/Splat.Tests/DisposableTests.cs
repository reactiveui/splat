// Copyright (c) 2025 ReactiveUI. All rights reserved.
// Licensed to ReactiveUI under one or more agreements.
// ReactiveUI licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Splat.Tests;

/// <summary>
/// Unit Tests for the disposable classes.
/// </summary>
[TestFixture]
public class DisposableTests
{
    /// <summary>
    /// Test BooleanDisposable initial state.
    /// </summary>
    [Test]
    public void BooleanDisposable_InitialState_IsNotDisposed()
    {
        // Arrange & Act
        var disposable = new BooleanDisposable();

        // Assert
        Assert.That(disposable.IsDisposed, Is.False);
    }

    /// <summary>
    /// Test BooleanDisposable dispose sets IsDisposed to true.
    /// </summary>
    [Test]
    public void BooleanDisposable_Dispose_SetsIsDisposedToTrue()
    {
        // Arrange
        var disposable = new BooleanDisposable();

        // Act
        disposable.Dispose();

        // Assert
        Assert.That(disposable.IsDisposed, Is.True);
    }

    /// <summary>
    /// Test BooleanDisposable multiple dispose calls.
    /// </summary>
    [Test]
    public void BooleanDisposable_MultipleDisposeCallsAreSafe()
    {
        // Arrange
        var disposable = new BooleanDisposable();

        // Act
        disposable.Dispose();
        disposable.Dispose();
        disposable.Dispose();

        // Assert
        Assert.That(disposable.IsDisposed, Is.True);
    }

    /// <summary>
    /// Test ActionDisposable executes action on dispose.
    /// </summary>
    [Test]
    public void ActionDisposable_Dispose_ExecutesAction()
    {
        // Arrange
        var actionExecuted = false;
        var disposable = new ActionDisposable(() => actionExecuted = true);

        // Act
        disposable.Dispose();

        // Assert
        Assert.That(actionExecuted, Is.True);
    }

    /// <summary>
    /// Test ActionDisposable executes action only once.
    /// </summary>
    [Test]
    public void ActionDisposable_MultipleDispose_ExecutesActionOnlyOnce()
    {
        // Arrange
        var executionCount = 0;
        var disposable = new ActionDisposable(() => executionCount++);

        // Act
        disposable.Dispose();
        disposable.Dispose();
        disposable.Dispose();

        // Assert
        Assert.That(executionCount, Is.EqualTo(1));
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
    [Test]
    public void CompositeDisposable_NegativeCapacity_ThrowsArgumentOutOfRangeException() =>

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new CompositeDisposable(-1));

    /// <summary>
    /// Test CompositeDisposable with array constructor.
    /// </summary>
    [Test]
    public void CompositeDisposable_ArrayConstructor_DisposesAllItems()
    {
        // Arrange
        var disposed1 = false;
        var disposed2 = false;
        var action1 = new ActionDisposable(() => disposed1 = true);
        var action2 = new ActionDisposable(() => disposed2 = true);

        var disposable = new CompositeDisposable(action1, action2);

        // Act
        disposable.Dispose();

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(disposed1, Is.True);
            Assert.That(disposed2, Is.True);
        }
    }

    /// <summary>
    /// Test CompositeDisposable with enumerable constructor.
    /// </summary>
    [Test]
    public void CompositeDisposable_EnumerableConstructor_DisposesAllItems()
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

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(disposed1, Is.True);
            Assert.That(disposed2, Is.True);
            Assert.That(disposed3, Is.True);
        }
    }

    /// <summary>
    /// Test CompositeDisposable throws for null array.
    /// </summary>
    [Test]
    public void CompositeDisposable_NullArray_ThrowsArgumentNullException() =>

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CompositeDisposable((IDisposable[])null!));

    /// <summary>
    /// Test CompositeDisposable throws for null enumerable.
    /// </summary>
    [Test]
    public void CompositeDisposable_NullEnumerable_ThrowsArgumentNullException() =>

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CompositeDisposable((IEnumerable<IDisposable>)null!));

    /// <summary>
    /// Test CompositeDisposable throws for null item in array.
    /// </summary>
    [Test]
    public void CompositeDisposable_NullItemInArray_ThrowsArgumentException() =>

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CompositeDisposable(new ActionDisposable(() => { }), null!));

    /// <summary>
    /// Test CompositeDisposable throws for null item in enumerable.
    /// </summary>
    [Test]
    public void CompositeDisposable_NullItemInEnumerable_ThrowsArgumentException()
    {
        // Arrange
        var disposables = new List<IDisposable> { new ActionDisposable(() => { }), null! };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CompositeDisposable(disposables));
    }

    /// <summary>
    /// Test CompositeDisposable multiple dispose calls are safe.
    /// </summary>
    [Test]
    public void CompositeDisposable_MultipleDisposeCallsAreSafe()
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
        Assert.That(executionCount, Is.EqualTo(1));
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
