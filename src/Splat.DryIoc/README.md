# Splat.DryIoc

## Using DryIoc

Splay.DryIoc is an adapter for `IMutableDependencyResolver`.  It allows you to register your application dependencies in a DryIoc `Container`.  You can then use the container as Splat's internal dependency resolver.

### Register the Container

```cs
var container = new Container();
container.Register<IViewFor<MainViewModel>, MainPage>();
container.Register<IViewFor<SecondaryViewModel>, SecondaryPage>();
container.Register<MainViewModel>();
container.Register<SecondaryViewModel>();
```

### Register the Adapter to Splat

```cs
container.UseDryIocDependencyResolver();
```

### Use the Locator

Now, when registering or resolving services using Locator.Current, or via ReactiveUI, they will be directed to the DryIoc DI container.
