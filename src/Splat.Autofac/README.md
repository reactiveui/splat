# Splat.Autofac

## Using Autofac

Splat.Autofac is an adapter for `IMutableDependencyResolver`.  It allows you to register your application dependencies in a Autofac `Container`.  You can then use the container as Splat's internal dependency resolver.

### Register the Container

```cs
var containerBuilder = new ContainerBuilder();
containerBuilder.RegisterType<MainPage>().As<IViewFor<MainViewModel>>();
containerBuilder.RegisterType<SecondaryPage>().As<IViewFor<SecondaryViewModel>>();
containerBuilder.RegisterType<MainViewModel>().AsSelf();
containerBuilder.RegisterType<SecondaryViewModel>().AsSelf();
// etc.
```

### Register the Adapter to Splat

```cs
// Creates and sets the Autofac resolver as the Locator
var autofacResolver = containerBuilder.UseAutofacDependencyResolver();

// Register the resolver in Autofac so it can be later resolved
containerBuilder.RegisterInstance(autofacResolver);

// Initialize ReactiveUI components
autofacResolver.InitializeReactiveUI();
```

### Set Autofac Locator's lifetime after the ContainerBuilder has been built

```cs
var autofacResolver = container.Resolve<AutofacDependencyResolver>();

// Set a lifetime scope (either the root or any of the child ones) to Autofac resolver
autofacResolver.SetLifetimeScope(container);`
```

### Use the Locator

Now, when registering or resolving services using Locator.Current, or via ReactiveUI, they will be directed to the Autofac DI container.
