# Splat.Ninject

## Using Ninject

Splat.Ninject is an adapter for `IMutableDependencyResolver`.  It allows you to register your application dependencies in a Ninject `IKernel`.  You can then use the container as Splat's internal dependency resolver.

### Register the Kernel

```cs
var kernel = new StandardKernel();
kernel.Bind<IViewFor<MainViewModel>>().To<MainPage>();
kernel.Bind<IViewFor<SecondaryViewModel>>().To<SecondaryPage>();
kernel.Bind<MainViewModel>().ToSelf();
kernel.Bind<SecondaryViewModel>().ToSelf();
```

### Register the Adapter to Splat

```cs
container.UseNinjectDependencyResolver();
```

### Use the Locator

Now, when registering or resolving services using Locator.Current, or via ReactiveUI, they will be directed to the Ninject DI container.
