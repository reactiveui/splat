# Splat.Autofac

## Using Autofac

Splat.Autofac is an adapter for `IMutableDependencyResolver`.  It allows you to register your application dependencies in a Autofac `Container`.  You can then use the container as Splat's internal dependency resolver.

### Register the Container

```cs
var builder = new ContainerBuilder();
builder.RegisterType<MainPage>().As<IViewFor<MainViewModel>>();
builder.RegisterType<SecondaryPage>().As<IViewFor<SecondaryViewModel>>();
builder.RegisterType<MainViewModel>().AsSelf();
builder.RegisterType<SecondaryViewModel>().AsSelf();
// etc.
```

### Register the Adapter to Splat

```cs
// Creates and sets the Autofac resolver as the Locator
var autofacResolver = builder.UseAutofacDependencyResolver();

// Register the resolver in Autofac so it can be later resolved
builder.RegisterInstance(autofacResolver);

// Initialize ReactiveUI components
autofacResolver.InitializeReactiveUI();

// If you need to override any service (such as the ViewLocator), register it after InitializeReactiveUI
// https://autofaccn.readthedocs.io/en/latest/register/registration.html#default-registrations
// builder.RegisterType<MyCustomViewLocator>().As<IViewLocator>().SingleInstance();
```

### Set Autofac Locator's lifetime after the ContainerBuilder has been built

```cs
var autofacResolver = container.Resolve<AutofacDependencyResolver>();

// Set a lifetime scope (either the root or any of the child ones) to Autofac resolver
// This is needed, because the previous steps did not Update the ContainerBuilder since they became immutable in Autofac 5+
// https://github.com/autofac/Autofac/issues/811
autofacResolver.SetLifetimeScope(container);`
```

### Use the Locator

Now, when registering or resolving services using Locator.Current, or via ReactiveUI, they will be directed to the Autofac DI container.
