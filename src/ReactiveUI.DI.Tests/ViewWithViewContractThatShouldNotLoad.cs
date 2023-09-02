using Splat.Common.Test;

namespace ReactiveUI.DI.Tests;

/// <summary>
/// This is a test view relating to issue #889.
/// It's intended to ensure that view registration by different DI\IoC implementations
/// does not create an instance at the point of registration.
/// </summary>
[ViewContract("somecontract")]
public sealed class ViewWithViewContractThatShouldNotLoad : IViewFor<ViewModelOne>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewWithViewContractThatShouldNotLoad"/> class.
    /// </summary>
    public ViewWithViewContractThatShouldNotLoad() => throw new InvalidOperationException("This view should not be created.");

    /// <inheritdoc />
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ViewModelOne?)value;
    }

    /// <inheritdoc />
    public ViewModelOne? ViewModel { get; set; }
}
