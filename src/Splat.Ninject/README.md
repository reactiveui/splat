# Splat.Ninject

## Using Ninject

Splay.Ninject is an adapter for `IMutableDependencyResolver`.  It allows you to register your application dependencies in a Ninject `IKernel`.  You can then use the container as Splat's internal dependency resolver.

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

Now calls to `Locator.Current` will resolve to the underlying Ninject container.  In the case of ReactiveUI, platform registrations will now happen in the Ninject container.  So when the platform calls to resolve dependencies, the will resolve from the Ninject container.