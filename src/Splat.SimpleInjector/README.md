# Splat.SimpleInjector

## Using SimpleInjector

Splat.SimpleInjector is an adapter for `IMutableDependencyResolver`.  It allows you to register your application dependencies in a SimpleInjector `Container`.  You can then use the container as Splat's internal dependency resolver.

### Register the Container

Because of internal SimpleInjector behaviours in order to register it as a container intermediate "dummy" container has to be used - `SimpleInjectorInitializer`.

```cs
SimpleInjectorInitializer initializer = new SimpleInjectorInitializer();
Locator.SetLocator(initializer);

// Register ReactiveUI dependencies
Locator.CurrentMutable.InitializeSplat();
Locator.CurrentMutable.InitializeReactiveUI();

// Registering Views
Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());

// Register SimpleInjector
Container container = new Container();

// Actual SimpleInjector registration
container.UseSimpleInjectorDependencyResolver(initializer);

// SimpleInjector dependency registrations
container.Register<Example>();
container.Register<Example2>();

// Optional overriding default implementations
container.Options.AllowOverridingRegistrations = true;
container.Register<IViewLocator, MyCustomViewLocator>();

```
