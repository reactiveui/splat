# Splat.Autofac

## Using Autofac

Splay.Autofac is an adapter for `IMutableDependencyResolver`.  It allows you to register your application dependencies in a Autofac `Container`.  You can then use the container as Splat's internal dependency resolver.

### Register the Container

```cs
var container = new ContainerBuilder();
container.RegisterType<MainPage>().As<IViewFor<MainViewModel>>();
container.RegisterType<SecondaryPage>().As<IViewFor<SecondaryViewModel>>();
container.RegisterType<MainViewModel>().AsSelf();
container.RegisterType<SecondaryViewModel>().AsSelf();
```

### Register the Adapter to Splat

```cs
container.UseAutofacDependencyResolver();
```

### Use the Locator

Now, when registering or resolving services using Locator.Current, or via ReactiveUI, they will be directed to the Autofac DI container.
