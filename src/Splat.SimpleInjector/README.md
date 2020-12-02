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

// SimpleInjector by default only allows a single registration of a service.
// Splat would typically allow multiple registrations and by default return the
// last registration.
// If you set AllowOverridingRegistrations on SimpleInjector it REPLACES the last
// registration so BE AWARE.
// For more details see: https://simpleinjector.readthedocs.io/en/latest/howto.html#override-existing-registrations
// We may produce a workaround in Splat in future, but for now keep the limitation
// in mind.
container.Options.AllowOverridingRegistrations = true;
container.Register<IViewLocator, MyCustomViewLocator>();

```
